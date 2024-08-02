using System.Drawing;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace MPhotoBoothAI.Infrastructure;

public class YoloFace(Net net, ResizeImageService resizeImageService)
{
    private readonly Net _net = net;
    private readonly ResizeImageService _resizeImageService = resizeImageService;
    private readonly int _inputHeight = 640;
    private readonly int _inputWidth = 640;
    private readonly int _numClass = 1;
    private readonly int _regMax = 16;
    private float confThreshold;

    public void Run(Mat frame, float confThreshold, float nmsThreshold)
    {
        var resized = _resizeImageService.Resize(frame);
        using var blob = DnnInvoke.BlobFromImage(resized.Image, 1 / 255.0, new Size(_inputWidth, _inputHeight), new MCvScalar(0, 0, 0), true, false);
        resized.Image.Dispose();
        _net.SetInput(blob);
        using var outs = new VectorOfMat();
        _net.Forward(outs, _net.UnconnectedOutLayersNames);

        float ratioh = (float)frame.Rows / resized.Newh, ratiow = (float)frame.Cols / resized.Neww;

        List<Rectangle> boxes = [];
        List<float> confidences = [];
        List<List<Point>> landmarks = [];
        this.confThreshold = confThreshold;
        GenerateProposal(outs[0], boxes, confidences, landmarks, frame.Rows, frame.Cols, ratioh, ratiow, resized.Padh, resized.Padw);
        GenerateProposal(outs[1], boxes, confidences, landmarks, frame.Rows, frame.Cols, ratioh, ratiow, resized.Padh, resized.Padw);
        GenerateProposal(outs[2], boxes, confidences, landmarks, frame.Rows, frame.Cols, ratioh, ratiow, resized.Padh, resized.Padw);

        var indices = DnnInvoke.NMSBoxes(boxes.ToArray(), [.. confidences], confThreshold, nmsThreshold);
        for (int i = 0; i < indices.Length; ++i)
        {
            int idx = indices[i];
            Rectangle box = boxes[idx];
            using var mat = frame.Clone();
            DrawPred(mat, box, confidences[idx]);
            mat.Save($"{i}.jpg");
        }
    }

    public void GenerateProposal(Mat outMat, List<Rectangle> boxes, List<float> confidences, List<List<Point>> landmarks,
             int imgh, int imgw, float ratioh, float ratiow, int padh, int padw)
    {
        int[] sizes = outMat.SizeOfDimension;
        int feat_h = sizes[2];
        int feat_w = sizes[3];
        int stride = (int)Math.Ceiling((float)_inputHeight / feat_h);
        int area = feat_h * feat_w;

        float[,,,] matData = (float[,,,])outMat.GetData();
        float[] ptr = FlattenArray(matData);

        int ptrClsSourceIndex = area * _regMax * 4;
        int ptr_cls_length = ptr.Length - ptrClsSourceIndex;
        float[] ptr_cls = new float[ptr_cls_length];
        Array.Copy(ptr, ptrClsSourceIndex, ptr_cls, 0, ptr_cls_length);

        int ptrKpSourceIndex = area * (_regMax * 4 + _numClass);
        int ptr_kp_length = ptr.Length - ptrKpSourceIndex;
        float[] ptr_kp = new float[ptr_kp_length];
        Array.Copy(ptr, ptrKpSourceIndex, ptr_kp, 0, ptr_kp_length);

        for (int i = 0; i < feat_h; i++)
        {
            for (int j = 0; j < feat_w; j++)
            {
                int index = i * feat_w + j;
                float max_conf = -10000;
                for (int k = 0; k < _numClass; k++)
                {
                    float conf = ptr_cls[k * area + index];
                    if (conf > max_conf)
                    {
                        max_conf = conf;
                    }
                }
                float box_prob = Sigmoid(max_conf);
                if (box_prob > confThreshold)
                {
                    float[] pred_ltrb = new float[4];
                    float[] dfl_value = new float[_regMax];
                    float[] dfl_softmax = new float[_regMax];
                    for (int k = 0; k < 4; k++)
                    {
                        for (int n = 0; n < _regMax; n++)
                        {
                            dfl_value[n] = ptr[(k * _regMax + n) * area + index];
                        }
                        Softmax(dfl_value, dfl_softmax, _regMax);

                        float dis = 0f;
                        for (int n = 0; n < _regMax; n++)
                        {
                            dis += n * dfl_softmax[n];
                        }

                        pred_ltrb[k] = dis * stride;
                    }
                    float cx = (j + 0.5f) * stride;
                    float cy = (i + 0.5f) * stride;
                    float xmin = Math.Max((cx - pred_ltrb[0] - padw) * ratiow, 0f);
                    float ymin = Math.Max((cy - pred_ltrb[1] - padh) * ratioh, 0f);
                    float xmax = Math.Min((cx + pred_ltrb[2] - padw) * ratiow, imgw - 1);
                    float ymax = Math.Min((cy + pred_ltrb[3] - padh) * ratioh, imgh - 1);
                    Rectangle box = new Rectangle((int)xmin, (int)ymin, (int)(xmax - xmin), (int)(ymax - ymin));
                    boxes.Add(box);
                    confidences.Add(box_prob);

                    List<Point> kpts = new List<Point>(5);
                    for (int k = 0; k < 5; k++)
                    {
                        float x = ((ptr_kp[(k * 3) * area + index] * 2 + j) * stride - padw) * ratiow;
                        float y = ((ptr_kp[(k * 3 + 1) * area + index] * 2 + i) * stride - padh) * ratioh;
                        kpts.Add(new Point((int)x, (int)y));
                    }
                    landmarks.Add(kpts);
                }
            }
        }
    }

    private static float[] FlattenArray(float[,,,] multiArray)
    {
        int totalLength = multiArray.GetLength(0) * multiArray.GetLength(1) * multiArray.GetLength(2) * multiArray.GetLength(3);
        float[] flatArray = new float[totalLength];
        int index = 0;
        for (int i = 0; i < multiArray.GetLength(0); i++)
        {
            for (int j = 0; j < multiArray.GetLength(1); j++)
            {
                for (int k = 0; k < multiArray.GetLength(2); k++)
                {
                    for (int l = 0; l < multiArray.GetLength(3); l++)
                    {
                        flatArray[index++] = multiArray[i, j, k, l];
                    }
                }
            }
        }
        return flatArray;
    }

    private static float Sigmoid(float x) => 1f / (1f + (float)MathF.Exp(-x));

    private static void Softmax(float[] x, float[] y, int length)
    {
        float sum = 0;
        for (int i = 0; i < length; i++)
        {
            y[i] = MathF.Exp(x[i]);
            sum += y[i];
        }
        for (int i = 0; i < length; i++)
        {
            y[i] /= sum;
        }
    }

    private static void DrawPred(Mat frame, Rectangle box, float conf)
    {
        CvInvoke.Rectangle(frame, box, new MCvScalar(0, 0, 255), 3);
        CvInvoke.PutText(frame, conf.ToString(), new Point(100, 100), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 5, new MCvScalar(0, 0, 255), 1);
    }
}

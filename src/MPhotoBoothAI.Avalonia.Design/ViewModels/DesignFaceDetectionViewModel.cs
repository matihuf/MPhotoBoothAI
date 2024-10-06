using Avalonia.Platform;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;

public class DesignFaceDetectionViewModel : FaceDetectionViewModel
{
    public DesignFaceDetectionViewModel() : base(new Mock<ICameraDevice>().Object, new Mock<IFaceSwapManager>().Object, new Mock<IFilePickerService>().Object)
    {
        byte[] rawFrame = ReadFully(AssetLoader.Open(new Uri("avares://MPhotoBoothAI.Avalonia.Design/Assets/nocamera.png")));
        var frame = new Mat();
        CvInvoke.Imdecode(rawFrame, ImreadModes.Unchanged, frame);
        Frame = frame;
    }

    private static byte[] ReadFully(Stream input)
    {
        using var ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
}

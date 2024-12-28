
using Emgu.CV;
using MPhotoBoothAI.Models.FaceSwaps;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IAddFaceSwapTemplateManager
{
    Task<FaceSwapTemplate?> PickTemplate();
    int SaveTemplate(int groupId, FaceSwapTemplate faceSwapTemplate);
    Mat SwapFaces(Mat source, Mat target);
}


using Emgu.CV;
using MPhotoBoothAI.Models.FaceSwaps;

namespace MPhotoBoothAI.Application.Interfaces;
public interface IAddFaceSwapTemplateManager
{
    Task<FaceSwapTemplate?> PickTemplate();
    void SaveTemplate(string groupName, FaceSwapTemplate faceSwapTemplate);
    Mat SwapFaces(Mat source, Mat target);
}

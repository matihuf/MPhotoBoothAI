using Emgu.CV;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceAlignManager
{
    FaceAlign? GetAlign(Mat frame);
    IEnumerable<FaceAlignDetails> GetAligns(Mat frame);
}

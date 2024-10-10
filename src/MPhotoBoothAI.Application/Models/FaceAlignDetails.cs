using Emgu.CV;

namespace MPhotoBoothAI.Application.Models;

public class FaceAlignDetails(Mat norm, Mat align, Gender gender) : FaceAlign(norm, align)
{
    public Gender Gender { get; private set; } = gender;
}

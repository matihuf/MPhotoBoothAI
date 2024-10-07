using Emgu.CV;
using MPhotoBoothAI.Application.Models;

namespace MPhotoBoothAI.Application.Interfaces;

public interface IFaceGenderService
{
    Gender Get(Mat face);
    Gender GetVgg(Mat face);
}

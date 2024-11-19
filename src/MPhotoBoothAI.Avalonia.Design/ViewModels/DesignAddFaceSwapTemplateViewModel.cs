using Avalonia.Platform;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;
public class DesignAddFaceSwapTemplateViewModel : AddFaceSwapTemplateViewModel
{
    public DesignAddFaceSwapTemplateViewModel() : base(new Mock<IAddFaceSwapTemplateManager>().Object, new Mock<ICameraManager>().Object)
    {
        byte[] rawFrame = ReadFully(AssetLoader.Open(new Uri("avares://MPhotoBoothAI.Avalonia.Design/Assets/nocamera.png")));
        var frame = new Mat();
        CvInvoke.Imdecode(rawFrame, ImreadModes.Unchanged, frame);
        Image = frame;
        CameraFrame = frame;
    }

    private static byte[] ReadFully(Stream input)
    {
        using var ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
}

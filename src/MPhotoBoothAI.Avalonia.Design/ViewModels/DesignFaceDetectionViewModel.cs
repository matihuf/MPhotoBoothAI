using Avalonia.Platform;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;
using SkiaSharp;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;

public class DesignFaceDetectionViewModel : FaceDetectionViewModel
{
    public DesignFaceDetectionViewModel() : base(new Mock<ICameraDevice>().Object, new Mock<IFaceSwapManager>().Object, new Mock<IFilePickerService>().Object)
    {
        byte[] rawFrame = ReadFully(AssetLoader.Open(new Uri("avares://MPhotoBoothAI.Avalonia.Design/Assets/nocamera.png")));
        var frame = SKBitmap.Decode(rawFrame);
        Frame = frame;
    }

    private static byte[] ReadFully(Stream input)
    {
        using var ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
}

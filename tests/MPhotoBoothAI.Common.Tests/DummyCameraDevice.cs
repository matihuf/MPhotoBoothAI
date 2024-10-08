using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;
using SkiaSharp;

namespace MPhotoBoothAI.Common.Tests;

public class DummyCameraDevice : ICameraDevice
{
    public void Attach(IObserver observer)
    {
        //no need to start camera for tests
    }

    public void Detach(IObserver observer)
    {
        //no need to start camera for tests
    }

    public void Notify(SKBitmap bitmap)
    {
        //no need to start camera for tests
    }

    public void Start()
    {
        //no need to start camera for tests
    }
}

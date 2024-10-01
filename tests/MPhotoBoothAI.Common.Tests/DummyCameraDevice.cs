using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.Interfaces.Observers;

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

    public void Notify(Mat mat)
    {
        //no need to start camera for tests
    }

    public void Start()
    {
        //no need to start camera for tests
    }
}

using Emgu.CV;

namespace MPhotoBoothAI.Application;

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(Mat mat);
}

namespace MPhotoBoothAI.Application.Interfaces;
public interface IWindowParam<T> where T : class
{
    public T? Parameters { get; set; }
}

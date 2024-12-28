namespace MPhotoBoothAI.Application.Interfaces;
public interface IWindowResult<T> where T : class
{
    public T? Result { get; set; }
}

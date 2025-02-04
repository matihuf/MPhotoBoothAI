namespace MPhotoBoothAI.Application.Interfaces;
public interface IApplicationInfoService
{
    string Company { get; }
    string Product { get; }
    string UserProfilePath { get; }
    string Version { get; }
    string BackgroundDirectory { get; }
}

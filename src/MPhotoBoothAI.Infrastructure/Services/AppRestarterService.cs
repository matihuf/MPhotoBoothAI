using MPhotoBoothAI.Application.Interfaces;
using System.Diagnostics;

namespace MPhotoBoothAI.Infrastructure.Services;
public class AppRestarterService : IAppRestarterService
{
    public void Restart()
    {
        var currentProcess = Process.GetCurrentProcess();
        if (currentProcess == null)
        {
            return;
        }
        var fileName = currentProcess.MainModule?.FileName;
        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }
        Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
        currentProcess.Kill();
    }
}

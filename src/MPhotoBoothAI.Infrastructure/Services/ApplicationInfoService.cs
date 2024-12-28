using MPhotoBoothAI.Application;
using MPhotoBoothAI.Application.Interfaces;
using System.Diagnostics;
using System.Reflection;

namespace MPhotoBoothAI.Infrastructure.Services;
public class ApplicationInfoService : IApplicationInfoService
{
    private readonly FileVersionInfo? _fvi;

    public string Company => _fvi?.CompanyName ?? string.Empty;
    public string Product => _fvi?.ProductName ?? string.Empty;
    public string UserProfilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Company, Product);
    public string Version => _fvi?.FileVersion ?? string.Empty;
    public string PostcardBackgroundPath => string.Concat(UserProfilePath, Consts.Background.PostcardBackground);
    public string StripeBackgroundPath => string.Concat(UserProfilePath, Consts.Background.StripeBackground);

    public ApplicationInfoService()
    {
        Assembly? assembly = Assembly.GetEntryAssembly();
        if (assembly != null)
        {
            _fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        }
    }
}

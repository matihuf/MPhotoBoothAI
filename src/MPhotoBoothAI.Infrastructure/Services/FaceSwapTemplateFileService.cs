using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.Services;
public class FaceSwapTemplateFileService(IApplicationInfoService applicationInfoService, IResizeImageService resizeImageService) : IFaceSwapTemplateFileService
{
    private readonly IApplicationInfoService _applicationInfoService = applicationInfoService;
    private readonly IResizeImageService _resizeImageService = resizeImageService;

    private readonly string _baseFolder = "Templates";
    private readonly string _thumbnailSuffix = "_thumbnail";

    public string Save(string groupName, string templateId, string filePath)
    {
        string directoryPath = Path.Combine(_applicationInfoService.UserProfilePath, _baseFolder, groupName, templateId);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string fileExtension = Path.GetExtension(filePath);
        string templatePath = Path.Combine(directoryPath, templateId);
        string fullTemplatePath = $"{templatePath}{fileExtension}";
        File.Copy(filePath, fullTemplatePath);

        using var template = CvInvoke.Imread(filePath);
        using var thumbnail = _resizeImageService.GetThumbnail(template, 0.6f);
        CvInvoke.Imwrite($"{templatePath}{_thumbnailSuffix}{fileExtension}", thumbnail);
        return fullTemplatePath;
    }
}

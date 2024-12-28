using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Infrastructure.Services;
public class FaceSwapTemplateFileService(IApplicationInfoService applicationInfoService, IResizeImageService resizeImageService) : IFaceSwapTemplateFileService
{
    private readonly IApplicationInfoService _applicationInfoService = applicationInfoService;
    private readonly IResizeImageService _resizeImageService = resizeImageService;

    private readonly string _baseFolder = "Templates";
    private readonly string _thumbnailSuffix = "_thumbnail";
    private readonly string _imageExtension = ".jpg";

    public void Save(int groupId, int templateId, string filePath)
    {
        string directoryPath = Path.Combine(_applicationInfoService.UserProfilePath, _baseFolder, groupId.ToString(), templateId.ToString());
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string templatePath = Path.Combine(directoryPath, templateId.ToString());
        using var image = CvInvoke.Imread(filePath);
        CvInvoke.Imwrite($"{templatePath}{_imageExtension}", image, new KeyValuePair<ImwriteFlags, int>(ImwriteFlags.JpegQuality, 100));

        using var thumbnail = _resizeImageService.GetThumbnail(image, 0.6f);
        CvInvoke.Imwrite($"{templatePath}{_thumbnailSuffix}{_imageExtension}", thumbnail, new KeyValuePair<ImwriteFlags, int>(ImwriteFlags.JpegQuality, 100));
    }

    public Stream ReadThumbnail(int groupId, int templateId) => File.OpenRead(GetFullTemplateThumbnailPath(groupId, templateId));

    public string GetFullTemplatePath(int groupId, int templateId) => Path.Combine(GetDirectoryPath(groupId, templateId), $"{templateId}{_imageExtension}");

    public string GetFullTemplateThumbnailPath(int groupId, int templateId) => Path.Combine(GetDirectoryPath(groupId, templateId), $"{templateId}{_thumbnailSuffix}{_imageExtension}");

    private string GetDirectoryPath(int groupId, int templateId) => Path.Combine(_applicationInfoService.UserProfilePath, _baseFolder, groupId.ToString(), templateId.ToString());
}

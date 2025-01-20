using Emgu.CV;
using Emgu.CV.CvEnum;
using MPhotoBoothAI.Application.Interfaces;

namespace MPhotoBoothAI.Application.Managers.FaceSwapTemplates;
public class FaceSwapTemplateFileManager(IApplicationInfoService applicationInfoService, IResizeImageService resizeImageService) : IFaceSwapTemplateFileManager
{
    private readonly IApplicationInfoService _applicationInfoService = applicationInfoService;
    private readonly IResizeImageService _resizeImageService = resizeImageService;

    private readonly string _baseFolder = "Templates";
    private readonly string _thumbnailSuffix = "_thumbnail";
    private readonly string _imageExtension = ".png";
    private readonly KeyValuePair<ImwriteFlags, int> _imageOptions = new(ImwriteFlags.PngCompression, 0);

    public void Save(int groupId, int templateId, string filePath)
    {
        string directoryPath = Path.Combine(_applicationInfoService.UserProfilePath, _baseFolder, groupId.ToString(), templateId.ToString());
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        string templatePath = Path.Combine(directoryPath, templateId.ToString());
        using var image = CvInvoke.Imread(filePath, ImreadModes.Unchanged);
        CvInvoke.Imwrite($"{templatePath}{_imageExtension}", image, _imageOptions);
        bool keepRatio = image.Height > image.Width;
        using var imageBgra = new Mat();
        CvInvoke.CvtColor(image, imageBgra, ColorConversion.Bgr2Bgra);
        using var thumbnail = _resizeImageService.Resize(imageBgra, 192, 340, keepRatio);
        CvInvoke.Imwrite($"{templatePath}{_thumbnailSuffix}{_imageExtension}", thumbnail.Image);
    }

    public void DeleteGroup(int groupId)
    {
        string groupPath = GetGroupDirectoryPath(groupId);
        if (Directory.Exists(groupPath))
        {
            Directory.Delete(groupPath, true);
        }
    }

    public void DeleteTemplate(int groupId, int templateId)
    {
        string templatePath = GetTemplateDirectoryPath(groupId, templateId);
        if (Directory.Exists(templatePath))
        {
            Directory.Delete(templatePath, true);
        }
    }

    public string GetFullTemplatePath(int groupId, int templateId) => Path.Combine(GetTemplateDirectoryPath(groupId, templateId), $"{templateId}{_imageExtension}");

    public string GetFullTemplateThumbnailPath(int groupId, int templateId) => Path.Combine(GetTemplateDirectoryPath(groupId, templateId), $"{templateId}{_thumbnailSuffix}{_imageExtension}");

    private string GetGroupDirectoryPath(int groupId) => Path.Combine(_applicationInfoService.UserProfilePath, _baseFolder, groupId.ToString());

    private string GetTemplateDirectoryPath(int groupId, int templateId) => Path.Combine(_applicationInfoService.UserProfilePath, _baseFolder, groupId.ToString(), templateId.ToString());
}


namespace MPhotoBoothAI.Application.Interfaces;
public interface IFaceSwapTemplateFileService
{
    string GetFullTemplatePath(int groupId, int templateId);
    string GetFullTemplateThumbnailPath(int groupId, int templateId);
    Stream ReadThumbnail(int groupId, int templateId);
    void Save(int groupId, int templateId, string filePath);
}

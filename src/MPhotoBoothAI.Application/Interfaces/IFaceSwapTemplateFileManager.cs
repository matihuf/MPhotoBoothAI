
namespace MPhotoBoothAI.Application.Interfaces;
public interface IFaceSwapTemplateFileManager
{
    void DeleteGroup(int groupId);
    void DeleteTemplate(int groupId, int templateId);
    string GetFullTemplatePath(int groupId, int templateId);
    string GetFullTemplateThumbnailPath(int groupId, int templateId);
    void Save(int groupId, int templateId, string filePath);
}

namespace MPhotoBoothAI.Application.Interfaces;
public interface IFaceSwapTemplateFileService
{
    /// <summary>
    /// Save template
    /// </summary>
    /// <param name="groupName"></param>
    /// <param name="template"></param>
    /// <returns>Full path to template</returns>
    string Save(string groupName, string templateId, string filePath);
}

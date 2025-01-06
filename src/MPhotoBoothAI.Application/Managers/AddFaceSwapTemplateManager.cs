using Emgu.CV;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.Entities;
using MPhotoBoothAI.Models.Enums;
using MPhotoBoothAI.Models.FaceSwaps;

namespace MPhotoBoothAI.Application.Managers;
public class AddFaceSwapTemplateManager(IFilePickerService filePickerService, IFaceDetectionManager faceDetectionManager,
    IFaceSwapTemplateFileManager faceSwapTemplateFileManager, IDatabaseContext databaseContext) : IAddFaceSwapTemplateManager
{
    private readonly IFilePickerService _filePickerService = filePickerService;
    private readonly IFaceDetectionManager _faceDetectionManager = faceDetectionManager;
    private readonly IFaceSwapTemplateFileManager _faceSwapTemplateFileManager = faceSwapTemplateFileManager;
    private readonly IDatabaseContext _databaseContext = databaseContext;

    public async Task<FaceSwapTemplate?> PickTemplate()
    {
        var filePath = await _filePickerService.PickFilePath([FilePickerFileType.Image]);
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        var image = CvInvoke.Imread(filePath);
        int faces = _faceDetectionManager.Mark(image);
        return new FaceSwapTemplate(filePath, faces, image);
    }

    public int SaveTemplate(int groupId, FaceSwapTemplate faceSwapTemplate)
    {
        var entity = new FaceSwapTemplateEntity
        {
            FaceSwapTemplateGroupId = groupId,
            Faces = faceSwapTemplate.Faces
        };
        _databaseContext.FaceSwapTemplates.Add(entity);
        _databaseContext.SaveChanges();
        _faceSwapTemplateFileManager.Save(groupId, entity.Id, faceSwapTemplate.FilePath);
        return entity.Id;
    }
}

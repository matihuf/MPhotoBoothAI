using Emgu.CV;
using Emgu.CV.Structure;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models.Entities;
using MPhotoBoothAI.Models.Enums;
using MPhotoBoothAI.Models.FaceSwaps;

namespace MPhotoBoothAI.Application.Managers;
public class AddFaceSwapTemplateManager(IFilePickerService filePickerService, IFaceDetectionService faceDetectionService,
    IFaceMultiSwapManager faceMultiSwapManager, IFaceSwapTemplateFileManager faceSwapTemplateFileManager, IDatabaseContext databaseContext) : IAddFaceSwapTemplateManager
{
    private readonly IFilePickerService _filePickerService = filePickerService;
    private readonly IFaceDetectionService _faceDetectionService = faceDetectionService;
    private readonly IFaceMultiSwapManager _faceMultiSwapManager = faceMultiSwapManager;
    private readonly IFaceSwapTemplateFileManager _faceSwapTemplateFileManager = faceSwapTemplateFileManager;
    private readonly IDatabaseContext _databaseContext = databaseContext;

    public async Task<FaceSwapTemplate?> PickTemplate()
    {
        var filePath = await _filePickerService.PickFilePath([FilePickerFileType.Image]);
        if (string.IsNullOrEmpty(filePath))
        {
            return null;
        }
        using var image = CvInvoke.Imread(filePath);
        int faces = DetectFaces(image);
        return new FaceSwapTemplate(filePath, faces);
    }

    private int DetectFaces(Mat frame)
    {
        int faceIndex = 0;
        foreach (var face in _faceDetectionService.Detect(frame, 0.8f, 0.5f))
        {
            CvInvoke.Rectangle(frame, face.Box, new MCvScalar(0, 0, 255));
            faceIndex++;
            face.Dispose();
        }
        return faceIndex;
    }

    public Mat SwapFaces(Mat source, Mat target) => _faceMultiSwapManager.Swap(source, target);

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

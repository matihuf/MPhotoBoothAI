using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels.FaceSwapTemplates;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;
public class DesignFaceSwapTemplatesViewModel : FaceSwapGroupTemplatesViewModel
{
    public DesignFaceSwapTemplatesViewModel() : base(DesignTimeDbContextFactory.CreateDbContext(), new Mock<IMessageBoxService>().Object,
        new Mock<IWindowService>().Object, new Mock<IFaceSwapTemplateFileManager>().Object)
    {
        Templates =
        [
            new Models.FaceSwaps.FaceSwapTemplateId(1, "avares://MPhotoBoothAI.Avalonia.Design/Assets/nocamera.png", 5)
        ];
    }
}

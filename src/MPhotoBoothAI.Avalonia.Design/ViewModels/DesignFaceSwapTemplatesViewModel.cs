using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;
public class DesignFaceSwapTemplatesViewModel : FaceSwapTemplatesViewModel
{
    public DesignFaceSwapTemplatesViewModel() : base(DesignTimeDbContextFactory.CreateDbContext(), new Mock<IMessageBoxService>().Object,
        new Mock<IWindowService>().Object, new Mock<IFaceSwapTemplateFileService>().Object)
    {
    }
}

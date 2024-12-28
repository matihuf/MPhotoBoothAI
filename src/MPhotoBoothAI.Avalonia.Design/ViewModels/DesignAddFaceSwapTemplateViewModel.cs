using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Application.ViewModels;

namespace MPhotoBoothAI.Avalonia.Design.ViewModels;
public class DesignAddFaceSwapTemplateViewModel : AddFaceSwapTemplateViewModel
{
    public DesignAddFaceSwapTemplateViewModel() : base(new Mock<IAddFaceSwapTemplateManager>().Object, new Mock<ICameraManager>().Object)
    {
    }
}

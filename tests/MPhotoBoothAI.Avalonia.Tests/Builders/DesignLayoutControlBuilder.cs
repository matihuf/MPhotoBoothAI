using Avalonia.Controls;
using Moq;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Avalonia.Controls;

namespace MPhotoBoothAI.Avalonia.Tests.Builders;
public class DesignLayoutControlBuilder
{
    private readonly DesignLayoutControl _control;

    private Mock<IFilePickerService> _filePickerMock = new();

    public DesignLayoutControlBuilder()
    {
        _control = new DesignLayoutControl();
        _control.FilePicker = _filePickerMock.Object;
        var window = new Window { Content = _control };
        window.Show();
    }

    public DesignLayoutControlBuilder WithLayoutBackgroundPath(string path)
    {
        _control.LayoutBackgroundPath = path;
        return this;
    }

    public DesignLayoutControlBuilder WithActiveLayerSwitch(bool isActive)
    {
        _control.ActiveLayerSwitch = isActive;
        return this;
    }

    public DesignLayoutControl Build() => _control;
}

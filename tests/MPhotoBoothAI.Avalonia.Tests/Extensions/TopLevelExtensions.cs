using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Input;

namespace MPhotoBoothAI.Avalonia.Tests.Extensions;

public static class TopLevelExtensions
{
    public static void MouseClick(this TopLevel topLevel, Point point, MouseButton button = MouseButton.Left, RawInputModifiers modifiers = RawInputModifiers.None)
    {
        topLevel.MouseDown(point, button);
        topLevel.MouseUp(point, button);
    }
}

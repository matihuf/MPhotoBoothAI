using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace MPhotoBoothAI.Avalonia.Tests.Extensions;

public static class ControlExtensions
{
    public static IEnumerable<T> FindControls<T>(this Control root) where T : Control
    {
        var controls = new List<T>();
        foreach (var child in root.GetVisualChildren())
        {
            if (child is T control)
            {
                controls.Add(control);
            }
            else if (child is Control childControl)
            {
                controls.AddRange(childControl.FindControls<T>());
            }
        }
        return controls;
    }

    public static void Press(this Control control, Window window)
    {
        control.Focus();
        window.KeyReleaseQwerty(PhysicalKey.Space, RawInputModifiers.None);
    }
}

using Avalonia.Controls;
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
}

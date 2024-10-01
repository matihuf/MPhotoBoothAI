using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaApplication = Avalonia.Application;

namespace MPhotoBoothAI.Avalonia.Converters;

public class IconConverters
{
    public static FuncValueConverter<string, StreamGeometry> IconConverter { get; } =
        new(iconKey =>
        {
            AvaloniaApplication.Current!.TryFindResource(iconKey, out var resource);
            return resource as StreamGeometry ?? StreamGeometry.Parse(StreamGeometryNotFound);
        });
}
namespace MPhotoBoothAI.Application.Interfaces;
using DiskInfo = (int? bytesPerSector, int? numberOfFreeClusters);

public interface IDiskInfoService
{
    DiskInfo GetBytesPerSector();
}

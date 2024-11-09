using MPhotoBoothAI.Application.Interfaces;
using System.Runtime.InteropServices;

namespace MPhotoBoothAI.Infrastructure.Services
{
    public class DiskInfoService : IDiskInfoService
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetDiskFreeSpace(
        string lpRootPathName,
        out uint lpSectorsPerCluster,
        out uint lpBytesPerSector,
        out uint lpNumberOfFreeClusters,
        out uint lpTotalNumberOfClusters);

        public (int?, int?) GetBytesPerSector()
        {
            uint sectorsPerCluster, bytesPerSector, numberOfFreeClusters, totalNumberOfClusters;
            string drive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
            bool success = GetDiskFreeSpace(drive, out sectorsPerCluster, out bytesPerSector, out numberOfFreeClusters, out totalNumberOfClusters);
            return success ? ((int)bytesPerSector, (int)numberOfFreeClusters) : (null, null);
        }
    }
}

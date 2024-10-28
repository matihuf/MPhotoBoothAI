using AutoMapper;
using AutoMapper.QueryableExtensions;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Services.Base;
using MPhotoBoothAI.Models;

namespace MPhotoBoothAI.Infrastructure.Services
{
    public class CameraSettingsService(IDatabaseContext databaseContext, IMapper mapper)
        : BaseSettingsService<CameraSettings>(databaseContext, mapper), ICameraSettingsService
    {
        public CameraSettings Value => SettingsValue;

        protected override CameraSettings Load()
        {
            var settings = _databaseContext.CameraSettings.ProjectTo<CameraSettings>(_mapper.ConfigurationProvider).FirstOrDefault();
            return settings ?? new CameraSettings();
        }
    }
}

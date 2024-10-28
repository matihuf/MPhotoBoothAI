using AutoMapper;
using AutoMapper.QueryableExtensions;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Infrastructure.Services.Base;
using MPhotoBoothAI.Models;

namespace MPhotoBoothAI.Infrastructure.Services;
public class UserSettingsService(IDatabaseContext databaseContext, IMapper mapper)
    : BaseSettingsService<UserSettings>(databaseContext, mapper), IUserSettingsService
{
    public UserSettings Value => SettingsValue;

    protected override UserSettings Load()
    {
        var settings = _databaseContext.UserSettings.ProjectTo<UserSettings>(_mapper.ConfigurationProvider).FirstOrDefault();
        return settings ?? new UserSettings();
    }
}

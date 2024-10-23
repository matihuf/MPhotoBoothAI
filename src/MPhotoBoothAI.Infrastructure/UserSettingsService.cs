using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models;

namespace MPhotoBoothAI.Infrastructure;
public class UserSettingsService : IUserSettingsService
{
    private readonly IDatabaseContext _databaseContext;
    private readonly IMapper _mapper;

    public UserSettings Value { get; }

    public UserSettingsService(IDatabaseContext databaseContext, IMapper mapper)
    {
        _databaseContext = databaseContext;
        _mapper = mapper;
        Value = Load();
        Value.PropertyChanged += UserSettings_PropertyChanged;
    }

    private void UserSettings_PropertyChanged(object? sender, PropertyChangedValueEventArgs e)
    {
        if (e.NewValue != null)
        {
            var sql = $"UPDATE [UserSettings] SET [{e.PropertyName}] = @p0";
            _databaseContext.Database.ExecuteSqlRaw(sql, e.NewValue);
        }
    }

    private UserSettings Load()
    {
        var userSettings = _databaseContext.UserSettings.ProjectTo<UserSettings>(_mapper.ConfigurationProvider).FirstOrDefault();
        return userSettings ?? new UserSettings();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Value.PropertyChanged -= UserSettings_PropertyChanged;
        }
    }
}

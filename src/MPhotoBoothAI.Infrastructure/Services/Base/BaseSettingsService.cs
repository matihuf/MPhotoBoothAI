using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models;
using MPhotoBoothAI.Models.Base;

namespace MPhotoBoothAI.Infrastructure.Services.Base
{
    public abstract class BaseSettingsService<T> : IDisposable where T : BaseSettings
    {
        protected IDatabaseContext _databaseContext;

        protected IMapper _mapper;

        public T Value { get; }

        protected BaseSettingsService(IDatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            Value = Load();
            Value.PropertyChanged += PropertyChanged;
        }

        protected abstract T Load();

        private void PropertyChanged(object? sender, PropertyChangedValueEventArgs e)
        {
            if (e.NewValue != null)
            {
                var sql = $"UPDATE [UserSettings] SET [{e.PropertyName}] = @p0";
                _databaseContext.Database.ExecuteSqlRaw(sql, e.NewValue);
            }
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
                Value.PropertyChanged -= PropertyChanged;
            }
        }


    }
}

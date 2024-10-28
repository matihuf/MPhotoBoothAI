using AutoMapper;
using MPhotoBoothAI.Application.Interfaces;
using MPhotoBoothAI.Models;
using MPhotoBoothAI.Models.Base;

namespace MPhotoBoothAI.Infrastructure.Services.Base
{
    public abstract class BaseSettingsService<T> : IDisposable where T : BaseSettings
    {
        protected IDatabaseContext _databaseContext;

        protected IMapper _mapper;

        protected T SettingsValue { get; }

        protected BaseSettingsService(IDatabaseContext databaseContext, IMapper mapper)
        {
            _databaseContext = databaseContext;
            _mapper = mapper;
            SettingsValue = Load();
            SettingsValue.PropertyChanged += PropertyChanged;
        }

        protected abstract T Load();

        protected abstract void PropertyChanged(object? sender, PropertyChangedValueEventArgs e);


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                SettingsValue.PropertyChanged -= PropertyChanged;
            }
        }


    }
}

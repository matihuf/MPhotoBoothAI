using System.Runtime.CompilerServices;

namespace MPhotoBoothAI.Models.Base
{
    public class BaseSettings
    {
        public delegate void PropertyChangedValueEventHandler(object? sender, PropertyChangedValueEventArgs e);

        public event PropertyChangedValueEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged(object newValue, [CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedValueEventArgs(propertyName, newValue));
    }
}

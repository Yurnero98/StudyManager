using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ModernDesign
{
    public class ObservableObject : ObservableObjectBase
    {
        private readonly Dictionary<string, object?> _values = new();

        protected T Get<T>([CallerMemberName] string? propertyName = null)
        {
            if (_values.TryGetValue(propertyName!, out var v) && v is T value)
            {
                return value;
            }

            return default!;
        }

        protected bool Set<T>(T value, [CallerMemberName] string? propertyName = null)
        {
            var field = Get<T>(propertyName);
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                _values[propertyName!] = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Freya.Desktop.Core.Framework;

public abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void SetAndRaisePropertyChanged<TRef>(
        ref TRef field, TRef value, [CallerMemberName] string? propertyName = null)
    {
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetAndRaisePropertyChangedIfDifferentValues<TRef>(
        ref TRef field, TRef value, [CallerMemberName] string? propertyName = null)
        where TRef : class
    {
        if (field == null || !field.Equals(value))
        {
            SetAndRaisePropertyChanged(ref field!, value, propertyName);
            return true;
        }
        return false;
    }

    protected void RaisePropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

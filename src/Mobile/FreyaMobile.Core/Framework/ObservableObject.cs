namespace FreyaMobile.Core.Framework;

public abstract class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetAndRaisePropertyChanged<TRef>(
    ref TRef field, TRef value, [CallerMemberName] string? propertyName = null)
    {
        if (field == null && value == null)
        {
            return false;
        }

        if (field == null || !field.Equals(value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        return false;
    }
}

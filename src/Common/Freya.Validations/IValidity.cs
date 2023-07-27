namespace Freya.Validations;

public interface IValidity : INotifyPropertyChanged
{
    bool IsValid { get; set; }

    List<string> Errors { get; }
}

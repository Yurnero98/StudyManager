namespace ModernDesign;

public class BaseViewModel : ObservableObject
{
    public string? Name
    {
        get => Get<string>();
        set => Set(value);
    }
}

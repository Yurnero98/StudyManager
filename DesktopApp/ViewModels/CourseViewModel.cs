using System.Collections.ObjectModel;

namespace ModernDesign;

public class CourseViewModel : BaseViewModel
{
    public int CourseId
    {
        get => Get<int>();
        set => Set(value);
    }

    public string? Description
    {
        get => Get<string>();
        set => Set(value);
    }

    public ObservableCollection<GroupViewModel> Groups { get; set; } = new ObservableCollection<GroupViewModel>();
}

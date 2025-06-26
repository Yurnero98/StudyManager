using System.Collections.ObjectModel;

namespace ModernDesign;

public class GroupViewModel : BaseViewModel
{
    public int GroupId
    {
        get => Get<int>();
        set => Set(value);
    }

    public int? CourseId
    {
        get => Get<int?>();
        set => Set(value);
    }

    public CourseViewModel? SelectedCourse
    {
        get => Get<CourseViewModel>();
        set
        {
            Set(value);
            
            if (value != null)
            {
                CourseId = value.CourseId; 
            }
        }
    }
    public ObservableCollection<StudentViewModel> Students { get; set; } = new ObservableCollection<StudentViewModel>();
}

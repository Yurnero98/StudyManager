namespace ModernDesign;

public class TeacherViewModel : BaseViewModel
{
    public int TeacherId
    {
        get => Get<int>();
        set => Set(value);
    }

    public string? FirstName
    {
        get => Get<string>();
        set
        {
            if(Set(value))
            {
                OnPropertyChanged(nameof(FullName));
            }
        }    
    }

    public string? LastName
    {
        get => Get<string>();
        set
        {
             if(Set(value))
             {  
                OnPropertyChanged(nameof(FullName));
             }
        }   
    }

    public int? CourseId
    {
        get => Get<int?>();  
        set => Set(value);
    }

    public string FullName => $"{FirstName} {LastName}";

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
}

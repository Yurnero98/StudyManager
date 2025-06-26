namespace ModernDesign;

public class StudentViewModel : BaseViewModel
{
    public int StudentId
    {
        get => Get<int>();
        set => Set(value);
    }

    public string? FirstName
    {
        get => Get<string>();
        set
        {
            if (Set(value))
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
            if (Set(value))
            {
                OnPropertyChanged(nameof(FullName));
            }
        }
    }

    public int? GroupId
    {
        get => Get<int?>();
        set => Set(value);
    }

    public string FullName => $"{FirstName} {LastName}";

    public GroupViewModel? SelectedGroup
    {
        get => Get<GroupViewModel>();
        set
        {
            Set(value);

            if (value != null)
            {
                GroupId = value.GroupId;
            }
        }
    }
}
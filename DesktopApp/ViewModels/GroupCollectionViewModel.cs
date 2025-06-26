using ModernDesign.DataBase;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;

namespace ModernDesign;

public class GroupCollectionViewModel : ObservableObject
{
    private readonly AppDbContext? _dbContext;
    private GroupViewModel? _selectedGroup;
    public ObservableCollection<GroupViewModel> Groups { get; }
    public ObservableCollection<CourseViewModel> Courses { get; }

    public ICommand AddGroupCommand { get; }
    public ICommand RemoveGroupCommand { get; }
    public ICommand SaveGroupCommand { get; }
    public ICommand CancelGroupCommand { get; }

    public GroupViewModel? SelectedGroup
    {
        get => _selectedGroup;
        set
        {
            _selectedGroup = value;
            OnPropertyChanged(nameof(SelectedGroup));
        }
    }

    public GroupCollectionViewModel() : this(null!) { }

    public GroupCollectionViewModel(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Courses = new ObservableCollection<CourseViewModel>
            {
                new() { CourseId = 10, Name = "Math", Description = "Basic Math Course" },
                new() { CourseId = 20, Name = "Science", Description = "Introduction to Science" }
            };

            Groups = new ObservableCollection<GroupViewModel>
            {
                new() { GroupId = 1, Name = "Group A", CourseId = 10, SelectedCourse = Courses.First() },
                new() { GroupId = 2, Name = "Group B", CourseId = 20, SelectedCourse = Courses.Last() }
            };

            SelectedGroup = Groups.FirstOrDefault();
        }
        else
        {         
            Groups = new ObservableCollection<GroupViewModel>();
            Courses = new ObservableCollection<CourseViewModel>();
            LoadGroups();
            LoadCourses();
        }

        AddGroupCommand = new RelayCommand(AddGroup);
        RemoveGroupCommand = new RelayCommand(RemoveGroup, parameter => SelectedGroup != null);
        SaveGroupCommand = new RelayCommand(SaveGroup, parameter => SelectedGroup != null);
        CancelGroupCommand = new RelayCommand(CancelGroup);
    }

    public void RefreshData()
    {
        //_logger.LogInformation("Refreshing data...");

        LoadCourses();
        LoadGroups();

        OnPropertyChanged(nameof(Groups));
        OnPropertyChanged(nameof(Courses));
    }

    private void LoadGroups()
    {
        Groups.Clear();
        var groupsFromDb = _dbContext!.Groups.Include(g => g.Course).ToList();
        foreach (var group in groupsFromDb)
        {
            Groups.Add(new GroupViewModel
            {
                GroupId = group.GroupId,
                Name = group.Name,
                CourseId = group.CourseId,
                SelectedCourse = Courses.FirstOrDefault(c => c.CourseId == group.CourseId)
            });
        }
    }

    private void LoadCourses()
    {
        Courses.Clear();
        var coursesFromDb = _dbContext!.Courses.ToList();
        foreach (var course in coursesFromDb)
        {
            Courses.Add(new CourseViewModel
            {
                CourseId = course.CourseId,
                Name = course.Name,
                Description = course.Description!
            });
        }
    }

    private void SaveGroup(object? parameter)
    {
        if (SelectedGroup != null)
        {
            var existingGroup = _dbContext!.Groups.FirstOrDefault(g => g.GroupId == SelectedGroup.GroupId);

            if (existingGroup != null)
            {
                existingGroup.Name = SelectedGroup.Name;
                existingGroup.CourseId = SelectedGroup.CourseId;

                _dbContext.Groups.Update(existingGroup);
                _dbContext.SaveChanges();
            }

            SelectedGroup = null;
        }
    }

    private void AddGroup(object? parameter)
    {
        var newGroup = new Group
        {
            Name = "New Group",
        };

        _dbContext!.Groups.Add(newGroup);
        _dbContext.SaveChanges();


        var newGroupViewModel = new GroupViewModel
        {
            GroupId = newGroup.GroupId,
            Name = newGroup.Name
        };

        Groups.Add(newGroupViewModel);
        SelectedGroup = newGroupViewModel;
    }

    private void RemoveGroup(object? parameter)
    {
        if (SelectedGroup != null)
        {
            var groupToRemove = _dbContext!.Groups.FirstOrDefault(g => g.GroupId == SelectedGroup.GroupId);

            if (groupToRemove != null)
            {
                _dbContext.Groups.Remove(groupToRemove);
                _dbContext.SaveChanges();
            }

            Groups.Remove(SelectedGroup);
            SelectedGroup = null;
        }
    }

    private void CancelGroup(object? parameter)
    {
        SelectedGroup = null;
    }
}
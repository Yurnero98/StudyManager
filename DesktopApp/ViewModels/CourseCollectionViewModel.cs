using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ModernDesign.DataBase;

namespace ModernDesign;

public class CourseCollectionViewModel : ObservableObject
{
    private readonly AppDbContext? _dbContext;
    private CourseViewModel? _selectedCourse;

    public ObservableCollection<CourseViewModel> Courses { get; }
    public ICommand AddCourseCommand { get; }
    public ICommand RemoveCourseCommand { get; }
    public ICommand SaveCourseCommand { get; }
    public ICommand CancelCourseCommand { get; }

    public CourseViewModel? SelectedCourse
    {
        get => _selectedCourse;
        set
        {
            _selectedCourse = value;
            OnPropertyChanged(nameof(SelectedCourse));
        }
    }

    public CourseCollectionViewModel() : this(null!) { }

    public CourseCollectionViewModel(AppDbContext dbContext)
    {     
        _dbContext = dbContext;
        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            // Dummy data for design mode
            Courses = new ObservableCollection<CourseViewModel>
            {
                new() { CourseId = 1, Name = "Math", Description = "Basic Math Course" },
                new() { CourseId = 2, Name = "Science", Description = "Introduction to Science" }
            };

            // Selected course in design mode
            SelectedCourse = Courses.FirstOrDefault();
        }
        else
        {
            // Logic for actual program operation
            
            Courses = new ObservableCollection<CourseViewModel>();
        }

        AddCourseCommand = new RelayCommand(AddCourse);
        RemoveCourseCommand = new RelayCommand(RemoveCourse, parameter => SelectedCourse != null);
        SaveCourseCommand = new RelayCommand(SaveCourse, parameter => !string.IsNullOrWhiteSpace(SelectedCourse?.Name));
        CancelCourseCommand = new RelayCommand(CancelCourse);
    }

    public void RefreshData()
    {
        //_logger.LogInformation("Refreshing data...");

        LoadCourses();

        OnPropertyChanged(nameof(Courses));
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
                Description = course.Description
            });
        }
    }

    private void AddCourse(object? parameter)
    {
        var newCourse = new Course
        {
            Name = "New Course",
            Description = "New Description"
        };

        _dbContext!.Courses.Add(newCourse);
        _dbContext.SaveChanges();

        var newCourseViewModel = new CourseViewModel
        {
            CourseId = newCourse.CourseId,
            Name = newCourse.Name,
            Description = newCourse.Description
        };

        Courses.Add(newCourseViewModel);
        SelectedCourse = newCourseViewModel;
    }

    private void RemoveCourse(object? parameter)
    {
        if (SelectedCourse == null)
            return;

        var teachersWithCourse = _dbContext!.Teachers.Where(t => t.CourseId == SelectedCourse.CourseId).ToList();
        foreach (var teacher in teachersWithCourse)
        {
            teacher.CourseId = null;
        }

        var groupsWithCourse = _dbContext!.Groups.Where(g => g.CourseId == SelectedCourse.CourseId).ToList();
        foreach (var group in groupsWithCourse)
        {
            group.CourseId = null;
        }

        _dbContext.SaveChanges();

        var courseToRemove = _dbContext.Courses
            .FirstOrDefault(c => c.CourseId == SelectedCourse.CourseId);

        if (courseToRemove != null)
        {
            _dbContext.Courses.Remove(courseToRemove);
            _dbContext.SaveChanges();
        }

        Courses.Remove(SelectedCourse);
        SelectedCourse = null;
    }

    private void SaveCourse(object? parameter)
    {
        if (SelectedCourse != null)
        {
            var existingCourse = _dbContext!.Courses
                .FirstOrDefault(c => c.CourseId == SelectedCourse.CourseId);

            if (existingCourse != null)
            {
                existingCourse.Name = SelectedCourse.Name;
                existingCourse.Description = SelectedCourse.Description;

                _dbContext.Courses.Update(existingCourse);
                _dbContext.SaveChanges();

                var courseToUpdate = Courses.FirstOrDefault(c => c.CourseId == existingCourse.CourseId);
                if (courseToUpdate != null)
                {
                    courseToUpdate.Name = existingCourse.Name;
                    courseToUpdate.Description = existingCourse.Description;
                }
            }

            SelectedCourse = null;
        }
    }

    private void CancelCourse(object? parameter)
    {
        SelectedCourse = null;
    }

    private object? selectedCourseId;

    public object SelectedCourseId { get => selectedCourseId!; set => Set(ref selectedCourseId, value); }
}

using Microsoft.Extensions.Logging;
using ModernDesign.DataBase;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;

namespace ModernDesign;

public class TeacherCollectionViewModel : ObservableObject
{
    private readonly AppDbContext? _dbContext;
    private readonly ILogger<TeacherCollectionViewModel> _logger;
    private TeacherViewModel? _selectedTeacher;

    public ObservableCollection<TeacherViewModel> Teachers { get; }
    public ObservableCollection<CourseViewModel> Courses { get; }

    public ICommand AddTeacherCommand { get; }
    public ICommand RemoveTeacherCommand { get; }
    public ICommand SaveTeacherCommand { get; }
    public ICommand CancelTeacherCommand { get; }

    public TeacherViewModel? SelectedTeacher
    {
        get => _selectedTeacher;
        set
        {
            _selectedTeacher = value;
            OnPropertyChanged(nameof(SelectedTeacher));
        }
    }

    public TeacherCollectionViewModel() : this(null!, null!) { }

    public TeacherCollectionViewModel(AppDbContext dbContext, ILogger<TeacherCollectionViewModel> logger)
    {
        _dbContext = dbContext;
        _logger = logger;

        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Courses = new ObservableCollection<CourseViewModel>
            {
                new() { CourseId = 10, Name = "Math", Description = "Basic Math Course" },
                new() { CourseId = 20, Name = "Science", Description = "Introduction to Science" }
            };

            Teachers = new ObservableCollection<TeacherViewModel>
            {
                new() { TeacherId = 1, FirstName = "John", LastName = "Doe", CourseId = 10, SelectedCourse = Courses.First() },
                new() { TeacherId = 2, FirstName = "Jane", LastName = "Smith", CourseId = 20, SelectedCourse = Courses.Last() }
            };

            SelectedTeacher = Teachers.FirstOrDefault();
        }
        else
        {
            Teachers = new ObservableCollection<TeacherViewModel>();
            Courses = new ObservableCollection<CourseViewModel>();
        }

        AddTeacherCommand = new RelayCommand(AddTeacher);
        RemoveTeacherCommand = new RelayCommand(RemoveTeacher, parameter => SelectedTeacher != null);
        SaveTeacherCommand = new RelayCommand(SaveTeacher, parameter => SelectedTeacher != null);
        CancelTeacherCommand = new RelayCommand(CancelTeacher);
    }

    public void RefreshData()
    {
        _logger.LogInformation("Refreshing data...");

        LoadCourses();
        LoadTeachers();

        OnPropertyChanged(nameof(Teachers));
        OnPropertyChanged(nameof(Courses));
    }

    private void LoadTeachers()
    {
        _logger.LogInformation("Loading teachers from the database...");

        Teachers.Clear();
        var teachersFromDb = _dbContext!.Teachers.Include(g => g.Course).ToList();
        foreach (var teacher in teachersFromDb)
        {
            Teachers.Add(new TeacherViewModel
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                CourseId = teacher.CourseId,
                SelectedCourse = Courses.FirstOrDefault(c => c.CourseId == teacher.CourseId)
            });
        }

        _logger.LogInformation($"Loaded {Teachers.Count} teachers.");
    }

    private void LoadCourses()
    {
        _logger.LogInformation("Loading courses from the database...");

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

        _logger.LogInformation($"Loaded {Courses.Count} courses.");
    }

    private void SaveTeacher(object? parameter)
    {
        if (SelectedTeacher != null)
        {
            var existingTeacher = _dbContext!.Teachers.Include(t => t.Course)
                .FirstOrDefault(t => t.TeacherId == SelectedTeacher.TeacherId);

            if (existingTeacher != null)
            {
                existingTeacher.FirstName = SelectedTeacher.FirstName;
                existingTeacher.LastName = SelectedTeacher.LastName;
                existingTeacher.CourseId = SelectedTeacher.CourseId;

                _dbContext.Teachers.Update(existingTeacher);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Teacher with ID {SelectedTeacher.TeacherId} changes were saved.");
            }

            SelectedTeacher = null;
        }
    }

    private void AddTeacher(object? parameter)
    {
        _logger.LogInformation("Adding a new teacher...");

        var newTeacher = new Teacher
        {
            FirstName = "New",
            LastName = "Teacher"
        };

        _dbContext!.Teachers.Add(newTeacher);
        _dbContext.SaveChanges();

        var newTeacherViewModel = new TeacherViewModel
        {
            TeacherId = newTeacher.TeacherId,
            FirstName = newTeacher.FirstName,
            LastName = newTeacher.LastName
        };

        Teachers.Add(newTeacherViewModel);
        SelectedTeacher = newTeacherViewModel;

        _logger.LogInformation($"New teacher added: {newTeacher.FirstName} {newTeacher.LastName}");
    }

    private void RemoveTeacher(object? parameter)
    {
        if (SelectedTeacher != null)
        {
            _logger.LogInformation($"Removing teacher: {SelectedTeacher.FirstName} {SelectedTeacher.LastName}");

            var teacherToRemove = _dbContext!.Teachers.FirstOrDefault(t => t.TeacherId == SelectedTeacher.TeacherId);
            if (teacherToRemove != null)
            {
                _dbContext.Teachers.Remove(teacherToRemove);
                _dbContext.SaveChanges();
                _logger.LogInformation($"Teacher {SelectedTeacher.FirstName} {SelectedTeacher.LastName} removed successfully.");
            }

            Teachers.Remove(SelectedTeacher);
            SelectedTeacher = null;
        }
    }

    private void CancelTeacher(object? parameter)
    {
        _logger.LogInformation("Canceling teacher selection.");
        SelectedTeacher = null;
    }
}

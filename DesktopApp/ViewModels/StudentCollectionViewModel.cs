using Microsoft.Extensions.Logging;
using ModernDesign.DataBase;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;

namespace ModernDesign;

public class StudentCollectionViewModel : ObservableObject
{
    private readonly AppDbContext? _dbContext;
    private readonly ILogger<StudentCollectionViewModel> _logger;
    private StudentViewModel? _selectedStudent;

    public ObservableCollection<StudentViewModel> Students { get; }
    public ObservableCollection<GroupViewModel> Groups { get; }

    public ICommand AddStudentCommand { get; }
    public ICommand RemoveStudentCommand { get; }
    public ICommand SaveStudentCommand { get; }
    public ICommand CancelStudentCommand { get; }

    public StudentViewModel? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnPropertyChanged(nameof(SelectedStudent));
        }
    }

    public StudentCollectionViewModel() : this(null!, null!) { }

    public StudentCollectionViewModel(AppDbContext dbContext, ILogger<StudentCollectionViewModel> logger)
    {
        _dbContext = dbContext;
        _logger = logger;

        if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        {
            Groups = new ObservableCollection<GroupViewModel>
            {
                new() { GroupId = 10, Name = "Group 1" },
                new() { GroupId = 20, Name = "Group 2" }
            };

            Students = new ObservableCollection<StudentViewModel>
            {
                new() { StudentId = 1, FirstName = "John", LastName = "Doe", GroupId = 10, SelectedGroup = Groups.First() },
                new() { StudentId = 2, FirstName = "Jane", LastName = "Smith", GroupId = 20, SelectedGroup = Groups.Last() }
            };

            SelectedStudent = Students.FirstOrDefault();
        }
        else
        {
            Students = new ObservableCollection<StudentViewModel>();
            Groups = new ObservableCollection<GroupViewModel>();
        }

        AddStudentCommand = new RelayCommand(AddStudent);
        RemoveStudentCommand = new RelayCommand(RemoveStudent, parameter => SelectedStudent != null);
        SaveStudentCommand = new RelayCommand(SaveStudent, parameter => SelectedStudent != null);
        CancelStudentCommand = new RelayCommand(CancelStudent);
    }

    public void RefreshData()
    {
        _logger.LogInformation("Refreshing data...");

        LoadGroups();
        LoadStudents();

        OnPropertyChanged(nameof(Students));
        OnPropertyChanged(nameof(Groups));
    }

    private void LoadStudents()
    {
        _logger.LogInformation("Loading students from the database...");

        Students.Clear();
        var studentsFromDb = _dbContext!.Students.Include(g => g.Group).ToList();
        foreach (var student in studentsFromDb)
        {
            Students.Add(new StudentViewModel
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                GroupId = student.GroupId,
                SelectedGroup = Groups.FirstOrDefault(c => c.GroupId == student.GroupId)
            });
        }

        _logger.LogInformation($"Loaded {Students.Count} students.");
    }

    private void LoadGroups()
    {
        _logger.LogInformation("Loading groups from the database...");

        Groups.Clear();
        var groupsFromDb = _dbContext!.Groups.ToList();
        foreach (var group in groupsFromDb)
        {
            Groups.Add(new GroupViewModel
            {
                GroupId = group.GroupId,
                Name = group.Name,
            });
        }

        _logger.LogInformation($"Loaded {Groups.Count} groups.");
    }

    private void SaveStudent(object? parameter)
    {
        if (SelectedStudent != null)
        {
            var existingStudent = _dbContext!.Students.Include(t => t.Group)
                .FirstOrDefault(t => t.StudentId == SelectedStudent.StudentId);

            if (existingStudent != null)
            {
                existingStudent.FirstName = SelectedStudent.FirstName;
                existingStudent.LastName = SelectedStudent.LastName;
                existingStudent.GroupId = SelectedStudent.GroupId;

                _dbContext.Students.Update(existingStudent);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Student with ID {SelectedStudent.StudentId} changes were saved.");
            }

            SelectedStudent = null;
        }
    }

    private void AddStudent(object? parameter)
    {
        _logger.LogInformation("Adding a new student...");

        var newStudent = new Student
        {
            FirstName = "New",
            LastName = "Student"
        };

        _dbContext!.Students.Add(newStudent);
        _dbContext.SaveChanges();

        var newStudentViewModel = new StudentViewModel
        {
            StudentId = newStudent.StudentId,
            FirstName = newStudent.FirstName,
            LastName = newStudent.LastName
        };

        Students.Add(newStudentViewModel);
        SelectedStudent = newStudentViewModel;

        _logger.LogInformation($"New student added: {newStudent.FirstName} {newStudent.LastName}");
    }

    private void RemoveStudent(object? parameter)
    {
        if (SelectedStudent != null)
        {
            _logger.LogInformation($"Removing student: {SelectedStudent.FirstName} {SelectedStudent.LastName}");

            var studentToRemove = _dbContext!.Students.FirstOrDefault(t => t.StudentId == SelectedStudent.StudentId);
            if (studentToRemove != null)
            {
                _dbContext.Students.Remove(studentToRemove);
                _dbContext.SaveChanges();
                _logger.LogInformation($"Student {SelectedStudent.FirstName} {SelectedStudent.LastName} removed successfully.");
            }

            Students.Remove(SelectedStudent);
            SelectedStudent = null;
        }
    }

    private void CancelStudent(object? parameter)
    {
        _logger.LogInformation("Canceling student selection.");
        SelectedStudent = null;
    }
}

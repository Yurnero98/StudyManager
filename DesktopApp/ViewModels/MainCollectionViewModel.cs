using System.Collections.ObjectModel;
using ModernDesign.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using System.Windows;

namespace ModernDesign;

public class MainCollectionViewModel : ObservableObject
{
    private readonly AppDbContext _dbContext;
    private CourseViewModel? _selectedCourse;
    private GroupViewModel? _selectedGroup;
    private readonly ICsvManager _csvManager;
    private readonly IFileDialogService _fileDialogService;

    public ObservableCollection<CourseViewModel> Courses { get; }
    public ObservableCollection<GroupViewModel> Groups { get; }
    public ObservableCollection<StudentViewModel> Students { get; }
    
    public ICommand ImportStudentsCommand { get; }
    public ICommand ExportStudentsCommand { get; }

    public CourseViewModel? SelectedCourse
    {
        get => _selectedCourse;
        set
        {
            _selectedCourse = value;
            OnPropertyChanged(nameof(SelectedCourse));
        }
    }

    public GroupViewModel? SelectedGroup
    {
        get => _selectedGroup;
        set
        {
            if (_selectedGroup != value)
            {
                _selectedGroup = value;
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }
    }

    public MainCollectionViewModel(AppDbContext dbContext, ICsvManager csvManager, IFileDialogService fileDialogService)
    {
        _dbContext = dbContext;
        _csvManager = csvManager;
        _fileDialogService = fileDialogService;

        Courses = new ObservableCollection<CourseViewModel>();
        Groups = new ObservableCollection<GroupViewModel>();
        Students = new ObservableCollection<StudentViewModel>();

        ImportStudentsCommand = new RelayCommand(_ => ImportStudentsFromCsv());
        ExportStudentsCommand = new RelayCommand(_ => ExportStudentsToCsv());

        LoadData();
    }

    public void RefreshData()
    {
        //_logger.LogInformation("Refreshing data...");

        LoadData();

        OnPropertyChanged(nameof(Groups));
        OnPropertyChanged(nameof(Courses));
        OnPropertyChanged(nameof(Students));
    }

    private void LoadData()
    {
        var coursesFromDb = _dbContext.Courses.ToList();

        var groupsFromDb = _dbContext.Groups.ToList();

        var studentsFromDb = _dbContext.Students.ToList();

        Courses.Clear();

        foreach (var course in coursesFromDb)
        {
            var courseGroups = groupsFromDb
                .Where(g => g.CourseId == course.CourseId)
                .ToList();

            var courseViewModel = new CourseViewModel
            {
                CourseId = course.CourseId,
                Name = course.Name!,
                Description = course.Description!,
                Groups = new ObservableCollection<GroupViewModel>(
                    courseGroups.Select(group => new GroupViewModel
                    {
                        GroupId = group.GroupId,
                        Name = group.Name,
                        Students = new ObservableCollection<StudentViewModel>(
                            studentsFromDb
                                .Where(s => s.GroupId == group.GroupId)
                                .Select(student => new StudentViewModel
                                {
                                    StudentId = student.StudentId,
                                    FirstName = student.FirstName,
                                    LastName = student.LastName
                                })
                        )
                    })
                )
            };

            Courses.Add(courseViewModel);
        }
    }

    public void ImportStudentsFromCsv()
    {
        try
        {
            if (SelectedGroup == null)
                return;

            var groupId = SelectedGroup.GroupId;

            var path = _fileDialogService.ShowOpenFileDialog("CSV files (*.csv)|*.csv");
            if (string.IsNullOrWhiteSpace(path))
                return;

            var oldStudents = _dbContext.Students.Where(s => s.GroupId == groupId);
            _dbContext.Students.RemoveRange(oldStudents);

            var newStudents = _csvManager.ImportStudentsFromCsv(path, groupId);
            _dbContext.Students.AddRange(newStudents);
            _dbContext.SaveChanges();

            var updatedStudents = _dbContext.Students
                .Where(s => s.GroupId == groupId)
                .ToList();

            SelectedGroup.Students.Clear();

            foreach (var s in updatedStudents)
            {
                var studentViewModel = new StudentViewModel
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    GroupId = s.GroupId,
                    SelectedGroup = SelectedGroup
                };

                SelectedGroup.Students.Add(studentViewModel);
            }

            System.Windows.MessageBox.Show("Students imported successfully.", "Import Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Error during import:\n{ex.Message}", "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void ExportStudentsToCsv()
    {
        try
        {
            if (SelectedGroup == null)
                return;

            var path = _fileDialogService.ShowSaveFileDialog("CSV files (*.csv)|*.csv");
            if (string.IsNullOrWhiteSpace(path))
                return;

            var students = SelectedGroup.Students.Select(s => new Student
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                GroupId = SelectedGroup.GroupId
            });

            _csvManager.ExportStudentsToCsv(students, path);

            System.Windows.MessageBox.Show("Students exported successfully.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Error during export:\n{ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
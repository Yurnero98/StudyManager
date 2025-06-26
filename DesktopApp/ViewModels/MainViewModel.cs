using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace ModernDesign;

public class MainViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    private object? _currentView;
    public object CurrentView
    {
        get => _currentView!;
        set => Set(ref _currentView, value);
    }

    public ICommand ShowMainCommand { get; }
    public ICommand ShowCoursesCommand { get; }
    public ICommand ShowTeachersCommand { get; }
    public ICommand ShowGroupsCommand { get; }
    public ICommand ShowStudentsCommand { get; }

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        ShowMainCommand = new RelayCommand(_ =>
        {
            var viewModel = _serviceProvider.GetRequiredService<MainCollectionViewModel>();
            viewModel.RefreshData();
            CurrentView = viewModel;
        });

        ShowCoursesCommand = new RelayCommand(_ =>
        {
            var viewModel = _serviceProvider.GetRequiredService<CourseCollectionViewModel>();
            viewModel.RefreshData();
            CurrentView = viewModel;
        });

        ShowTeachersCommand = new RelayCommand(_ =>
        {
            var viewModel = _serviceProvider.GetRequiredService<TeacherCollectionViewModel>();
            viewModel.RefreshData();
            CurrentView = viewModel;
        });

        ShowGroupsCommand = new RelayCommand(_ =>
        {
            var viewModel = _serviceProvider.GetRequiredService<GroupCollectionViewModel>();
            viewModel.RefreshData();
            CurrentView = viewModel;
        });

        ShowStudentsCommand = new RelayCommand(_ =>
        {
            var viewModel = _serviceProvider.GetRequiredService<StudentCollectionViewModel>();
            viewModel.RefreshData();
            CurrentView = viewModel;
        });
        
        var mainViewModel = _serviceProvider.GetRequiredService<MainCollectionViewModel>();
        mainViewModel.RefreshData();
        CurrentView = mainViewModel;
    }
}
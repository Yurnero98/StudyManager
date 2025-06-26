using DesktopApplication;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ModernDesign
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider?.GetRequiredService<MainViewModel>();
        }
    }
}
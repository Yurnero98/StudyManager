using System.Windows.Controls;

namespace ModernDesign;

public partial class TeacherControl : UserControl
{
    public TeacherControl()
    {
        InitializeComponent();
    }
    public TeacherCollectionViewModel ViewModel => (TeacherCollectionViewModel)DataContext;
}
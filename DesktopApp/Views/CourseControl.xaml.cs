using System.Windows.Controls;

namespace ModernDesign;

public partial class CourseControl : UserControl
{
    public CourseControl()
    {
        InitializeComponent();
    }

    public CourseCollectionViewModel ViewModel => (CourseCollectionViewModel)DataContext;
}

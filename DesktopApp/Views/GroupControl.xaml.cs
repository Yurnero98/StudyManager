using System.Windows.Controls;

namespace ModernDesign;

public partial class GroupControl : UserControl
{
    public GroupControl()
    {
        InitializeComponent();
    }

    public CourseCollectionViewModel ViewModel => (CourseCollectionViewModel)DataContext;
}

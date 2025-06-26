using System.Windows.Controls;

namespace ModernDesign;

public partial class StudentControl : UserControl
{
    public StudentControl()
    {
        InitializeComponent();
    }
    public StudentCollectionViewModel ViewModel => (StudentCollectionViewModel)DataContext;
}
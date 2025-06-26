using System.Windows;
using System.Windows.Controls;

namespace ModernDesign
{
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();
        }

        public MainCollectionViewModel ViewModel => (MainCollectionViewModel)DataContext;

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (ViewModel != null)
            {
                if (e.NewValue is GroupViewModel selectedGroup)
                {
                    ViewModel.SelectedGroup = selectedGroup;
                }
                else
                {
                    ViewModel.SelectedGroup = null;
                }
            }
        }
    }
}

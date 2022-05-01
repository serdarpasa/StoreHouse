using System.Windows;

namespace Storehouse.View
{
    
    public partial class WinCategoryEdit
    {
        public WinCategoryEdit()
        {
            InitializeComponent();
        }
        public StorehouseEntities Db { get; set; }
        private void ButtonBaseSave_OnClick(object sender, RoutedEventArgs e)
        {
            var item = DataContext as Category;
            if (item == null)
                return;
            if (string.IsNullOrEmpty(item.Name))
            {
                MessageBox.Show("Название не может быть пустым!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }
    }
}

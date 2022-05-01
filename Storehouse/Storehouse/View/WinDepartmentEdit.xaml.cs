using System.Windows;

namespace Storehouse.View
{
    
    public partial class WinDepartmentEdit
    {
        public WinDepartmentEdit()
        {
            InitializeComponent();
        }
        public StorehouseEntities Db { get; set; }
        private void ButtonBaseSave_OnClick(object sender, RoutedEventArgs e)
        {
            var department = DataContext as Department;
            if (department == null)
                return;
            if (string.IsNullOrEmpty(department.Name))
            {
                MessageBox.Show("Название не может быть пустым!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }
    }
}

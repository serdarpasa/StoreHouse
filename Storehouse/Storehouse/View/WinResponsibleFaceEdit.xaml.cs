using System.Windows;
using System.Linq;

namespace Storehouse.View
{
    /// <summary>
    /// Логика взаимодействия для WinResponsibleFaceEdit.xaml
    /// </summary>
    public partial class WinResponsibleFaceEdit
    {
        public WinResponsibleFaceEdit()
        {
            InitializeComponent();
        }
        public StorehouseEntities Db { get; set; }
        private void ButtonBaseSave_OnClick(object sender, RoutedEventArgs e)
        {
            var rf = DataContext as Employeer;
            if (rf == null)
                return;
            if (string.IsNullOrEmpty(rf.LastName))
            {
                MessageBox.Show("Фамилия обязательна к заполнению!","",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
           
            DialogResult = true;
        }
      
        
        private void ButtonBasePasswordReset_OnClick(object sender, RoutedEventArgs e)
        {
            var user = DataContext as Employeer;
            if (user == null)
                return;
            user.Password = string.Empty;
            MessageBox.Show("Пароль установлен пустым\nНе забудьте сохранить изменения");
        }

        private void WinUserEdit_OnLoaded(object sender, RoutedEventArgs e)
        {
            var departments = Db.Departments.OrderBy(_ => _.Name).ToList();
            CbxDepartments.ItemsSource = departments;
        }
    }
}

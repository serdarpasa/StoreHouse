using System;
using System.Windows;
using System.Linq;
using System.Windows.Input;


namespace Storehouse.View.Directories
{   
    public partial class WinDirDepartments
    {
        private StorehouseEntities _db = new StorehouseEntities();
        public WinDirDepartments()
        {
            InitializeComponent();
        }
        
        private void ReloadList()
        {
            DgrCatalog.ItemsSource = _db.Departments.OrderBy(_=>_.Name).ToList();   
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadList();
        }

       
       
        
        private void _add_Record(object sender, RoutedEventArgs e)
        {

            var item = new Department();
            _db.AddToDepartments(item);
            Edit(item);
        }

        private void Edit(Department item)
        {
            var nw = new WinDepartmentEdit()
            {
                DataContext = item,
                Db = _db
            };
            nw.ShowDialog();
            if (nw.DialogResult == true)
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {
                    //
                }

            ReloadList();
        }

        private void _editRecord(object sender, RoutedEventArgs e)
        {
            var item = DgrCatalog.SelectedItem as Department;
            if (item == null)
                return;
            Edit(item);
        }
      
        private void DeleteRecord()
        {
            var dr = DgrCatalog.SelectedItem as Department;
            if (dr == null) return;

            if (
                MessageBox.Show("Запись о " + dr.Name + " будет удалена полностью!"
                + "\nПродолжить выполнение?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try
            {
                _db.Departments.DeleteObject(dr);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                //
            }

            ReloadList();
        }

        private void _delRecord(object sender, RoutedEventArgs e)
        {
            DeleteRecord();
        }
      
        private void dgr_catalog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = DgrCatalog.SelectedItem as Department;
            if (item == null)
                return;
            Edit(item);
        }


        private void dgr_catalog_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete: { DeleteRecord(); break; } 
            }
        }
    }
}

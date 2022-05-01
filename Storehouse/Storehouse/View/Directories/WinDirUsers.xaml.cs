using System;
using System.Windows;
using System.Linq;
using System.Windows.Input;

//СПРАВОЧНИК ПОЛЬЗОВАТЕЛЕЙ
namespace Storehouse.View.Directories
{   
    public partial class WinDirUsers
    {
        private StorehouseEntities _db = new StorehouseEntities();
        public WinDirUsers()
        {
            InitializeComponent();
        }
        private void ReloadList()
        {
            DgrUsers.ItemsSource = _db.Employeers.Where(_=>_.Role!=2).OrderBy(_ => _.LastName).ToList();   
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadList();
        }

       
       
        
        private void _addRecord(object sender, RoutedEventArgs e)
        {
           
            var user = new Employeer();
            _db.AddToEmployeers(user);
            Edit(user);
        }

        private void Edit(Employeer user)
        {
            var nw = new WinUserEdit()
            {
                DataContext = user,
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
            _db = new StorehouseEntities();
            ReloadList();
        }

        private void _editRecord(object sender, RoutedEventArgs e)
        {
            var user = DgrUsers.SelectedItem as Employeer;
            if (user == null)
                return;
            Edit(user);
        }
      
        private void DeleteRecord()
        {
            var dr = DgrUsers.SelectedItem as Employeer;
            if (dr == null) return;

            if (
                MessageBox.Show("Запись о " + dr.LastName+" "+dr.FirstName+" "+dr.MiddleName + " будет удалена!\n Продолжить выполнение?", "Внимание!",
                    MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            try
            {
                _db.Employeers.DeleteObject(dr);
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
            var user = DgrUsers.SelectedItem as Employeer;
            if (user == null)
                return;
            Edit(user);
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

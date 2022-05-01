using System;
using System.Windows;
using System.Linq;
using System.Windows.Input;

//СПРАВОЧНИК МОЛ
namespace Storehouse.View.Directories
{
    public partial class WinDirResponsibleFaces
    {
        private StorehouseEntities _db = new StorehouseEntities();
        public WinDirResponsibleFaces()
        {
            InitializeComponent();
        }
        private void ReloadList()
        {
            DgrCatalog.ItemsSource = _db.Employeers.Where(_=>_.Role==2).OrderBy(_ => _.LastName).ToList();   
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadList();
        }

       
       
        
        private void _addRecord(object sender, RoutedEventArgs e)
        {
           
            var rf = new Employeer()
            {
                Role =  2
            };
            _db.AddToEmployeers(rf);
            Edit(rf);
        }

        private void Edit(Employeer rf)
        {
            var nw = new WinResponsibleFaceEdit()
                {
                DataContext = rf,
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
            var rf = DgrCatalog.SelectedItem as Employeer;
            if (rf == null)
                return;
            Edit(rf);
        }
      
        private void DeleteRecord()
        {
            var dr = DgrCatalog.SelectedItem as Employeer;
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
            var rf = DgrCatalog.SelectedItem as Employeer;
            if (rf == null)
                return;
            Edit(rf);
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

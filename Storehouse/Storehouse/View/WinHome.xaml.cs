using System.ComponentModel;
using System.Windows;
using Storehouse.Controls;
using Storehouse.View.Directories;


namespace Storehouse.View
{
    /// <summary>
    /// Логика взаимодействия для WinHome.xaml
    /// </summary>
    public partial class WinHome
    {

        public WinHome()
        {
            InitializeComponent();
        }

        private UcTransactions _ucArrivals;
        private UcTransactions _ucWriteOffs;
        private UcTransactions _ucMovement;
        private UcGoodsHistory _ucGoodsHistory;
        private UcStore _ucStore;
     
        private void MenuPassChange_OnClick(object sender, RoutedEventArgs e)
        {
            (new WinPasswordChange()).ShowDialog();
        }

        private void MenuItemDirUsers_OnClick(object sender, RoutedEventArgs e)
        {
            var nw = new WinDirUsers();
            nw.ShowDialog();
        }

        private void WinHome_OnLoaded(object sender, RoutedEventArgs e)
        {
            TblcCurrentUser.Text = "Вход: " + App.CurrentUser.Post + " " + App.CurrentUser.FIO_Full;
            
            _ucArrivals = new UcTransactions(1);
            GrdArrivals.Children.Add(_ucArrivals);
            _ucMovement = new UcTransactions(0);
            GrdMovementGoods.Children.Add(_ucMovement);
            _ucWriteOffs = new UcTransactions(-1);
            GrdWriteOffs.Children.Add(_ucWriteOffs);
            _ucStore = new UcStore();
            GrdBalances.Children.Add(_ucStore);
            _ucGoodsHistory = new UcGoodsHistory();
            GrdGoodsHistory.Children.Add(_ucGoodsHistory);
            MiDir.Visibility = App.CurrentUser.Role != 1 ? Visibility.Collapsed : Visibility.Visible;
        }

      

        private void MenuItemAbout_OnClick(object sender, RoutedEventArgs e)
        {
           var nw = new WinAbout();
            nw.Show();
        }

        private void MenuItemDirCategories_OnClick(object sender, RoutedEventArgs e)
        {
            (new WinDirCategories()).Show();
        }
        private void MenuItemDirDepartments_OnClick(object sender, RoutedEventArgs e)
        {
            new WinDirDepartments().Show();
        }

        private void MenuItemDirResponsibleFaces_OnClick(object sender, RoutedEventArgs e)
        {
            new WinDirResponsibleFaces().Show();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WinHome_OnClosing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите закончить работу?", "Подтверждение", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
                App.Current.Shutdown(0);
            else e.Cancel = true;
        }
    }

    
}

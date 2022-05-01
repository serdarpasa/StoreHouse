using System;
using System.Linq;
using System.Windows;

namespace Storehouse.View
{
    /// <summary>
    /// Логика взаимодействия для WinArrivalEdit.xaml
    /// </summary>
    public partial class WinArrivalEdit
    {
        public StorehouseEntities Db { get; set; }
        public WinArrivalEdit()
        {
            InitializeComponent();
        }

        private void _addRecord(object sender, RoutedEventArgs e)
        {
            var transaction = DataContext as Transaction;
            if (transaction == null)
                return;
            var nw = new WinGoodsAddToArrival()
            {
                TransactionArrival = transaction,
                Db = Db
            };
            nw.ShowDialog();
            RelodDetails();
        }

        private void RelodDetails()
        {
            var tr = DataContext as Transaction;
            if (tr == null)
                return;
            DgrDetails.ItemsSource = tr.TransactionDetails.ToList();
        }
        private void _delRecord(object sender, RoutedEventArgs e)
        {
            var tr = DataContext as Transaction;
            if (tr == null)
                return;
            var goods = DgrDetails.SelectedItem as TransactionDetail;
            if (goods == null)
                return;
            Db.DeleteObject(goods);
            RelodDetails();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var transaction = DataContext as Transaction;
            if (transaction == null)
                return;
           
            try
            {
                Db.SaveChanges();
                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении");
                DialogResult = false;
            }
            
        }

        private void WinArrivalEdit_OnLoaded(object sender, RoutedEventArgs e)
        {
            CbxResponsibleFaces.ItemsSource = Db.Employeers.OrderBy(_ => _.LastName).ToList();
            RelodDetails();
        }

        private void MenuItemPrint_OnClick(object sender, RoutedEventArgs e)
        {
            var transaction = DataContext as Transaction;
            if (transaction == null)
                return;
           // Export.ExportTransaction(transaction);
            Export.TransactionPrint(transaction);
        }
    }
}

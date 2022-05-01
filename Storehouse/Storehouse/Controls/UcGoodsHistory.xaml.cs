using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Storehouse.Controls
{
    /// <summary>
    /// Логика взаимодействия для UcGoodsHistory.xaml
    /// </summary>
    public partial class UcGoodsHistory : UserControl
    {
        public UcGoodsHistory()
        {
            InitializeComponent();
        }

        public void ReloadControl()
        { 
            _db = new StorehouseEntities();
            ReloadGoods();
        }
        private StorehouseEntities _db;
        private void ReloadGoods()
        {
            var _goods = _db.Goods.ToList();
            var filtr = TbxFilter.Text.ToUpper();
            if (!string.IsNullOrEmpty(filtr))
                _goods = _goods.Where(_ => _.Name.ToUpper().Contains(filtr) || _.Code.ToUpper().Contains(filtr)).ToList();
            DgrGoods.ItemsSource = _goods.OrderBy(_=>_.Name).ThenBy(_=>_.Price).ToList();
            TblcRecordCount.Text = (_goods.Any()) ? _goods.Count().ToString() : "0";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadControl();
        }

        private void DgrGoods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var goods = DgrGoods.SelectedItem as Good;
            if (goods == null)
            {
                DgrGoodsHistory.ItemsSource = null;
                return;
            }
            ReloadHistory(goods);
        }
        private void ReloadHistory(Good goods)
        {
            var details = goods.TransactionDetails.ToList();
            if (!details.Any())
            {
                DgrGoodsHistory.ItemsSource = null;
                return;
            }
            var histories = new List<Model.GoodsMovementHistory>();
            foreach(var d in details)
            {
                var h = new Model.GoodsMovementHistory() 
                { 
                
                    Date = d.Transaction.Date,
                    TransactionTypeName = d.Transaction.TypeName,
                    Amount = d.Amount,
                    TransactionType = d.Transaction.Type

                };
                switch(d.Transaction.Type)
                {
                    case 1:
                        {
                            h.EmployeerFirst = d.Transaction.User.FIO_Short;
                            break;
                        }
                    case 0:
                        {
                            h.EmployeerFirst = d.Transaction.User.FIO_Short;
                            h.EmployeerSecond = d.Transaction.Employeer.FIO_Short;
                            break;
                        }
                    case -1:
                        {
                            h.EmployeerSecond = d.Transaction.Employeer.FIO_Short;
                            break;
                        }
                }
                histories.Add(h);
            }
            DgrGoodsHistory.ItemsSource = histories.OrderBy(_ => _.Date).ToList();
            if (histories.Any())
            {
                TblcBalance.Text = "Остаток : " + histories.Sum(_=>_.Amount * _.TransactionType);
                
            }
            else
            {
                TblcBalance.Text = "";
                
            }
        }

        private void TbxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbxFilter.Text))
                ReloadGoods();
        }

        private void TbxFilter_OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        ReloadGoods();
                        break;
                    }
            }
        }
    }
}

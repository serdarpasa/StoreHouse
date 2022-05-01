using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Storehouse.View
{
    /// <summary>
    /// Логика взаимодействия для WinWriteOffEdit.xaml
    /// </summary>
    public partial class WinWriteOffEdit
    {
        public StorehouseEntities Db { get; set; }
        public WinWriteOffEdit()
        {
            InitializeComponent();
        }

        private List<Balance> _balance; 

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var transaction = DataContext as Transaction;
            if (transaction == null)
                return;
            if (transaction.Employeer == null)
            {
                MessageBox.Show("Необходимо указать сотрудника");
            }
            try
            {
                Db.SaveChanges();
            }
            catch (Exception)
            {
                //
            }
            DialogResult = true;
        }

        private void CbxResponsibleFaces_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadBalance();
        }

        private void ReloadBalance()
        {
            var responsibleFace = CbxResponsibleFaces.SelectedItem as Employeer;
            if (responsibleFace == null)
            {
                DgrBalance.ItemStringFormat = null;
                return;
            }
            IQueryable<Balance> transactions = Db.Balances;
            var movementMinus = Db.Balances.Where(_ => _.Type == 0);
            var movementPlus = Db.Balances.Where(_ => _.Type == 0);

            var filter = TbxFilter.Text;
            filter = filter.ToUpper();
            if (!string.IsNullOrEmpty(filter))
            {
                transactions = transactions.Where(_ => _.Code.ToUpper().Contains(filter));
                movementMinus = movementMinus.Where(_ => _.Name.ToUpper().Contains(filter) || _.Code.ToUpper().Contains(filter));
                movementPlus = movementPlus.Where(_ => _.Name.ToUpper().Contains(filter) || _.Code.ToUpper().Contains(filter));

            }

            if (responsibleFace != null)
            {
                transactions = transactions.Where(_ => _.EmployeerId == responsibleFace.Id);
                movementMinus = movementMinus.Where(_ => _.UserId == responsibleFace.Id);
                movementPlus = movementPlus.Where(_ => _.EmployeerId == responsibleFace.Id);
            }
            
            _balance = new List<Balance>();

            var goodsIds = transactions.Select(_ => _.GoodsId).Distinct().ToList();

            foreach (var gId in goodsIds)
            {
                var ggs = transactions.Where(_ => _.GoodsId == gId).ToList();
                if (ggs.Any())
                {
                    var g = ggs.First();
                    var balanceGoods = new Balance()
                    {
                        Code = g.Code,
                        Name = g.Name,
                        Amount = ggs.Sum(_ => _.Amount * _.Type),
                        TotalPrice = ggs.Sum(_ => _.TotalPrice),
                        Unit = g.Unit,
                        Price = g.Price,
                        CategoryName = g.CategoryName,
                        GoodsId = g.GoodsId
                    };



                    _balance.Add(balanceGoods);
                }
            }

            foreach (var movement in movementMinus)
            {
                var balance = _balance.FirstOrDefault(_ => _.GoodsId == movement.GoodsId);
                if (balance != null)
                {
                    balance.Amount -= movement.Amount;
                    balance.TotalPrice -= movement.TotalPrice;
                }
            }
            foreach (var movement in movementPlus)
            {
                var balance = _balance.FirstOrDefault(_ => _.GoodsId == movement.GoodsId);
                if (balance != null)
                {
                    balance.Amount += movement.Amount;
                    balance.TotalPrice += movement.TotalPrice;
                }
                else
                {
                    var balanceGoods = new Balance()
                    {
                        Code = movement.Code,
                        Name = movement.Name,
                        Amount = movement.Amount,
                        TotalPrice = movement.TotalPrice,
                        Unit = movement.Unit,
                        Price = movement.Price,
                        CategoryName = movement.CategoryName,
                        GoodsId = movement.GoodsId
                    };
                    _balance.Add(balanceGoods);
                }

            }

            var transaction = DataContext as Transaction;
            if (transaction != null)
            {
                foreach (var tt in transaction.TransactionDetails.Where(_=>_.Id == 0).ToList())
                {
                    var z = _balance.FirstOrDefault(_ => _.GoodsId == tt.GoodsId);
                    if (z != null)
                        z.Amount -= tt.Amount;
                }
            }
                

            DgrBalance.ItemsSource = _balance.Where(_=>_.Amount>0).OrderBy(_ => _.Name).ToList();

        }

        private void WinWriteOffEdit_OnLoaded(object sender, RoutedEventArgs e)
        {
            CbxResponsibleFaces.ItemsSource = Db.Employeers.OrderBy(_ => _.LastName).ToList();
            var transaction = DataContext as Transaction;
            if (transaction != null)
            {
                if (transaction.Employeer != null)
                    CbxResponsibleFaces.SelectedItem =
                        Db.Employeers.FirstOrDefault(_ => _.Id == transaction.EmployeerId);
            }
            TbxCount.Text = "1";
            ReloadBalance();
            ReloadDetails();
        }
        public void textBox_InputNumberFloat(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                var tb = textBox.Text;

                var key = e.Text[0];
                tb += key;
                var isAllowed = false;
                try
                {
                    var x = Convert.ToDouble(tb);
                    isAllowed = true;
                }
                catch
                {
                }


                e.Handled = !isAllowed;
            }
        }

        public void _PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                // Запрет клавиши пробел, которая не генерирует событие PreviewTextlnput.
                e.Handled = true;
            }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            var gb = DgrBalance.SelectedItem as Balance;
            if (gb == null)
                return;
            var goods = Db.Goods.FirstOrDefault(_ => _.Id == gb.GoodsId);
            if (goods == null)
                return;
            var transaction = DataContext as Transaction;
            if (transaction == null)
                return;
             var responsibleFace = CbxResponsibleFaces.SelectedItem as Employeer;
            if (responsibleFace != null)
                transaction.Employeer = responsibleFace;
            float count = 1;

            try
            {
                count = float.Parse(TbxCount.Text);
            }
            catch (Exception)
            {
                
            }

            if (gb.Amount < count)
            {
                MessageBox.Show("Такого количества нет для списания");
                return;
            }
            var transactionDetail = transaction.TransactionDetails.FirstOrDefault(_ => _.GoodsId == goods.Id);
            if (transactionDetail != null)
                transactionDetail.Amount += count;
            else
            {
                transactionDetail = new TransactionDetail()
                {
                    TransactionId = transaction.Id,
                    GoodsId = goods.Id,
                    Amount = count
                };
                transaction.TransactionDetails.Add(transactionDetail);
            }
            try
            {
                Db.SaveChanges();
            }
            catch (Exception)
            {

            }
            ReloadBalance();
            ReloadDetails();
        }

        private void ReloadDetails()
        {
            var transaction = DataContext as Transaction;
            if (transaction == null)
                return;
            DgrTransactionDetails.ItemsSource = transaction.TransactionDetails.ToList();
            CbxResponsibleFaces.IsEnabled = !transaction.TransactionDetails.Any();
        }

        private void TbxFilter_OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        ReloadBalance();
                        break;
                    }
            }
        }


        private void Buttondel_OnClick(object sender, RoutedEventArgs e)
        {
            var td = DgrTransactionDetails.SelectedItem as TransactionDetail;
            if (td == null)
                return;
            try
            {
                Db.TransactionDetails.DeleteObject(td);
                Db.SaveChanges();
            }
            catch
            {}
            

            ReloadBalance();
            ReloadDetails();
        }

        private void MenuItemPrint_OnClick(object sender, RoutedEventArgs e)
        {
            var transaction = DataContext as Transaction;
            if (transaction == null)
                return;
            Export.TransactionPrint(transaction);
        }

        private void TbxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbxFilter.Text))
                ReloadBalance();
        }
    }
}

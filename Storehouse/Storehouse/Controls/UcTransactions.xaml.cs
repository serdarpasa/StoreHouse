using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Storehouse.View;

namespace Storehouse.Controls
{
    /// <summary>
    /// Логика взаимодействия для UcTransactions.xaml
    /// </summary>
    public partial class UcTransactions
    {
        /// <summary>
        /// тип транзакции: 1 - поступление; -1 - списание
        /// </summary>
        public int TransactionType {get; set; }
        public UcTransactions()
        {
            InitializeComponent();
        }
        public UcTransactions(int transactionType)
        {
            InitializeComponent();
            TransactionType = transactionType;
        }

        public void ReloadControl()
        {
            _db = new StorehouseEntities();
            ReloadTransactions();
        }



        private StorehouseEntities _db;
        private bool _isLoading;
        private List<Transaction> _transactions; 

        private void ReloadTransactions()
        {
            if (_isLoading)
                return;
            _isLoading = true;
            var dtB = DtpFilter.DateBegin ?? new DateTime(1900, 1, 1);
            var dtE = DtpFilter.DateEnd ?? new DateTime(3000, 12, 31);
            var transaction =
                _db.Transactions.Where(_ => _.Date>=dtB && _.Date <= dtE);
            
            switch(TransactionType)
            {
                case 1:
                    {
                        transaction = transaction.Where(_ => _.Type == 1);
                        break;
                    }
                case -1:
                    {
                        transaction = transaction.Where(_ => _.Type == -1);
                        break;
                    }
                case 0:
                    {
                        transaction = transaction.Where(_ => _.Type == 0);
                        break;
                    }
            }

            var user = CbxUsers.SelectedItem as Employeer;
            if (user != null)
                transaction = transaction.Where(_ => _.UserId == user.Id);
            var responsibleFace = CbxEmployeers.SelectedItem as Employeer;
            if (responsibleFace != null)
                transaction = transaction.Where(_ => _.EmployeerId == responsibleFace.Id);

            
            _transactions = transaction.OrderByDescending(_ => _.Date).ToList();

            DgrTransactions.ItemsSource = _transactions;
            CalcTotal();
            _isLoading = false;

        }
        private void _addRecord(object sender, RoutedEventArgs e)
        {
            var transaction = new Transaction()
            {
                UserId = App.CurrentUser.Id,
                Type = TransactionType,
                Date = DateTime.Now
                
            };
            switch (TransactionType)
            {
                case 1:
                    {
                        transaction.EmployeerId = App.CurrentUser.Id;
                        break;
                    }
                
            };
            _db.AddToTransactions(transaction);
            TransactionEdit(transaction);
        }

        private void _editRecord(object sender, RoutedEventArgs e)
        {
            var transaction = DgrTransactions.SelectedItem as Transaction;
            if (transaction == null)
                return;
            TransactionEdit(transaction);
        }

        private void _delRecord(object sender, RoutedEventArgs e)
        {
            var dr = DgrTransactions.SelectedItem as Transaction;
            if (dr == null) return;


            if (dr.UserId != App.CurrentUser.Id && App.CurrentUser.Role != 1)
            {
                MessageBox.Show("Удалить 'чужую' запись может только руководитель");
                return;
            }


            if (
                MessageBox.Show("Запись будет удалена полностью!"
                + "\nПродолжить выполнение?", "Внимание!",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
            try
            {
                _db.Transactions.DeleteObject(dr);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                //
            }

            ReloadTransactions();
        }

        private void TransactionEdit(Transaction transaction)
        {
            switch (TransactionType)
            {
                case 1:
                {
                    var winArrival = new WinArrivalEdit()
                    {
                        DataContext = transaction,
                        Db = _db
                    };
                    winArrival.ShowDialog();
                    break;
                }
                case -1:
                {
                    var winWriteOff = new WinWriteOffEdit()
                    {
                        DataContext = transaction,
                        Db = _db
                    };
                    winWriteOff.ShowDialog();
                    break;
                }
                case 0:
                {
                    var winMovement = new WinMovement()
                    {
                        DataContext = transaction,
                        Db = _db
                    };
                    winMovement.ShowDialog();
                    break;
                }
            }
            
            ReloadControl();   
        }

        private void UcArrivals_OnLoaded(object sender, RoutedEventArgs e)
        {
            ReloadControl();
            switch(TransactionType)
            {
                case 1:
                    {
                        DgrColEmployeer.Visibility = Visibility.Collapsed;
                        break;
                    }
                case -1:
                    {
                        DgrColUser.Visibility = Visibility.Collapsed;
                        break;
                    }
            }
            _isLoading = true;
            CbxUsers.ItemsSource = _db.Employeers.Where(_=>_.Role!=2).OrderBy(_ => _.LastName).ToList();
            CbxUsers.SelectedItem = null;
            CbxEmployeers.ItemsSource = _db.Employeers.OrderBy(_ => _.LastName).ToList();
            CbxEmployeers.SelectedItem = null;
            _isLoading = false;
        }

        private void ButtonUsersFilterReset_OnClick(object sender, RoutedEventArgs e)
        {
            CbxUsers.SelectedItem = null;
        }

        private void ButtonUserFilterReset_OnClick(object sender, RoutedEventArgs e)
        {
            CbxEmployeers.SelectedItem = null;
        }

        private void DgrTransactions_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var transction = DgrTransactions.SelectedItem as Transaction;
            if (transction == null)
                return;
            TransactionEdit(transction);
        }

        private void Cbx_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadTransactions();
        }

        private void DtpFilter_OnSelectedDateChanged(object sender, EventArgs e)
        {
            ReloadTransactions();
        }

        private void DgrTransactions_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var transaction = DgrTransactions.SelectedItem as Transaction;
            if (transaction == null)
                DgrDetails.ItemsSource = null;
            else
            {
                DgrDetails.ItemsSource = transaction.TransactionDetails.ToList();
            }
        }

        private void CalcTotal()
        {
            var recordCount = 0;
            float totalPrice = 0;
            if (_transactions.Any())
            {
                recordCount = _transactions.Count;
                totalPrice = _transactions.Sum(_ => _.TotalPrice);
            }
            TblcRecordCount.Text = recordCount.ToString();
            
            
            TblcTotalPrice.Text = string.Format("{0:N2}", totalPrice);
        }

        private void MenuItemPrint_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_transactions.Any())
                return;
            var list = new List<Model.TransactionsListItem>();
            foreach(var t in _transactions)
            {
                var tr = new Model.TransactionsListItem()
                {
                    Date = t.Date,
                    EmployyerFirst = t.User.FIO_Short,
                    EmployeerSecond = t.Employeer.FIO_Short,
                    TotalPrice = t.TotalPrice
                };
                list.Add(tr);
            }
            var nw = new reportViewer.wrTransactionsList()
            {
                Lr = list,
                CurrentTransactionType = TransactionType
            };
            nw.Show();
        }
       
    }
}

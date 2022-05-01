using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Storehouse.Controls
{
    /// <summary>
    /// Логика взаимодействия для UcStore.xaml
    /// </summary>
    public partial class UcStore
    {
        public UcStore()
        {
            InitializeComponent();
            _isLoading = true;
        }

        public void ReloadControl()
        {
            _db = new StorehouseEntities();
            ReloadBalance();
        }

        private StorehouseEntities _db;
        private bool _isLoading = false;
        private List<Balance> _balances; 
        private void ReloadBalance()
        {
            if (_isLoading)
                return;
            _isLoading = true;
            var dt = DtpFilter.SelectedDate ?? DateTime.Now;
            dt = new DateTime(dt.Year, dt.Month, dt.Day, 23,59,59);
            var responsibleFace = CbxResponsibleFaces.SelectedItem as Employeer;
            var category = CbxCategory.SelectedItem as Category;
            var department = CbxDepartments.SelectedItem as Department;
            var filter = TbxFilter.Text;




            App.RunInBackground(
              (o, ea) =>
              {
                      var transactions = _db.Balances.Where(_ => _.Date <= dt);
                      var movementMinus = _db.Balances.Where(_ => _.Date <= dt && _.Type == 0);
                      var movementPlus = _db.Balances.Where(_ => _.Date <= dt && _.Type == 0);
                      filter = filter.ToUpper();
                      if (!string.IsNullOrEmpty(filter))
                      {
                          movementMinus = movementMinus.Where(_ => _.Name.ToUpper().Contains(filter) || _.Code.Contains(filter));
                          movementPlus = movementPlus.Where(_ => _.Name.ToUpper().Contains(filter) || _.Code.Contains(filter));
                          transactions = transactions.Where(_ => _.Name.ToUpper().Contains(filter) || _.Code.Contains(filter));
                      }
                      if (responsibleFace != null)
                      {
                          transactions = transactions.Where(_ => _.EmployeerId == responsibleFace.Id);
                          movementMinus = movementMinus.Where(_ => _.UserId == responsibleFace.Id);
                          movementPlus = movementPlus.Where(_ => _.EmployeerId == responsibleFace.Id);
                      }
                      if (department != null)
                      {
                          transactions = transactions.Where(_ => _.DepartmentId == department.Id);
                          movementMinus = movementMinus.Where(_ => _.DepatmentUserId == department.Id);
                          movementPlus = movementPlus.Where(_ => _.DepartmentId == department.Id);
                      }
                      if (category != null)
                      {
                          transactions = transactions.Where(_ => _.CategoryId == category.Id);
                          movementMinus = movementMinus.Where(_ => _.CategoryId == category.Id);
                          movementPlus = movementPlus.Where(_ => _.CategoryId == category.Id);
                      }
                      _balances = new List<Balance>();

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
                              _balances.Add(balanceGoods);
                          }
                      }
                      foreach(var movement in movementMinus)
                      {
                          var balance = _balances.FirstOrDefault(_ => _.GoodsId == movement.GoodsId);
                          if (balance != null)
                          {
                              balance.Amount -= movement.Amount;
                              balance.TotalPrice -= movement.TotalPrice;
                          }
                      }
                      foreach (var movement in movementPlus)
                      {
                          var balance = _balances.FirstOrDefault(_ => _.GoodsId == movement.GoodsId);
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
                              _balances.Add(balanceGoods);
                          }

                      }
                  },
                  (o, ea) =>
                  {
                      DgrBalance.ItemsSource = _balances.OrderBy(_ => _.Name).Where(_ => _.Amount > 0).ToList();
                      CalcTotal();
                      _isLoading = false;
                  }
                  );
        }
        private void Cbx_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadBalance();
        }

        private void ButtonResponsibleFaceFilterReset_OnClick(object sender, RoutedEventArgs e)
        {
            CbxResponsibleFaces.SelectedItem = null;
        }

        private void UcStore_OnLoaded(object sender, RoutedEventArgs e)
        {
            _db = new StorehouseEntities();
            _isLoading = true;
            CbxResponsibleFaces.ItemsSource = _db.Employeers.OrderBy(_ => _.LastName).ToList();
            CbxResponsibleFaces.SelectedItem = null;
            DtpFilter.SelectedDate = DateTime.Now;
            CbxDepartments.ItemsSource = _db.Departments.OrderBy(_ => _.Name).ToList();
            CbxCategory.ItemsSource = _db.Categories.OrderBy(_ => _.Name).ToList();
            _isLoading = false;
            ReloadBalance();
            SbiExcel.Visibility = Visibility.Collapsed;


        }

        private void DtpFilter_OnSelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadBalance();
        }

        private void CalcTotal()
        {
            var recordCount = 0;
            float totalPrice = 0;
            if (_balances.Any())
            {
                recordCount = _balances.Count;
                var sum = _balances.Sum(_ => _.TotalPrice);
                if (sum != null) totalPrice = (float)sum;
            }
            TblcRecordCount.Text = recordCount.ToString();


            TblcTotalPrice.Text = string.Format("{0:N2}", totalPrice);
        }

        private void ButtonDepartments_OnClick(object sender, RoutedEventArgs e)
        {
            CbxDepartments.SelectedItem = null;
        }

        private void ButtonCategory_OnClick(object sender, RoutedEventArgs e)
        {
            CbxCategory.SelectedItem = null;
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

        private void TbxFilter_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbxFilter.Text))
                ReloadBalance();
        }

     

        private void MenuItemPrint_OnClick(object sender, RoutedEventArgs e)
        {
            SbiExcel.Visibility = Visibility.Visible;
            App.RunInBackground(
               (o, ea) =>
               {
                   
                  Export.ExportBalance(_balances);
               },
               (o, ea) =>
               {
                   SbiExcel.Visibility = Visibility.Collapsed;
               }
               );
           
        }
    }
}

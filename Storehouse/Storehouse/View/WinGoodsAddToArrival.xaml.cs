using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Storehouse.View
{
    /// <summary>
    /// Логика взаимодействия для WinGoodsAddToArrival.xaml
    /// </summary>
    public partial class WinGoodsAddToArrival
    {
        public Transaction TransactionArrival;
        public StorehouseEntities Db { get; set; }
        public WinGoodsAddToArrival()
        {
            InitializeComponent();
        }

        private List<Good> _goods;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            var category = CbxCategory.SelectedItem as Category;
            if (category == null)
            {
                MessageBox.Show("Укажите категорию!");
                return;
            }

            var code = TbxCode.Text;
            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Поле 'Инвентарный номер' должно быть заполнено!");
                TbxCode.Focus();
                return;
            }
            var name = CbxName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Поле 'Наименование' должно быть заполнено!");
                CbxName.Focus();
                return;
            }
            var priceText = TbxPrice.Text;
            if (string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Поле 'Цена' должно быть заполнено!");
                TbxPrice.Focus();
                return;
            }
            decimal price = 0;
            try
            {
                price = decimal.Parse(priceText);
            }
            catch (Exception)
            {
                MessageBox.Show("Неверный формат данных в поле 'Цена'");
                TbxPrice.Focus();
                return;
            }
            var amountText = TbxAmount.Text;
            if (string.IsNullOrEmpty(amountText))
            {
                MessageBox.Show("Поле 'Количество' должно быть заполнено!");
                TbxAmount.Focus();
                return;
            }
            float amount = 0;
            try
            {
                amount = float.Parse(amountText);
            }
            catch (Exception)
            {
                MessageBox.Show("Неверный формат данных в поле 'Количество'");
                TbxAmount.Focus();
                return;
            }
            var unit = TbxUnit.Text;

            var goods = AddGoods(category, code, name, unit, price);
            var td = new TransactionDetail()
            {
                Good = goods,
                Transaction = TransactionArrival,
                Amount = amount
            };
            Db.TransactionDetails.AddObject(td);


            DialogResult = true;
        }

        private Good AddGoods(Category category, string code, string name, string unit, decimal price)
        {
            var goods = Db.Goods.FirstOrDefault(_ =>_.CategoryId == category.Id && _.Code == code && _.Name == name && _.Unit == unit && _.Price == price);
            if (goods == null)
            {
                goods = new Good()
                {
                    Code = code,
                    Name = name,
                    Unit = unit,
                    Price = price,
                    CategoryId = category.Id
                };
                Db.Goods.AddObject(goods);
            }
            return goods;
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

        private void WinGoodsAddToArrival_OnLoaded(object sender, RoutedEventArgs e)
        {
            CbxCategory.ItemsSource = Db.Categories.OrderBy(_ => _.Name).ToList();
            _goods = Db.Goods.OrderBy(_ => _.Name).ToList();
            CbxName.ItemsSource = _goods;

        }

        private void CbxName_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var goods = CbxName.SelectedItem as Good;
            if (goods == null)
            {
                return;
            }
            CbxCategory.SelectedItem = goods.Category;
            TbxCode.Text = goods.Code;
            TbxUnit.Text = goods.Unit;
            TbxPrice.Text = string.Format("{0:N2}", goods.Price);
        }

        private void TbxCode_OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                {
                    CbxName.SelectedItem = _goods.FirstOrDefault(_ => _.Code == TbxCode.Text);
                        break;
                    }
            }
        }
    }
}

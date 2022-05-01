using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace Storehouse
{
    class Export
    {
        public static void ExportBalance(List<Balance> _balances)
        {
            if (!_balances.Any())
                return;


            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            // Создаём экземпляр рабочий книги Excel
            Workbook workBook;
            // Создаём экземпляр листа Excel
            Worksheet workSheet;

            workBook = excelApp.Workbooks.Add(1);
            //workSheet = (Worksheet)workBook.Worksheets.get_Item(1);
            workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets[1];
            var excelcells = workSheet.get_Range("A1", "L1");
            excelcells.Font.Size = 8;
            excelcells.Font.Bold = true;
            excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;

            workSheet.Cells[1, 1] = "Категория";
            workSheet.Columns[1].ColumnWidth = 15;
            workSheet.Cells[1, 2] = "Инв. номер";
            workSheet.Columns[2].ColumnWidth = 15;
            workSheet.Cells[1, 3] = "Наименование";
            workSheet.Columns[3].ColumnWidth = 25;
            workSheet.Cells[1, 4] = "Количество";
            workSheet.Columns[4].ColumnWidth = 8;
            workSheet.Cells[1, 5] = "Ед. измрения";
            workSheet.Columns[5].ColumnWidth = 10;
            workSheet.Cells[1, 6] = "Цена";
            workSheet.Columns[6].ColumnWidth = 10;
            workSheet.Cells[1, 7] = "Сумма";
            workSheet.Columns[7].ColumnWidth = 10;

            var r = 2;
            foreach (var b in _balances)
            {
                workSheet.Cells[r, 1] = b.CategoryName;
                workSheet.Cells[r, 2] = b.Code;
                workSheet.Cells[r, 3] = b.Name;

                workSheet.Cells[r, 4] = b.Amount;

                workSheet.Cells[r, 5] = b.Unit;
                workSheet.Cells[r, 6] = b.Price;
                workSheet.Cells[r, 7] = string.Format("{0:N2}", b.TotalPrice);

                r++;
            }
            r--;


            excelcells = workSheet.get_Range("F2", "G" + r);
            excelcells.HorizontalAlignment = Constants.xlRight;


            excelcells = workSheet.get_Range("A1", "G" + r);
            excelcells.Borders.ColorIndex = 1;
            excelcells.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            excelcells.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;


            excelcells = workSheet.get_Range("C1", "C" + r);
            excelcells.WrapText = true;


            // Открываем созданный excel-файл
            excelApp.Visible = true;
            excelApp.UserControl = true;
        }

        
        public static void TransactionPrint(Transaction transaction)
        {
            using (var _db = new StorehouseEntities())
            {
                var goods = new List<Model.TransactionItems>();
                foreach(var t  in transaction.TransactionDetails)
                {
                    var x = new Model.TransactionItems()
                    {
                        GoodsCode = t.Good.Code,
                        GoodsName = t.Good.Name,
                        GoodsUnit = t.Good.Unit,
                        Amount = t.Amount,
                        Price = (float)t.Good.Price
                    };
                    goods.Add(x);
                }



                var nw = new reportViewer.wrTransaction()
            {
            Lr = goods,
            CurrentTransaction = transaction
            }
            ;
            nw.Show();
            }            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storehouse.Model
{
    public class GoodsMovementHistory
    {
        public DateTime? Date { get; set; }
        public string TransactionTypeName { get; set; }
        public string EmployeerFirst { get; set; }
        public string EmployeerSecond { get; set; }
        public int TransactionType { get; set; }
        public float Amount { get; set; }
    }
}

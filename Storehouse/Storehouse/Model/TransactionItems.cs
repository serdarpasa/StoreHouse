using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storehouse.Model
{
    public class TransactionItems
    {

        public string GoodsCode { get; set; }
        public string GoodsName { get; set; }
        public string GoodsUnit { get; set; }
        public float Amount { get; set; }
        public float Price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storehouse.Model
{
    public  class TransactionsListItem
    {
        public DateTime? Date {get; set;}
        public string EmployyerFirst {get; set;}
        public string EmployeerSecond {get; set;}
        public float TotalPrice {get; set;}
    }
}

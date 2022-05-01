using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storehouse
{
    public partial class Transaction
    {
        public float TotalPrice
        {
            get
            {
                return TransactionDetails.Sum(td => td.TotalPrice);
            }
        }
        public string TypeName
        {
            get
            {
                var res = string.Empty;
                switch(Type)
                {
                    case 1:
                        {
                            res = "поступление";
                            break;
                        }
                    case 0:
                        {
                            res = "перемещение";
                            break;
                        }
                    case -1:
                        {
                            res = "списание";
                            break;
                        }
                }
                return res;
            }
        }
    }
}

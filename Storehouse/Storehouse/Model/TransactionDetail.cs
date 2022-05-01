using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storehouse
{
    public partial class TransactionDetail
    {
        public float TotalPrice
        {
            get
            {
                float res = 0;
                try
                {
                    res = Amount*(float) Good.Price;
                }
                catch (Exception)
                {
                    
                }
                return res;
            }
        }
    }
}

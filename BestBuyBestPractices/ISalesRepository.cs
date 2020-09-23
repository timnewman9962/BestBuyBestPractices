using System;
using System.Collections.Generic;
using System.Text;

namespace BestBuyBestPractices
{
    interface ISalesRepository
    {
        IEnumerable<Sales> GetSalesItem(int productID);
        void RemoveProduct(int productID);
    }
}

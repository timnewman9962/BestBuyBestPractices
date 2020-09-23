using System;
using System.Collections.Generic;
using System.Text;

namespace BestBuyBestPractices
{
    interface IReviewsRepository
    {
        IEnumerable<Reviews> GetReviewItem(int productID);
        void RemoveProduct(int productID);
    }
}

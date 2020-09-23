using System;
using System.Collections.Generic;
using System.Text;

namespace BestBuyBestPractices
{
    interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        void CreateProduct(string name, double price, int categoryID);
        void UpdateProduct(string name, double price, int categoryID);
        void RemoveProduct(int categoryID);
    }
}

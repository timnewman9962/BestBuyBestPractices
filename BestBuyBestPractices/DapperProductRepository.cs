using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BestBuyBestPractices
{
    class DapperProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;
        public DapperProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }
        public void CreateProduct(string name, double price, int categoryID)
        {
            _conn.Execute("INSERT INTO products (Name, Price, CategoryID)" +
                          "VALUES (@chkName, @chkPrice, @chkCategoryID);",
                          new { chkName = name, chkPrice = price, chkCategoryID = categoryID });
        }

        public void UpdateProduct(string name, double price, int productID)
        {
            _conn.Execute("UPDATE products SET Name = @chkName, Price = @chkPrice " +
                          "WHERE ProductID = @chkProductID;",
                          new { chkName = name, chkPrice = price, chkProductID = productID });
        }

        public void RemoveProduct(int productID)
        {
            _conn.Execute("DELETE FROM products WHERE ProductID = @chkProductID",
                          new { chkProductID = productID });
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _conn.Query<Product>("SELECT * FROM products");
        }

        public Product GetProduct(int productID)
        {
            try
            {
                return _conn.Query<Product>("SELECT * FROM products WHERE ProductID = @chkProductID;",
                                            new {chkProductID =  productID}).First();
            }
            catch 
            {
                return null;
            }

        }

    }
}

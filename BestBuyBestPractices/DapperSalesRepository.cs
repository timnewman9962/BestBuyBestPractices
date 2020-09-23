using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BestBuyBestPractices
{
    class DapperSalesRepository : ISalesRepository
    {
        public DapperSalesRepository(IDbConnection conn)
        {
            _conn = conn;
        }
        private readonly IDbConnection _conn;

        public IEnumerable<Sales> GetSalesItem(int productID)
        {
            try
            {
                return _conn.Query<Sales>("SELECT * FROM sales WHERE ProductID = @chkProductID",
                                        new { chkProductID = productID });
            }
            catch
            {
                return null;
            }
        }

        public void RemoveProduct(int productID)
        {
            try
            {

                _conn.Execute("DELETE FROM sales WHERE ProductID = @chkProductID",
                                        new { chkProductID = productID });
            }
            catch { }
       }
    }
}

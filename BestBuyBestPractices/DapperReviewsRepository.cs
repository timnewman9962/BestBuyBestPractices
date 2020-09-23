using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BestBuyBestPractices
{
    class DapperReviewsRepository : IReviewsRepository
    {
        private readonly IDbConnection _conn;
        public DapperReviewsRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public IEnumerable<Reviews> GetReviewItem(int productID)
        {
            return _conn.Query<Reviews>("SELECT * FROM reviews WHERE ProductID = @chkProductID;",
                                        new {chkProductID = productID});
        }

        public void RemoveProduct(int productID)
        {
            try
            {
                _conn.Execute("DELETE FROM reviews WHERE ProductID = @chkProductID",
                                        new { chkProductID = productID });
            }
            catch { }
        }
    }
}

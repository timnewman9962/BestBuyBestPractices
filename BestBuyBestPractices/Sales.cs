using System;
using System.Collections.Generic;
using System.Text;

namespace BestBuyBestPractices
{
    class Sales
    {
        public int SalesID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double PricePerUnit { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeID { get; set; }
    }
}

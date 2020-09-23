using System;
using System.Collections.Generic;
using System.Text;

namespace BestBuyBestPractices
{
    interface IDepartmentRepository
    {
        IEnumerable<Department> GetAllDepartments();
        IEnumerable<Department> GetDepartment(string name);
        void DeleteDepartment(string name);
    }
}

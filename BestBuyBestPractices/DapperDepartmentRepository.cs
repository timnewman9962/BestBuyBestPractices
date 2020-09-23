using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BestBuyBestPractices
{
    class DapperDepartmentRepository : IDepartmentRepository
    {
        public DapperDepartmentRepository(IDbConnection conn)
        {
            _conn = conn;
        }
        private readonly IDbConnection _conn;
        public IDbConnection conn { get; set; }

        public IEnumerable<Department> GetAllDepartments()
        {
            return _conn.Query<Department>("SELECT * FROM departments;");
        }

        public void InsertDepartment(string newDepartmentName)
        {
            _conn.Execute("INSERT INTO departments (Name) VALUES (@departmentName);",
                new { departmentName = newDepartmentName });
        }

        public IEnumerable<Department> GetDepartment(string name)
        {
            var rslt = _conn.Query<Department>("SELECT * FROM departments WHERE Name = @chkName;",
                                            new { chkName = name });
            if (rslt.Count() == 0)
                return null;
            return rslt;
        }

        public void DeleteDepartment(string name)
        {
            _conn.Execute("DELETE FROM departments WHERE Name = @chkName;",
                new { chkName = name });
            
        }
    }
}

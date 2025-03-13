using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Entity.EMR;

namespace WindowsFormsApp1.Dapper
{
    public class DepartmentRepositories
    {
        DapperHelper BaseDataContext = new DapperHelper("BaseData");
         
        public List<Department> GeDepartmentList()
        {
            var Departmentsql = "select * from Department";
            var list = BaseDataContext.Query<Department>(Departmentsql).ToList();
            return list;
        }

    }
}

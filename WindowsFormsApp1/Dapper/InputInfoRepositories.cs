using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMR;

namespace WindowsFormsApp1.Dapper
{
    public class InputInfoRepositories
    {
        DapperHelper EMRContext = new DapperHelper("EMR");

        public List<InputInfo> GeInputInfoList()
        {
            var sql = "select* from [dbo].[InputInfo]";
            var list = EMRContext.Query<InputInfo>(sql).ToList();
            return list;
        }

    }
}

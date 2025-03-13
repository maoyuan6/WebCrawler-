using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMR;

namespace WindowsFormsApp1.Dapper
{
    public class SymbolRepositories
    {
        DapperHelper EMRContext = new DapperHelper("EMR");

        public List<Symbol> GetSymbolList()
        {
            var sql = "select* from [EMR].[SYMBOLS]";
            var list = EMRContext.Query<Symbol>(sql).ToList();
            return list;
        }
    }
}

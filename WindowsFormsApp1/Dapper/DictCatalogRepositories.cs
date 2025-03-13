using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Dapper
{
    public class DictCatalogRepositories
    { 
        DapperHelper EMRContext = new DapperHelper("EMR");

        public List<DictCatalog> GeDictCatalogList()
        {
            var sql = "select* from [EMR].[DICT_CATALOG]";
            var list = EMRContext.Query<DictCatalog>(sql).ToList();
            return list;
        }

    }
}

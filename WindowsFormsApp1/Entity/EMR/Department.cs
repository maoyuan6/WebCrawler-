using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Entity.EMR
{
    /// <summary>
    /// 科室表实体类，对应数据库表 [dbo].[DEPARTMENT]
    /// </summary>
    public class Department
    {
        public int ID { get; set; }
        public int PARENTID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public int ATTRIBUTE { get; set; }
        public int STATE { get; set; }
        public string ADDRESS { get; set; }
        public string CONTACT { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string EMAIL { get; set; }
        public string HOMEPAGE { get; set; }
        public string INPUTCODE1 { get; set; }
        public string INPUTCODE2 { get; set; }
        public string REMARKS { get; set; }
        public string UPDATED { get; set; }
        public int ORGCODE { get; set; }
        public int? DEPTORWARD { get; set; }
        public string JOBRANGE { get; set; }
    }
}

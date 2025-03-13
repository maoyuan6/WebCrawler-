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
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Attribute { get; set; }
        public int State { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Homepage { get; set; }
        public string InputCode1 { get; set; }
        public string InputCode2 { get; set; }
        public string Remarks { get; set; }
        public string Updated { get; set; }
        public int OrgCode { get; set; }
        public int? DeptOrWard { get; set; }
        public string JobRange { get; set; }
    }
}

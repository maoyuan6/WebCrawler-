using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Clinic.Case.Business;
using DevExpress.Utils.Extensions;
using Sunny.UI;

namespace WindowsFormsApp1
{
    public partial class TemplateSet : UIForm
    {
        public TemplateSet()
        {
            InitializeComponent(); 
            uiPanel1.AddControl(new CaseTemplete());
        }
    }
}

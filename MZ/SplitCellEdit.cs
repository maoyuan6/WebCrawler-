using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text; 
using System.Windows.Forms;
using WinnerHIS.Common;

namespace Clinic.Case.Business
{
    public partial class SplitCellEdit : BaseModuleForm
    {
        List<int> rownums;
        public SplitCellEdit(int maxRow)
        {
            InitializeComponent();
            rownums = new List<int>();
            string numstr = "";
            if (maxRow!=1)
            {
                for (int i = 1; i <= maxRow; i++)
                {
                    if (maxRow % i == 0)
                    {
                        rownums.Add(i);
                        numstr += i + "、";
                        numRow.Value = i;
                    }
                }
                if (numstr != "")
                {
                    labSplit.Text = "可选择行数值为：" + numstr.Substring(0, numstr.Length - 1);
                }
            }
            
        }
        public SplitCellEdit(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numctr = sender as NumericUpDown;
            if (numctr.Value<=0)
            {
                numctr.Value = 1;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (rownums.Count>0)
            {
                if (!rownums.Contains((int)numRow.Value))
                {
                    MessageBox.Show("请输入正确的行数");
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}

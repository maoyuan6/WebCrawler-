using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text; 
using System.Windows.Forms;
using WinnerHIS.Common;

namespace Clinic.Case.Business
{
    public partial class TableEdit : BaseModuleForm
    {
        public TableEdit()
        {
            InitializeComponent();
            DrawTable();
        }
        public void DrawTable()
        {
            
        }
        private void panelTable_Paint(object sender, PaintEventArgs e)
        {
            float r = (float)(panelTable.Height/numRow.Value);
            float c = (float)(panelTable.Width/numColumn.Value);
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < numRow.Value; i++)
            {
                g.DrawLine(pen, 0, r * i, panelTable.Width, r * i);
            }
            for (int i = 0; i < numColumn.Value; i++)
            {
                g.DrawLine(pen, c * i, 0, c * i, panelTable.Height);
            }
        }

        private void numRow_ValueChanged(object sender, EventArgs e)
        {
            if (numRow.Value<=0 || numColumn.Value<=0)
            {
                return;
            }
            panelTable.CreateGraphics().Clear(Color.White);
            float r = (float)(panelTable.Height / numRow.Value);
            float c = (float)(panelTable.Width / numColumn.Value);
            Graphics g = panelTable.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < numRow.Value; i++)
            {
                g.DrawLine(pen, 0, r * i, panelTable.Width, r * i);
            }
            for (int i = 0; i < numColumn.Value; i++)
            {
                g.DrawLine(pen, c * i, 0, c * i, panelTable.Height);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            List<int> list = new List<int>();
            this.Tag = list;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCacel_Click(object sender, EventArgs e)
        {

        }
    }
}

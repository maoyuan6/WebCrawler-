using HIS.Clinic.ClinicCase.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private static MzCotextEditor _mzcontent = null;
        public Form2()
        {
            InitializeComponent();
            // 使用事件委托绑定方法
            myTabControl1.TabAdding += Add;
            myTabControl1.TabSwitched += TabSwitched;
            myTabControl1.SelectedIndex = 0;

            // 首页
            _mzcontent = new MzCotextEditor() { Dock = DockStyle.Fill };
            this.panel2.Controls.Add(_mzcontent);
        }

        public string Add()
        {
            MessageBox.Show("是否要新增");
            return "";
        }
        public void TabSwitched(object sender, int index, string name)
        {
            if (index != 0)
            {
                MessageBox.Show("你已经切换tab 到" + name);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _mzcontent.myWriterControl.PrintDocument();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a = _mzcontent.myWriterControl.XMLText;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "所有文件 (*.*)|*.*"; // 过滤文件类型
            openFileDialog.Multiselect = false; // 是否允许多选
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName; // 获取文件的绝对路径 
                string content = File.ReadAllText(filePath);
                _mzcontent.SetMyWriterControlContent(content);
            }
        }
    }
}

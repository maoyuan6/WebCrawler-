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
using DevExpress.Utils.Extensions;
using HIS.Clinic.ClinicCase.UI;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private static MzCotextEditor _mzcontent = null;
        public void Init()
        {
            // 首页
            _mzcontent = new MzCotextEditor() { Dock = DockStyle.Fill };
            this.panel3.Controls.Add(_mzcontent);
            string path = "C:\\Users\\maoyuan0174\\Desktop\\新建 TXT 文件.txt";
            string content = File.ReadAllText(path);
            _mzcontent.SetMyWriterControlContent(content);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            _mzcontent.myWriterControl.ClearContent(); 
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

        private void button4_Click(object sender, EventArgs e)
        {
            _mzcontent.myWriterControl.PrintDocument();
            //// 创建打印预览对话框
            //PrintPreviewDialog previewDialog = new PrintPreviewDialog
            //{
            //    Document = // 绑定 WriterControl 的打印文档
            //};
            //previewDialog.ShowDialog(); // 显示打印预览 
        }
    }
}

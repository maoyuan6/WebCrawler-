using System.Diagnostics;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using Aspose.Html;
using Aspose.Html.Converters;
using Aspose.Html.Dom;
using Aspose.Html.Rendering.Pdf;
using Aspose.Html.Saving;
using HtmlToPDFCore;
using SelectPdf;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        //private string pdfFile = @"C:\Users\maoyuan0174\Desktop\新建文本文档 (6).pdf";
        private string htmlFile = @"..\..\..\html\新建文本文档 (3).html";
        private void button1_Click(object sender, EventArgs e)
        {
            PrintHtmlFromFile(htmlFile);
        }

        private void PrintHtmlFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                // 从指定文件中读取 HTML 内容
                string htmlContent = File.ReadAllText(filePath);
                //var html = htmlContent; 
                //var pdf = new HtmlToPDF();
                //var buffer = pdf.ReturnPDF(html);
                //if (File.Exists(pdfFile)) File.Delete(pdfFile);
                //using (var f = new FileStream(pdfFile, FileMode.Create))
                //{
                //    f.Write(buffer, 0, buffer.Length);
                //    f.Flush();
                //    f.Close();
                //} 
                //if (System.IO.File.Exists(pdfFile))
                //{
                //    Process.Start(new ProcessStartInfo
                //    {
                //        FileName = pdfFile,
                //        UseShellExecute = true  // 确保使用操作系统的默认程序打开文件
                //    });
                //}
                //else
                //{
                //    MessageBox.Show("文件不存在！");
                //}
                // htmlContent = htmlContent.Replace("&nbsp;", "");
                //创建 WebBrowser 控件
                WebBrowser webBrowser = new WebBrowser();
                webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                webBrowser.DocumentText = htmlContent; // 加载 HTML 内容
            }
            else
            {
                MessageBox.Show("指定的文件不存在");
            }
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // ActiveAspose();
            WebBrowser webBrowser = sender as WebBrowser; 
            if (webBrowser != null)
            {
                // 调用打印功能
                webBrowser.ShowPrintPreviewDialog();
            }
        } 

    }
}

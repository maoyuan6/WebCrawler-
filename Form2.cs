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

        private void button1_Click(object sender, EventArgs e)
        { 
            PrintHtmlFromFile("C:\\Users\\maoyuan0174\\Desktop\\新建文本文档 (4).html");
        }

        private void PrintHtmlFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
           
                // 从指定文件中读取 HTML 内容
                string htmlContent = File.ReadAllText(filePath);

                var html = htmlContent;

                var pdfFile = @"C:\Users\maoyuan0174\Desktop\新建文本文档 (6).pdf";

              
                var pdf = new HtmlToPDF();
                var buffer = pdf.ReturnPDF(html);
                if (File.Exists(pdfFile)) File.Delete(pdfFile);
                using (var f = new FileStream(pdfFile, FileMode.Create))
                {
                    f.Write(buffer, 0, buffer.Length);
                    f.Flush();
                    f.Close();
                } 
                 
                // htmlContent = htmlContent.Replace("&nbsp;", "");
                // 创建 WebBrowser 控件
                //WebBrowser webBrowser = new WebBrowser();
                //webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
                //webBrowser.DocumentText = htmlContent; // 加载 HTML 内容
            }
            else
            {
                MessageBox.Show("指定的文件不存在");
            }
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ActiveAspose();
            WebBrowser webBrowser = sender as WebBrowser;
            if (webBrowser != null)
            { 
                // 调用打印功能
                webBrowser.ShowPrintPreviewDialog();
            }
        }

        
        /// <summary>
        /// 激活Aspose
        /// </summary>
        private void ActiveAspose()
        {
            string LData = "DQo8TGljZW5zZT4NCjxEYXRhPg0KPExpY2Vuc2VkVG8+VGhlIFdvcmxkIEJhbms8L0xpY2Vuc2VkVG8+DQo8RW1haWxUbz5ra3VtYXIzQHdvcmxkYmFua2dyb3VwLm9yZzwvRW1haWxUbz4NCjxMaWNlbnNlVHlwZT5EZXZlbG9wZXIgU21hbGwgQnVzaW5lc3M8L0xpY2Vuc2VUeXBlPg0KPExpY2Vuc2VOb3RlPjEgRGV2ZWxvcGVyIEFuZCAxIERlcGxveW1lbnQgTG9jYXRpb248L0xpY2Vuc2VOb3RlPg0KPE9yZGVySUQ+MjEwMzE2MTg1OTU3PC9PcmRlcklEPg0KPFVzZXJJRD43NDQ5MTY8L1VzZXJJRD4NCjxPRU0+VGhpcyBpcyBub3QgYSByZWRpc3RyaWJ1dGFibGUgbGljZW5zZTwvT0VNPg0KPFByb2R1Y3RzPg0KPFByb2R1Y3Q+QXNwb3NlLlRvdGFsIGZvciAuTkVUPC9Qcm9kdWN0Pg0KPC9Qcm9kdWN0cz4NCjxFZGl0aW9uVHlwZT5Qcm9mZXNzaW9uYWw8L0VkaXRpb25UeXBlPg0KPFNlcmlhbE51bWJlcj4wM2ZiMTk5YS01YzhhLTQ4ZGItOTkyZS1kMDg0ZmYwNjZkMGM8L1NlcmlhbE51bWJlcj4NCjxTdWJzY3JpcHRpb25FeHBpcnk+MjAyMjA1MTY8L1N1YnNjcmlwdGlvbkV4cGlyeT4NCjxMaWNlbnNlVmVyc2lvbj4zLjA8L0xpY2Vuc2VWZXJzaW9uPg0KPExpY2Vuc2VJbnN0cnVjdGlvbnM+aHR0cHM6Ly9wdXJjaGFzZS5hc3Bvc2UuY29tL3BvbGljaWVzL3VzZS1saWNlbnNlPC9MaWNlbnNlSW5zdHJ1Y3Rpb25zPg0KPC9EYXRhPg0KPFNpZ25hdHVyZT5XbkJYNnJOdHpCclNMV3pBdFlqOEtkdDFLSUI5MlFrL2xEbFNmMlM1TFRIWGdkcS9QQ2NqWHVORmp0NEJuRmZwNFZLc3VsSjhWeFExakIwbmM0R1lWcWZLek14SFFkaXFuZU03NTJaMjlPbmdyVW40Yk0rc1l6WWVSTE9UOEpxbE9RN05rRFU0bUk2Z1VyQ3dxcjdnUVYxbDJJWkJxNXMzTEFHMFRjQ1ZncEE9PC9TaWduYXR1cmU+DQo8L0xpY2Vuc2U+DQo=";
            MemoryStream stream3 = new MemoryStream(Convert.FromBase64String(LData));
            Aspose.Html.License license3 = new Aspose.Html.License();
            stream3.Seek(0, SeekOrigin.Begin);
            license3.SetLicense(stream3);
        }

    }
}

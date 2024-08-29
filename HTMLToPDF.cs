using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using SelectPdf;
using Orientation = System.Windows.Forms.Orientation;

namespace WinFormsApp1
{
    public class HTMLToPDF
    { 
        /// <summary>
        /// Html导出PDF
        /// </summary>
        /// <returns></returns> 
        public string ToPDF(string htmlString)
        { 
            HtmlToPdf Renderer = new HtmlToPdf();
            //设置Pdf参数
            Renderer.Options.PdfPageOrientation = PdfPageOrientation.Landscape;//设置页面方式-横向  PdfPageOrientation.Portrait  竖向
            Renderer.Options.PdfPageSize = PdfPageSize.A4;//设置页面大小，30种页面大小可以选择
            Renderer.Options.MarginTop = 10;   //上下左右边距设置  
            Renderer.Options.MarginBottom = 10;
            Renderer.Options.MarginLeft = 10;
            Renderer.Options.MarginRight = 10;

            //设置更多额参数可以去HtmlToPdfOptions里面选择设置
            var docHtml = Renderer.ConvertHtmlString(htmlString);//根据html内容导出PDF
           
            string webRootPath = "D://";  //获取项目运行绝对路径
            var path = $"ExportPDF/{DateTime.Now.ToString("yyyyMMdd")}/";//文件相对路径
            var savepathHtml = $"{webRootPath}{path}{Guid.NewGuid().ToString()}-Html.pdf";//保存绝对路径
            if (!Directory.Exists(Path.GetDirectoryName(webRootPath + path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(webRootPath + path));
            }
            docHtml.Save(savepathHtml);

            return savepathHtml;
        }
    }
}

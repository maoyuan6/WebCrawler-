using System.Collections.Concurrent;
using System.ComponentModel;
using System.Security.Policy;
using System.Windows.Forms;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;



namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private BackgroundWorker backgroundWorker;
        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true; // 启用进度报告功能
            backgroundWorker.DoWork += BackgroundWorker_DoWork; // 定义后台工作方法
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged; // 定义进度变更事件处理方法 
        }
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // 在主线程中更新进度条
            progressBar1.Value = e.ProgressPercentage;

            // 更新Label2以显示当前进度的百分比
            label2.Text = $@"{e.ProgressPercentage}%";
        }

        public Form1()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!Directory.Exists(label6.Text))
            {
                MessageBox.Show("文件保存路径不存在！");
                return;
            }
            // 初始化ChromeDriver为无头模式
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");
            options.AddAdditionalChromeOption("useAutomationExtension", false);
            options.AddArguments("--disable-blink-features=AutomationControlled");
            options.AddArgument("--user-agent='Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3'");
            IWebDriver driver = new ChromeDriver(options);

            // 导航至目标网址
            driver.Navigate().GoToUrl(textBox1.Text);
            // 模拟滚动到底部以加载懒加载内容
            int scrollPauseTime = 100; // 滚动后暂停的时间，单位为毫秒
            int scrollIterations = 100; // 滚动次数，可以根据需要调整

            if (!checkBox1.Checked)
            {
                scrollPauseTime = 0;
                scrollIterations = 0;
            }
            else
            {
                numericUpDown1.Text ??= "0";
                scrollIterations = Convert.ToInt32(numericUpDown1.Text);
            }
            for (int i = 0; i < scrollIterations; i++)
            {
                // 执行JavaScript代码滚动到底部
                ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollTo(0, {i * 100});");

                // 暂停一段时间，等待懒加载内容加载
                Thread.Sleep(scrollPauseTime);
            }

            if (checkBox1.Checked)
            {
                // 简化处理，这里不直接等待固定时间，而是尝试一种简化的资源收集方式
                // 实际应用中，可能需要根据具体情况实现更复杂的等待逻辑，比如等待某些元素出现
                Task.Delay(5000).GetAwaiter().GetResult(); // 等待足够的时间让动态内容加载，这里是示例，具体时间可能需要调整

            }

            // 获取经过JavaScript执行后完整的页面源码
            string fullPageSource = driver.PageSource;

            // 关闭WebDriver
            driver.Quit();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(fullPageSource);
            HttpClient client = new HttpClient();
            //HttpResponseMessage response = client.GetAsync(textBox1.Text).GetAwaiter().GetResult();
            //string htmlContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            //HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            //doc.LoadHtml(htmlContent);
            var resourceUrls = new List<string>();
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img[@src]|//a[@href]|//link[@href]"))
            {
                string url = link.GetAttributeValue("src", "href"); // 根据标签类型获取src或href属性值
                resourceUrls.Add(url);
            }
            resourceUrls = resourceUrls.Where(a => a != "href").ToList();
            resourceUrls = resourceUrls.Distinct(new ProtocolInsensitiveUrlEqualityComparer()).ToList();
            for (int i = 0; i < resourceUrls.Count; i++)
            {
                if (resourceUrls[i].StartsWith("//"))
                {
                    resourceUrls[i] = "https:" + resourceUrls[i];
                }
            }

            int completedResources = 0;
            for (int i = 0; i < resourceUrls.Count; i++)
            {
                try
                {
                    using HttpResponseMessage clientResponse = client.GetAsync(resourceUrls[i], HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();
                    if (clientResponse.IsSuccessStatusCode)
                    {
                        string fileName = Path.GetFileName(new Uri(resourceUrls[i]).LocalPath); // 从URL获取文件名
                        if (fileName.IndexOf(".", StringComparison.Ordinal) == -1)
                        {
                            fileName += ".jpg";
                        }
                        string filePath = Path.Combine(label6.Text, fileName); // 指定本地保存路径
                        filePath = GetFilePath(filePath);
                        using Stream contentStream = clientResponse.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                        using FileStream fileStream = File.Create(filePath);
                        contentStream.CopyToAsync(fileStream).GetAwaiter().GetResult();

                        completedResources++;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading resource {resourceUrls[i]}: {ex.Message}");
                }

                // 更新进度
                double progressPercentage = ((double)completedResources / resourceUrls.Count) * 100;
                if (i + 1 == resourceUrls.Count)
                {
                    progressPercentage = 100;
                }
                backgroundWorker.ReportProgress((int)progressPercentage);
            }

            MessageBox.Show("下载完成");
        }

        public string GetFilePath(string originalFilePath)
        {
            string directoryPath = Path.GetDirectoryName(originalFilePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFilePath);
            string extension = Path.GetExtension(originalFilePath);

            int counter = 1;
            string newFilePath = originalFilePath;

            while (File.Exists(newFilePath))
            {
                newFilePath = Path.Combine(directoryPath, $"{fileNameWithoutExtension}({counter}){extension}");
                counter++;
            }

            return newFilePath;
            // 如果newFilePath与originalFilePath不同，说明文件已存在并已被重命名
            //if (newFilePath != originalFilePath)
            //{
            //    // 文件存在，执行重命名操作
            //    File.Move(originalFilePath, newFilePath);
            //    Console.WriteLine($"文件已重命名为: {newFilePath}");
            //}
            //else
            //{
            //    // 文件不存在，无需重命名
            //    Console.WriteLine("文件不存在，无需重命名。");
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy) // 防止重复启动
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 100;
                backgroundWorker.RunWorkerAsync(); // 启动后台工作线程
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // 设置初始目录（可选）
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer; // 或其他SpecialFolder枚举值，如Desktop、Documents等 
                // 是否允许用户创建新文件夹（可选）
                folderBrowserDialog.ShowNewFolderButton = true;
                // 显示对话框并获取用户选择结果
                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    // 使用selectedFolderPath，例如：
                    label6.Text = selectedFolderPath;
                    MessageBox.Show("文件将会保存在: " + selectedFolderPath);
                }
            }
        }
    }
}

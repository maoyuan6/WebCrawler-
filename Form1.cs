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
            backgroundWorker.WorkerReportsProgress = true; // ���ý��ȱ��湦��
            backgroundWorker.DoWork += BackgroundWorker_DoWork; // �����̨��������
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged; // ������ȱ���¼������� 
        }
        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // �����߳��и��½�����
            progressBar1.Value = e.ProgressPercentage;

            // ����Label2����ʾ��ǰ���ȵİٷֱ�
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
                MessageBox.Show("�ļ�����·�������ڣ�");
                return;
            }
            // ��ʼ��ChromeDriverΪ��ͷģʽ
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("headless");
            options.AddAdditionalChromeOption("useAutomationExtension", false);
            options.AddArguments("--disable-blink-features=AutomationControlled");
            options.AddArgument("--user-agent='Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3'");
            IWebDriver driver = new ChromeDriver(options);

            // ������Ŀ����ַ
            driver.Navigate().GoToUrl(textBox1.Text);
            // ģ��������ײ��Լ�������������
            int scrollPauseTime = 100; // ��������ͣ��ʱ�䣬��λΪ����
            int scrollIterations = 100; // �������������Ը�����Ҫ����

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
                // ִ��JavaScript����������ײ�
                ((IJavaScriptExecutor)driver).ExecuteScript($"window.scrollTo(0, {i * 100});");

                // ��ͣһ��ʱ�䣬�ȴ����������ݼ���
                Thread.Sleep(scrollPauseTime);
            }

            if (checkBox1.Checked)
            {
                // �򻯴������ﲻֱ�ӵȴ��̶�ʱ�䣬���ǳ���һ�ּ򻯵���Դ�ռ���ʽ
                // ʵ��Ӧ���У�������Ҫ���ݾ������ʵ�ָ����ӵĵȴ��߼�������ȴ�ĳЩԪ�س���
                Task.Delay(5000).GetAwaiter().GetResult(); // �ȴ��㹻��ʱ���ö�̬���ݼ��أ�������ʾ��������ʱ�������Ҫ����

            }

            // ��ȡ����JavaScriptִ�к�������ҳ��Դ��
            string fullPageSource = driver.PageSource;

            // �ر�WebDriver
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
                string url = link.GetAttributeValue("src", "href"); // ���ݱ�ǩ���ͻ�ȡsrc��href����ֵ
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
                        string fileName = Path.GetFileName(new Uri(resourceUrls[i]).LocalPath); // ��URL��ȡ�ļ���
                        if (fileName.IndexOf(".", StringComparison.Ordinal) == -1)
                        {
                            fileName += ".jpg";
                        }
                        string filePath = Path.Combine(label6.Text, fileName); // ָ�����ر���·��
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

                // ���½���
                double progressPercentage = ((double)completedResources / resourceUrls.Count) * 100;
                if (i + 1 == resourceUrls.Count)
                {
                    progressPercentage = 100;
                }
                backgroundWorker.ReportProgress((int)progressPercentage);
            }

            MessageBox.Show("�������");
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
            // ���newFilePath��originalFilePath��ͬ��˵���ļ��Ѵ��ڲ��ѱ�������
            //if (newFilePath != originalFilePath)
            //{
            //    // �ļ����ڣ�ִ������������
            //    File.Move(originalFilePath, newFilePath);
            //    Console.WriteLine($"�ļ���������Ϊ: {newFilePath}");
            //}
            //else
            //{
            //    // �ļ������ڣ�����������
            //    Console.WriteLine("�ļ������ڣ�������������");
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy) // ��ֹ�ظ�����
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 100;
                backgroundWorker.RunWorkerAsync(); // ������̨�����߳�
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                // ���ó�ʼĿ¼����ѡ��
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer; // ������SpecialFolderö��ֵ����Desktop��Documents�� 
                // �Ƿ������û��������ļ��У���ѡ��
                folderBrowserDialog.ShowNewFolderButton = true;
                // ��ʾ�Ի��򲢻�ȡ�û�ѡ����
                if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string selectedFolderPath = folderBrowserDialog.SelectedPath;
                    // ʹ��selectedFolderPath�����磺
                    label6.Text = selectedFolderPath;
                    MessageBox.Show("�ļ����ᱣ����: " + selectedFolderPath);
                }
            }
        }
    }
}

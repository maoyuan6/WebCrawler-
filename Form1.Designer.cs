namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            label4 = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            button2 = new Button();
            label5 = new Label();
            label6 = new Label();
            checkBox1 = new CheckBox();
            numericUpDown1 = new NumericUpDown();
            label7 = new Label();
            dateTimePicker1 = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(665, 398);
            button1.Name = "button1";
            button1.Size = new Size(123, 40);
            button1.TabIndex = 0;
            button1.Text = "开始下载";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 358);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(776, 29);
            progressBar1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(2, 322);
            label1.Name = "label1";
            label1.Size = new Size(84, 20);
            label1.TabIndex = 2;
            label1.Text = "下载进度：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(92, 322);
            label2.Name = "label2";
            label2.Size = new Size(31, 20);
            label2.TabIndex = 3;
            label2.Text = "0%";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(117, 16);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(671, 27);
            textBox1.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 19);
            label3.Name = "label3";
            label3.Size = new Size(99, 20);
            label3.TabIndex = 5;
            label3.Text = "请输入链接：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 64);
            label4.Name = "label4";
            label4.Size = new Size(159, 20);
            label4.TabIndex = 6;
            label4.Text = "请选择文件存放路径：";
            // 
            // button2
            // 
            button2.Location = new Point(168, 60);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 7;
            button2.Text = "路径选择";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 104);
            label5.Name = "label5";
            label5.Size = new Size(129, 20);
            label5.TabIndex = 8;
            label5.Text = "文件存放路径为：";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(138, 104);
            label6.Name = "label6";
            label6.Size = new Size(0, 20);
            label6.TabIndex = 9;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(14, 140);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(166, 24);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "网页是否存在懒加载";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(638, 137);
            numericUpDown1.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(150, 27);
            numericUpDown1.TabIndex = 11;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(536, 141);
            label7.Name = "label7";
            label7.Size = new Size(84, 20);
            label7.TabIndex = 12;
            label7.Text = "懒加载深度";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(104, 212);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(250, 27);
            dateTimePicker1.TabIndex = 13;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dateTimePicker1);
            Controls.Add(label7);
            Controls.Add(numericUpDown1);
            Controls.Add(checkBox1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "网页图片下载助手";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private ProgressBar progressBar1;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private Label label3;
        private Label label4;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button2;
        private Label label5;
        private Label label6;
        private CheckBox checkBox1;
        private NumericUpDown numericUpDown1;
        private Label label7;
        private DateTimePicker dateTimePicker1;
    }
}

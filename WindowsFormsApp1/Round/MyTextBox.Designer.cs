
namespace TextBoxDemo
{
    partial class MyTextBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tbContent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbContent
            // 
            this.tbContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.tbContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbContent.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tbContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.tbContent.Location = new System.Drawing.Point(13, 9);
            this.tbContent.Name = "tbContent";
            this.tbContent.Size = new System.Drawing.Size(172, 16);
            this.tbContent.TabIndex = 0;
            this.tbContent.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbContent_MouseClick);
            this.tbContent.TextChanged += new System.EventHandler(this.tbContent_TextChanged);
            this.tbContent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbContent_KeyDown);
            this.tbContent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbContent_KeyPress);
            this.tbContent.MouseLeave += new System.EventHandler(this.tbContent_MouseLeave);
            this.tbContent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tbContent_MouseMove);
            // 
            // BiolabTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.Controls.Add(this.tbContent);
            this.Name = "BiolabTextBox";
            this.Size = new System.Drawing.Size(200, 32);
            this.Load += new System.EventHandler(this.BiolabTextBox_Load);
            this.MouseLeave += new System.EventHandler(this.BiolabTextBox1_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BiolabTextBox1_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbContent;
    }
}

namespace WindowsFormsApp1
{
    partial class TemplateSelect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.myTextBox1 = new TextBoxDemo.MyTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.roundedButton1 = new RoundedButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.roundedButton2 = new RoundedButton();
            this.roundedButton3 = new RoundedButton();
            this.roundedButton4 = new RoundedButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.templateId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.templatename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dept = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creater = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.roundedButton5 = new RoundedButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // myTextBox1
            // 
            this.myTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.myTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.myTextBox1.IsMiddle = false;
            this.myTextBox1.LeftMargin = 16;
            this.myTextBox1.Location = new System.Drawing.Point(94, 12);
            this.myTextBox1.MaxLength = 32767;
            this.myTextBox1.Name = "myTextBox1";
            this.myTextBox1.ReadOnly = false;
            this.myTextBox1.RightMargin = 20;
            this.myTextBox1.SelectedText = "";
            this.myTextBox1.SelectionLength = 0;
            this.myTextBox1.SelectionStart = 0;
            this.myTextBox1.Size = new System.Drawing.Size(304, 35);
            this.myTextBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "模版名称";
            // 
            // roundedButton1
            // 
            this.roundedButton1.BackColor = System.Drawing.Color.Transparent;
            this.roundedButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton1.BorderSize = 1;
            this.roundedButton1.CornerRadius = 8;
            this.roundedButton1.FlatAppearance.BorderSize = 0;
            this.roundedButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.roundedButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.roundedButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundedButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton1.Location = new System.Drawing.Point(404, 12);
            this.roundedButton1.Name = "roundedButton1";
            this.roundedButton1.Size = new System.Drawing.Size(123, 35);
            this.roundedButton1.TabIndex = 2;
            this.roundedButton1.Text = "搜索";
            this.roundedButton1.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.roundedButton1);
            this.panel1.Controls.Add(this.myTextBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(552, 64);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(552, 530);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.roundedButton4);
            this.panel3.Controls.Add(this.roundedButton3);
            this.panel3.Controls.Add(this.roundedButton2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(552, 53);
            this.panel3.TabIndex = 0;
            // 
            // roundedButton2
            // 
            this.roundedButton2.BackColor = System.Drawing.Color.Transparent;
            this.roundedButton2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton2.BorderSize = 1;
            this.roundedButton2.CornerRadius = 8;
            this.roundedButton2.FlatAppearance.BorderSize = 0;
            this.roundedButton2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.roundedButton2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.roundedButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundedButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton2.Location = new System.Drawing.Point(3, 9);
            this.roundedButton2.Name = "roundedButton2";
            this.roundedButton2.Size = new System.Drawing.Size(169, 35);
            this.roundedButton2.TabIndex = 3;
            this.roundedButton2.Text = "全部模版";
            this.roundedButton2.UseVisualStyleBackColor = false;
            // 
            // roundedButton3
            // 
            this.roundedButton3.BackColor = System.Drawing.Color.Transparent;
            this.roundedButton3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton3.BorderSize = 1;
            this.roundedButton3.CornerRadius = 8;
            this.roundedButton3.FlatAppearance.BorderSize = 0;
            this.roundedButton3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.roundedButton3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.roundedButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundedButton3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton3.Location = new System.Drawing.Point(178, 9);
            this.roundedButton3.Name = "roundedButton3";
            this.roundedButton3.Size = new System.Drawing.Size(163, 35);
            this.roundedButton3.TabIndex = 4;
            this.roundedButton3.Text = "部门模版";
            this.roundedButton3.UseVisualStyleBackColor = false;
            // 
            // roundedButton4
            // 
            this.roundedButton4.BackColor = System.Drawing.Color.Transparent;
            this.roundedButton4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton4.BorderSize = 1;
            this.roundedButton4.CornerRadius = 8;
            this.roundedButton4.FlatAppearance.BorderSize = 0;
            this.roundedButton4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.roundedButton4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.roundedButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundedButton4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton4.Location = new System.Drawing.Point(347, 9);
            this.roundedButton4.Name = "roundedButton4";
            this.roundedButton4.Size = new System.Drawing.Size(171, 35);
            this.roundedButton4.TabIndex = 5;
            this.roundedButton4.Text = "个人模版";
            this.roundedButton4.UseVisualStyleBackColor = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.templateId,
            this.templatename,
            this.dept,
            this.creater});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 53);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(552, 477);
            this.dataGridView1.TabIndex = 1;
            // 
            // templateId
            // 
            this.templateId.HeaderText = "模版ID";
            this.templateId.MinimumWidth = 6;
            this.templateId.Name = "templateId";
            this.templateId.Width = 125;
            // 
            // templatename
            // 
            this.templatename.HeaderText = "模版名称";
            this.templatename.MinimumWidth = 6;
            this.templatename.Name = "templatename";
            this.templatename.Width = 125;
            // 
            // dept
            // 
            this.dept.HeaderText = "所属部门";
            this.dept.MinimumWidth = 6;
            this.dept.Name = "dept";
            this.dept.Width = 125;
            // 
            // creater
            // 
            this.creater.HeaderText = "创建者";
            this.creater.MinimumWidth = 6;
            this.creater.Name = "creater";
            this.creater.Width = 125;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.roundedButton5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 485);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(552, 45);
            this.panel4.TabIndex = 2;
            // 
            // roundedButton5
            // 
            this.roundedButton5.BackColor = System.Drawing.Color.Transparent;
            this.roundedButton5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton5.BorderSize = 1;
            this.roundedButton5.CornerRadius = 8;
            this.roundedButton5.FlatAppearance.BorderSize = 0;
            this.roundedButton5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.roundedButton5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.roundedButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roundedButton5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(196)))), ((int)(((byte)(248)))));
            this.roundedButton5.Location = new System.Drawing.Point(417, 7);
            this.roundedButton5.Name = "roundedButton5";
            this.roundedButton5.Size = new System.Drawing.Size(123, 35);
            this.roundedButton5.TabIndex = 3;
            this.roundedButton5.Text = "选择该模版";
            this.roundedButton5.UseVisualStyleBackColor = false;
            this.roundedButton5.Click += new System.EventHandler(this.roundedButton5_Click);
            // 
            // TemplateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 594);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "TemplateSelect";
            this.Text = "TemplateSelect";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TextBoxDemo.MyTextBox myTextBox1;
        private System.Windows.Forms.Label label1;
        private RoundedButton roundedButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private RoundedButton roundedButton4;
        private RoundedButton roundedButton3;
        private RoundedButton roundedButton2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn templateId;
        private System.Windows.Forms.DataGridViewTextBoxColumn templatename;
        private System.Windows.Forms.DataGridViewTextBoxColumn dept;
        private System.Windows.Forms.DataGridViewTextBoxColumn creater;
        private System.Windows.Forms.Panel panel4;
        private RoundedButton roundedButton5;
    }
}
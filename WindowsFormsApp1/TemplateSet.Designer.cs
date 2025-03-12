namespace WindowsFormsApp1
{
    partial class TemplateSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateSet));
            this.uiPanel1 = new Sunny.UI.UIPanel();
            this.uiCheckBoxGroup1 = new Sunny.UI.UICheckBoxGroup();
            this.uiCheckBoxGroup2 = new Sunny.UI.UICheckBoxGroup();
            this.uiPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiPanel1
            // 
            this.uiPanel1.Controls.Add(this.uiCheckBoxGroup2);
            this.uiPanel1.Controls.Add(this.uiCheckBoxGroup1);
            this.uiPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1409, 711);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = "uiPanel1";
            this.uiPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiCheckBoxGroup1
            // 
            this.uiCheckBoxGroup1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiCheckBoxGroup1.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.uiCheckBoxGroup1.Location = new System.Drawing.Point(749, 38);
            this.uiCheckBoxGroup1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiCheckBoxGroup1.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBoxGroup1.Name = "uiCheckBoxGroup1";
            this.uiCheckBoxGroup1.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiCheckBoxGroup1.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("uiCheckBoxGroup1.SelectedIndexes")));
            this.uiCheckBoxGroup1.Size = new System.Drawing.Size(8, 8);
            this.uiCheckBoxGroup1.TabIndex = 0;
            this.uiCheckBoxGroup1.Text = "uiCheckBoxGroup1";
            this.uiCheckBoxGroup1.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiCheckBoxGroup2
            // 
            this.uiCheckBoxGroup2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiCheckBoxGroup2.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(236)))), ((int)(((byte)(255)))));
            this.uiCheckBoxGroup2.Location = new System.Drawing.Point(440, 233);
            this.uiCheckBoxGroup2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uiCheckBoxGroup2.MinimumSize = new System.Drawing.Size(1, 1);
            this.uiCheckBoxGroup2.Name = "uiCheckBoxGroup2";
            this.uiCheckBoxGroup2.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.uiCheckBoxGroup2.SelectedIndexes = ((System.Collections.Generic.List<int>)(resources.GetObject("uiCheckBoxGroup2.SelectedIndexes")));
            this.uiCheckBoxGroup2.Size = new System.Drawing.Size(8, 8);
            this.uiCheckBoxGroup2.TabIndex = 1;
            this.uiCheckBoxGroup2.Text = "uiCheckBoxGroup2";
            this.uiCheckBoxGroup2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TemplateSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1409, 711);
            this.Controls.Add(this.uiPanel1);
            this.Name = "TemplateSet";
            this.Text = "TemplateSet";
            this.uiPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIPanel uiPanel1;
        private Sunny.UI.UICheckBoxGroup uiCheckBoxGroup2;
        private Sunny.UI.UICheckBoxGroup uiCheckBoxGroup1;
    }
}
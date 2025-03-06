
namespace Clinic.Case.Business
{
    partial class TableEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableEdit));
            this.txtID = new DevExpress.XtraEditors.TextEdit();
            this.numColumn = new System.Windows.Forms.NumericUpDown();
            this.numRow = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.panelTable = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCacel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).BeginInit();
            this.groupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(111, 14);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(113, 20);
            this.txtID.TabIndex = 0;
            // 
            // numColumn
            // 
            this.numColumn.Location = new System.Drawing.Point(111, 76);
            this.numColumn.Name = "numColumn";
            this.numColumn.Size = new System.Drawing.Size(113, 22);
            this.numColumn.TabIndex = 1;
            this.numColumn.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numColumn.ValueChanged += new System.EventHandler(this.numRow_ValueChanged);
            // 
            // numRow
            // 
            this.numRow.Location = new System.Drawing.Point(111, 44);
            this.numRow.Name = "numRow";
            this.numRow.Size = new System.Drawing.Size(113, 22);
            this.numRow.TabIndex = 1;
            this.numRow.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numRow.ValueChanged += new System.EventHandler(this.numRow_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "编号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "行数：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "列数：";
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.panelTable);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 112);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(573, 205);
            this.groupBox.TabIndex = 3;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "预览";
            // 
            // panelTable
            // 
            this.panelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTable.Location = new System.Drawing.Point(3, 18);
            this.panelTable.Name = "panelTable";
            this.panelTable.Size = new System.Drawing.Size(567, 184);
            this.panelTable.TabIndex = 0;
            this.panelTable.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTable_Paint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtID);
            this.panel1.Controls.Add(this.numColumn);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.numRow);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(573, 112);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCacel);
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 317);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(573, 38);
            this.panel2.TabIndex = 5;
            // 
            // btnCacel
            // 
            this.btnCacel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCacel.Location = new System.Drawing.Point(471, 7);
            this.btnCacel.LookAndFeel.SkinName = "DevExpress Style";
            this.btnCacel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCacel.Name = "btnCacel";
            this.btnCacel.Size = new System.Drawing.Size(87, 27);
            this.btnCacel.TabIndex = 0;
            this.btnCacel.Text = "取消(&C)";
            this.btnCacel.Click += new System.EventHandler(this.btnCacel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(360, 7);
            this.btnOk.LookAndFeel.SkinName = "DevExpress Style";
            this.btnOk.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 27);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "确认(&Q）";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // TableEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 355);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("TableEdit.IconOptions.Image")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TableEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "插入表格";
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).EndInit();
            this.groupBox.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelTable;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton btnCacel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        public DevExpress.XtraEditors.TextEdit txtID;
        public System.Windows.Forms.NumericUpDown numColumn;
        public System.Windows.Forms.NumericUpDown numRow;
    }
}

namespace Clinic.Case.Business
{
    partial class SplitCellEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplitCellEdit));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numCol = new System.Windows.Forms.NumericUpDown();
            this.numRow = new System.Windows.Forms.NumericUpDown();
            this.btnCacel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.labSplit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "列数：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "行数：";
            // 
            // numCol
            // 
            this.numCol.Location = new System.Drawing.Point(67, 15);
            this.numCol.Name = "numCol";
            this.numCol.Size = new System.Drawing.Size(97, 22);
            this.numCol.TabIndex = 1;
            this.numCol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCol.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numRow
            // 
            this.numRow.Location = new System.Drawing.Point(67, 51);
            this.numRow.Name = "numRow";
            this.numRow.Size = new System.Drawing.Size(97, 22);
            this.numRow.TabIndex = 1;
            this.numRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRow.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btnCacel
            // 
            this.btnCacel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCacel.Location = new System.Drawing.Point(105, 114);
            this.btnCacel.LookAndFeel.SkinName = "DevExpress Style";
            this.btnCacel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCacel.Name = "btnCacel";
            this.btnCacel.Size = new System.Drawing.Size(59, 21);
            this.btnCacel.TabIndex = 2;
            this.btnCacel.Text = "取消(&C)";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(16, 113);
            this.btnOk.LookAndFeel.SkinName = "DevExpress Style";
            this.btnOk.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(59, 21);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "确认(&Q）";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labSplit
            // 
            this.labSplit.AutoSize = true;
            this.labSplit.Location = new System.Drawing.Point(64, 88);
            this.labSplit.Name = "labSplit";
            this.labSplit.Size = new System.Drawing.Size(0, 14);
            this.labSplit.TabIndex = 0;
            // 
            // SplitCellEdit
            // 
            this.ClientSize = new System.Drawing.Size(240, 150);
            this.Controls.Add(this.btnCacel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.numRow);
            this.Controls.Add(this.numCol);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labSplit);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("SplitCellEdit.IconOptions.Image")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplitCellEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "拆分表格";
            ((System.ComponentModel.ISupportInitialize)(this.numCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SimpleButton btnCacel;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        public System.Windows.Forms.NumericUpDown numCol;
        public System.Windows.Forms.NumericUpDown numRow;
        private System.Windows.Forms.Label labSplit;
    }
}

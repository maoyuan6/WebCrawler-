
namespace Clinic.Case.Business
{
    partial class TempletEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TempletEdit));
            this.txtCode = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTitle = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbType = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbDept = new DevExpress.XtraEditors.LookUpEdit();
            this.checklist = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.checkShowTitle = new DevExpress.XtraEditors.CheckEdit();
            this.radioType = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checklist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkShowTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(89, 25);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(247, 20);
            this.txtCode.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "模板编码：";
            // 
            // btnCancel
            // 
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.Location = new System.Drawing.Point(261, 252);
            this.btnCancel.LookAndFeel.SkinName = "DevExpress Style";
            this.btnCancel.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消(&E)";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(89, 81);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(247, 20);
            this.txtName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "模板名称：";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(89, 109);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(173, 20);
            this.txtTitle.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "病程标题：";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "所属科室：";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(20, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "模板类型：";
            // 
            // cmbType
            // 
            this.cmbType.Location = new System.Drawing.Point(89, 53);
            this.cmbType.Name = "cmbType";
            this.cmbType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CNAME", "")});
            this.cmbType.Properties.DisplayMember = "CNAME";
            this.cmbType.Properties.NullText = "";
            this.cmbType.Properties.ShowFooter = false;
            this.cmbType.Properties.ShowHeader = false;
            this.cmbType.Properties.ShowLines = false;
            this.cmbType.Properties.ValueMember = "CCODE";
            this.cmbType.Size = new System.Drawing.Size(247, 20);
            this.cmbType.TabIndex = 1;
            // 
            // cmbDept
            // 
            this.cmbDept.Location = new System.Drawing.Point(89, 137);
            this.cmbDept.Name = "cmbDept";
            this.cmbDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDept.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name3")});
            this.cmbDept.Properties.DisplayMember = "Name";
            this.cmbDept.Properties.NullText = "";
            this.cmbDept.Properties.ShowFooter = false;
            this.cmbDept.Properties.ShowHeader = false;
            this.cmbDept.Properties.ShowLines = false;
            this.cmbDept.Properties.ValueMember = "DeptID";
            this.cmbDept.Size = new System.Drawing.Size(247, 20);
            this.cmbDept.TabIndex = 4;
            // 
            // checklist
            // 
            this.checklist.CheckOnClick = true;
            this.checklist.Items.AddRange(new DevExpress.XtraEditors.Controls.CheckedListBoxItem[] {
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(0, "首次病程"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(1, "新页开始"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(2, "新页结束"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(3, "医患沟通"),
            new DevExpress.XtraEditors.Controls.CheckedListBoxItem(4, "页面配置")});
            this.checklist.Location = new System.Drawing.Point(89, 193);
            this.checklist.MultiColumn = true;
            this.checklist.Name = "checklist";
            this.checklist.Size = new System.Drawing.Size(247, 44);
            this.checklist.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.ImageOptions.Image")));
            this.btnOK.Location = new System.Drawing.Point(169, 252);
            this.btnOK.LookAndFeel.SkinName = "DevExpress Style";
            this.btnOK.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "保存(&S)";
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // checkShowTitle
            // 
            this.checkShowTitle.Location = new System.Drawing.Point(268, 109);
            this.checkShowTitle.Name = "checkShowTitle";
            this.checkShowTitle.Properties.Caption = "显示标题";
            this.checkShowTitle.Size = new System.Drawing.Size(75, 20);
            this.checkShowTitle.TabIndex = 6;
            // 
            // radioType
            // 
            this.radioType.Location = new System.Drawing.Point(89, 163);
            this.radioType.Name = "radioType";
            this.radioType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "科室模板"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "个人模板")});
            this.radioType.Size = new System.Drawing.Size(247, 24);
            this.radioType.TabIndex = 4;
            // 
            // TempletEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 298);
            this.Controls.Add(this.radioType);
            this.Controls.Add(this.checkShowTitle);
            this.Controls.Add(this.checklist);
            this.Controls.Add(this.cmbDept);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TempletEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "模板属性";
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checklist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkShowTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.TextEdit txtCode;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        public DevExpress.XtraEditors.TextEdit txtName;
        private System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.TextEdit txtTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        public DevExpress.XtraEditors.LookUpEdit cmbType;
        public DevExpress.XtraEditors.LookUpEdit cmbDept;
        public DevExpress.XtraEditors.CheckEdit checkShowTitle;
        public DevExpress.XtraEditors.CheckedListBoxControl checklist;
        public DevExpress.XtraEditors.RadioGroup radioType;
    }
}
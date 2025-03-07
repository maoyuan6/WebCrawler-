
namespace Clinic.Case.Business
{
    partial class HeadEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HeadEdit));
            this.gridHeadInfo = new DevExpress.XtraGrid.GridControl();
            this.gvHeadInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridHeadInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeadInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // gridHeadInfo
            // 
            this.gridHeadInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridHeadInfo.Location = new System.Drawing.Point(0, 0);
            this.gridHeadInfo.MainView = this.gvHeadInfo;
            this.gridHeadInfo.Name = "gridHeadInfo";
            this.gridHeadInfo.Size = new System.Drawing.Size(334, 363);
            this.gridHeadInfo.TabIndex = 0;
            this.gridHeadInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHeadInfo});
            // 
            // gvHeadInfo
            // 
            this.gvHeadInfo.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(158)))), ((int)(((byte)(179)))));
            this.gvHeadInfo.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvHeadInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gvHeadInfo.GridControl = this.gridHeadInfo;
            this.gvHeadInfo.Name = "gvHeadInfo";
            this.gvHeadInfo.OptionsView.ShowGroupPanel = false;
            this.gvHeadInfo.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "模板名称";
            this.gridColumn1.FieldName = "NAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // HeadEdit
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 363);
            this.Controls.Add(this.gridHeadInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("HeadEdit.IconOptions.Image")));
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.LookAndFeel.UseWindowsXPTheme = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HeadEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "页眉管理器";
            ((System.ComponentModel.ISupportInitialize)(this.gridHeadInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHeadInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridHeadInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvHeadInfo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}
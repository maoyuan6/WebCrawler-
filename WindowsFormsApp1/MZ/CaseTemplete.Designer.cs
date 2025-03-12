
namespace Clinic.Case.Business
{
    partial class CaseTemplete
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaseTemplete));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.treeInfo = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnModeUp = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSetPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCopyMode = new System.Windows.Forms.ToolStripMenuItem();
            this.btnstick = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDeleteMode = new System.Windows.Forms.ToolStripMenuItem();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDept = new DevExpress.XtraEditors.SimpleButton();
            this.btnPerson = new DevExpress.XtraEditors.SimpleButton();
            this.searchControl1 = new DevExpress.XtraEditors.SearchControl();
            this.panelMain = new DevExpress.XtraEditors.PanelControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.navBarControl1 = new DevExpress.XtraNavBar.NavBarControl();
            this.navBarGroupBasicInfo = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroupControlContainer3 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.treeListBasicInfo = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.navBarGroupControlContainer2 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.treeListImage = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.navBarGroupControlContainer1 = new DevExpress.XtraNavBar.NavBarGroupControlContainer();
            this.treeListSymbol = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.navBarGroup_Image = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarGroup2 = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeInfo)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).BeginInit();
            this.navBarControl1.SuspendLayout();
            this.navBarGroupControlContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListBasicInfo)).BeginInit();
            this.navBarGroupControlContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListImage)).BeginInit();
            this.navBarGroupControlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListSymbol)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.treeInfo);
            this.panelControl1.Controls.Add(this.panel1);
            this.panelControl1.Controls.Add(this.searchControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(264, 724);
            this.panelControl1.TabIndex = 0;
            // 
            // treeInfo
            // 
            this.treeInfo.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
            this.treeInfo.ContextMenuStrip = this.contextMenuStrip1;
            this.treeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeInfo.HtmlImages = this.imageCollection1;
            this.treeInfo.KeyFieldName = "CODE";
            this.treeInfo.Location = new System.Drawing.Point(0, 20);
            this.treeInfo.Name = "treeInfo";
            this.treeInfo.OptionsBehavior.Editable = false;
            this.treeInfo.OptionsMenu.ShowExpandCollapseItems = false;
            this.treeInfo.OptionsSelection.SelectNodesOnRightClick = true;
            this.treeInfo.OptionsView.ShowColumns = false;
            this.treeInfo.OptionsView.ShowIndicator = false;
            this.treeInfo.OptionsView.ShowVertLines = false;
            this.treeInfo.ParentFieldName = "FATHERCODE";
            this.treeInfo.SelectImageList = this.imageCollection1;
            this.treeInfo.Size = new System.Drawing.Size(264, 677);
            this.treeInfo.TabIndex = 14;
            this.treeInfo.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeInfo_FocusedNodeChanged);
            this.treeInfo.DoubleClick += new System.EventHandler(this.treeInfo_DoubleClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "NAME";
            this.treeListColumn1.FieldName = "NAME";
            this.treeListColumn1.MinWidth = 49;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnModeUp,
            this.btnSetPerson,
            this.btnCopyMode,
            this.btnstick,
            this.btnDeleteMode});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 114);
            // 
            // btnModeUp
            // 
            this.btnModeUp.Name = "btnModeUp";
            this.btnModeUp.Size = new System.Drawing.Size(160, 22);
            this.btnModeUp.Text = "修改";
            this.btnModeUp.Click += new System.EventHandler(this.btnModeUp_Click);
            // 
            // btnSetPerson
            // 
            this.btnSetPerson.Name = "btnSetPerson";
            this.btnSetPerson.Size = new System.Drawing.Size(160, 22);
            this.btnSetPerson.Text = "修改为个人模板";
            this.btnSetPerson.Click += new System.EventHandler(this.btnSetPerson_Click);
            // 
            // btnCopyMode
            // 
            this.btnCopyMode.Name = "btnCopyMode";
            this.btnCopyMode.Size = new System.Drawing.Size(160, 22);
            this.btnCopyMode.Text = "复制模板";
            this.btnCopyMode.Click += new System.EventHandler(this.btnCopyMode_Click);
            // 
            // btnstick
            // 
            this.btnstick.Name = "btnstick";
            this.btnstick.Size = new System.Drawing.Size(160, 22);
            this.btnstick.Text = "粘贴模板";
            this.btnstick.Click += new System.EventHandler(this.btnstick_Click);
            // 
            // btnDeleteMode
            // 
            this.btnDeleteMode.Name = "btnDeleteMode";
            this.btnDeleteMode.Size = new System.Drawing.Size(160, 22);
            this.btnDeleteMode.Text = "删除模板";
            this.btnDeleteMode.Click += new System.EventHandler(this.btnDeleteMode_Click);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "临床路径科室.png");
            this.imageCollection1.Images.SetKeyName(1, "文件夹 (1).png");
            this.imageCollection1.Images.SetKeyName(2, "购物清单.png");
            this.imageCollection1.Images.SetKeyName(3, "单选-选中.png");
            this.imageCollection1.Images.SetKeyName(4, "科室1.png");
            this.imageCollection1.Images.SetKeyName(5, "个人_2.png");
            this.imageCollection1.Images.SetKeyName(6, "填报.png");
            this.imageCollection1.Images.SetKeyName(7, "文件夹 (2).png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDept);
            this.panel1.Controls.Add(this.btnPerson);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 697);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 27);
            this.panel1.TabIndex = 16;
            // 
            // btnDept
            // 
            this.btnDept.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDept.Appearance.Options.UseFont = true;
            this.btnDept.AppearanceHovered.BackColor = System.Drawing.Color.Gray;
            this.btnDept.AppearanceHovered.Options.UseBackColor = true;
            this.btnDept.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnDept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDept.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDept.ImageOptions.Image")));
            this.btnDept.Location = new System.Drawing.Point(0, 0);
            this.btnDept.Margin = new System.Windows.Forms.Padding(0);
            this.btnDept.Name = "btnDept";
            this.btnDept.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btnDept.Size = new System.Drawing.Size(129, 27);
            this.btnDept.TabIndex = 1;
            this.btnDept.Text = "科室模板";
            this.btnDept.Click += new System.EventHandler(this.btnDept_Click);
            // 
            // btnPerson
            // 
            this.btnPerson.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPerson.Appearance.Options.UseFont = true;
            this.btnPerson.AppearanceHovered.BackColor = System.Drawing.Color.Gray;
            this.btnPerson.AppearanceHovered.Options.UseBackColor = true;
            this.btnPerson.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnPerson.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPerson.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnPerson.ImageOptions.Image")));
            this.btnPerson.Location = new System.Drawing.Point(129, 0);
            this.btnPerson.Margin = new System.Windows.Forms.Padding(0);
            this.btnPerson.Name = "btnPerson";
            this.btnPerson.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            this.btnPerson.Size = new System.Drawing.Size(135, 27);
            this.btnPerson.TabIndex = 2;
            this.btnPerson.Text = "个人模板";
            this.btnPerson.Click += new System.EventHandler(this.btnPerson_Click);
            // 
            // searchControl1
            // 
            this.searchControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchControl1.Location = new System.Drawing.Point(0, 0);
            this.searchControl1.Name = "searchControl1";
            this.searchControl1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.searchControl1.Size = new System.Drawing.Size(264, 20);
            this.searchControl1.TabIndex = 1;
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(264, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(747, 724);
            this.panelMain.TabIndex = 2;
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel1.ID = new System.Guid("3d2ccbed-701c-4386-a92a-f4b648a46526");
            this.dockPanel1.Location = new System.Drawing.Point(1011, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(196, 200);
            this.dockPanel1.Size = new System.Drawing.Size(196, 724);
            this.dockPanel1.Text = "工具箱";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.navBarControl1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 26);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(189, 695);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // navBarControl1
            // 
            this.navBarControl1.ActiveGroup = this.navBarGroupBasicInfo;
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer3);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer2);
            this.navBarControl1.Controls.Add(this.navBarGroupControlContainer1);
            this.navBarControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navBarControl1.Groups.AddRange(new DevExpress.XtraNavBar.NavBarGroup[] {
            this.navBarGroup_Image,
            this.navBarGroup2,
            this.navBarGroupBasicInfo});
            this.navBarControl1.Items.AddRange(new DevExpress.XtraNavBar.NavBarItem[] {
            this.navBarItem1});
            this.navBarControl1.Location = new System.Drawing.Point(0, 0);
            this.navBarControl1.Name = "navBarControl1";
            this.navBarControl1.OptionsNavPane.ExpandedWidth = 189;
            this.navBarControl1.OptionsNavPane.ShowExpandButton = false;
            this.navBarControl1.OptionsNavPane.ShowOverflowButton = false;
            this.navBarControl1.OptionsNavPane.ShowOverflowPanel = false;
            this.navBarControl1.Size = new System.Drawing.Size(189, 695);
            this.navBarControl1.SkinExplorerBarViewScrollStyle = DevExpress.XtraNavBar.SkinExplorerBarViewScrollStyle.ScrollBar;
            this.navBarControl1.StoreDefaultPaintStyleName = true;
            this.navBarControl1.TabIndex = 0;
            this.navBarControl1.Text = "个人模板";
            this.navBarControl1.ActiveGroupChanged += new DevExpress.XtraNavBar.NavBarGroupEventHandler(this.navBarControl1_ActiveGroupChanged);
            // 
            // navBarGroupBasicInfo
            // 
            this.navBarGroupBasicInfo.Caption = "基本信息";
            this.navBarGroupBasicInfo.ControlContainer = this.navBarGroupControlContainer3;
            this.navBarGroupBasicInfo.Expanded = true;
            this.navBarGroupBasicInfo.GroupClientHeight = 80;
            this.navBarGroupBasicInfo.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroupBasicInfo.Name = "navBarGroupBasicInfo";
            // 
            // navBarGroupControlContainer3
            // 
            this.navBarGroupControlContainer3.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.navBarGroupControlContainer3.Appearance.Options.UseBackColor = true;
            this.navBarGroupControlContainer3.Controls.Add(this.treeListBasicInfo);
            this.navBarGroupControlContainer3.Name = "navBarGroupControlContainer3";
            this.navBarGroupControlContainer3.Size = new System.Drawing.Size(189, 80);
            this.navBarGroupControlContainer3.TabIndex = 2;
            // 
            // treeListBasicInfo
            // 
            this.treeListBasicInfo.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn4});
            this.treeListBasicInfo.ContextMenuStrip = this.contextMenuStrip1;
            this.treeListBasicInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListBasicInfo.HtmlImages = this.imageCollection1;
            this.treeListBasicInfo.KeyFieldName = "CODE";
            this.treeListBasicInfo.Location = new System.Drawing.Point(0, 0);
            this.treeListBasicInfo.Name = "treeListBasicInfo";
            this.treeListBasicInfo.OptionsBehavior.Editable = false;
            this.treeListBasicInfo.OptionsMenu.ShowExpandCollapseItems = false;
            this.treeListBasicInfo.OptionsView.ShowColumns = false;
            this.treeListBasicInfo.OptionsView.ShowIndicator = false;
            this.treeListBasicInfo.OptionsView.ShowVertLines = false;
            this.treeListBasicInfo.ParentFieldName = "FATHERCODE";
            this.treeListBasicInfo.SelectImageList = this.imageCollection1;
            this.treeListBasicInfo.Size = new System.Drawing.Size(189, 80);
            this.treeListBasicInfo.TabIndex = 17;
            this.treeListBasicInfo.DoubleClick += new System.EventHandler(this.treeListBasicInfo_DoubleClick);
            this.treeListBasicInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeListBasicInfo_MouseDown);
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.Caption = "NAME";
            this.treeListColumn4.FieldName = "NAME";
            this.treeListColumn4.MinWidth = 49;
            this.treeListColumn4.Name = "treeListColumn4";
            this.treeListColumn4.Visible = true;
            this.treeListColumn4.VisibleIndex = 0;
            // 
            // navBarGroupControlContainer2
            // 
            this.navBarGroupControlContainer2.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.navBarGroupControlContainer2.Appearance.Options.UseBackColor = true;
            this.navBarGroupControlContainer2.Controls.Add(this.treeListImage);
            this.navBarGroupControlContainer2.Name = "navBarGroupControlContainer2";
            this.navBarGroupControlContainer2.Size = new System.Drawing.Size(189, 80);
            this.navBarGroupControlContainer2.TabIndex = 1;
            // 
            // treeListImage
            // 
            this.treeListImage.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn3});
            this.treeListImage.ContextMenuStrip = this.contextMenuStrip1;
            this.treeListImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListImage.HtmlImages = this.imageCollection1;
            this.treeListImage.KeyFieldName = "CODE";
            this.treeListImage.Location = new System.Drawing.Point(0, 0);
            this.treeListImage.Name = "treeListImage";
            this.treeListImage.OptionsBehavior.Editable = false;
            this.treeListImage.OptionsMenu.ShowExpandCollapseItems = false;
            this.treeListImage.OptionsView.ShowColumns = false;
            this.treeListImage.OptionsView.ShowIndicator = false;
            this.treeListImage.OptionsView.ShowVertLines = false;
            this.treeListImage.ParentFieldName = "FATHERCODE";
            this.treeListImage.Size = new System.Drawing.Size(189, 80);
            this.treeListImage.StateImageList = this.imageCollection1;
            this.treeListImage.TabIndex = 16;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "NAME";
            this.treeListColumn3.FieldName = "NAME";
            this.treeListColumn3.MinWidth = 49;
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.Visible = true;
            this.treeListColumn3.VisibleIndex = 0;
            // 
            // navBarGroupControlContainer1
            // 
            this.navBarGroupControlContainer1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.navBarGroupControlContainer1.Appearance.Options.UseBackColor = true;
            this.navBarGroupControlContainer1.Controls.Add(this.treeListSymbol);
            this.navBarGroupControlContainer1.Name = "navBarGroupControlContainer1";
            this.navBarGroupControlContainer1.Size = new System.Drawing.Size(189, 80);
            this.navBarGroupControlContainer1.TabIndex = 0;
            // 
            // treeListSymbol
            // 
            this.treeListSymbol.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.treeListSymbol.ContextMenuStrip = this.contextMenuStrip1;
            this.treeListSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListSymbol.HtmlImages = this.imageCollection1;
            this.treeListSymbol.KeyFieldName = "CODE";
            this.treeListSymbol.Location = new System.Drawing.Point(0, 0);
            this.treeListSymbol.Name = "treeListSymbol";
            this.treeListSymbol.OptionsBehavior.Editable = false;
            this.treeListSymbol.OptionsMenu.ShowExpandCollapseItems = false;
            this.treeListSymbol.OptionsView.ShowColumns = false;
            this.treeListSymbol.OptionsView.ShowIndicator = false;
            this.treeListSymbol.OptionsView.ShowVertLines = false;
            this.treeListSymbol.ParentFieldName = "FATHERCODE";
            this.treeListSymbol.Size = new System.Drawing.Size(189, 80);
            this.treeListSymbol.StateImageList = this.imageCollection1;
            this.treeListSymbol.TabIndex = 15;
            this.treeListSymbol.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeListBasicInfo_MouseDown);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "NAME";
            this.treeListColumn2.FieldName = "NAME";
            this.treeListColumn2.MinWidth = 49;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            // 
            // navBarGroup_Image
            // 
            this.navBarGroup_Image.Caption = "特殊符号";
            this.navBarGroup_Image.ControlContainer = this.navBarGroupControlContainer1;
            this.navBarGroup_Image.Expanded = true;
            this.navBarGroup_Image.GroupClientHeight = 80;
            this.navBarGroup_Image.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup_Image.Name = "navBarGroup_Image";
            // 
            // navBarGroup2
            // 
            this.navBarGroup2.Caption = "医学图片";
            this.navBarGroup2.ControlContainer = this.navBarGroupControlContainer2;
            this.navBarGroup2.Expanded = true;
            this.navBarGroup2.GroupClientHeight = 80;
            this.navBarGroup2.GroupStyle = DevExpress.XtraNavBar.NavBarGroupStyle.ControlContainer;
            this.navBarGroup2.Name = "navBarGroup2";
            // 
            // navBarItem1
            // 
            this.navBarItem1.Caption = "navBarItem1";
            this.navBarItem1.Name = "navBarItem1";
            // 
            // CaseTemplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.dockPanel1);
            this.Name = "CaseTemplete";
            this.Size = new System.Drawing.Size(1207, 724);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeInfo)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.searchControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.navBarControl1)).EndInit();
            this.navBarControl1.ResumeLayout(false);
            this.navBarGroupControlContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListBasicInfo)).EndInit();
            this.navBarGroupControlContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListImage)).EndInit();
            this.navBarGroupControlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListSymbol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelMain;
        private DevExpress.XtraEditors.SearchControl searchControl1;
        private DevExpress.XtraTreeList.TreeList treeInfo;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnModeUp;
        private System.Windows.Forms.ToolStripMenuItem btnSetPerson;
        private System.Windows.Forms.ToolStripMenuItem btnCopyMode;
        private System.Windows.Forms.ToolStripMenuItem btnDeleteMode;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
        private DevExpress.XtraNavBar.NavBarControl navBarControl1;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup_Image;
        private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer1;
        private DevExpress.XtraTreeList.TreeList treeListSymbol;
        private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer3;
        private DevExpress.XtraTreeList.TreeList treeListBasicInfo;
        private DevExpress.XtraNavBar.NavBarGroupControlContainer navBarGroupControlContainer2;
        private DevExpress.XtraTreeList.TreeList treeListImage;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroup2;
        private DevExpress.XtraNavBar.NavBarGroup navBarGroupBasicInfo;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnDept;
        private DevExpress.XtraEditors.SimpleButton btnPerson;
        private System.Windows.Forms.ToolStripMenuItem btnstick;
    }
}
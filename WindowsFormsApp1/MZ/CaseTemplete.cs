using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Dapper;
using WindowsFormsApp1.Entity.EMR;
using Clinic.Case.Interface;
using DCSoft.Writer.Dom;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using EMR;
using HPSoft.FrameWork;
using HPSoft.FrameWork.WinForm;
using WinnerHIS.Common;
using WinnerHIS.Integral.Personnel.DAL.Interface;
using WinnerMIS.Integral.Personnel;
using WinnerSoft.Collections;
using WinnerSoft.Data.Access;

namespace Clinic.Case.Business
{
    public partial class CaseTemplete : BaseExplorerControl
    {
        CaseFrm caseFrm;
        public CaseTemplete()
        {
            InitializeComponent();
            caseFrm = new CaseFrm(this);
            caseFrm.Dock = DockStyle.Fill;
            this.panelMain.Controls.Add(caseFrm); 
            Initialize();
        }
        DapperHelper EMRContext = new DapperHelper("EMR");
        DapperHelper BaseDataContext = new DapperHelper("BaseData");
        DepartmentRepositories departmentRepositories = new DepartmentRepositories();
        EmrTempletRepositories emrTempletRepositories = new EmrTempletRepositories();
        SymbolRepositories  symbolRepositories = new SymbolRepositories();
        InputInfoRepositories inputInfoRepositories = new InputInfoRepositories();
        DictCatalogRepositories dictCatalogRepositories = new DictCatalogRepositories();
        #region 重写 ExplorerControl 属性/方法

        /// <summary>
        /// 重写Windows 扩展 ExplorerControl 属性Description。
        /// </summary>
        public override string Description
        {
            get
            {
                return "门诊医生工作台";
            }
        }

        /// <summary>
        /// 重写Windows 扩展 ExplorerControl 属性Guid属性。
        /// </summary>
        public override Guid Guid
        {
            get
            {
                return new System.Guid("869CFDD2-FCBD-43DE-829C-84480F12E4A3");
            }
        }

        public override string Group
        {
            get
            {
                return "基础设置";
            }
        }

        /// <summary>
        /// 重写Windows 扩展 ExplorerControl 属性ObjectName属性。
        /// </summary>
        public override string ObjectName
        {
            get
            {
                return "病历模板";
            }
        }

        /// <summary>
        /// 重写Windows 扩展 ExplorerControl 属性ObjectName属性。
        /// </summary>
        public override string ModuleName
        {
            get
            {
                return "病历模板";
            }
        }

        /// <summary>
        /// 重写Windows 扩展 IS.Windows.UI.Forms.IModuleForm 属性Run方法。
        /// </summary>
        /// <param name="parameters">运行参数。</param>
        public override IPlugIn Run(params object[] parameters)
        {
            if (this.Account.OriginalID == "")
            {
                MessageBox.Show("对不起，当前账户没有有效的员工原型信息，无法进行业务操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            SetWaitDialogCaption("界面正在初始化...");

            this.Initialize();
            HideWaitDialog();//隐藏等待窗口
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            return plg;
        }
        #endregion
        List<DictCatalog> dictList;
        List<Department> deptlist;
        List<EmrTemplet> tempList;
        int tempType = 0;
        private void Initialize()
        {

            dictList = dictCatalogRepositories.GeDictCatalogList();

            //dictList = WinnerHIS.Clinic.Case.DAL.Interface.DALHelper.DALManager.CreateDICT_CATALOGList();
            //dictList.Session = this.Session;
            //dictList.GetAllList();
            //deptlist = WinnerHIS.Integral.Personnel.DAL.Interface.DALHelper.DALManager.CreateDepartmentList();
            //deptlist.Session = this.Session;

            if (true)
            {
                deptlist = departmentRepositories.GeDepartmentList();
                tempType = 0;
            }
            else
            {
                string sql = "select * from BaseData.dbo.VW_EMPLOYEEALL where EMPID = " + WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
                //IConnection dbcn = WinnerHIS.Common.ContextHelper.Context.Container.GetComponentInstances(typeof(IConnection))[0] as IConnection;
                System.Data.DataTable table = BaseDataContext.QueryToDataTable(sql);
                // System.Data.DataTable table = (System.Data.DataTable)dbcn.CreateAccessor().Query(sql.ToString(), WinnerSoft.Data.Access.ResultType.DataTable);
                foreach (DataRow dataRow in table.Rows)
                {
                    Department dept = new Department();
                    dept.ID = Convert.ToInt32(dataRow["DEPTID"]);
                    dept.NAME = WinnerHIS.Common.DataConvertHelper.GetDeptName(Convert.ToInt32(dataRow["DEPTID"]));
                    deptlist.Add(dept);
                }
                tempType = 1;
            }


            tempList = emrTempletRepositories.GeEmrTempletList();

            LoadTreeData(tempType);
            LoadTreeBasicInfo();
            LoadTreeSymbols();
            this.navBarControl1.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
        }

        private void LoadTreeSymbols()
        {

            List<Symbol> symList = symbolRepositories.GetSymbolList();
                 
            //ISYMBOLSList symList = WinnerHIS.Clinic.Case.DAL.Interface.DALHelper.DALManager.CreateSYMBOLSList();
            //symList.Session = this.Session;
            //symList.Query();
            foreach (Symbol item in symList)
            {
                TreeListNode node = treeListSymbol.AppendNode(null, -1);
                node.SetValue("ID", item.ID);
                node.SetValue("NAME", item.RTF);
                node.ImageIndex = 6;
                node.SelectImageIndex = 3;
                node.Tag = item;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
        }
        void LoadTreeBasicInfo()
        {
            var infoList = inputInfoRepositories.GeInputInfoList();

            //IInputInfoList infoList = WinnerHIS.Clinic.Case.DAL.Interface.DALHelper.DALManager.CreateInputInfoList();
            //infoList.Session = this.Session;
            //infoList.Query();
            foreach (InputInfo item in infoList)
            {
                TreeListNode node = treeListBasicInfo.AppendNode(null, -1);
                node.SetValue("ID", item.FileName);
                node.SetValue("NAME", item.BackText);
                node.ImageIndex = 6;
                node.SelectImageIndex = 3;
                node.Tag = item;
            }
        }
        void LoadTreeImage()
        {

        }
        void LoadTreeData(int attr)
        {
            treeInfo.Nodes.Clear();
            TreeListNode titleNode = treeInfo.AppendNode(null, -1);
            titleNode.SetValue("ID", 0);
            if (attr == 0)
            {
                titleNode.SetValue("NAME", "科室模板");
                titleNode.ImageIndex = 4;
                treeInfo.Tag = 0;
                caseFrm.attr = 0;
            }
            else
            {
                titleNode.SetValue("NAME", "个人模板");
                titleNode.ImageIndex = 5;
                treeInfo.Tag = 1;
                caseFrm.attr = 1;
            }
            foreach (Department dept in deptlist)
            {
                TreeListNode deptNode = treeInfo.AppendNode(null, titleNode);
                deptNode.SetValue("ID", dept.ID);
                deptNode.SetValue("NAME", dept.NAME);
                deptNode.ImageIndex = 0;
                deptNode.Tag = dept.ID;

                foreach (DictCatalog dict in dictList)
                {
                    TreeListNode dictNode = treeInfo.AppendNode(null, deptNode);
                    dictNode.SetValue("ID", dict.CCODE);
                    dictNode.SetValue("NAME", dict.CNAME);
                    dictNode.ImageIndex = 1;
                    dictNode.SelectImageIndex = 7;
                    dictNode.Tag = dict.CCODE;
                    foreach (EmrTemplet templet in tempList)
                    {
                        if (templet.DEPT_ID == dept.ID && templet.MR_CLASS == dict.CCODE && templet.MR_ATTR == attr)
                        {
                            // 取消限制
                            //if ((int)ContextHelper.Employee.Type!=8)
                            //{
                            //    if (templet.CREATOR_ID != ContextHelper.Employee.EmployeeID)
                            //    {
                            //        continue;
                            //    }
                            //}
                            TreeListNode tempNode = treeInfo.AppendNode(null, dictNode);
                            tempNode.SetValue("ID", templet.CREATOR_ID);
                            tempNode.SetValue("NAME", templet.MR_NAME);
                            tempNode.ImageIndex = 2;
                            tempNode.SelectImageIndex = 3;
                            tempNode.Tag = templet;
                        }
                    }
                }
            }
            this.treeInfo.EndUpdate();
            this.treeInfo.ExpandAll();
        }

        public void AddTreeNode(IEMRTEMPLET templet)
        {
            //tempList.AddEntity(templet);
            LoadTreeData(templet.MR_ATTR);
        }
        public void AddTreeNode(EmrTemplet templet)
        {
            tempList.Add(templet);
            LoadTreeData(templet.MR_ATTR ?? 0);
        }
        private void treeInfo_DoubleClick(object sender, EventArgs e)
        {
            EmrTemplet templet = this.treeInfo.FocusedNode.Tag as EmrTemplet;
            if (templet != null)
            {
                caseFrm.isEdit = true;
                caseFrm.myWriterControl.XMLText = templet.XML_DOC_NEW;
                caseFrm.MedicalCodeEditValue(templet.XYZhenDuan);
                caseFrm.ChineseMedicineIcdEditValue(templet.ZYZhenDuan);
                caseFrm.newEmpTemplet = templet;
                caseFrm.editSatus = 0;
            }
        }

        private void treeInfo_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            TreeListNode node = this.treeInfo.FocusedNode;
            if (node == null)
            {
                return;
            }
            if (node.Level == 3)
            {
                caseFrm.mrClass = node.ParentNode.Tag.ToString();
                caseFrm.deptid = Convert.ToInt32(node.ParentNode.ParentNode.Tag);
            }
            if (node.Level == 2)
            {
                caseFrm.mrClass = node.Tag.ToString();
                caseFrm.deptid = Convert.ToInt32(node.ParentNode.Tag);
            }
            if (node.Level == 1)
            {
                caseFrm.deptid = Convert.ToInt32(node.Tag);
            }
        }
         
        private void btnModeUp_Click(object sender, EventArgs e)
        {
            TreeListNode node = this.treeInfo.FocusedNode;
            if (node.Tag is EmrTemplet)
            {
                EmrTemplet emrtemplet = node.Tag as EmrTemplet;
                TempletEdit tempFrm = new TempletEdit(emrtemplet);
                if (tempFrm.ShowDialog() == DialogResult.OK)
                {
                    LoadTreeData(Convert.ToInt32(treeInfo.Tag));
                    MessageBox.Show("模板修改成功！");
                } 
            }
        }

        private void btnDeleteMode_Click(object sender, EventArgs e)
        {
            TreeListNode node = this.treeInfo.FocusedNode;
            if (node.Tag is IEMRTEMPLET)
            {
                if ((int)ContextHelper.Employee.Type != 8)
                {
                    MessageBox.Show("该账号没有权限删除，请通知管理员进行删除！");
                    return;
                }



                //IEMRTEMPLET emrtemplet = node.Tag as IEMRTEMPLET;
                //emrtemplet.Refresh();
                //emrtemplet.Delete();
                //tempList.DeleteEntity(emrtemplet);
                //treeInfo.Nodes.Remove(node);
                //for (int i = 0; i < tempList.Rows.Count; i++)
                //{
                //    if ((tempList.Rows[i] as IEMRTEMPLET).TEMPLET_ID == emrtemplet.TEMPLET_ID)
                //    {
                //        tempList.RemoveAt(i);
                //    }
                //}v
            }
        }


        private void treeListBasicInfo_DoubleClick(object sender, EventArgs e)
        {
            TreeListNode node = treeListBasicInfo.FocusedNode;
            if (node != null)
            {
                IInputInfo inputInfo = node.Tag as IInputInfo;
                XTextInputFieldElement field = new XTextInputFieldElement();
                field.Name = inputInfo.FileName;
                field.BackgroundText = inputInfo.BackText;
                caseFrm.myWriterControl.ExecuteCommand("InsertInputField", false, field);
            }
        }

        private void treeListBasicInfo_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = ((TreeList)sender).CalcHitInfo(e.Location);
                ((TreeList)sender).FocusedNode = hitInfo.Node;
                TreeList tree = (TreeList)sender;
                DoDragDorpInfo(tree);
            }
            catch (Exception ex)
            {
                HPSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        private void DoDragDorpInfo(TreeList tree)
        {
            try
            {
                if (tree.FocusedNode == null) return;
                if (tree.Name == treeListImage.Name)//插入图片
                {
                    //DataRow row = (DataRow)tree.FocusedNode.Tag;
                    //byte[] img = m_SqlManger.GetImage(row["ID"].ToString());
                    //KeyValuePair<string, object> data = new KeyValuePair<string, object>("ZYTextImage", img);
                    //tree.DoDragDrop(data, DragDropEffects.All);
                }
                else if (tree.Name == treeListBasicInfo.Name)//插入基础信息
                {
                    IInputInfo inputInfo = tree.FocusedNode.Tag as IInputInfo;
                    XTextInputFieldElement field = new XTextInputFieldElement();
                    field.Name = inputInfo.FileName;
                    field.BackgroundText = inputInfo.BackText;
                    KeyValuePair<string, object> data = new KeyValuePair<string, object>(field.Name, field);
                    treeListBasicInfo.DoDragDrop(field, DragDropEffects.Copy);
                }
                else if (tree.Name == treeListSymbol.Name)
                {
                    ISYMBOLS inputInfo = tree.FocusedNode.Tag as ISYMBOLS;
                    treeListSymbol.DoDragDrop(inputInfo.RTF, DragDropEffects.Copy);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void navBarControl1_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            try
            {
                if (e.Group == navBarGroup_Image)//特殊字符
                {
                    if (treeListImage.Nodes.Count < 1)
                    {
                        //treeListImage.BeginUnboundLoad();
                        //foreach (DataRow row in m_SqlManger.ImageGallery.Rows)
                        //{
                        //    TreeListNode node = treeListImage.AppendNode(new object[] { row["名称"] }, null);
                        //    node.Tag = row;
                        //}
                        //treeListImage.EndUnboundLoad();
                    }
                }
            }
            catch (Exception ex)
            {
                HPSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        //科室
        private void btnDept_Click(object sender, EventArgs e)
        {
            LoadTreeData(0);
        }
        //个人
        private void btnPerson_Click(object sender, EventArgs e)
        {
            LoadTreeData(1);
        }

        private void btnSetPerson_Click(object sender, EventArgs e)
        {
            TreeListNode node = this.treeInfo.FocusedNode;
            if (node.Tag is IEMRTEMPLET)
            {
                IEMRTEMPLET emrtemplet = node.Tag as IEMRTEMPLET;
                if (emrtemplet.CREATOR_ID != WinnerHIS.Common.ContextHelper.Employee.EmployeeID && (int)WinnerHIS.Common.ContextHelper.Employee.Type != 8)
                {
                    MessageBox.Show("该模板不是该账号创建的模板，无法设为个人模板？");
                    return;
                }
                if (MessageBox.Show("确认把改模板设为个人模板吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    emrtemplet.Refresh();
                    emrtemplet.MR_ATTR = 1;
                    emrtemplet.Update();
                    LoadTreeData(emrtemplet.MR_ATTR);
                    MessageBox.Show("模板修改成功！");
                }
            }
        }
        IEMRTEMPLET copyTemplet;//复制模板
        private void btnCopyMode_Click(object sender, EventArgs e)
        {
            IEMRTEMPLET templet = this.treeInfo.FocusedNode.Tag as IEMRTEMPLET;
            if (templet != null)
            {
                copyTemplet = templet;
                MessageBox.Show("已复制到剪贴板，请选择科室进行粘贴");
            }
        }

        private void btnstick_Click(object sender, EventArgs e)
        {
            IEMRTEMPLET templet = WinnerHIS.Clinic.Case.DAL.Interface.DALHelper.DALManager.CreataEMRTEMPLET();
            templet.Session = this.Session;
            for (int i = 0; i < copyTemplet.PropertyCount; i++)
            {
                WinnerSoft.Data.ORM.Property prop = copyTemplet.GetProperty(i);

                if (templet.ContainsProperty(prop.Name))
                {
                    templet[prop.Name] = copyTemplet[i];
                }
            }
            templet.TEMPLET_ID = templet.MaxID();
            templet.DEPT_ID = caseFrm.deptid;
            templet.MR_CLASS = caseFrm.mrClass;
            templet.CREATOR_ID = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
            templet.MR_ATTR = caseFrm.attr;
            templet.Insert();
            AddTreeNode(templet);

        }
    }
}

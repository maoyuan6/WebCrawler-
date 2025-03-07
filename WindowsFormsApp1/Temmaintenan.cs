using DevExpress.XtraTreeList.Nodes;
using HIS.Clinic.DoctorWorkstation.Medical;
using HPSoft.Core;
using HPSoft.FrameWork;
using HPSoft.FrameWork.WinForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinnerHIS.Clinic.DAL.Interface;
using WinnerHIS.Common;
using WinnerHIS.Integral.AP.DAL.Interface;
using WinnerMIS.Integral.Personnel.DAL.Interface;
using WinnerSoft.Data.Access;

namespace HIS.Clinic.DoctorWorkstation
{
    public partial class Temmaintenan :UserControl//: WinnerHIS.Common.BaseExplorerControl
    {

        /// <summary>
        /// 左边部门的数据集
        /// </summary>
        DataTable dtDept;

        /// <summary>
        /// 首页
        /// </summary>
        public MzContent mzcontent = null;

        public IMedicalPatern medical;

        /// <summary>
        /// 病历是否可以用
        /// </summary>
        public bool MedicalEnable = true;


        public Temmaintenan()
        {
            InitializeComponent();
        }

        #region 系统会话相关

        /// <summary>
        /// 获取或设置当前登录系统的系统账户。
        /// </summary>
        [Browsable(false)]
        public WinnerExplorer.Users.IAccount Account
        {
            get
            {
                return ContextHelper.Account;
            }
        }

        /// <summary>
        /// 获取或设置当前登录账号与系统的会话。
        /// </summary>
        [Browsable(false)]
        public WinnerSoft.Sessions.ISession Session
        {
            get
            {
                return ContextHelper.Session;
            }
        }

        /// <summary>
        /// 获取或设置当前登录账号与系统的会话。
        /// </summary>
        [Browsable(false)]
        public WinnerSoft.Data.Access.IConnection DbConnection
        {
            get
            {
                return (IConnection)this.Session.Resouces.FindResources(typeof(IConnection))[0];
            }
        }


        IDataAccess m_sqlHelper = null;

        public IDataAccess SqlHelper
        {
            get
            {
                if (m_sqlHelper == null)
                    DataAccessFactory.GetSqlDataAccess();
                return DataAccessFactory.DefaultDataAccess;
            }
        }

        #endregion


        //#region 继承父类方法
        //public override string Description
        //{
        //    get
        //    {
        //        return "病历模板维护。";
        //    }
        //}

        //public override System.Guid Guid
        //{
        //    get
        //    {
        //        return new System.Guid("B576D603-E078-4DE0-B805-7CF8C7A0EE24");
        //    }
        //}

        //public override string ModuleName
        //{
        //    get
        //    {
        //        return "病历模板维护";
        //    }
        //}

        //public override string ObjectName
        //{
        //    get
        //    {
        //        return ModuleName;
        //    }
        //}


        //public override string Group
        //{
        //    get
        //    {
        //        return "病历模板维护";
        //    }
        //}


        //public override IPlugIn Run(params object[] parameters)
        //{
        //    if (this.Account.OriginalID == "")
        //    {
        //        MessageShow("对不起，当前账户没有有效的员工原型信息，无法进行业务操作！", CustomMessageBoxKind.ErrorOk);
        //        return null;
        //    }
        //    SetWaitDialogCaption("正在加载模块【" + this.ModuleName + "】...");//显示等待窗口
        //    this.Initialize();
        //    HideWaitDialog();//隐藏等待窗口

        //    IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
        //    return plg;
        //}
        //#endregion

        public void Initialize()
        {
            InitList(true);
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        private void InitList(bool Enable)
        {

            dtDept = new DataTable("Dept");
            dtDept.Columns.Add("ID");
            dtDept.Columns.Add("PARENTID");
            dtDept.Columns.Add("NAME");
            dtDept.Columns.Add("STATUS");

            LoadOrgs();

            this.mzcontent = new MzContent() { Dock = DockStyle.Fill };
            this.DiagnosisPanel.Controls.Add(this.mzcontent);
        }

        /// <summary>
        /// 机构加载
        /// </summary>
        private void LoadOrgs()
        {
            WinnerHIS.Integral.Personnel.DAL.Interface.IOrganizationInfoList list = WinnerHIS.Integral.Personnel.DAL.Interface.DALHelper.DALManager.CreateOrganizationInfoList();
            list.Session = this.Session;

            //组织机构的根
            WinnerHIS.Integral.Personnel.DAL.Interface.IOrganizationInfo orgRoot = WinnerHIS.Integral.Personnel.DAL.Interface.DALHelper.DALManager.CreateOrganizationInfo();
            orgRoot.Session = this.Session;

            //1、原型不为空加载当前组织机构下属的所有机构
            //2、帐号原型为空时加载所有组织机构
            if (this.Account.OriginalID != string.Empty)
            {
                string s = "SELECT OrgCode FROM BASEDATA.DBO.EMPLOYEE WHERE EMPID=" + this.Account.OriginalID;
                int orgCode = Int32.Parse(this.DbConnection.CreateAccessor().Query(s).ToString());

                orgRoot.OrgCode = orgCode;
                orgRoot.Refresh();
            }
            else
            {
                list.GetOrganizationRootInfoList();
                orgRoot = list.Rows[0] as WinnerHIS.Integral.Personnel.DAL.Interface.IOrganizationInfo;
            }



            this.treeListDept.Nodes.Clear();

            DataRow drRoot = dtDept.NewRow();
            drRoot["ID"] = orgRoot.OrgCode;
            drRoot["PARENTID"] = DBNull.Value;
            drRoot["NAME"] = orgRoot.OrgName;
            drRoot["STATUS"] = "0";
            dtDept.Rows.Add(drRoot);

            this.treeListDept.DataSource = dtDept;
            this.treeListDept.ExpandAll();
            this.treeListDept.BeginUpdate();
            TreeListNode nodeRoot = this.treeListDept.Nodes[0];
            nodeRoot.StateImageIndex = 0;
            LoadDepts(orgRoot, nodeRoot);

            this.treeListDept.EndUpdate();
            this.treeListDept.ExpandAll();

            this.treeListDept.FocusedNode = nodeRoot;
        }

        /// <summary>
        /// 部门加载
        /// </summary>
        private void LoadDepts(WinnerHIS.Integral.Personnel.DAL.Interface.IOrganizationInfo org, TreeListNode nodeRoot)
        {
            IDepartment dept = WinnerHIS.Integral.Personnel.DAL.Interface.DALHelper.DALManager.CreateDepartment();
            dept.Session = this.Session;
            dept.OrgCode = org.OrgCode;
            dept.DeptID = org.OrgCode;
            dept.Refresh();

            if (!dept.Exists)
                return;

            //只显示启用记录
            IDepartment[] subDepartments = dept.GetEnabledSubDepartments();

            foreach (IDepartment var in subDepartments)
            {
                TreeListNode modNode = this.treeListDept.AppendNode(null, nodeRoot);
                modNode.SetValue("ID", var.DeptID.ToString());
                modNode.SetValue("NAME", var.Name + "[" + var.DeptID.ToString() + "]");
                modNode.SetValue("PARENTID", dept.DeptID);
                modNode.SetValue("STATUS", "1");
                modNode.StateImageIndex = modNode.Level;
                modNode.Tag = var;
                treeListDept.SetFocusedNode(treeListDept.FindNodeByKeyID(var.Name));

                this.LoadDepts(var, modNode);
            }
        }
        /// <summary>
        /// 部门加载
        /// </summary>
        /// <param name="dept"></param>
        /// <param name="nodeRoot"></param>
        private void LoadDepts(IDepartment dept, TreeListNode nodeRoot)
        {

            //只显示启用记录
            IDepartment[] subDepartments = dept.GetEnabledSubDepartments();

            foreach (IDepartment var in subDepartments)
            {
                TreeListNode modNode = this.treeListDept.AppendNode(null, nodeRoot);
                modNode.SetValue("ID", var.DeptID.ToString());
                modNode.SetValue("NAME", var.Name + "[" + var.DeptID.ToString() + "]");
                modNode.SetValue("PARENTID", var.ParentID);
                modNode.SetValue("STATUS", "1");
                modNode.StateImageIndex = modNode.Level;
                modNode.Tag = var;
                treeListDept.SetFocusedNode(treeListDept.FindNodeByKeyID(var.Name));

                this.LoadDepts(var, modNode);
            }
        }

        public void ShowMed(string id)
        {
            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append(" select a.NAME dname,b.EMPID, b.ID, b.NAME mname,b.OrgCode,b.YiJian,b.FZJianCha,b.MedicineIcd,b.JianCha,b.JiWangShi, b.ZhenDuan, b.ZhuSu, b.Flag,b.NowMedicalHistory,b.SyndromeDifferentiation,b.ReconnaissanceSituation,b.MedicineIcd,b.Epidemic,b.PersonalHistory,b.FamilyHistory,b.AllergicHistory,b.State ");
            sqlstr.Append(" from BaseData.dbo.DEPARTMENT as a ");
            sqlstr.Append(" join [BaseData].[dbo].[MedicalPatern] as b on a.ID = b.depid ");
            sqlstr.Append(" where b.depid = '"+id+"' ");
            sqlstr.Append(" GROUP BY a.NAME,b.EMPID, b.ID, b.NAME,b.OrgCode,b.YiJian,b.FZJianCha,b.MedicineIcd,b.JianCha,b.JiWangShi, b.ZhenDuan, b.ZhuSu, b.Flag,b.NowMedicalHistory,b.SyndromeDifferentiation,b.ReconnaissanceSituation,b.MedicineIcd,b.Epidemic,b.PersonalHistory,b.FamilyHistory,b.AllergicHistory,b.State ");
            DataTable Sumtable = WinnerHIS.Common.ContextHelper.ExeSqlDataTable(sqlstr.ToString());
            gridControl1.DataSource = Sumtable;

        }

        private void treeListDept_DoubleClick(object sender, EventArgs e)
        {
            TreeListNode nodeFocuse = this.treeListDept.FocusedNode;
            string accountTmp = nodeFocuse.GetValue("ID").ToString();

            ShowMed(accountTmp);
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Flag")
            {
                e.DisplayText = e.Value.ToString() == "1" ? "个人" : "公开";
            }
            else if (e.Column.FieldName == "State")
            {
                e.DisplayText = e.Value.ToString() == "1" ? "启用" : "禁用";
            }
            if (e.Column.FieldName == "EMPID")
            {
                e.DisplayText =DataConvertHelper.GetEmpName(Convert.ToInt32(e.Value));
            }
        }

        #region 西医诊断
        public void MedicalCodeEditValue(string value_name)
        {//第1-5医保码
            string[] strs = value_name == "" ? new string[] { } : value_name.Split(new char[] { '^' });
            for (int i = 0; i < 8; i++)
            {
                string[] strs2 = i < strs.Length ? strs[i].Split(new char[] { '#' }) : new string[] { "", "" };
                switch (i)
                {
                    case 0:
                        mzcontent.searchLookUpEdit1.Text = strs2[1] == "" ? null : strs2[1];
                        mzcontent.searchLookUpEdit1.EditValue = strs2[0] == "" ? null : strs2[0];
                        break;
                    case 1:
                        mzcontent.searchLookUpEdit2.Text = strs2[1] == "" ? null : strs2[1];
                        mzcontent.searchLookUpEdit2.EditValue = strs2[0] == "" ? null : strs2[0];
                        break;
                    case 2:
                        mzcontent.searchLookUpEdit3.Text = strs2[1] == "" ? null : strs2[1];
                        mzcontent.searchLookUpEdit3.EditValue = strs2[0] == "" ? null : strs2[0];
                        break;
                    case 3:
                        mzcontent.searchLookUpEdit4.Text = strs2[1] == "" ? null : strs2[1];
                        mzcontent.searchLookUpEdit4.EditValue = strs2[0] == "" ? null : strs2[0];
                        break;
                    case 4:
                        mzcontent.searchLookUpEdit5.Text = strs2[1] == "" ? null : strs2[1];
                        mzcontent.searchLookUpEdit5.EditValue = strs2[0] == "" ? null : strs2[0];
                        break;
                    case 5:
                        mzcontent.searchLookUpEdit6.Text = strs2[1] == "" ? null : strs2[1];
                        mzcontent.searchLookUpEdit6.EditValue = strs2[0] == "" ? null : strs2[0];
                        break;
                    case 6:
                        mzcontent.searchLookUpEdit7.Text = strs2[1] == "" ? null : strs2[1];
                        mzcontent.searchLookUpEdit7.EditValue = strs2[0] == "" ? null : strs2[0];
                        break;
                    case 7:
                        mzcontent.searchLookUpEdit8.Text = strs2[1] == "" ? null : strs2[1];
                        mzcontent.searchLookUpEdit8.EditValue = strs2[0] == "" ? null : strs2[0];
                        break;
                }

            }
        }
        #endregion

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {

            int selectHandle = this.gridView1.GetSelectedRows()[0];
            //MessageBox.Show(this.gridView1.GetRowCellValue(selectHandle,"ID").ToString());
            //TreeListNode node = TreeList.FocusedNode;
            //IPatientDetail Fudetail = node.Tag as IPatientDetail;
            //if (e.Node.ParentNode != null && !e.Node.HasChildren)
            //{
            //    Fudetail.Refresh();
            //    patientList.GetRegcode(detail.RegCode);
            //    if (patientList.Rows.Count > 0)
            //    {
                    //IPatientDetail Fudetail = patientList.Rows[0] as IPatientDetail;
                    //就诊时间
                    //this.clickTime = Convert.ToDateTime(Fudetail.InTime);
                    //主诉
                    mzcontent.txtZhushu.Text =this.gridView1.GetRowCellValue(selectHandle,"ZhuSu").ToString();
                    //既往史
                    mzcontent.txtJWS.Text =this.gridView1.GetRowCellValue(selectHandle,"JiWangShi").ToString();
                    //体格检查
                    mzcontent.rbJianCha.Text = this.gridView1.GetRowCellValue(selectHandle, "JianCha").ToString();
                    //门诊诊断
                    mzcontent.rbZhenDuan.Text =this.gridView1.GetRowCellValue(selectHandle,"ZhenDuan").ToString();
                    //辅助检查
                    mzcontent.rbFZJianCha.Text = this.gridView1.GetRowCellValue(selectHandle, "FZJianCha").ToString();
                    //治疗意见
                    mzcontent.rbYJ.Text =this.gridView1.GetRowCellValue(selectHandle,"YiJian").ToString();
                    //现病史
                    mzcontent.txtXBS.Text =this.gridView1.GetRowCellValue(selectHandle,"NowMedicalHistory").ToString();
                    //中医辨证
                    mzcontent.SyndromeDifferentiation.Text =this.gridView1.GetRowCellValue(selectHandle,"SyndromeDifferentiation").ToString();
                    //中医辨证
                    mzcontent.ReconnaissanceSituation.Text =this.gridView1.GetRowCellValue(selectHandle,"ReconnaissanceSituation").ToString();

                    //西医诊断初始化值
                    //this.MedicalCodeEditValue(this.gridView1.GetRowCellValue(selectHandle, "Fudetail").ToString());
                    //中医诊断初始化值
                    //this.ChineseMedicineIcdEditValue(Fudetail.ChineseMedicineIcd);

                    //流行病学史
                    mzcontent.rbEpidemic.Text =this.gridView1.GetRowCellValue(selectHandle,"Epidemic").ToString();
                    //个人史
                    mzcontent.rbPersonalHistory.Text =this.gridView1.GetRowCellValue(selectHandle,"PersonalHistory").ToString();
                    //家族史
                    mzcontent.rbFamilyHistory.Text =this.gridView1.GetRowCellValue(selectHandle,"FamilyHistory").ToString();
                    //过敏史
                    mzcontent.rbAllergicHistory.Text =this.gridView1.GetRowCellValue(selectHandle,"AllergicHistory").ToString();
               // }
            //}
                    #region 病历质控
                    int empCode = ContextHelper.Employee.EmployeeID;
                    DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    //质控时间打印0 并且是已经就诊过的
                    //if (RecordDateTime > 0 && regCode.TransFlag == 1 && detail.RecordPlan > 0)
                    //{
                    if (empCode != 7)
                    {
                        mzcontent.RecordDateLable.Visible = true;
                        if (Convert.ToInt32(row["EMPID"]) == empCode)
                        {
                            RecordLocking(true);

                        }
                        else
                        {
                            RecordLocking(false);
                        }
                    }
                    //}
                    //else
                    //{
                      //  RecordLocking(true);
                      //  mzcontent.RecordDateLable.Visible = false;
                    //}
                    #endregion

        }

        public void RecordLocking(Boolean sf)
        {

            sf = true;
            DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //病历是否可以编辑
            MedicalEnable = sf;
            if (sf)
            {
                mzcontent.RecordDateLable.Text = "【可修改】";
                mzcontent.RecordDateLable.ForeColor = Color.Blue;
            }
            else
            {
                mzcontent.RecordDateLable.Text = "【已锁定】";
                mzcontent.RecordDateLable.ForeColor = Color.Red;
            }
            if (Convert.ToInt32(row["EMPID"]) != WinnerHIS.Common.ContextHelper.Employee.EmployeeID)
            {
                sf = false;
            }
            //主诉
            mzcontent.txtZhushu.ReadOnly = !sf;
            //既往史
            mzcontent.txtJWS.ReadOnly = !sf;
            //体格检查
            mzcontent.rbJianCha.ReadOnly = !sf;
            //门诊诊断
            mzcontent.rbZhenDuan.ReadOnly = !sf;
            //辅助检查
            mzcontent.rbFZJianCha.ReadOnly = !sf;
            //治疗意见
            mzcontent.rbYJ.ReadOnly = !sf;
            //现病史
            mzcontent.txtXBS.ReadOnly = !sf;
            //中医辨证
            mzcontent.SyndromeDifferentiation.ReadOnly = !sf;
            //中医辨证
            mzcontent.ReconnaissanceSituation.ReadOnly = !sf;

            mzcontent.searchLookUpEdit1.ReadOnly = !sf;
            mzcontent.searchLookUpEdit2.ReadOnly = !sf;
            mzcontent.searchLookUpEdit3.ReadOnly = !sf;
            mzcontent.searchLookUpEdit4.ReadOnly = !sf;
            mzcontent.searchLookUpEdit5.ReadOnly = !sf;
            mzcontent.searchLookUpEdit6.ReadOnly = !sf;
            mzcontent.searchLookUpEdit7.ReadOnly = !sf;
            mzcontent.searchLookUpEdit8.ReadOnly = !sf;

            mzcontent.chineseZd1.ReadOnly = !sf;
            mzcontent.chineseZd2.ReadOnly = !sf;
            mzcontent.chineseZd3.ReadOnly = !sf;
            mzcontent.chineseZd4.ReadOnly = !sf;
            mzcontent.chineseZd5.ReadOnly = !sf;
            mzcontent.chineseZd6.ReadOnly = !sf;
            mzcontent.chineseZd7.ReadOnly = !sf;
            mzcontent.chineseZd8.ReadOnly = !sf;


            //流行病学史
            mzcontent.rbEpidemic.ReadOnly = !sf;
            //个人史
            mzcontent.rbPersonalHistory.ReadOnly = !sf;
            //家族史
            mzcontent.rbFamilyHistory.ReadOnly = !sf;
            //过敏史
            mzcontent.rbAllergicHistory.ReadOnly = !sf;

            mzcontent.btnUse.Enabled = sf;



        }



        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.MaintainAdd();
        }

        /// <summary>
        /// 右击添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripAdd_Click(object sender, EventArgs e)
        {
            this.MaintainAdd();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            this.MaintainUpdate();
        }

        /// <summary>
        /// 右击修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripAttribute_Click(object sender, EventArgs e)
        {
            this.MaintainUpdate();
        }

        protected void ListRefresh()
        {
            this.ShowMed("210268");
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ListRefresh();
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.MaintainStop();
        }

        /// <summary>
        /// 右击禁用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripDelete_Click(object sender, EventArgs e)
        {
            this.MaintainStop();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            medical = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateMedicalPatern();
            medical.Session = WinnerHIS.Common.ContextHelper.Session;
            medical.ID = Convert.ToInt32(row["ID"]);
            medical.OrgCode = Convert.ToInt32(row["OrgCode"]);
            medical.Flag = Convert.ToInt32(row["Flag"]);
            medical.State = Convert.ToInt32(row["State"]);
            medical.Refresh();

            medical.ZhuSu = mzcontent.txtZhushu.Text;
            medical.JiWangShi = mzcontent.txtJWS.Text;
            medical.FZJianCha = mzcontent.rbFZJianCha.Text;
            medical.ZhenDuan = mzcontent.rbZhenDuan.Text;
            medical.JianCha = mzcontent.rbJianCha.Text;
            medical.YiJian = mzcontent.rbYJ.Text;
            medical.NowMedicalHistory = mzcontent.txtXBS.Text;
            medical.SyndromeDifferentiation = mzcontent.SyndromeDifferentiation.Text;
            medical.ReconnaissanceSituation = mzcontent.ReconnaissanceSituation.Text;
            medical.Epidemic = mzcontent.rbEpidemic.Text;
            medical.PersonalHistory = mzcontent.rbPersonalHistory.Text;
            medical.FamilyHistory = mzcontent.rbFamilyHistory.Text;
            medical.AllergicHistory = mzcontent.rbAllergicHistory.Text;

            medical.Update();
            MessageBox.Show("保存成功~~");

            ListRefresh();

        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonEnable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.MaintainEnable();
        }

        /// <summary>
        /// 右击启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 启用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MaintainEnable();
        }


        #region 添加方法
         public void MaintainAdd()
        {
            TreeListNode nodeFocuse = this.treeListDept.FocusedNode;
            string accountTmp = nodeFocuse.GetValue("ID").ToString();

            BMaintainAdd bm = new BMaintainAdd(accountTmp, null);

            if (bm.ShowDialog() == DialogResult.OK)
            {
                this.ListRefresh();
            }
        }
        #endregion

        #region 修改方法
         public void MaintainUpdate()
        {
            TreeListNode nodeFocuse = this.treeListDept.FocusedNode;
            string accountTmp = nodeFocuse.GetValue("ID").ToString();

            DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            medical = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateMedicalPatern();
            medical.Session = WinnerHIS.Common.ContextHelper.Session;
            medical.ID = Convert.ToInt32(row["ID"]);
            medical.OrgCode = Convert.ToInt32(row["OrgCode"]);
            medical.Flag = Convert.ToInt32(row["Flag"]);
            medical.State = Convert.ToInt32(row["State"]);
            medical.Refresh();

            BMaintainAdd bm = new BMaintainAdd(accountTmp, medical);

            if (bm.ShowDialog() == DialogResult.OK)
            {
                this.ListRefresh();
            }
        }
         #endregion

        #region 禁用方法
         public void MaintainStop()
        {
            DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            medical = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateMedicalPatern();
            medical.Session = WinnerHIS.Common.ContextHelper.Session;
            medical.ID = Convert.ToInt32(row["ID"]);
            medical.OrgCode = Convert.ToInt32(row["OrgCode"]);
            medical.Flag = Convert.ToInt32(row["Flag"]);
            medical.State = Convert.ToInt32(row["State"]);
            medical.Refresh();

            if (!medical.Exists)
            {
                return;
            }

            if (MessageBox.Show("您确认要禁用所选择的记录么？\n删除记录可能造成历史数据的查询错误\n请确认您的操作。", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                medical.State = 0;
                medical.Update();
                MessageBox.Show("禁用成功");

                ListRefresh();

            }
        }
         #endregion

        #region 启用方法
         public void MaintainEnable()
        {
            DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            medical = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateMedicalPatern();
            medical.Session = WinnerHIS.Common.ContextHelper.Session;
            medical.ID = Convert.ToInt32(row["ID"]);
            medical.OrgCode = Convert.ToInt32(row["OrgCode"]);
            medical.Flag = Convert.ToInt32(row["Flag"]);
            medical.State = Convert.ToInt32(row["State"]);
            medical.Refresh();

            if (!medical.Exists)
            {
                return;
            }

            if (MessageBox.Show("您确认要禁用所选择的记录么？\n删除记录可能造成历史数据的查询错误\n请确认您的操作。", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                medical.State = 1;
                medical.Update();
                MessageBox.Show("启用成功");

                ListRefresh();

            }
        }
         #endregion

        /// <summary>
        /// 右击刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ListRefresh();
        }

        

        

        

        


    }
}

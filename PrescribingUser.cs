using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WinnerHIS.Integral.DataCenter.DAL.Interface;
using WinnerHIS.Common;
using WinnerHIS.Clinic.DAL.Interface;
using WinnerHIS.Advice;
using DevExpress.XtraGrid.Views.Grid;
using WinnerHIS.AP.Business.common;
using WinnerHIS.Integral.AP.DAL.Interface;
using WinnerHIS.Drug.DrugAdvice.DAL.Interface;
using WinnerHIS.Integral.AP.UI;
using System.Text.RegularExpressions;
using WinnerHIS.Integral.AIOCS.DAL.Interface;
using WinnerHIS.Drug.DrugStore.DAL.Interface;
using System.Collections;
using WinnerSoft.Report.Interface;
using WinnerSoft.Data.Access;
using System.Xml;
using WinnerHIS.Drug.Dict.DAL.Interface;
using WinnerHIS.Analyse.Decision.DAL.Interface;
using DevExpress.XtraGrid.Columns;
using WinnerHIS.Material.MaterialStore.DAL.Interface;
using WinnerHIS.Material.Dict.DAL.Interface;
using HIS.Clinic.DoctorWorkstation;
using CacheHelper = WinnerHIS.Common.CacheHelper;

namespace WinnerHIS.Diagnosis.Clinic.DoctorWorkstation.UI
{
    public partial class PrescribingUser : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 主界面
        /// </summary>
        Workstation _NewWorkstation = null;

        /// <summary>
        /// 门诊西药房
        /// </summary>
        public int WesternMedicineRoom = 0;
        /// <summary>
        /// 门诊中药房
        /// </summary>
        public int TCMPharmacy = 0;


        /// <summary>
        /// 获取是否启用医保
        /// </summary>
        public bool enabledYB = false;
        /// <summary>
        /// 获取是否启用销售耗材库
        /// </summary>
        public bool enableMaterialStore = false;
        /// <summary>
        /// 项目是否需要上传审批
        /// </summary>
        public bool ItemApproveFlag = false;

        /// <summary>
        /// 医保等级list
        /// </summary>
        public Dictionary<int, string> insureTypeList = new Dictionary<int, string>();

        /// <summary>
        /// 医生信息
        /// </summary>
        public WinnerHIS.Integral.Personnel.DAL.Interface.IEmployeeEx doctor;

        /// <summary>
        /// 诊疗信息
        /// </summary>
        public IItemInfoListEx itemList;

        /// <summary>
        /// 检查项目集合
        /// </summary>
        public IItemInfoListEx checkItems;

        /// <summary>
        /// 优惠项目集合
        /// </summary>
        public IItemInfoListEx DiscountList;


        /// <summary>
        /// 医嘱绑定总的集合
        /// </summary>
        public DataTable sumTable;

        /// <summary>
        /// 医嘱状态
        /// </summary>
        public List<int> StateList;

        /// <summary>
        /// 未保存的数据
        /// </summary>
        public DataTable NotSavedList;

        /// <summary>
        /// 处方类型
        /// </summary>
        public WinnerHIS.Integral.DataCenter.DAL.Interface.IGBCodeListEx prescriptionTypeList;

        /// <summary>
        /// 默认处方类型
        /// </summary>
        int advTypeDefault;

        /// <summary>
        /// 一卡通实体类
        /// </summary>
        private IICCard icCard;

        /// <summary>
        /// 当前病人
        /// </summary>
        IRegCode currentCode;

        /// <summary>
        /// 病人详细信息
        /// </summary>
        IPatientDetail patInfo;

        /// <summary>
        /// 是否启动一卡通
        /// </summary>
        private bool isUseCard;

        /// <summary>
        /// 优惠编码
        /// </summary>
        private string DiscountItemCode;

        /// <summary>
        /// 挂号有效天数
        /// </summary>
        public int regValidDays = 0;

        /// <summary>
        /// 当前库存
        /// </summary>
        WinnerHIS.Drug.DrugStore.DAL.Interface.IDrugStoreExLetList storeList;
        /// <summary>
        /// 耗材
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        WinnerHIS.Material.MaterialStore.DAL.Interface.IMaterialStoreExLetList mstoreList;

        /// <summary>
        /// 处方
        /// </summary>
        WinnerHIS.Clinic.DAL.Interface.IDrugAdviceList drugAdvList;
        /// <summary>
        ///  门诊西药处方签显示药品上限
        /// </summary>
        private int prescriptionWDrugNumber = 1;

        /// <summary>
        ///  门诊中药处方签显示药品上限
        /// </summary>
        private int prescriptionCDrugNumber = 1;
        /// <summary>
        /// 药品类型编码
        /// </summary>
        public string drugItemCodes;

        /// <summary>
        /// 耗材是否进入计费流程
        /// </summary>
        public bool materialFlag;

        /// <summary>
        /// 耗材存储类型
        /// </summary>
        public int materialStoreType;

        /// <summary>
        /// 
        /// </summary>
        public int materialStoreID;

        /// <summary>
        /// 申请单和科室对应关系
        /// </summary>
        public string requestDeptList;//申请单和科室对应关系
        /// <summary>
        /// 申请单和科室对应关系
        /// </summary>
        public Dictionary<string, string> reqDeptList = new Dictionary<string, string>();//申请单和科室对应关系

        /// <summary>
        /// 是否启动优惠审核
        /// </summary>
        bool cmCouponCheckFlag;
        /// <summary>
        /// 处方复制剪贴板
        /// </summary>
        List<object> copyAdviceList;


        /// <summary>
        /// 是否启用抗菌药物
        /// </summary>
        /// <param name="NewWorkstation"></param>
        public bool enabledAntibacterialControl;

        public PrescribingUser(Workstation NewWorkstation)
        {
            InitializeComponent();
            initialization();
            _NewWorkstation = NewWorkstation;
            cmCouponCheckFlag = NewWorkstation.cmCouponCheckFlag;
            TableColumnsAdd();

        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void initialization()
        {
            this.icCard = DALHelper.AIOCSDAL.CreateICCard();
            this.icCard.Session = WinnerHIS.Common.ContextHelper.Session;
            DiscountItemCode = WinnerHIS.Common.AppSettingHelper.Instance.GetAppValue("YHF").Substring(0, 2);
            requestDeptList = WinnerHIS.Common.AppSettingHelper.Instance.GetAppValue("RequestAndDeptment");
            this.prescriptionWDrugNumber = int.Parse(WinnerHIS.Common.AppSettingHelper.Instance.GetAppValue("PrescriptionWDrug"));
            this.prescriptionCDrugNumber = int.Parse(WinnerHIS.Common.AppSettingHelper.Instance.GetAppValue("PrescriptionCDrug"));
            drugItemCodes = WinnerHIS.Common.AppSettingHelper.Instance.GetAppValue("Drug2ItemCode");
            enabledAntibacterialControl = int.Parse(WinnerHIS.Common.AppSettingHelper.Instance.GetAppValue("EnabledAntibacterialControl")) == 1;
            string materialSet = WinnerHIS.Common.AppSettingHelper.Instance.GetAppValue("MaterialChargeSet");
            if (!string.IsNullOrEmpty(materialSet))
            {
                XmlDocument matdoc = new XmlDocument();
                matdoc.LoadXml(materialSet);
                string fareDateStatus = matdoc.GetElementsByTagName("Status")[0] == null ? "0" : (matdoc.GetElementsByTagName("Status")[0].InnerText);
                materialFlag = fareDateStatus == "1" ? true : false;
                materialStoreType = int.Parse(matdoc.GetElementsByTagName("StoreType")[0] == null ? "0" : (matdoc.GetElementsByTagName("StoreType")[0].InnerText));
                if (materialStoreType == 0)
                    materialStoreID = WinnerHIS.Common.ContextHelper.Employee.DepartmentID;
                else
                    materialStoreID = int.Parse(matdoc.GetElementsByTagName("StoreID")[0] == null ? "0" : (matdoc.GetElementsByTagName("StoreID")[0].InnerText));
            }


            storeList = WinnerHIS.Drug.DrugStore.DAL.Interface.DALHelper.DALManager.CreateDrugStoreExLetList();
            storeList.Session = WinnerHIS.Common.ContextHelper.Session;

            mstoreList = WinnerHIS.Material.MaterialStore.DAL.Interface.DALHelper.DALManager.CreateMaterialStoreExLetList();
            mstoreList.Session = WinnerHIS.Common.ContextHelper.Session;

            copyAdviceList = new List<object>();
            #region 动态添加右击(药品处方类型)
            prescriptionTypeList = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateGBCodeListEx();
            prescriptionTypeList.Session = WinnerHIS.Common.ContextHelper.Session;
            prescriptionTypeList.GetCodeList(1060);
            advTypeDefault = ((IGBCode)prescriptionTypeList.Rows[0]).Code;
            foreach (IGBCode obj in prescriptionTypeList.Rows)
            {
                ToolStripMenuItem Item = new ToolStripMenuItem();
                Item.Name = "Prescription" + obj.Code;
                Item.Size = new System.Drawing.Size(152, 22);
                Item.Text = obj.Name;
                Item.Tag = obj.Code.ToString();
                Item.Image = imageCollection1.Images[7];
                Item.ImageScaling = ToolStripItemImageScaling.None;

                this.toolStripMenuItem6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { Item });
                Item.Click += new System.EventHandler(PrescriptionType_Click);
            }
            #endregion

            #region 需要显示哪些状态的医嘱
            NotSavedBar.EditValue = true;
            ReviewedBar.EditValue = true;
            ChargedBar.EditValue = true;
            ExecutedBar.EditValue = true;
            RetiredBar.EditValue = true;
            checkCacel.EditValue = true;

            StateList = new List<int>();
            StateList.Add(-1);
            StateList.Add(1);
            StateList.Add(2);
            StateList.Add(3);
            StateList.Add(4);
            StateList.Add(5);
            #endregion

            #region 药品  诊疗  检查 信息
            WesternMedicineRoom = int.Parse(Common.AppSettingHelper.Instance.GetAppValue("DrugStoreXYFCode"));
            TCMPharmacy = int.Parse(Common.AppSettingHelper.Instance.GetAppValue("DrugStoreZYFCode"));
            enableMaterialStore = int.Parse(WinnerHIS.Common.AppSettingHelper.GetAppSettingsString("EnableMaterialStore")) == 1;
            //是否启动医保
            enabledYB = int.Parse(Common.AppSettingHelper.Instance.GetAppValue("EnableMedicare")) == 1;
            if (enabledYB)
            {
                IGBCodeExList gbList = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateGBCodeExList();
                gbList.Session = WinnerHIS.Common.ContextHelper.Session;
                gbList.GetYiBaoZhiFuTypeGBCodeList();
                foreach (IGBCodeEx gbEx in gbList.Rows)
                {
                    insureTypeList.Add(gbEx.Code, gbEx.Name);

                }
            }
            //项目是否需要上次审批
            this.ItemApproveFlag = int.Parse(Common.AppSettingHelper.Instance.GetAppValue("MedicareFareApproveFlag")) == 1;
            //员工信息
            this.doctor = WinnerHIS.Integral.Personnel.DAL.Interface.DALHelper.DALManager.CreateEmployeeEx();
            doctor.Session = Common.ContextHelper.Session;
            doctor.EmployeeID = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
            doctor.Refresh();

            //诊疗项目
            itemList = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoListEx();
            itemList.Session = WinnerHIS.Common.ContextHelper.Session;
            itemList.GetCureItemInfoList("");

            //检验检查项目
            checkItems = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoListEx();
            checkItems.Session = WinnerHIS.Common.ContextHelper.Session;
            checkItems.GetCheckItemInfoList("");

            //优惠项目
            DiscountList = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoListEx();
            DiscountList.Session = WinnerHIS.Common.ContextHelper.Session;
            DiscountList.GetDiscountList();
            #endregion

            AddFrequencyGLU();

            string[] rdList = requestDeptList.Split(',');
            string[] deptVar;
            foreach (var rdVar in rdList)
            {
                deptVar = rdVar.Split('-');
                reqDeptList.Add(deptVar[1], deptVar[0]);
            }
        }
        /// <summary>
        /// 更改药品处方类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrescriptionType_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem Menu = sender as ToolStripMenuItem;
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            if (row != null)
            {
                foreach (DataRow dr in (gridControlAdvice.DataSource as DataTable).Rows)
                {
                    String State = dr["AdvState"].ToString();
                    if (State == "-1")//未保存
                    {
                        MessageBox.Show("先保存处方然后再修改类型！");
                        return;
                    }
                    if (row["AdviceCode"].ToString() == dr["AdviceCode"].ToString())
                    {
                        IDrugAdvice obj = dr["Object"] as IDrugAdvice;
                        obj.Refresh();
                        obj.PrescriptionType = Menu.Tag as string;
                        dr["Object"] = obj;
                        obj.Save();
                    }
                }
            }
            Binding();
            XtraMessageBox.Show("操作成功！");

        }
        #region 加载频次集合
        /// <summary>
        /// 加载频次集合
        /// </summary>
        private void AddFrequencyGLU()
        {
            repItemGLUFrequecey.View.Columns.Clear();

            repItemGLUFrequecey.DisplayMember = "NAME";
            repItemGLUFrequecey.ValueMember = "CODE";

            // 为riglup增加3列
            GridColumn reqCode = this.repItemGLUFrequecey.View.Columns.AddField("CODE");
            reqCode.Caption = "编号";
            reqCode.VisibleIndex = 0;
            reqCode.Width = 20;

            GridColumn reqName = this.repItemGLUFrequecey.View.Columns.AddField("NAME");
            reqName.Caption = "名称";
            reqName.VisibleIndex = 1;
            reqName.Width = 35;

            GridColumn reqSymbol = this.repItemGLUFrequecey.View.Columns.AddField("SYMBOL");
            reqSymbol.Caption = "简称";
            reqSymbol.VisibleIndex = 2;
            reqSymbol.Width = 25;

            IDrugFrequencyList freqList = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateDrugFrequencyList();
            freqList.Session = WinnerHIS.Common.ContextHelper.Session;
            freqList.CacheQuery();

            repItemGLUFrequecey.DataSource = freqList.DataTable;

            repItemGLUFrequecey.View.Columns.Add(reqCode);
            repItemGLUFrequecey.View.Columns.Add(reqName);
            repItemGLUFrequecey.View.Columns.Add(reqSymbol);

            //repItemGLUFrequecey.EditValueChanged += (m, n) =>
            //{
            //    this.gridViewTmp.SetFocusedRowCellValue("ColorId", 0);
            //};


        }
        #endregion
        #region 绑定
        /// <summary>
        /// 表格添加列
        /// </summary>
        public void TableColumnsAdd()
        {
            #region 生成列
            sumTable = new DataTable();
            sumTable.Columns.Add("ID", typeof(Int32));
            sumTable.Columns.Add("No");
            sumTable.Columns.Add("AdviceCode");
            sumTable.Columns.Add("AdvState");
            sumTable.Columns.Add("Code");
            sumTable.Columns.Add("Content");
            sumTable.Columns.Add("Mode");
            sumTable.Columns.Add("Frequency");
            sumTable.Columns.Add("Days");
            sumTable.Columns.Add("Spec");
            sumTable.Columns.Add("Unit");
            sumTable.Columns.Add("Price");
            sumTable.Columns.Add("Number");
            sumTable.Columns.Add("Cash");
            sumTable.Columns.Add("CheckTime");
            sumTable.Columns.Add("CreationTime", typeof(DateTime));
            sumTable.Columns.Add("CreationDept");
            sumTable.Columns.Add("CreationDoc");
            sumTable.Columns.Add("ExeDept");
            sumTable.Columns.Add("GroupNo");
            sumTable.Columns.Add("GroupNoSymbol");
            sumTable.Columns.Add("Remarks");
            sumTable.Columns.Add("CheckObj", typeof(bool));
            sumTable.Columns.Add("Object", typeof(object));
            sumTable.Columns.Add("Dosage");

            //设置主键
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = sumTable.Columns["ID"];
            sumTable.PrimaryKey = PrimaryKeyColumns;

            gridControlAdvice.DataSource = sumTable;
            NotSavedList = sumTable.Clone();
            #endregion
        }
        /// <summary>
        /// 刷新SumTable
        /// </summary>
        /// <param name="sf">是否全部刷新</param>
        public void SumTableRefresh(IRegCode regCode, IPatientDetail pd, bool isCardFalg, int regVDays, bool sf)
        {
            this.txtName.Text = regCode.Name;
            this.txtSex.Text = regCode.Sex == 1000951 ? "男" : "女";
            //WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime()
            this.txtAge.Text = WinnerHIS.Common.ContextHelper.GetAge(Convert.ToDateTime(regCode.Birthday), regCode.EventTime);
            if (regCode.Tag != null)
                this.txtDiagnosis.Text = (string)regCode.Tag;

            currentCode = regCode;
            patInfo = pd;
            isUseCard = isCardFalg;
            regValidDays = regVDays;
            DateTime startDate = Convert.ToDateTime(regCode.EventTime.ToShortDateString());
            DateTime endDate = Convert.ToDateTime(WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime().ToShortDateString());
            TimeSpan spDate = endDate.Subtract(startDate);
            bool enableFlag = true;
            if (regValidDays != -1)
            {
                enableFlag = (regValidDays - spDate.Days) >= 0 ? true : false;
            }
            else
            {
                if (regCode.reExamFlag == 0)//如果是初诊，默认有效天数是当天
                {
                    enableFlag = (0 - spDate.Days) >= 0 ? true : false;
                }
            }
            btnOpenBar.Enabled = enableFlag;
            btnAgreementBar.Enabled = enableFlag;
            btnPrescriptionBar.Enabled = enableFlag;
            lblTip.Text = enableFlag ? string.Empty : "注意：该号已经过期，请重新挂号！";
            toolSMINew.Enabled = enableFlag;

            if (_NewWorkstation.currentReg == null)
            {
                return;
            }
            sumTable = this.gridControlAdvice.DataSource as DataTable;
            if (sf)//删除未保存的数据
            {
                sumTable.Rows.Clear();
                NotSavedList.Clear();
            }
            else
            {
                for (int i = this.sumTable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = sumTable.Rows[i];
                    if (dr["AdvState"].ToString() == "-1")
                    {
                        if (NotSavedList.Rows.Contains(dr["ID"].ToString()))
                        {
                            for (int s = NotSavedList.Rows.Count - 1; s >= 0; s--)
                            {
                                DataRow row = NotSavedList.Rows[s];
                                NotSavedList.Rows.Remove(row);
                            }
                        }
                        NotSavedList.ImportRow(dr);

                    }
                    sumTable.Rows.Remove(dr);
                }
                NotSavedList.DefaultView.Sort = "CreationTime asc";//倒序
                if (StateList.Contains(-1))
                {
                    foreach (DataRow dr in NotSavedList.Rows)
                    {
                        if (!sumTable.Rows.Contains(dr["ID"].ToString()))
                        {
                            sumTable.ImportRow(dr);
                        }
                    }
                }

            }

            #region 从Oracle读药品处方

            DataTable OracleDtDrug = null;
            DataTable OracleMethodMapping = null;
            string message = string.Empty;
            string message1 = string.Empty;
            string sql = $"select * from cm_drugadvice where regcode = '{_NewWorkstation.currentReg.Code}'";
            string sql1 = $"select * from cm_yyff "; //获取Oracle的服药方式
            int ii = 0;
            OracleDtDrug = OracleHelpher.SelectSql(sql, ref message);
            OracleMethodMapping = OracleHelpher.SelectSql(sql1, ref message1);
            try
            {
                if (OracleDtDrug == null || OracleDtDrug.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    for (int i = 0; i < OracleDtDrug.Rows.Count; i++)
                    {
                        WinnerHIS.Clinic.DAL.Interface.IDrugAdvice drug = DALHelper.ClinicDAL.CreateDrugAdvice();
                        ii = i;
                        drug.ID = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["ID"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["ID"].ToString());
                        drug.AdviceCode = OracleDtDrug.Rows[i]["ADVICECODE"].ToString();
                        drug.Patient = OracleDtDrug.Rows[i]["PATIENT"].ToString();
                        drug.RegCode = OracleDtDrug.Rows[i]["REGCODE"].ToString();
                        drug.Name = OracleDtDrug.Rows[i]["NAME"].ToString();
                        drug.DrugCode = OracleDtDrug.Rows[i]["DRUGCODE"].ToString();
                        drug.DrugID = OracleDtDrug.Rows[i]["DRUGID"].ToString();
                        drug.DrugName = OracleDtDrug.Rows[i]["DRUGNAME"].ToString();
                        drug.Spec = OracleDtDrug.Rows[i]["SPEC"].ToString();
                        drug.BigUnit = OracleDtDrug.Rows[i]["BIGUNIT"].ToString();
                        drug.SmallUnit = OracleDtDrug.Rows[i]["SMALLUNIT"].ToString();
                        drug.PackRule = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["PACKRULE"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["PACKRULE"].ToString());
                        drug.PackType = PackType.Big;
                        drug.InsureType = InsureType.A;
                        drug.Attribute = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["ATTRIBUTE"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["ATTRIBUTE"].ToString());
                        drug.OtcType = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["OTCTYPE"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["OTCTYPE"].ToString());
                        drug.Action = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["ACTION"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["ACTION"].ToString());
                        //drug.Type = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["TYPE"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["TYPE"].ToString());
                        drug.AgentType = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["AGENTTYPE"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["AGENTTYPE"].ToString());
                        drug.Dosage = OracleDtDrug.Rows[i]["DOSAGE"].ToString();
                        drug.Price = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["PRICEL"].ToString()) ? 0 : Convert.ToDecimal(OracleDtDrug.Rows[i]["PRICEL"].ToString());
                        drug.Number = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["SL"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["SL"].ToString());
                        drug.Pack = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["PACK"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["PACK"].ToString());
                        drug.Cash = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["CASH"].ToString()) ? 0 : Convert.ToDecimal(OracleDtDrug.Rows[i]["CASH"].ToString());
                        //drug.Method = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["METHOD"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["METHOD"].ToString());
                        drug.Method = TransMethodMap1(string.IsNullOrEmpty(OracleDtDrug.Rows[i]["METHOD"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["METHOD"].ToString()));
                        drug.Frequence = TransFrequenceMap(string.IsNullOrEmpty(OracleDtDrug.Rows[i]["FREQUENCE"].ToString()) ? "0" : OracleDtDrug.Rows[i]["FREQUENCE"].ToString());
                        drug.CreateTime = Convert.ToDateTime(OracleDtDrug.Rows[i]["CREATETIME"].ToString());
                        drug.Creator = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["CREATOR"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["CREATOR"].ToString());
                        drug.DeptID = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["DEPTID"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["DEPTID"].ToString());
                        drug.CheckTime = Convert.ToDateTime(OracleDtDrug.Rows[i]["CHECKTIME"].ToString());
                        drug.Checker = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["CHECKER"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["CHECKER"].ToString());
                        drug.ExecTime = Convert.ToDateTime(OracleDtDrug.Rows[i]["EXECTIME"].ToString());
                        drug.Executer = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["EXECUTER"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["EXECUTER"].ToString());
                        drug.ExecDept = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["EXECDEPT"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["EXECDEPT"].ToString());
                        //drug.CancelTime = Convert.ToDateTime(OracleDtDrug.Rows[i]["CANCELTIME"].ToString());
                        drug.Canceler = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["CANCELER"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["CANCELER"].ToString());
                        drug.State = WinnerSoft.Data.ORM.EntityState.New;
                        drug.Memo = OracleDtDrug.Rows[i]["Memo"].ToString();
                        drug.CardID = OracleDtDrug.Rows[i]["CARDID"].ToString();
                        drug.OrgCode = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["OrgCode"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["OrgCode"].ToString());
                        drug.ZuHe = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["ZuHe"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["ZuHe"].ToString());
                        drug.Remarks = OracleDtDrug.Rows[i]["REMARKS"].ToString();
                        drug.SkinTest = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["SkinTest"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["SkinTest"].ToString());
                        drug.SortNum = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["SortNum"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["SortNum"].ToString());
                        drug.DripSpeed = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["DripSpeed"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["DripSpeed"].ToString());
                        drug.Days = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["Days"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["Days"].ToString());
                        drug.GroupNo = string.IsNullOrEmpty(OracleDtDrug.Rows[i]["GroupNo"].ToString()) ? 0 : Convert.ToInt32(OracleDtDrug.Rows[i]["GroupNo"].ToString());
                        drug.GroupNoSymbol = OracleDtDrug.Rows[i]["GroupNoSymbol"].ToString();
                        drug.PrescriptionType = OracleDtDrug.Rows[i]["PrescriptionType"].ToString();
                        drug.DosageUnit = OracleDtDrug.Rows[i]["DosageUnit"].ToString();
                        drug.BillCode = OracleDtDrug.Rows[i]["BillCode"].ToString();
                        drug.rv = Encoding.Default.GetBytes(OracleDtDrug.Rows[i]["rv"].ToString());
                        DrugTableAdd(drug);

                        if (StateList.Contains((int)drug.AdvState))
                        {
                        }
                    }
                }

            }
            catch (Exception err)
            {

                throw;
            }


            #endregion


            //WinnerHIS.Clinic.DAL.Interface.IDrugAdviceList mzadviceList = DALHelper.ClinicDAL.CreateDrugAdviceList();
            //mzadviceList.Session = CacheHelper.Session;
            //mzadviceList.GetDrugList(_NewWorkstation.currentReg.Code);
            //foreach (IDrugAdvice drug in mzadviceList.Rows)
            //{
            //    if (StateList.Contains((int)drug.AdvState))
            //    {
            //        DrugTableAdd(drug);
            //    }

            //}




            //ICureAdviceList mzcureList = DALHelper.ClinicDAL.CreateCureAdviceList();
            //mzcureList.Session = CacheHelper.Session;
            //mzcureList.GetCureList(_NewWorkstation.currentReg.Code);
            //foreach (ICureAdvice cure in mzcureList.Rows)
            //{
            //    if (StateList.Contains((int)cure.AdvState))
            //    {
            //        CureTableAdd(cure);
            //    }
            //}

            //ICheckAdviceList mzcheckList = DALHelper.ClinicDAL.CreateCheckAdviceList();
            //mzcheckList.Session = CacheHelper.Session;
            //mzcheckList.GetCheckList(_NewWorkstation.currentReg.Code);
            //foreach (ICheckAdvice check in mzcheckList.Rows)
            //{
            //    if (StateList.Contains((int)check.AdvState))
            //    {
            //        CheckTableAdd(check);
            //    }

            //}

            Binding();

            //显示卡余额
            if (isCardFalg && false)
            {
                this.txtCardID.Text = regCode.CardID;
                this.icCard.CardID = this.txtCardID.Text;
                this.icCard.Refresh();
                if (icCard.Exists)
                {
                    this.lblCash.Text = icCard.Cash.ToString("F2");
                }
            }

        }
        

        public int TransFrequenceMap(string key)
        {
            key = key.Trim();
            Dictionary<string, int> map = new Dictionary<string, int>()
        {
            {"stat",1001061},
            {"qn",1001062},
            {"hs",1001063},
            {"prn",1001064},
            {"qd",1001065},
            {"bid",1001066},
            {"tid",1001067},
            {"qid",1001068},
            {"qw",1001069},
            {"biw",1001070},
            {"q1h",1001071},
            {"q2h",1001072},
            {"q3h",1001073},
            {"q4h",1001074},
            {"q6h",1001075},
            {"qod",1001076},
            {"q8h",1001077},
            {"q12h",1001078},
            {"q72h",1001079},
            {"q30m",1001080},
            {"q15m",1001081},
            {"sq30m",1001082},
            {"sq4h",1001083},
            {"sq6h",1001084},
            {"sq12h",1001085},
            {"tiw",2015738}
        };
            if (map.ContainsKey(key))
            {
                return map[key];
            }
            return 0;
        }

        public int TransMethodMap(string key)
        {
            key = key.Trim();
            Dictionary<string, int> map = new Dictionary<string, int>()
        {
            {"po", 2000090},
            {"kh", 2000091},
            {"sl", 2000092},
            {"inhl", 2000093},
            {"gpa", 2000094},
            {"id",2000095},
            {"hd",2000096},
            {"im",2000097},
            {"ivgtt",2000098},
            {"ext",2000099},
            {"irri",2000100},
            {"la",2000101},
            {"qt",2000102},
            {"gutt",2000103},
            {"qm",2000104},
            {"qn",2000105},
            {"ac",2000106},
            {"fc",2000107},
            {"am",2000108},
            {"pm",2000109},
            {"prn",2000110},
            {"sos",2000111},
            {"stl",2000112},
            {"citol",2000113},
            {"lent",2000114},
            {"nar",2000115},
            {"v",2000116},
            {"jf",2015050},
            {"inh",2015096},
            {"zg",2015097},
            {"ip",2015103},
            {"ym",2015104},
            {"o2inhal",2015105},
            {"pr",2015106},
            {"gjzs",2015108},
            {"iv",2015236},
            {"iv",2015238},
            {"x",2015539},
            {"sg",2015739},
            {"KK",2015747},
            {"JFGE",2015750},
            {"PWIT",2015773}
        };

            if (map.ContainsKey(key))
            {
                return map[key];
            }

            return 0;

        }

        public int TransMethodMap1(int key)
        {
            Dictionary<int, int> map = new Dictionary<int, int>()
            {
                 {2709, 2000090},
                 {2699, 2000091},
                 {2715, 2000092},
                 {2222, 2000093},                 
                 {2714, 2000095},
                 {2706, 2000096},
                 {2707, 2000097},
                 {2705, 2000098},
                 {2704, 2000099},
                 {2722, 2000100},            
                 {2697, 2000103},        
                 {2708, 2000116},
                 {5061,2015050},
                 {2716,2015096},
                 {2719,2015103},          
                 {2721,2015236},
                 {5521,2015238},
                 {2724,2015539},
                 {2712,2015739}            
                
        };

            if (map.ContainsKey(key))
            {
                return map[key];
            }

            return 0;

        }
        /// <summary>
        /// 全选或者取消全选
        /// </summary>
        public void AllCheck(bool sf)
        {
            int columnscount = gridViewAdvice.DataRowCount;
            for (int i = 0; i < columnscount; i++)
            {
                gridViewAdvice.SetRowCellValue(i, gridViewAdvice.Columns["CheckObj"], sf);

            }
            gridControlAdvice.Refresh();
        }
        public void Binding()
        {
            sumTable = this.gridControlAdvice.DataSource as DataTable;
            DataTable dtCopy = sumTable.Copy();
            DataView dv = sumTable.DefaultView;
            dv.Sort = "CreationTime";
            sumTable = dv.ToTable();

            //西药
            Decimal MedicineSum = 0.00M;
            //中药
            Decimal ChineseSum = 0.00M;
            //诊疗
            Decimal CureSum = 0.00M;
            //检查
            Decimal CehckSum = 0.00M;

            Dictionary<string, int> NoDict = new Dictionary<string, int>();
            int NO = 1;
            foreach (DataRow dr in sumTable.Rows)
            {
                string prescriptionType = "";
                string adviceCode = dr["AdviceCode"].ToString();
                if (!NoDict.ContainsKey(adviceCode))
                {
                    NoDict.Add(adviceCode, NO++);
                }
                #region 求和
                object objectAdvice = dr["Object"];
                if (objectAdvice is IDrugAdvice)
                {
                    IDrugAdvice drug = objectAdvice as IDrugAdvice;
                    //if (drug.Frequence == 0 && drug.Method == 0)//中药
                    //{
                    //    ChineseSum += drug.Cash;
                    //}
                    //else
                    //{
                    //    MedicineSum += drug.Cash;
                    //}
                    MedicineSum += drug.Cash;

                    prescriptionType = drug.PrescriptionType;
                    dr["No"] = NoDict[adviceCode] + GetPrescription(prescriptionType);
                }
                else if (objectAdvice is ICureAdvice)
                {
                    ICureAdvice cure = objectAdvice as ICureAdvice;
                    if (cure.AdvState != WinnerHIS.Advice.AdviceState.IsStop)
                        CureSum += cure.Cash;
                    dr["No"] = NoDict[adviceCode];
                }
                else if (objectAdvice is ICheckAdvice)
                {
                    ICheckAdvice check = objectAdvice as ICheckAdvice;
                    CehckSum += check.Cash * Convert.ToDecimal(check.DISCOUNT);
                    dr["No"] = NoDict[adviceCode];
                }
                #endregion


            }


            labelControl1.Text = " ¥" + MedicineSum.ToString("F2");
            labelControl2.Text = " ¥" + ChineseSum.ToString("F2");
            labelControl3.Text = " ¥" + CureSum.ToString("F2");
            labelControl4.Text = " ¥" + CehckSum.ToString("F2");
            labelControl5.Text = " ¥" + (MedicineSum + ChineseSum + CureSum + CehckSum).ToString("F2");


            this.gridViewAdvice.ClearSorting();
            gridControlAdvice.DataSource = sumTable;
            AllCheck(false);
        }

        public string GetPrescription(string prescriptionCode)
        {
            if (prescriptionCode == "")
            {
                IGBCode obj = (IGBCode)prescriptionTypeList.Rows[0];
                return obj.InputCode2;
            }
            foreach (IGBCode obj in prescriptionTypeList.Rows)
            {
                if (obj.Code.ToString() == prescriptionCode)
                {
                    return obj.InputCode2;
                }

            }
            return "[普]";
        }

        /// <summary>
        /// 药品
        /// </summary>
        /// <param name="advDrug"></param>
        public void DrugTableAdd(IDrugAdvice advDrug)
        {
            if (advDrug.CreateTime > WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime().Date && advDrug.CreateTime < WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime().Date.AddDays(1))
            {
                #region 药品
                sumTable = this.gridControlAdvice.DataSource as DataTable;
                foreach (DataRow dr in sumTable.Rows)
                {
                    if (dr["Code"].ToString() == advDrug.DrugCode && (dr["AdvState"].ToString() == "-1"))
                    // && (dr["AdvState"].ToString() == "-1" || dr["AdvState"].ToString() == "1"))
                    {
                        XtraMessageBox.Show("该项目[" + advDrug.DrugName + "]已经存在，可以修改数量或者删除重开！");
                        return;
                    }
                }
                DataRow newRow = sumTable.NewRow();
                newRow["ID"] = advDrug.ID;
                newRow["AdviceCode"] = advDrug.AdviceCode.Trim();
                newRow["AdvState"] = (int)advDrug.AdvState;
                newRow["Code"] = advDrug.DrugCode;
                newRow["Content"] = advDrug.DrugName;
                if (advDrug.Method == 0 && advDrug.Frequence == 0)
                {
                    newRow["Mode"] = advDrug.Memo;
                    newRow["Days"] = advDrug.Days + "/" + advDrug.Pack;
                    newRow["Number"] = int.Parse(advDrug.Dosage) * advDrug.Pack;
                    newRow["Unit"] = advDrug.SmallUnit;
                }
                else
                {
                    newRow["Mode"] = ReturnMethod(advDrug.Method);
                    //newRow["Mode"] = advDrug.Method;
                    newRow["Frequency"] = advDrug.Frequence;
                    newRow["Days"] = advDrug.Days;

                    if (advDrug.Number % advDrug.PackRule == 0)
                    {
                        newRow["Number"] = advDrug.Number / advDrug.PackRule;
                        newRow["Unit"] = advDrug.BigUnit;
                    }
                    else
                    {
                        newRow["Number"] = advDrug.Number;
                        newRow["Unit"] = advDrug.SmallUnit;
                    }


                }
                newRow["Dosage"] = advDrug.Dosage + advDrug.DosageUnit;
                newRow["Spec"] = advDrug.Spec;
                newRow["Price"] = advDrug.Price;
                newRow["Cash"] = advDrug.Cash;
                newRow["CheckTime"] = advDrug.CheckTime.Date.ToString() == "1900-01-01" ? "" : advDrug.CheckTime.ToString();
                newRow["CreationTime"] = advDrug.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                newRow["CreationDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(advDrug.DeptID);
                newRow["CreationDoc"] = WinnerHIS.Common.DataConvertHelper.GetEmpName(advDrug.Creator);
                newRow["ExeDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(advDrug.ExecDept);
                newRow["GroupNo"] = advDrug.GroupNo == 0 ? "" : advDrug.GroupNo.ToString();
                newRow["GroupNoSymbol"] = advDrug.GroupNoSymbol;
                newRow["Remarks"] = advDrug.Remarks;
                newRow["Object"] = advDrug;
                newRow["CheckObj"] = false;
                sumTable.Rows.Add(newRow);
                #endregion
            }

        }
        /// <summary>
        /// 诊疗
        /// </summary>
        /// <param name="cureAdv"></param>
        public void CureTableAdd(ICureAdvice cureAdv)
        {
            if (cureAdv.CreateTime > WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime().Date && cureAdv.CreateTime < WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime().Date.AddDays(1))
            {
                #region 诊疗
                sumTable = this.gridControlAdvice.DataSource as DataTable;
                foreach (DataRow dr in sumTable.Rows)
                {
                    if (dr["Code"].ToString() == cureAdv.ItemCode
                        && dr["AdvState"].ToString() == "-1")
                    {
                        XtraMessageBox.Show("该项目已经存在，可以修改数量或者删除重开！");
                        return;
                    }
                }

                DataRow newRow = sumTable.NewRow();
                newRow["ID"] = cureAdv.ID;
                newRow["AdviceCode"] = cureAdv.AdviceCode.Trim();
                newRow["AdvState"] = (int)cureAdv.AdvState;
                newRow["Code"] = cureAdv.ItemCode;
                newRow["Content"] = cureAdv.ItemName;
                newRow["Frequency"] = cureAdv.Frequency;
                newRow["Days"] = cureAdv.Days;
                newRow["Spec"] = "--";
                newRow["Unit"] = cureAdv.Unit;
                newRow["Price"] = cureAdv.Price;
                newRow["Number"] = cureAdv.Number;
                newRow["Cash"] = cureAdv.Cash;
                newRow["CheckTime"] = cureAdv.CheckTime.Date.ToString() == "1900-01-01" ? "" : cureAdv.CheckTime.ToString();
                newRow["CreationTime"] = cureAdv.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                newRow["CreationDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(cureAdv.DeptID);
                newRow["CreationDoc"] = WinnerHIS.Common.DataConvertHelper.GetEmpName(cureAdv.Creator);
                newRow["ExeDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(cureAdv.ExecDept);
                newRow["GroupNo"] = cureAdv.GroupNo == 0 ? "" : cureAdv.GroupNo.ToString();
                newRow["GroupNoSymbol"] = cureAdv.GroupNoSymbol;
                newRow["Remarks"] = cureAdv.Remarks;
                newRow["Object"] = cureAdv;
                newRow["CheckObj"] = false;
                sumTable.Rows.Add(newRow);
                #endregion
            }
        }
        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="Advice"></param>
        public void CheckTableAdd(ICheckAdvice Advice)
        {
            if (Advice.CreateTime > WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime().Date && Advice.CreateTime < WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime().Date.AddDays(1))
            {
                #region 检查
                sumTable = this.gridControlAdvice.DataSource as DataTable;
                foreach (DataRow dr in sumTable.Rows)
                {
                    if (dr["Code"].ToString() == Advice.ItemCode
                        && dr["AdvState"].ToString() == "-1")
                    {
                        XtraMessageBox.Show("该项目已经存在，可以修改数量或者删除重开！");
                        return;
                    }
                }

                DataRow newRow = sumTable.NewRow();
                newRow["ID"] = Advice.ID;
                newRow["AdviceCode"] = Advice.AdviceCode.Trim();
                newRow["AdvState"] = (int)Advice.AdvState;
                newRow["Code"] = Advice.ItemCode;
                newRow["Content"] = Advice.ItemName;
                newRow["Mode"] = "--";
                newRow["Spec"] = "--";
                newRow["Days"] = "--";
                newRow["Unit"] = Advice.Unit;
                newRow["Price"] = (Advice.Price * Convert.ToDecimal(Advice.DISCOUNT)).ToString("F2");
                newRow["Number"] = Advice.Number;
                newRow["Cash"] = (Advice.Cash * Convert.ToDecimal(Advice.DISCOUNT)).ToString("F2");
                newRow["CheckTime"] = Advice.CheckTime.Date.ToString() == "1900-01-01" ? "" : Advice.CheckTime.ToString();
                newRow["CreationTime"] = Advice.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                newRow["CreationDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(Advice.DeptID);
                newRow["CreationDoc"] = WinnerHIS.Common.DataConvertHelper.GetEmpName(Advice.Creator);
                newRow["ExeDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(Advice.ExecDept);
                newRow["GroupNo"] = Advice.GroupNo == 0 ? "" : Advice.GroupNo.ToString();
                newRow["GroupNoSymbol"] = Advice.GroupNoSymbol;
                newRow["Remarks"] = Advice.Remarks;
                newRow["Object"] = Advice;
                newRow["CheckObj"] = false;
                sumTable.Rows.Add(newRow);
                #endregion
            }
        }
        /// <summary>
        /// 根据编码返回频率
        /// </summary>
        /// <returns></returns>
        public string ReturnFrequency(int FrequencyCode)
        {
            if (FrequencyCode == 0)
            {
                return "";
            }
            return WinnerHIS.Common.DataConvertHelper.GetGbCode(FrequencyCode).Name;
        }
        /// <summary>
        /// 返回用药方式
        /// </summary>
        /// <param name="Method"></param>
        /// <returns></returns>
        public string ReturnMethod(int Method)
        {
            try
            {
                if (Method == 0)
                {
                    return "";
                }
                return WinnerHIS.Common.DataConvertHelper.GetGbCode(Method).Name;
            }
            catch (Exception)
            {
                return Method.ToString();
            }

        }


        #endregion

        #region gridViewAdvice相关
        /// <summary>
        /// 行的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewAdvice_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            DataRow dr = gridViewAdvice.GetDataRow(e.RowHandle);
            if (dr != null)
            {
                String State = dr["AdvState"].ToString();
                if (State == "-1")//未保存
                {
                    e.Appearance.ForeColor = NotSavedBar.ItemAppearance.Normal.ForeColor;
                }
                else if (State == "1")//已审核
                {
                    e.Appearance.ForeColor = ReviewedBar.ItemAppearance.Normal.ForeColor;
                }
                else if (State == "2")//已收费
                {
                    e.Appearance.ForeColor = ChargedBar.ItemAppearance.Normal.ForeColor;
                }
                else if (State == "3")//已执行
                {
                    e.Appearance.ForeColor = ExecutedBar.ItemAppearance.Normal.ForeColor;
                }
                else if (State == "4")//已退费
                {
                    e.Appearance.ForeColor = RetiredBar.ItemAppearance.Normal.ForeColor;
                }
                else//作废
                {
                    e.Appearance.ForeColor = checkCacel.ItemAppearance.Normal.ForeColor;
                }
            }
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewAdvice_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            #region 合并行
            GridView view = sender as GridView;
            string GroupName = "GroupNo";
            //组号
            string GroupNametColumn1 = Convert.ToString(view.GetRowCellValue(e.RowHandle1, view.Columns[GroupName]));
            string GroupNametColumn2 = Convert.ToString(view.GetRowCellValue(e.RowHandle2, view.Columns[GroupName]));
            if (e.Column.FieldName == GroupName)//组号判断
            {
                if (GroupNametColumn1 == "" && GroupNametColumn2 == "")
                {
                    e.Merge = false;
                    e.Handled = true;
                }
                else if (GroupNametColumn1 == "0" && GroupNametColumn2 == "0")
                {
                    e.Merge = false;
                    e.Handled = true;
                }
                else
                {
                    e.Merge = GroupNametColumn1 == GroupNametColumn2;
                    e.Handled = true;
                }
            }
            else
            {
                string NameColumn1 = Convert.ToString(view.GetRowCellValue(e.RowHandle1, view.Columns[e.Column.FieldName]));
                string NameColumn2 = Convert.ToString(view.GetRowCellValue(e.RowHandle2, view.Columns[e.Column.FieldName]));
                if (e.Column.FieldName == "AdviceCode" && NameColumn1 == NameColumn2)
                {
                    e.Merge = true;
                    e.Handled = true;
                }
                else if (e.Column.FieldName == "No" && NameColumn1 == NameColumn2)
                {
                    e.Merge = true;
                    e.Handled = true;
                }
                else if ((GroupNametColumn1 == "" && GroupNametColumn2 == "") || (GroupNametColumn1 == "0" && GroupNametColumn2 == "0"))
                {
                    e.Merge = false;
                    e.Handled = true;
                }
                else
                {
                    e.Merge = GroupNametColumn1 == GroupNametColumn2 && NameColumn1 == NameColumn2;
                    e.Handled = true;
                }
            }
            #endregion
        }
        /// <summary>
        /// gridViewAdvice 选中更改CheckObj的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemCheckEdit6_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit chEdit = sender as DevExpress.XtraEditors.CheckEdit;
            DataRow dr = gridViewAdvice.GetFocusedDataRow();
            if (chEdit.CheckState == CheckState.Checked)
            {
                dr["CheckObj"] = true;
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)//Ctrl健被按下,则不支持多选
                {
                    return;
                }
                DataTable tb = this.gridControlAdvice.DataSource as DataTable;
                foreach (DataRow row in tb.Rows)
                {
                    if (dr["GroupNo"].ToString() != "")
                    {
                        if (dr["GroupNo"].ToString() == row["GroupNo"].ToString())
                        {
                            row["CheckObj"] = true;
                        }

                    }
                    else
                    {
                        if (dr["AdviceCode"].ToString() == row["AdviceCode"].ToString())
                        {
                            row["CheckObj"] = true;
                        }
                    }
                }
            }
            else
            {
                dr["CheckObj"] = false;
            }
        }
        /// <summary>
        /// 单击选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewAdvice_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // 判断是否是用鼠标点击  
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo ghi = gridViewAdvice.CalcHitInfo(new Point(e.X, e.Y));
                if (ghi.InRow)  // 判断光标是否在行内  
                {
                    if ((bool)gridViewAdvice.GetDataRow(ghi.RowHandle)["CheckObj"] == true)
                    {
                        gridViewAdvice.GetDataRow(e.RowHandle)["CheckObj"] = false;
                    }
                    else
                    {
                        gridViewAdvice.GetDataRow(e.RowHandle)["CheckObj"] = true;
                    }
                }
            }
        }
        /// <summary>
        /// 双击修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewAdvice_DoubleClick(object sender, EventArgs e)
        {
            //int updateNum = 0;
            //DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            //if (row != null)
            //{
            //    object objectAdvice = row["Object"];
            //    if (row["AdvState"].ToString() == "2")
            //    {
            //        XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
            //        return;
            //    }
            //    if (row["AdvState"].ToString() == "3")
            //    {
            //        XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
            //        return;
            //    }
            //    PrescriptionUpdate From = null;

            //    #region 判断类型
            //    if (objectAdvice is IDrugAdvice)
            //    {
            //        IDrugAdvice obj = objectAdvice as IDrugAdvice;

            //        int Original = (int)obj.AdvState;
            //        if (obj.AdvState != AdviceState.NotSave)
            //            obj.Refresh();
            //        if (Original != (int)obj.AdvState)
            //        {
            //            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
            //            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
            //            return;
            //        }
            //        if (Original == 2)
            //        {
            //            XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
            //            return;
            //        }
            //        if (Original == 3)
            //        {
            //            XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
            //            return;
            //        }

            //        if (obj.Type == (int)Common.DrugTypeEnum.Western || obj.Type == (int)Common.DrugTypeEnum.ChinesePatent)
            //        {
            //            From = new PrescriptionUpdate(this, "西药", objectAdvice);
            //        }
            //        else if (obj.Type == (int)Common.DrugTypeEnum.ChineseHerbal)
            //        {
            //            From = new PrescriptionUpdate(this, "中药", objectAdvice);
            //        }
            //        else if (obj.Type == (int)Common.DrugTypeEnum.Material)
            //        {
            //            From = new PrescriptionUpdate(this, "耗材", objectAdvice);
            //        }
            //        else
            //        {
            //            From = new PrescriptionUpdate(this, obj.Type.ToString(), objectAdvice);
            //        }
            //    }
            //    else if (objectAdvice is ICureAdvice)
            //    {
            //        ICureAdvice obj = objectAdvice as ICureAdvice;
            //        int Original = (int)obj.AdvState;
            //        if (obj.AdvState != AdviceState.NotSave)
            //            obj.Refresh();
            //        if (Original != (int)obj.AdvState)
            //        {
            //            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
            //            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
            //            return;
            //        }
            //        if (obj.ItemCode.Substring(0, 2).ToString() == DiscountItemCode)
            //        {
            //            From = new PrescriptionUpdate(this, "优惠", objectAdvice);
            //        }
            //        else
            //        {
            //            From = new PrescriptionUpdate(this, "诊疗", objectAdvice);
            //        }
            //    }
            //    else if (objectAdvice is ICheckAdvice)
            //    {
            //        From = new PrescriptionUpdate(this, "检查", objectAdvice);
            //        ICheckAdvice check = objectAdvice as ICheckAdvice;


            //        int Original = (int)check.AdvState;
            //        if (check.AdvState != AdviceState.NotSave)
            //            check.Refresh();
            //        if (Original != (int)check.AdvState)
            //        {
            //            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
            //            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
            //            return;
            //        }


            //        if (check.ItemCode.Substring(0, 2).ToString() == "AP")
            //        {
            //            XtraMessageBox.Show("打包好的项目,无法进行修改！");
            //            return;
            //        }

            //    }
            //    #endregion

            //    if (From.ShowDialog() == DialogResult.OK)
            //    {
            //        row["Object"] = From.Tag;
            //        #region 药品、耗材
            //        if (objectAdvice is IDrugAdvice)
            //        {
            //            IDrugAdvice adv = objectAdvice as IDrugAdvice;
            //            //row["ID"] = Advice.ID;
            //            row["AdviceCode"] = adv.AdviceCode.Trim();
            //            row["AdvState"] = (int)adv.AdvState;
            //            row["Code"] = adv.DrugCode;
            //            row["Content"] = adv.DrugName;
            //            if (adv.Method == 0 && adv.Frequence == 0)
            //            {
            //                row["Mode"] = adv.Memo;
            //                row["Days"] = adv.Days + "/" + adv.Pack;
            //                row["Unit"] = adv.SmallUnit;
            //                row["Number"] = adv.Number * adv.Pack;
            //            }
            //            else
            //            {
            //                row["Mode"] = ReturnMethod(adv.Method) + " " + ReturnFrequency(adv.Frequence);
            //                row["Frequency"] = ReturnFrequency(adv.Frequence);
            //                row["Days"] = adv.Days;
            //                if (adv.Number % adv.PackRule == 0)
            //                {
            //                    row["Number"] = adv.Number / adv.PackRule;
            //                    row["Unit"] = adv.BigUnit;
            //                }
            //                else
            //                {
            //                    row["Number"] = adv.Number;
            //                    row["Unit"] = adv.SmallUnit;
            //                }

            //            }
            //            row["Dosage"] = adv.Dosage + adv.DosageUnit;
            //            row["Spec"] = adv.Spec;
            //            row["Price"] = adv.Price;
            //            row["Cash"] = adv.Cash;
            //            row["CheckTime"] = adv.CheckTime.Date.ToString() == "1900-01-01" ? "" : adv.CheckTime.ToString();
            //            row["CreationTime"] = adv.CreateTime;
            //            row["CreationDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(adv.DeptID);
            //            row["CreationDoc"] = WinnerHIS.Common.DataConvertHelper.GetEmpName(adv.Creator);
            //            row["ExeDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(adv.ExecDept);
            //            row["GroupNo"] = adv.GroupNo == 0 ? "" : adv.GroupNo.ToString();
            //            row["GroupNoSymbol"] = adv.GroupNoSymbol;
            //            row["Object"] = adv;
            //            row["CheckObj"] = false;
            //            row["Remarks"] = adv.Remarks;
            //            if (adv.AdvState != AdviceState.NotSave)
            //            {
            //                row["ID"] = adv.ID;
            //                adv.RestoreUpdatePK();
            //                updateNum = adv.Update();
            //                if (updateNum == 0)
            //                {
            //                    XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
            //                    SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
            //                    return;
            //                }
            //            }
            //        }
            //        #endregion
            //        #region 诊疗
            //        else if (objectAdvice is ICureAdvice)
            //        {
            //            ICureAdvice Advice = objectAdvice as ICureAdvice;
            //            //row["ID"] = Advice.ID;
            //            row["AdviceCode"] = Advice.AdviceCode.Trim();
            //            row["AdvState"] = (int)Advice.AdvState;
            //            row["Code"] = Advice.ItemCode;
            //            row["Content"] = Advice.ItemName;
            //            row["Frequency"] = ReturnFrequency(Advice.Frequency);
            //            row["Spec"] = "--";
            //            row["Days"] = Advice.Days.ToString();
            //            row["Unit"] = Advice.Unit;
            //            row["Price"] = Advice.Price;
            //            row["Number"] = Advice.Number;
            //            row["Cash"] = Advice.Cash;
            //            row["Number"] = Advice.Number;
            //            row["CheckTime"] = Advice.CheckTime.Date.ToString() == "1900-01-01" ? "" : Advice.CheckTime.ToString();
            //            row["CreationTime"] = Advice.CreateTime;
            //            row["CreationDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(Advice.DeptID);
            //            row["CreationDoc"] = WinnerHIS.Common.DataConvertHelper.GetEmpName(Advice.Creator);
            //            row["ExeDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(Advice.ExecDept);
            //            row["GroupNo"] = Advice.GroupNo == 0 ? "" : Advice.GroupNo.ToString();
            //            row["GroupNoSymbol"] = Advice.GroupNoSymbol;
            //            row["Object"] = Advice;
            //            row["CheckObj"] = false;
            //            row["Remarks"] = Advice.Remarks;
            //            if (Advice.AdvState != AdviceState.NotSave)
            //            {
            //                row["ID"] = Advice.ID;
            //                Advice.RestoreUpdatePK();
            //                updateNum = Advice.Update();
            //                if (updateNum == 0)
            //                {
            //                    XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
            //                    SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
            //                    return;
            //                }
            //            }


            //        }
            //        #endregion
            //        #region 检查
            //        else if (objectAdvice is ICheckAdvice)
            //        {
            //            ICheckAdvice Advice = objectAdvice as ICheckAdvice;
            //            //row["ID"] = Advice.ID;
            //            row["AdviceCode"] = Advice.AdviceCode.Trim();
            //            row["AdvState"] = (int)Advice.AdvState;
            //            row["Code"] = Advice.ItemCode;
            //            row["Content"] = Advice.ItemName;
            //            row["Mode"] = "--";
            //            row["Spec"] = "--";
            //            row["Unit"] = Advice.Unit;
            //            row["Price"] = Advice.Price;
            //            row["Number"] = Advice.Number;
            //            row["Cash"] = Advice.Cash;
            //            row["Number"] = Advice.Number;
            //            row["CheckTime"] = Advice.CheckTime.Date.ToString() == "1900-01-01" ? "" : Advice.CheckTime.ToString();
            //            row["CreationTime"] = Advice.CreateTime;
            //            row["CreationDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(Advice.DeptID);
            //            row["CreationDoc"] = WinnerHIS.Common.DataConvertHelper.GetEmpName(Advice.Creator);
            //            row["ExeDept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(Advice.ExecDept);
            //            row["GroupNo"] = Advice.GroupNo == 0 ? "" : Advice.GroupNo.ToString();
            //            row["GroupNoSymbol"] = Advice.GroupNoSymbol;
            //            row["Object"] = Advice;
            //            row["CheckObj"] = false;
            //            row["Remarks"] = Advice.Remarks;
            //            if (Advice.AdvState != AdviceState.NotSave)
            //            {
            //                row["ID"] = Advice.ID;
            //                Advice.RestoreUpdatePK();
            //                updateNum = Advice.Update();
            //                if (updateNum == 0)
            //                {
            //                    XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
            //                    SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
            //                    return;
            //                }
            //            }
            //        }
            //        #endregion
            //        Binding();
            //    }

            //}

        }
        #endregion

        #region barManager相关按钮事件
        /// <summary>
        /// 开立
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //RecipeCreationForm rcFrom = new RecipeCreationForm(this, null, 1);
            //rcFrom.AdvTypeDefault = advTypeDefault;
            //rcFrom.inputCompleted += new RecipeCreationForm.InputCompletedHandler(drugInput_InputCompleted);
            //rcFrom.ShowDialog();
        }
        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drugInput_InputCompleted(object sender, object  inputArg)
        {
            //RecipeCreationForm rcFrom = sender as RecipeCreationForm;
            //int advType = inputArg.AdvType;
            //DateTime nowDate = Common.ContextHelper.ServerTime.GetCurrentTime();
            //DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);

            //AdviceState AdvState = AdviceState.NotSave;
            //if (row != null)
            //{
            //    object obj = row["Object"] as object;
            //    if (obj is IDrugAdvice)
            //    {
            //        IDrugAdvice Adv = (IDrugAdvice)row["Object"];
            //        AdvState = Adv.AdvState;
            //    }
            //    else if (obj is ICureAdvice)
            //    {
            //        ICureAdvice Adv = (ICureAdvice)row["Object"];
            //        AdvState = Adv.AdvState;
            //    }
            //    else if (obj is ICheckAdvice)
            //    {
            //        ICheckAdvice Adv = (ICheckAdvice)row["Object"];
            //        AdvState = Adv.AdvState;
            //    }
            //}

            //switch (advType)
            //{
            //    case 0:
            //        #region 西药
            //        IDrugAdvice drugXYAdvice = rcFrom.Tag as IDrugAdvice;
            //        if (rcFrom.AddType == 2 && drugXYAdvice.AdviceCode != string.Empty)
            //        {
            //            //IDrugAdvice drgAdv = (IDrugAdvice)row["Object"];
            //            if (AdvState == AdviceState.NotSave)
            //            {
            //                drugXYAdvice.Patient = currentCode.Patient;
            //                drugXYAdvice.RegCode = currentCode.Code;
            //                drugXYAdvice.Name = currentCode.Name;
            //                drugXYAdvice.CardID = currentCode.CardID;
            //                drugXYAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                drugXYAdvice.CheckTime = nowDate;
            //                drugXYAdvice.AdvState = AdviceState.NotSave;
            //            }
            //            else
            //            {
            //                drugXYAdvice.ID = drugXYAdvice.GetMaxID();
            //                drugXYAdvice.Patient = currentCode.Patient;
            //                drugXYAdvice.RegCode = currentCode.Code;
            //                drugXYAdvice.Name = currentCode.Name;
            //                drugXYAdvice.CardID = currentCode.CardID;
            //                drugXYAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                drugXYAdvice.CheckTime = nowDate;
            //                drugXYAdvice.AdvState = AdviceState.IsAuditing;
            //                drugXYAdvice.Insert();
            //            }

            //        }
            //        this.DrugTableAdd(drugXYAdvice);
            //        break;
            //    #endregion
            //    case 1:
            //        #region 中药
            //        IDrugAdvice drugZYAdvice = rcFrom.Tag as IDrugAdvice;
            //        if (rcFrom.AddType == 2 && drugZYAdvice.AdviceCode != string.Empty)
            //        {
            //            //IDrugAdvice drgAdv = (IDrugAdvice)row["Object"];
            //            if (AdvState == AdviceState.NotSave)
            //            {
            //                drugZYAdvice.Patient = currentCode.Patient;
            //                drugZYAdvice.RegCode = currentCode.Code;
            //                drugZYAdvice.Name = currentCode.Name;
            //                drugZYAdvice.CardID = currentCode.CardID;
            //                drugZYAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                drugZYAdvice.CheckTime = nowDate;
            //                drugZYAdvice.AdvState = AdviceState.NotSave;
            //            }
            //            else
            //            {
            //                drugZYAdvice.ID = drugZYAdvice.GetMaxID();
            //                drugZYAdvice.Patient = currentCode.Patient;
            //                drugZYAdvice.RegCode = currentCode.Code;
            //                drugZYAdvice.Name = currentCode.Name;
            //                drugZYAdvice.CardID = currentCode.CardID;
            //                drugZYAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                drugZYAdvice.CheckTime = nowDate;
            //                drugZYAdvice.AdvState = AdviceState.IsAuditing;
            //                drugZYAdvice.Insert();
            //            }
            //        }
            //        this.DrugTableAdd(drugZYAdvice);
            //        break;
            //    #endregion
            //    case 2:
            //        #region 诊疗
            //        ICureAdvice adviceAdd = rcFrom.Tag as ICureAdvice;
            //        if (rcFrom.AddType == 2 && adviceAdd.AdviceCode != string.Empty)
            //        {
            //            //ICureAdvice curegAdv = (ICureAdvice)row["Object"];
            //            if (AdvState == AdviceState.NotSave)
            //            {
            //                adviceAdd.Patient = currentCode.Patient;
            //                adviceAdd.RegCode = currentCode.Code;
            //                adviceAdd.Name = currentCode.Name;
            //                adviceAdd.CardID = currentCode.CardID;
            //                adviceAdd.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                adviceAdd.CheckTime = nowDate;
            //                adviceAdd.AdvState = AdviceState.NotSave;
            //            }
            //            else
            //            {
            //                adviceAdd.ID = adviceAdd.GetMaxID();
            //                adviceAdd.Patient = currentCode.Patient;
            //                adviceAdd.RegCode = currentCode.Code;
            //                adviceAdd.Name = currentCode.Name;
            //                adviceAdd.CardID = currentCode.CardID;
            //                adviceAdd.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                adviceAdd.CheckTime = nowDate;
            //                adviceAdd.AdvState = AdviceState.IsAuditing;
            //                adviceAdd.Insert();
            //            }

            //        }

            //        this.CureTableAdd(adviceAdd);
            //        break;
            //    #endregion
            //    case 3:
            //        #region 检查
            //        ICheckAdvice checkAdvice = rcFrom.Tag as ICheckAdvice;
            //        if (rcFrom.AddType == 2 && checkAdvice.AdviceCode != string.Empty)
            //        {
            //            //ICheckAdvice ckgAdv = (ICheckAdvice)row["Object"];
            //            if (AdvState == AdviceState.NotSave)
            //            {
            //                checkAdvice.Patient = currentCode.Patient;
            //                checkAdvice.RegCode = currentCode.Code;
            //                checkAdvice.Name = currentCode.Name;
            //                checkAdvice.CARDID = currentCode.CardID;
            //                checkAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                checkAdvice.CheckTime = nowDate;
            //                checkAdvice.AdvState = AdviceState.NotSave;
            //            }
            //            else
            //            {
            //                checkAdvice.ID = checkAdvice.GetMaxID();
            //                checkAdvice.Patient = currentCode.Patient;
            //                checkAdvice.RegCode = currentCode.Code;
            //                checkAdvice.Name = currentCode.Name;
            //                checkAdvice.CARDID = currentCode.CardID;
            //                checkAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                checkAdvice.CheckTime = nowDate;
            //                checkAdvice.AdvState = AdviceState.IsAuditing;
            //                checkAdvice.Insert();
            //            }

            //        }

            //        this.CheckTableAdd(checkAdvice);
            //        break;
            //    #endregion
            //    case 4:
            //        #region 优惠
            //        ICureAdvice cureYHAdvice = rcFrom.Tag as ICureAdvice;
            //        if (rcFrom.AddType == 2 && cureYHAdvice.AdviceCode != string.Empty)
            //        {
            //            //ICureAdvice ckgAdv = (ICureAdvice)row["Object"];
            //            if (AdvState == AdviceState.NotSave)
            //            {
            //                cureYHAdvice.Patient = currentCode.Patient;
            //                cureYHAdvice.RegCode = currentCode.Code;
            //                cureYHAdvice.Name = currentCode.Name;
            //                cureYHAdvice.CardID = currentCode.CardID;
            //                cureYHAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                cureYHAdvice.CheckTime = nowDate;
            //                cureYHAdvice.AdvState = AdviceState.NotSave;
            //            }
            //            else
            //            {
            //                cureYHAdvice.ID = cureYHAdvice.GetMaxID();
            //                cureYHAdvice.Patient = currentCode.Patient;
            //                cureYHAdvice.RegCode = currentCode.Code;
            //                cureYHAdvice.Name = currentCode.Name;
            //                cureYHAdvice.CardID = currentCode.CardID;
            //                cureYHAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                cureYHAdvice.CheckTime = nowDate;
            //                cureYHAdvice.AdvState = AdviceState.IsAuditing;
            //                cureYHAdvice.Insert();
            //            }

            //            if (cureYHAdvice.Cash < 0 && cmCouponCheckFlag)//门诊优惠申请启用则插入优惠申请表
            //            {
            //                ICouponCheckApply couponApply = WinnerHIS.Analyse.Decision.DAL.Interface.DALHelper.DALManager.CreateCouponCheckApply();
            //                couponApply.Session = WinnerHIS.Common.ContextHelper.Session;
            //                couponApply.CardID = currentCode.CardID;
            //                couponApply.PatientNo = currentCode.Code;
            //                couponApply.ItemCode = cureYHAdvice.ItemCode;
            //                couponApply.ItemName = cureYHAdvice.ItemName;
            //                couponApply.PName = currentCode.Name;
            //                couponApply.AdviceCode = cureYHAdvice.AdviceCode;
            //                couponApply.Unit = cureYHAdvice.Unit;
            //                couponApply.Price = cureYHAdvice.Price;
            //                couponApply.Number = cureYHAdvice.Number;
            //                couponApply.Cash = cureYHAdvice.Cash;
            //                couponApply.Status = 0;//未审核
            //                couponApply.InType = 0;//门诊
            //                couponApply.CreateTime = WinnerHIS.Common.ContextHelper.CurrentTime;
            //                couponApply.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
            //                couponApply.Insert();
            //            }
            //        }

            //        this.CureTableAdd(cureYHAdvice);
            //        break;
            //    #endregion
            //    case 5:
            //        #region 耗材
            //        IDrugAdvice materialAdvice = rcFrom.Tag as IDrugAdvice;
            //        if (rcFrom.AddType == 2 && materialAdvice.AdviceCode != string.Empty)
            //        {
            //            //ICureAdvice ckgAdv = (ICureAdvice)row["Object"];
            //            if (AdvState == AdviceState.NotSave)
            //            {
            //                materialAdvice.Patient = currentCode.Patient;
            //                materialAdvice.RegCode = currentCode.Code;
            //                materialAdvice.Name = currentCode.Name;
            //                materialAdvice.CardID = currentCode.CardID;
            //                materialAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                materialAdvice.CheckTime = nowDate;
            //                materialAdvice.AdvState = AdviceState.NotSave;
            //            }
            //            else
            //            {
            //                materialAdvice.ID = materialAdvice.GetMaxID();
            //                materialAdvice.Patient = currentCode.Patient;
            //                materialAdvice.RegCode = currentCode.Code;
            //                materialAdvice.Name = currentCode.Name;
            //                materialAdvice.CardID = currentCode.CardID;
            //                materialAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                materialAdvice.CheckTime = nowDate;
            //                materialAdvice.AdvState = AdviceState.IsAuditing;
            //                materialAdvice.Insert();
            //            }
            //        }

            //        this.DrugTableAdd(materialAdvice);
            //        break;
            //    #endregion
            //    default:
            //        IDrugAdvice drugAdvice = rcFrom.Tag as IDrugAdvice;
            //        if (rcFrom.AddType == 2 && drugAdvice.AdviceCode != string.Empty)
            //        {
            //            //IDrugAdvice drgAdv = (IDrugAdvice)row["Object"];
            //            if (AdvState == AdviceState.NotSave)
            //            {
            //                drugAdvice.Patient = currentCode.Patient;
            //                drugAdvice.RegCode = currentCode.Code;
            //                drugAdvice.Name = currentCode.Name;
            //                drugAdvice.CardID = currentCode.CardID;
            //                drugAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                drugAdvice.CheckTime = nowDate;
            //                drugAdvice.AdvState = AdviceState.NotSave;
            //            }
            //            else
            //            {
            //                drugAdvice.ID = drugAdvice.GetMaxID();
            //                drugAdvice.Patient = currentCode.Patient;
            //                drugAdvice.RegCode = currentCode.Code;
            //                drugAdvice.Name = currentCode.Name;
            //                drugAdvice.CardID = currentCode.CardID;
            //                drugAdvice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
            //                drugAdvice.CheckTime = nowDate;
            //                drugAdvice.AdvState = AdviceState.IsAuditing;
            //                drugAdvice.Insert();
            //            }
            //        }
            //        this.DrugTableAdd(drugAdvice);
            //        break;
            //}
            //this.Binding();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bool sf = true;
            DataTable tb = this.gridControlAdvice.DataSource as DataTable;
            for (int i = tb.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = tb.Rows[i];
                if (Convert.ToBoolean(row["CheckObj"].ToString()))
                {
                    if (row["AdvState"].ToString() == "2")
                    {
                        XtraMessageBox.Show("已经收费的医嘱无法再进行修改！");
                        return;
                    }
                    if (row["AdvState"].ToString() == "3")
                    {
                        XtraMessageBox.Show("已执行的医嘱无法再进行修改！");
                        return;
                    }
                    string State = row["AdvState"].ToString();
                    if (State == "1")
                    {
                        object obj = row["Object"] as object;
                        if (obj is IDrugAdvice)
                        {
                            IDrugAdvice Advice = obj as IDrugAdvice;
                            int Original = (int)Advice.AdvState;
                            Advice.Refresh();
                            if (Original != (int)Advice.AdvState)
                            {
                                XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                                SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                                return;
                            }
                            if (Original == 2)
                            {
                                XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
                                return;
                            }
                            if (Original == 3)
                            {
                                XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
                                return;
                            }
                            Advice.Delete();

                        }
                        else if (obj is ICureAdvice)
                        {
                            ICureAdvice Advice = obj as ICureAdvice;
                            int Original = (int)Advice.AdvState;
                            Advice.Refresh();
                            if (Original != (int)Advice.AdvState)
                            {
                                XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                                SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                                return;
                            }
                            if (Original == 2)
                            {
                                XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
                                return;
                            }
                            if (Original == 3)
                            {
                                XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
                                return;
                            }
                            if (Advice.Price < 0 && _NewWorkstation.cmCouponCheckFlag)//启用了收费优惠审核
                            {
                                ICouponCheckApplyList couponApplyList = WinnerHIS.Analyse.Decision.DAL.Interface.DALHelper.DALManager.CreateCouponCheckApplyList();
                                couponApplyList.Session = Common.ContextHelper.Session;
                                ICouponCheckApply couponApply = couponApplyList.GetEntity(Advice.AdviceCode, Advice.ItemCode);
                                if (couponApply != null)
                                {
                                    couponApply.Refresh();
                                    couponApply.Delete();
                                }
                            }
                            Advice.Delete();
                        }
                        else if (obj is ICheckAdvice)
                        {
                            ICheckAdvice Advice = obj as ICheckAdvice;
                            int Original = (int)Advice.AdvState;
                            Advice.Refresh();
                            if (Original != (int)Advice.AdvState)
                            {
                                XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                                SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                                return;
                            }
                            if (Original == 2)
                            {
                                XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
                                return;
                            }
                            if (Original == 3)
                            {
                                XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
                                return;
                            }
                            Advice.Delete();
                        }
                    }
                    tb.Rows.Remove(row);
                    sf = false;

                }
            }
            if (sf)
            {
                XtraMessageBox.Show("请勾选再删除！");
                return;
            }
            Binding();


            XtraMessageBox.Show("删除成功！");
        }
        /// <summary>
        /// 一组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int updateNum = 0;
            DataTable tb = this.gridControlAdvice.DataSource as DataTable;
            #region 判断
            //选中的行数，至少>=2
            int CountCheck = 0;


            int MaxTb = 0;
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                DataRow dr = tb.Rows[i];
                if (Convert.ToBoolean(dr["CheckObj"]))
                {
                    CountCheck++;
                    if (dr["AdvState"].ToString() == "2")
                    {
                        XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
                        return;
                    }
                    if (dr["AdvState"].ToString() == "3")
                    {
                        XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
                        return;
                    }
                }

                if (dr["GroupNo"].ToString() != "")
                {
                    int No = -1;
                    int.TryParse(dr["GroupNo"].ToString(), out No);
                    if (No > MaxTb)
                    {
                        MaxTb = No;
                    }

                }
            }
            #endregion

            if (CountCheck < 2)
            {
                XtraMessageBox.Show("一组,处方数量必须大于2！");
                return;
            }

            int ChekID = CountCheck;
            int MaxGroupNo = WinnerHIS.Common.ContextHelper.CM_MaxGroupNo(_NewWorkstation.currentReg.Code);
            if (MaxTb >= MaxGroupNo)
            {
                MaxGroupNo = MaxTb + 1;
            }

            DateTime CreateData = Convert.ToDateTime("1900-01-01 00:00:00.000");
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                DataRow dr = tb.Rows[i];
                if (Convert.ToBoolean(dr["CheckObj"]))
                {
                    #region 符号赋值
                    string fuhao = "";
                    if (CountCheck == ChekID)
                    {
                        fuhao = " ┑";
                    }
                    else if (ChekID == 1)
                    {
                        fuhao = " ┙";
                    }
                    else
                    {
                        fuhao = " |";

                    }
                    #endregion
                    #region 时间
                    if (CreateData == Convert.ToDateTime("1900-01-01 00:00:00.000"))
                    {
                        CreateData = Convert.ToDateTime(dr["CreationTime"].ToString());
                    }
                    else
                    {
                        CreateData = CreateData.AddMilliseconds(1000);

                    }
                    #endregion
                    #region 修改Object对象
                    object objectAdvice = dr["Object"];
                    if (objectAdvice is IDrugAdvice)
                    {
                        IDrugAdvice drug = objectAdvice as IDrugAdvice;

                        int Original = (int)drug.AdvState;
                        if (Original != (int)drug.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            return;
                        }


                        drug.GroupNoSymbol = fuhao;
                        drug.CreateTime = CreateData;
                        drug.GroupNo = MaxGroupNo;
                        tb.Rows[i]["Object"] = drug;
                        if (drug.AdvState != AdviceState.NotSave)
                        {
                            drug.RestoreUpdatePK();
                            updateNum = drug.Update();
                            if (updateNum == 0)
                            {
                                XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                                //SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                                return;
                            }
                        }
                    }
                    else if (objectAdvice is ICureAdvice)
                    {
                        ICureAdvice cure = objectAdvice as ICureAdvice;

                        int Original = (int)cure.AdvState;
                        if (Original != (int)cure.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            return;
                        }

                        cure.GroupNoSymbol = fuhao;
                        cure.GroupNo = MaxGroupNo;
                        cure.CreateTime = CreateData;
                        tb.Rows[i]["Object"] = cure;
                        if (cure.AdvState != AdviceState.NotSave)
                        {
                            cure.RestoreUpdatePK();
                            updateNum = cure.Update();
                            if (updateNum == 0)
                            {
                                XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                                //SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                                return;
                            }
                        }
                    }
                    else if (objectAdvice is ICheckAdvice)
                    {
                        ICheckAdvice check = objectAdvice as ICheckAdvice;


                        int Original = (int)check.AdvState;
                        if (Original != (int)check.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            return;
                        }

                        check.GroupNoSymbol = fuhao;
                        check.GroupNo = MaxGroupNo;
                        check.CreateTime = CreateData;
                        tb.Rows[i]["Object"] = check;
                        if (check.AdvState != AdviceState.NotSave)
                        {
                            check.RestoreUpdatePK();
                            updateNum = check.Update();
                            if (updateNum == 0)
                            {
                                XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                                //SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                                return;
                            }
                        }
                    }
                    tb.Rows[i]["CreationTime"] = CreateData;
                    tb.Rows[i]["GroupNoSymbol"] = fuhao;
                    tb.Rows[i]["GroupNo"] = MaxGroupNo;
                    #endregion
                    ChekID--;
                }
            }
            XtraMessageBox.Show("标记一组成功！");
            AllCheck(false);

        }
        /// <summary>
        /// 取消一组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelAGroup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //是否选中了
            bool sf = true;
            int updateNum = 0;
            DataTable tb = this.gridControlAdvice.DataSource as DataTable;
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                DataRow dr = tb.Rows[i];
                if (Convert.ToBoolean(dr["CheckObj"]))
                {

                    #region 修改Object对象
                    object objectAdvice = dr["Object"];
                    if (objectAdvice is IDrugAdvice)
                    {
                        IDrugAdvice drug = objectAdvice as IDrugAdvice;

                        int Original = (int)drug.AdvState;
                        if (Original != (int)drug.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            return;
                        }
                        drug.GroupNoSymbol = "";
                        drug.GroupNo = 0;
                        tb.Rows[i]["Object"] = drug;
                        tb.Rows[i]["Content"] = drug.DrugName;
                        if (drug.AdvState != AdviceState.NotSave)
                        {
                            drug.RestoreUpdatePK();
                            updateNum = drug.Update();
                            if (updateNum == 0)
                            {
                                XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                                //SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                                return;
                            }
                        }
                    }
                    else if (objectAdvice is ICureAdvice)
                    {
                        ICureAdvice cure = objectAdvice as ICureAdvice;

                        int Original = (int)cure.AdvState;
                        cure.Refresh();
                        if (Original != (int)cure.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            return;
                        }


                        cure.GroupNoSymbol = "";
                        cure.GroupNo = 0;
                        tb.Rows[i]["Object"] = cure;
                        tb.Rows[i]["Content"] = cure.ItemName;

                        cure.RestoreUpdatePK();
                        updateNum = cure.Update();
                        if (updateNum == 0)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            //SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }


                    }
                    else if (objectAdvice is ICheckAdvice)
                    {
                        ICheckAdvice check = objectAdvice as ICheckAdvice;

                        int Original = (int)check.AdvState;
                        check.Refresh();
                        if (Original != (int)check.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            return;
                        }

                        check.GroupNoSymbol = "";
                        check.GroupNo = 0;
                        tb.Rows[i]["Object"] = check;
                        tb.Rows[i]["Content"] = check.ItemName;


                        check.RestoreUpdatePK();
                        updateNum = check.Update();
                        if (updateNum == 0)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            //SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                    }

                    tb.Rows[i]["GroupNoSymbol"] = "";
                    tb.Rows[i]["GroupNo"] = null;
                    #endregion
                    sf = false;
                }
            }
            if (sf)
            {
                XtraMessageBox.Show("请勾选！");
                return;
            }
            XtraMessageBox.Show("取消一组成功！");
            AllCheck(false);
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
        }
        /// <summary>
        /// 退费申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (_NewWorkstation.currentReg.PatientType != 1000401)
            {
                XtraMessageBox.Show("医保病人请右击退费申请！");
                return;
            }

            List<string> AdviceList = new List<string>();
            DataTable tb = this.gridControlAdvice.DataSource as DataTable;
            Advice.DAL.Interface.IAdvice adv;
            foreach (DataRow dr in tb.Rows)
            {
                if (!AdviceList.Contains(dr["AdviceCode"].ToString()))
                {
                    adv = (Advice.DAL.Interface.IAdvice)dr["Object"];
                    if (adv.Creator != Common.ContextHelper.Employee.EmployeeID)
                    {
                        XtraMessageBox.Show("该处方的开立医生是【" + Common.DataConvertHelper.GetEmpName(adv.Creator) + "】,请选择自己开立处方！");
                        return;
                    }
                    AdviceList.Add(dr["AdviceCode"].ToString());
                }
            }
            if (AdviceList.Count == 0)
            {
                XtraMessageBox.Show("没有查询到处方！");
                return;
            }
            //RefundFrom From = new RefundFrom(AdviceList, _NewWorkstation.currentReg);
            //if (From.ShowDialog(this.ParentForm) == DialogResult.OK)
            //{
            //    SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
            //}

        }
        /// <summary>
        /// 处方调阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrescriptionBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //PrescriptionCall From = new PrescriptionCall();
            //if (From.ShowDialog(this.ParentForm) == DialogResult.OK)
            //{
            //    List<object> Advice = From.Tag as List<object>;
            //    bool storeFlag = false;
            //    string msg = string.Empty;
            //    WinnerHIS.Integral.DataCenter.DAL.Interface.IItemInfoEx itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
            //    itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;

            //    foreach (object obj in Advice)
            //    {
            //        if (obj is IDrugAdvice)
            //        {
            //            IDrugAdvice drug = obj as IDrugAdvice;

            //            if (drug.Type != (int)DrugTypeEnum.Material)//如果药品
            //            {
            //                IDrugDict drugDict = WinnerHIS.Common.DataConvertHelper.GetDrugDict(drug.DrugCode);
            //                if (drugDict.Status == 1)
            //                {
            //                    XtraMessageBox.Show("该药品[" + drugDict.DrugName + "]已经被禁用，请选择其他药品！");
            //                    continue;
            //                }
            //                storeFlag = storeList.GetDrugNumFlagByCode(drug.ExecDept, drug.DrugCode, drug.Price, drug.Number * drug.Pack, ref msg);
            //            }
            //            else//
            //            {

            //                IMaterialDict dict = Common.DataConvertHelper.GetMaterialDict(drug.DrugCode);//判断耗材字典是否禁用
            //                if (dict.Status == 1)
            //                {
            //                    XtraMessageBox.Show("该耗材[" + dict.MaterialName + "]已经被禁用，请选择其他药品！");
            //                    continue;
            //                }
            //                storeFlag = mstoreList.GetDrugNumFlagByCode(drug.ExecDept, drug.DrugCode, drug.Price, drug.Number * drug.Pack, ref msg);
            //            }

            //            if (!storeFlag)
            //            {
            //                MessageBox.Show(this, "编号:[" + drug.DrugCode + "]" + drug.DrugName + "(¥" + drug.Price.ToString() + ")库存不足," + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                return;
            //            }
            //            else
            //            {
            //                DrugTableAdd(obj as IDrugAdvice);
            //            }
            //        }
            //        else if (obj is ICureAdvice)
            //        {
            //            ICureAdvice cureAdv = obj as ICureAdvice;
            //            itemInfo.Code = cureAdv.ItemCode;
            //            itemInfo.Refresh();

            //            if (itemInfo.Attribute == 2)
            //            {
            //                XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
            //                continue;
            //            }
            //            CureTableAdd(cureAdv);
            //        }
            //        else if (obj is ICheckAdvice)
            //        {
            //            ICheckAdvice checkAdv = obj as ICheckAdvice;
            //            itemInfo.Code = checkAdv.ItemCode;
            //            itemInfo.Refresh();

            //            if (itemInfo.Attribute == 2)
            //            {
            //                XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
            //                continue;
            //            }
            //            CheckTableAdd(checkAdv);
            //        }

            //    }
            //    Binding();
            //}

        }

        public decimal GetNumber(string spec)
        {
            string rx = @"-?[0-9]+(\.[0-9]+)?";

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(rx);

            string text = regex.Match(spec).Value;

            if (text == "")
            {
                return 0.0M;
            }
            else
            {
                return decimal.Parse(text);
            }
        }

        public bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 协定处方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgreementBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            AgreedIOrders From = new AgreedIOrders();
            if (From.ShowDialog(this.ParentForm) == DialogResult.OK)
            {

                if (From.Stype == "西药")
                {
                    IApinfo ap = From.Tag as IApinfo;
                    ApDrug(ap);
                }
                else if (From.Stype == "中药")
                {
                    IApinfo ap = From.Tag as IApinfo;
                    ApChinese(ap);
                }
                else if (From.Stype == "诊疗")
                {
                    IApinfo ap = From.Tag as IApinfo;
                    ApCure(ap);
                }
                else if (From.Stype == "检查")
                {
                    List<IApinfo> apList = From.Tag as List<IApinfo>;
                    ApCheck(apList, From);
                }
                else if (From.Stype == "检验")
                {
                    List<IApinfo> apList = From.Tag as List<IApinfo>;
                    ApTInspec(apList, From);
                }
                else if (From.Stype == "耗材")
                {
                    IApinfo ap = From.Tag as IApinfo;
                    ApConsumableLoad(ap);
                }
                else if (From.Stype == "综合")
                {
                    IApinfo ap = From.Tag as IApinfo;
                    #region 综合
                    IApAllItemList alllist = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateApAllItemList();
                    alllist.Session = WinnerHIS.Common.ContextHelper.Session;
                    alllist.GetApAllItemList(ap.Id);
                    foreach (IApAllItem apAllItem in alllist.Rows)
                    {
                        IApinfo info = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateApinfo();
                        info.Session = WinnerHIS.Common.ContextHelper.Session;
                        info.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                        info.Code = apAllItem.Code;
                        info.Refresh();
                        #region 类型选择
                        if (info.Exists)
                        {
                            if (info.Type == (int)APType.WDrug)
                            {
                                ApDrug(info);
                            }
                            else if (info.Type == (int)APType.Drug)
                            {
                                ApChinese(info);
                            }
                            else if (info.Type == (int)APType.Consumable)
                            {
                                ApConsumableLoad(info);
                            }
                            else if (info.Type == (int)APType.ItemCure)
                            {
                                ApCure(info);
                            }
                            else if (info.Type == (int)APType.ItemCheck)
                            {
                                List<IApinfo> apList = new List<IApinfo>();
                                apList.Add(info);
                                ApCheck(apList, From);
                            }
                            else if (info.Type == (int)APType.Inspect)
                            {
                                List<IApinfo> apList = new List<IApinfo>();
                                apList.Add(info);
                                ApTInspec(apList, From);
                            }
                        }
                        #endregion

                    }
                    #endregion
                }
                else
                {
                    throw new Exception("类型出现异常！");
                }
                Binding();
            }

        }
        #region 协定添加
        /// <summary>
        /// 西药协定添加
        /// </summary>
        public void ApDrug(IApinfo ap)
        {
            #region 西药
            WinnerHIS.Integral.AP.DAL.Interface.IApdrugList list = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateApdrugList();
            list.Session = WinnerHIS.Common.ContextHelper.Session;
            list.GetApInfoList(ap.Code.Trim());
            string msg = string.Empty;
            bool storeFlag = false;
            WinnerHIS.Drug.DrugStore.DAL.Interface.IDrugStoreExLet drugStore = null;
            foreach (WinnerHIS.Integral.AP.DAL.Interface.IApdrug drug in list.Rows)
            {
                //storeList.GetDrugListOrderByBatch(WesternMedicineRoom, drug.Drugcode);
                IDrugDict dict = Common.DataConvertHelper.GetDrugDict(drug.Drugcode);//判断药品字典是否禁用
                if (dict.Status == 1)
                {
                    MessageBox.Show(this, "药品编号:[" + drug.Drugcode + "],名称:(" + drug.Drugname + ")已禁用，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }
                //storeFlag = storeList.GetDrugNumFlagByCode(WesternMedicineRoom, drug.Drugcode, 0, drug.Number, ref msg);
                //if (!storeFlag)
                //{
                //    DataTable drgDt = storeList.GetDrugNumFlagByCode(WesternMedicineRoom, drug.Drugcode, 0);
                //    string warnMsg = "药品编号:[" + drug.Drugcode + "],名称:(" + drug.Drugname + ")库存不足," + msg;
                //    DrugNumWarning drgWarningForm = new DrugNumWarning(warnMsg, drgDt);
                //    drgWarningForm.DrgDataTable = drgDt;
                //    drgWarningForm.ShowDialog();
                //    continue;
                //}
                //else
                //{
                storeList.GetDrugListOrderByBatch(WesternMedicineRoom, drug.Drugcode);
                bool isEnough = false;
                foreach (WinnerHIS.Drug.DrugStore.DAL.Interface.IDrugStoreExLet storeLet in storeList.Rows)
                {
                    if (storeLet.Status == 1)//药房设置禁用
                    {
                        continue;
                    }
                    storeFlag = storeList.GetDrugNumFlagByCode(WesternMedicineRoom, drug.Drugcode, storeLet.Price, drug.Number, ref msg);
                    if (!storeFlag)
                    {
                        continue;
                    }
                    else
                    {
                        drugStore = storeLet;
                        isEnough = true;
                        break;
                    }
                }
                if (!isEnough)
                {
                    DataTable drgDt = storeList.GetDrugNumFlagByCode(WesternMedicineRoom, drug.Drugcode, 0);
                    string warnMsg = "药品编号:[" + drug.Drugcode + "],名称:(" + drug.Drugname + ")库存不足," + msg;
                    //DrugNumWarning drgWarningForm = new DrugNumWarning(warnMsg, drgDt);
                    //drgWarningForm.DrgDataTable = drgDt;
                    //drgWarningForm.ShowDialog();
                    continue;
                }
                //}

                IDrugAdvice drugAdvice = DALHelper.ClinicDAL.CreateDrugAdvice();
                drugAdvice.Session = WinnerHIS.Common.ContextHelper.Session;

                for (int i = 0; i < drugStore.PropertyCount; i++)
                {
                    WinnerSoft.Data.ORM.Property prop = drugStore.GetProperty(i);

                    if (drugAdvice.ContainsProperty(prop.Name))
                    {
                        drugAdvice[prop.Name] = drugStore[i];
                    }
                }
                if (!IsNumeric(drug.Dosage) && !IsInt(drug.Dosage))
                {
                    drugAdvice.Dosage = GetNumber(drug.Dosage).ToString();
                }
                else
                {
                    drugAdvice.Dosage = drug.Dosage;
                }
                drugAdvice.AdviceCode = ap.Code.Trim();// "";// ap.Code;
                drugAdvice.DrugCode = drugStore.DrugCode;
                drugAdvice.DrugName = drugStore.DrugName;
                drugAdvice.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                drugAdvice.AdvState = AdviceState.NotSave;
                drugAdvice.InsureType = (InsureType)drugStore.InsureRate;
                drugAdvice.Pack = drugAdvice.Pack;


                //用药方式
                drugAdvice.Method = drug.Mothod;
                //频率
                drugAdvice.Frequence = drug.Frequency;
                //数量
                drugAdvice.Number = drug.Number;
                //天数
                drugAdvice.Days = drug.Days;

                //皮试
                drugAdvice.SkinTest = drug.SKINTEST;

                int DripSpeed = 0;
                int.TryParse(drug.DripSpeed, out DripSpeed);

                //滴速
                drugAdvice.DripSpeed = DripSpeed;

                //备注
                drugAdvice.Remarks = drug.Memo;



                //创建时间
                drugAdvice.CreateTime = WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime();
                //创建医生
                drugAdvice.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
                //总金额
                drugAdvice.Cash = drugAdvice.Price * drugAdvice.Number;
                //执行科室
                drugAdvice.ExecDept = WesternMedicineRoom;
                //部门
                drugAdvice.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;

                drugAdvice.CheckTime = Convert.ToDateTime("1900-01-01");
                drugAdvice.PrescriptionType = advTypeDefault.ToString();//默认处方类型
                DrugTableAdd(drugAdvice);
            }
            #endregion
        }
        /// <summary>
        /// 中药协定添加
        /// </summary>
        public void ApChinese(IApinfo ap)
        {

            #region 中药
            WinnerHIS.Integral.AP.DAL.Interface.IApdrugList list = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateApdrugList();
            list.Session = WinnerHIS.Common.ContextHelper.Session;
            list.GetApInfoList(ap.Code);

            string msg = string.Empty;
            bool storeFlag = false;
            WinnerHIS.Drug.DrugStore.DAL.Interface.IDrugStoreExLet drugStore = null;
            foreach (WinnerHIS.Integral.AP.DAL.Interface.IApdrug drug in list.Rows)
            {
                IDrugDict dict = Common.DataConvertHelper.GetDrugDict(drug.Drugcode);
                if (dict.Status == 1)
                {
                    MessageBox.Show(this, "药品编号:[" + drug.Drugcode + "],名称:(" + drug.Drugname + ")已禁用，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }
                //if (!storeFlag)
                //{
                //    MessageBox.Show(this, "药品编号:[" + drug.Drugcode + "],名称:(" + drug.Drugname + ")库存不足," + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    continue;
                //}
                //else
                //{
                storeList.GetDrugListOrderByBatch(TCMPharmacy, drug.Drugcode);
                bool isEnough = false;
                foreach (WinnerHIS.Drug.DrugStore.DAL.Interface.IDrugStoreExLet storeLet in storeList.Rows)
                {
                    if (storeLet.Status == 1)//药房设置禁用
                    {
                        continue;
                    }
                    storeFlag = storeList.GetDrugNumFlagByCode(TCMPharmacy, drug.Drugcode, storeLet.Price, drug.Number * drug.Pack, ref msg);
                    if (!storeFlag)
                    {
                        continue;
                    }
                    else
                    {
                        drugStore = storeLet;
                        isEnough = true;
                        break;
                    }
                }
                if (!isEnough)
                {
                    MessageBox.Show(this, "药品编号:[" + drug.Drugcode + "],名称:(" + drug.Drugname + ")库存不足", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }
                //}

                IDrugAdvice drugAdvice = DALHelper.ClinicDAL.CreateDrugAdvice();
                drugAdvice.Session = WinnerHIS.Common.ContextHelper.Session;

                for (int i = 0; i < drugStore.PropertyCount; i++)
                {
                    WinnerSoft.Data.ORM.Property prop = drugStore.GetProperty(i);

                    if (drugAdvice.ContainsProperty(prop.Name))
                    {
                        drugAdvice[prop.Name] = drugStore[i];
                    }
                }
                if (!IsNumeric(drug.Dosage) && !IsInt(drug.Dosage))
                {
                    drugAdvice.Dosage = GetNumber(drug.Dosage).ToString();
                }
                else
                {
                    drugAdvice.Dosage = drug.Dosage;
                }
                drugAdvice.AdviceCode = "";// ap.Code;
                drugAdvice.DrugCode = drugStore.DrugCode;
                drugAdvice.DrugName = drugStore.DrugName;
                drugAdvice.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                drugAdvice.AdvState = AdviceState.NotSave;
                drugAdvice.InsureType = (InsureType)drugStore.InsureRate;

                //剂量单位
                drugAdvice.DosageUnit = drugStore.DosageUnit;
                //剂量
                drugAdvice.Dosage = drug.Dosage;
                //付数
                drugAdvice.Pack = drug.Pack;
                //用药方式
                drugAdvice.Method = 0;
                //频率
                drugAdvice.Frequence = 0;
                //数量
                drugAdvice.Number = drug.Number;
                //天数
                drugAdvice.Days = drug.Days;

                //皮试
                drugAdvice.SkinTest = 0;
                //滴速
                drugAdvice.DripSpeed = 0;

                //用药方式
                drugAdvice.Memo = drug.Memo;

                //创建时间
                drugAdvice.CreateTime = WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime();
                //创建医生
                drugAdvice.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
                //总金额
                drugAdvice.Cash = drugAdvice.Price * drugAdvice.Pack * int.Parse(drugAdvice.Dosage);
                //执行科室
                drugAdvice.ExecDept = TCMPharmacy;
                //部门
                drugAdvice.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;

                drugAdvice.CheckTime = Convert.ToDateTime("1900-01-01");
                DrugTableAdd(drugAdvice);
            }
            #endregion
        }
        /// <summary>
        /// 诊疗协定添加
        /// </summary>
        public void ApCure(IApinfo ap)
        {
            #region 诊疗
            WinnerHIS.Integral.AP.DAL.Interface.IApitemList apItemList = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateApitemList();
            apItemList.Session = WinnerHIS.Common.ContextHelper.Session;
            apItemList.GetApInfoList(ap.Code);

            foreach (WinnerHIS.Integral.AP.DAL.Interface.IApitem var in apItemList.Rows)
            {
                WinnerHIS.Integral.DataCenter.DAL.Interface.IItemInfoEx itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
                itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                itemInfo.Code = var.Itemcode;
                itemInfo.Refresh();


                if (itemInfo.Attribute == 2)
                {
                    XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
                    continue;
                }

                ICureAdvice cureAdvice = DALHelper.ClinicDAL.CreateCureAdvice();
                cureAdvice.Session = WinnerHIS.Common.ContextHelper.Session;

                cureAdvice.AdviceCode = ap.Code;
                cureAdvice.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                cureAdvice.AdvState = AdviceState.NotSave;

                cureAdvice.ItemCode = itemInfo.Code;
                cureAdvice.ItemName = itemInfo.Name;
                cureAdvice.ExeConfirm = itemInfo.ExeFlag;

                //单位
                cureAdvice.Unit = itemInfo.Unit;
                //价格
                cureAdvice.Price = itemInfo.Price;
                //数量
                cureAdvice.Number = (int)var.Number;
                //总金额
                cureAdvice.Cash = cureAdvice.Price * cureAdvice.Number;
                //天数
                cureAdvice.Days = var.Days;
                //备注
                cureAdvice.Remarks = var.Remarks;

                cureAdvice.Frequency = var.FREQUENCY;


                //创建时间
                cureAdvice.CreateTime = WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime();
                //创建医生
                cureAdvice.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;

                //执行科室
                cureAdvice.ExecDept = var.Deptid;
                //部门
                cureAdvice.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;

                cureAdvice.CheckTime = Convert.ToDateTime("1900-01-01");
                CureTableAdd(cureAdvice);

            }
            #endregion
        }
        /// <summary>
        /// 检查协定添加
        /// </summary>
        public void ApCheck(List<IApinfo> apList, AgreedIOrders From)
        {
            #region 检查
            string advCode = string.Empty;//如果检查使用同一个处方号
            foreach (IApinfo ap in apList)
            {
                WinnerHIS.Integral.AP.DAL.Interface.IApitemList apItemList = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateApitemList();
                apItemList.Session = WinnerHIS.Common.ContextHelper.Session;
                apItemList.GetApInfoList(ap.Code);

                foreach (WinnerHIS.Integral.AP.DAL.Interface.IApitem var in apItemList.Rows)
                {
                    WinnerHIS.Integral.DataCenter.DAL.Interface.IItemInfoEx itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
                    itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                    itemInfo.Code = var.Itemcode;
                    itemInfo.Refresh();

                    if (itemInfo.Attribute == 2)
                    {
                        XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
                        continue;
                    }

                    ICheckAdvice checkadvice = DALHelper.ClinicDAL.CreateCheckAdvice();
                    checkadvice.Session = WinnerHIS.Common.ContextHelper.Session;
                    if (advCode == string.Empty)
                        advCode = ap.Code;
                    checkadvice.AdviceCode = advCode;
                    checkadvice.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                    checkadvice.AdvState = AdviceState.NotSave;

                    checkadvice.ItemCode = itemInfo.Code;
                    checkadvice.ItemName = itemInfo.Name;


                    //单位
                    checkadvice.Unit = itemInfo.Unit;
                    //价格
                    checkadvice.Price = itemInfo.Price;
                    //数量
                    checkadvice.Number = (int)var.Number;
                    //总金额
                    checkadvice.Cash = checkadvice.Price * checkadvice.Number;

                    checkadvice.DISCOUNT = ap.Discounts.ToString();
                    //诊断
                    checkadvice.MEMO2 = From.ZhenDuan;
                    //检查部位
                    checkadvice.BuWei = From.BuWei;
                    //目的
                    checkadvice.MuDi = From.MuDi;
                    //标本
                    checkadvice.BiaoBen = From.BiaoBen;
                    //主诉
                    checkadvice.ZhuSu = From.ZhuSu;
                    //备注
                    checkadvice.Remarks = var.Remarks;

                    //创建时间
                    checkadvice.CreateTime = WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime();
                    //创建医生
                    checkadvice.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;

                    //执行科室
                    checkadvice.ExecDept = var.Deptid;
                    //部门
                    checkadvice.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;

                    checkadvice.CheckTime = Convert.ToDateTime("1900-01-01");


                    CheckTableAdd(checkadvice);

                }
            }

            #endregion
        }
        /// <summary>
        /// 检验协定添加
        /// </summary>
        public void ApTInspec(List<IApinfo> apList, AgreedIOrders From)
        {
            #region 检验
            string advCode = string.Empty;
            foreach (IApinfo ap in apList)
            {
                WinnerHIS.Integral.AP.DAL.Interface.IApitemList apItemList = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateApitemList();
                apItemList.Session = WinnerHIS.Common.ContextHelper.Session;
                apItemList.GetApInfoList(ap.Code);

                if (ap.Flag == 1)//如果是组套
                {
                    #region 组套打包
                    ICheckAdvice checkadvice = DALHelper.ClinicDAL.CreateCheckAdvice();
                    checkadvice.Session = WinnerHIS.Common.ContextHelper.Session;

                    if (advCode == string.Empty)
                        advCode = ap.Code;
                    checkadvice.AdviceCode = advCode;
                    checkadvice.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                    checkadvice.AdvState = AdviceState.NotSave;

                    checkadvice.ItemCode = ap.Code;
                    checkadvice.ItemName = ap.Name;
                    checkadvice.ExecDept = ap.ExeDept;
                    checkadvice.ZTFlag = 1;

                    //数量
                    checkadvice.Number = 1;
                    decimal Sum = 0.00M;
                    bool isActive = true;
                    foreach (WinnerHIS.Integral.AP.DAL.Interface.IApitem var in apItemList.Rows)
                    {
                        WinnerHIS.Integral.DataCenter.DAL.Interface.IItemInfoEx itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
                        itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                        itemInfo.Code = var.Itemcode;
                        itemInfo.Refresh();

                        if (itemInfo.Exists)
                        {

                            if (itemInfo.Attribute == 2)//如果禁用了。则提示
                            {
                                XtraMessageBox.Show($"项目【{itemInfo.Name}】已经禁用，请检查该协定组套【{ap.Name}】");
                                isActive = false;
                                break;
                            }
                            //价格
                            Sum += Convert.ToDecimal(itemInfo.Price * var.Number);
                        }
                        else
                        {
                            //价格
                            Sum += Convert.ToDecimal(var.Price * var.Number);
                        }
                        //单位
                        checkadvice.Unit = var.Unit;

                    }
                    if (!isActive)//如果存在被禁用的项目，则该组套不被添加
                    {
                        continue;
                    }
                    checkadvice.Price = Sum;
                    checkadvice.Cash = checkadvice.Price * checkadvice.Number;
                    checkadvice.DISCOUNT = ap.Discounts.ToString();
                    //诊断
                    checkadvice.MEMO2 = From.ZhenDuan;
                    //检查部位
                    checkadvice.Remarks = From.BuWei;
                    //目的
                    checkadvice.MuDi = From.MuDi;
                    //标本
                    checkadvice.BiaoBen = From.BiaoBen;
                    //主诉
                    checkadvice.ZhuSu = From.ZhuSu;
                    //创建时间
                    checkadvice.CreateTime = WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime();
                    //创建医生
                    checkadvice.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
                    //部门
                    checkadvice.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;

                    checkadvice.CheckTime = Convert.ToDateTime("1900-01-01");


                    CheckTableAdd(checkadvice);

                    #endregion
                }
                else
                {
                    #region 非组套则加载明细
                    foreach (WinnerHIS.Integral.AP.DAL.Interface.IApitem var in apItemList.Rows)
                    {
                        WinnerHIS.Integral.DataCenter.DAL.Interface.IItemInfoEx itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
                        itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                        itemInfo.Code = var.Itemcode;
                        itemInfo.Refresh();
                        if (itemInfo.Attribute != 1)
                            continue;
                        ICheckAdvice checkadvice = DALHelper.ClinicDAL.CreateCheckAdvice();
                        checkadvice.Session = WinnerHIS.Common.ContextHelper.Session;

                        checkadvice.AdviceCode = var.Apcode;
                        checkadvice.ItemCode = ap.Code;
                        checkadvice.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                        checkadvice.AdvState = AdviceState.NotSave;

                        checkadvice.ItemCode = itemInfo.Code;
                        checkadvice.ItemName = itemInfo.Name;
                        checkadvice.ZTFlag = 0;


                        //单位
                        checkadvice.Unit = itemInfo.Unit;
                        //价格
                        checkadvice.Price = itemInfo.Price;
                        //数量
                        checkadvice.Number = (int)var.Number;
                        //总金额
                        checkadvice.Cash = checkadvice.Price * checkadvice.Number;
                        checkadvice.DISCOUNT = ap.Discounts.ToString();
                        //诊断
                        checkadvice.MEMO2 = From.ZhenDuan;
                        //检查部位
                        checkadvice.Remarks = From.BuWei;
                        //目的
                        checkadvice.MuDi = From.MuDi;
                        //标本
                        checkadvice.BiaoBen = From.BiaoBen;
                        //主诉
                        checkadvice.ZhuSu = From.ZhuSu;


                        //创建时间
                        checkadvice.CreateTime = WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime();
                        //创建医生
                        checkadvice.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;

                        //执行科室
                        checkadvice.ExecDept = itemInfo.DepartmentID;//var.Deptid;不是组套的项目，用每个项目的执行科室
                        //部门
                        checkadvice.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;

                        checkadvice.CheckTime = Convert.ToDateTime("1900-01-01");
                        CheckTableAdd(checkadvice);

                    }
                    #endregion
                }
            }

            #endregion
        }
        /// <summary>
        /// 耗材协定添加
        /// </summary>
        public void ApConsumableLoad(IApinfo ap)
        {
            #region 耗材

            WinnerHIS.Integral.AP.DAL.Interface.IApConsumableList list = WinnerHIS.Integral.AP.DAL.Interface.DALHelper.DALManager.CreateApConsumableList();
            list.Session = WinnerHIS.Common.ContextHelper.Session;
            list.GetApInfoList(ap.Code.Trim());
            string msg = string.Empty;
            bool storeFlag = false;

            IMaterialStoreVwList mStoreList = WinnerHIS.Material.MaterialStore.DAL.Interface.DALHelper.DALManager.CreateMaterialStoreVwList();
            mStoreList.Session = WinnerHIS.Common.ContextHelper.Session;
            mStoreList.GetDStoreOpenList(0);//获取所有销售耗材
            int mstoreID = 0;
            foreach (WinnerHIS.Integral.AP.DAL.Interface.IApConsumable drug in list.Rows)
            {
                IMaterialDict dict = Common.DataConvertHelper.GetMaterialDict(drug.Code);//判断耗材字典是否禁用
                if (dict.Status == 1)
                {
                    MessageBox.Show(this, "耗材编号:[" + drug.Code + "],名称:(" + drug.Name + ")已禁用，请重新选择", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    continue;
                }
                WinnerHIS.Material.MaterialStore.DAL.Interface.IMaterialStoreVw mdrugStore = null;
                mstoreID = drug.Deptid == 0 ? WinnerHIS.Common.ContextHelper.Account.DeptID : drug.Deptid;
                bool isEnough = false;
                foreach (WinnerHIS.Material.MaterialStore.DAL.Interface.IMaterialStoreVw storeLet in mStoreList.Rows)
                {
                    mdrugStore = null;
                    if (storeLet.CODE != drug.Code || storeLet.Storeid != mstoreID)
                        continue;
                    if (storeLet.Number < drug.Number)//库存不足
                    {
                        string warnMsg = $"耗材编号:[{drug.Code}],名称:({drug.Name})库存【{storeLet.Number}】不足";
                        MessageBox.Show(warnMsg);
                        continue;
                    }
                    storeFlag = mstoreList.GetDrugNumFlagByCode(mstoreID, drug.Code, storeLet.Price, drug.Number, ref msg);
                    if (!storeFlag)
                    {
                        continue;
                    }
                    else
                    {
                        mdrugStore = storeLet;
                        isEnough = true;
                        break;
                    }
                }
                if (mdrugStore == null)
                {
                    string warnMsg = $"耗材编号:[{drug.Code}],名称:({drug.Name})没有库存";
                    MessageBox.Show(warnMsg);
                    continue;
                }
                if (!isEnough)
                {
                    DataTable drgDt = mstoreList.GetDrugNumFlagByCode(mstoreID, drug.Code, 0);
                    string warnMsg = "耗材编号:[" + drug.Code + "],名称:(" + drug.Name + ")库存不足," + msg;
                    //DrugNumWarning drgWarningForm = new DrugNumWarning(warnMsg, drgDt);
                    //drgWarningForm.DrgDataTable = drgDt;
                    //drgWarningForm.ShowDialog();
                    continue;
                }

                IDrugAdvice drugAdvice = DALHelper.ClinicDAL.CreateDrugAdvice();
                drugAdvice.Session = WinnerHIS.Common.ContextHelper.Session;

                for (int i = 0; i < mdrugStore.PropertyCount; i++)
                {
                    WinnerSoft.Data.ORM.Property prop = mdrugStore.GetProperty(i);

                    if (drugAdvice.ContainsProperty(prop.Name))
                    {
                        drugAdvice[prop.Name] = mdrugStore[i];
                    }
                }
                if (!IsNumeric(drug.Dosage) && !IsInt(drug.Dosage))
                {
                    drugAdvice.Dosage = GetNumber(drug.Dosage).ToString();
                }
                else
                {
                    drugAdvice.Dosage = drug.Dosage;
                }

                drugAdvice.DrugID = mdrugStore.MATERIALID;
                drugAdvice.Type = (int)DrugTypeEnum.Material;
                drugAdvice.AdviceCode = "";// ap.Code;
                drugAdvice.DrugCode = mdrugStore.CODE;
                drugAdvice.DrugName = mdrugStore.Cname;
                drugAdvice.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                drugAdvice.AdvState = AdviceState.NotSave;
                drugAdvice.InsureType = (InsureType)mdrugStore.InsureRate;
                drugAdvice.Pack = drugAdvice.Pack;


                //用药方式
                drugAdvice.Method = drug.Mothod;
                //频率
                drugAdvice.Frequence = drug.Frequency;
                //数量
                drugAdvice.Number = drug.Number;
                //天数
                drugAdvice.Days = drug.Days;

                //皮试
                drugAdvice.SkinTest = 0;

                //滴速
                drugAdvice.DripSpeed = 0;

                //备注
                drugAdvice.Remarks = drug.Memo;



                //创建时间
                drugAdvice.CreateTime = WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime();
                //创建医生
                drugAdvice.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
                //总金额
                drugAdvice.Cash = drugAdvice.Price * drugAdvice.Number;
                //执行科室
                drugAdvice.ExecDept = mdrugStore.Storeid;
                //部门
                drugAdvice.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;

                drugAdvice.CheckTime = Convert.ToDateTime("1900-01-01");
                drugAdvice.PrescriptionType = advTypeDefault.ToString();//默认处方类型

                DrugTableAdd(drugAdvice);
            }
            #endregion
        }
        #endregion
        /// <summary>
        /// 中药配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChineseBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ChineseSettings From = new ChineseSettings();
            if (From.ShowDialog() == DialogResult.OK)
            {
                int updateNum = 0;
                string Memo = From.Tag as String;
                DataTable tb = this.gridControlAdvice.DataSource as DataTable;
                bool sf = false;
                string msg = string.Empty;
                bool storeFlag = false;
                foreach (DataRow dr in tb.Rows)
                {
                    object objectAdvice = dr["Object"];
                    if (objectAdvice is IDrugAdvice)
                    {
                        IDrugAdvice drugAdv = objectAdvice as IDrugAdvice;
                        if (drugAdv.Frequence == 0 && drugAdv.Method == 0 && Convert.ToBoolean(dr["CheckObj"].ToString()))
                        {
                            if ((int)drugAdv.AdvState == 2)
                            {
                                XtraMessageBox.Show("已经收费的医嘱无法再进行修改！");
                                return;
                            }
                            if ((int)drugAdv.AdvState == 3)
                            {
                                XtraMessageBox.Show("已执行的医嘱无法再进行修改！");
                                return;
                            }
                            #region 库存+预售判断
                            int OldNumber = 0;
                            int NewNumber = drugAdv.Number * From.Payment;
                            if (drugAdv.AdvState != AdviceState.NotSave)
                            {
                                OldNumber = drugAdv.Number * drugAdv.Pack;
                            }
                            if (NewNumber > OldNumber)
                            {
                                storeFlag = storeList.GetDrugNumFlagByCode(drugAdv.ExecDept, drugAdv.DrugCode, drugAdv.Price, NewNumber, ref msg);

                                if (!storeFlag)
                                {
                                    MessageBox.Show(this, "药品编号:[" + drugAdv.DrugCode + "]" + drugAdv.DrugName + "(¥" + drugAdv.Price.ToString() + ")库存不足," + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    continue;
                                }

                            }
                            #endregion
                            dr["Mode"] = Memo;
                            drugAdv.Memo = Memo;
                            drugAdv.Pack = From.Payment;
                            drugAdv.Cash = drugAdv.Pack * Convert.ToDecimal(drugAdv.Dosage) * drugAdv.Price;
                            dr["Cash"] = drugAdv.Cash;
                            dr["Days"] = drugAdv.Days + "/" + drugAdv.Pack;
                            dr["Object"] = drugAdv;
                            sf = true;
                            if (drugAdv.AdvState != AdviceState.NotSave)
                            {
                                drugAdv.RestoreUpdatePK();
                                updateNum = drugAdv.Update();
                                if (updateNum == 0)
                                {
                                    XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                                    //SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                                    return;
                                }
                            }
                        }
                    }
                }
                if (sf == true)
                {
                    AllCheck(false);
                    XtraMessageBox.Show("中草药配置成功！");
                    Binding();
                }
                else
                {
                    XtraMessageBox.Show("请勾选勾选中草药或者库存够的！");
                }


            }
        }
        private void repositoryItemCheckEdit7_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit chEdit = sender as DevExpress.XtraEditors.CheckEdit;
            AllCheck(chEdit.Checked);
        }
        #endregion

        #region 更改医嘱的显示状态
        public void RefreshState(object sender, int State)
        {
            DevExpress.XtraEditors.CheckEdit chEdit = sender as DevExpress.XtraEditors.CheckEdit;
            if (chEdit.Checked)
            {
                if (!StateList.Contains(State))
                {
                    StateList.Add(State);
                }
            }
            else
            {
                StateList.Remove(State);
            }
            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, false);
        }
        /// <summary>
        /// 未保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ItemCheckEditNotSave_CheckedChanged(object sender, EventArgs e)
        {
            RefreshState(sender, -1);
        }
        /// <summary>
        /// 已审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemCheckEditIsAudit_CheckedChanged(object sender, EventArgs e)
        {
            RefreshState(sender, 1);
        }

        /// <summary>
        /// 已收费
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemCheckEditCharge_CheckedChanged(object sender, EventArgs e)
        {
            RefreshState(sender, 2);
        }
        /// <summary>
        /// 已执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemCheckEditExec_CheckedChanged(object sender, EventArgs e)
        {
            RefreshState(sender, 3);
        }
        /// <summary>
        /// 已退费
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemCheckEditBack_CheckedChanged(object sender, EventArgs e)
        {
            RefreshState(sender, 4);
        }
        private void ItemCheckEditCancel_CheckedChanged(object sender, EventArgs e)
        {
            RefreshState(sender, 5);
        }
        #endregion

        #region 右击相关
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gridViewAdvice_DoubleClick(null, null);
        }
        /// <summary>
        /// 新增开立
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSMINew_Click(object sender, EventArgs e)
        {
            OpenBar_ItemClick(null, null);
        }
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSMIAppend_Click(object sender, EventArgs e)
        {
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            if (row != null)
            {
                if (row["AdvState"].ToString() == "2")
                {
                    XtraMessageBox.Show("已收费的医嘱无法再进行追加！");
                    return;
                }
                if (row["AdvState"].ToString() == "3")
                {
                    XtraMessageBox.Show("已经执行的医嘱无法再进行追加！");
                    return;
                }
                if (row["CreationDoc"].ToString() != WinnerHIS.Common.ContextHelper.Employee.Name)
                {
                    XtraMessageBox.Show("无法对其他医生的处方进行追加！");
                    return;
                }
                List<string> appendAdv = new List<string>();
                appendAdv.Add(row["AdviceCode"].ToString());
                appendAdv.Add(row["CreationTime"].ToString());
                if (row["Object"] is IDrugAdvice)
                {
                    IDrugAdvice drgAdv = (IDrugAdvice)row["Object"];
                    appendAdv.Add(drgAdv.PrescriptionType);
                }
                //RecipeCreationForm From = new RecipeCreationForm(this, appendAdv, 2);
                //From.inputCompleted += new RecipeCreationForm.InputCompletedHandler(drugInput_InputCompleted);
                //From.ShowDialog();
            }

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int deleteNum = 0;
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            DataTable tb = this.gridControlAdvice.DataSource as DataTable;
            if (row != null)
            {
                if (row["AdvState"].ToString() == "2")
                {
                    XtraMessageBox.Show("已收费的医嘱无法再进行删除！");
                    return;
                }
                if (row["AdvState"].ToString() == "3")
                {
                    XtraMessageBox.Show("已经执的医嘱无法再进行删除！");
                    return;
                }

                string State = row["AdvState"].ToString();
                if (State == "1")
                {
                    object obj = row["Object"] as object;
                    if (obj is IDrugAdvice)
                    {
                        IDrugAdvice adv = obj as IDrugAdvice;
                        int Original = (int)adv.AdvState;
                        adv.Refresh();
                        if (Original != (int)adv.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请及时刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                        if (Original == 2)
                        {
                            XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
                            return;
                        }
                        if (Original == 3)
                        {
                            XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
                            return;
                        }

                        //删除备份
                        //if (adv.AdvState != AdviceState.NotSave)
                        //{
                        //    DrugAdviceBack(adv.ID, "Drug");
                        //}
                        adv.RestoreUpdatePK();
                        deleteNum = adv.Delete();

                        if (deleteNum == 0)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请及时刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                    }
                    else if (obj is ICureAdvice)
                    {
                        ICureAdvice adv = obj as ICureAdvice;
                        int Original = (int)adv.AdvState;
                        adv.Refresh();
                        if (Original != (int)adv.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                        if (Original == 2)
                        {
                            XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
                            return;
                        }
                        if (Original == 3)
                        {
                            XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
                            return;
                        }
                        //删除备份
                        //if (adv.AdvState != AdviceState.NotSave)
                        //{
                        //    DrugAdviceBack(adv.ID, "Cure");
                        //}
                        if (adv.Price < 0 && _NewWorkstation.cmCouponCheckFlag)//启用了收费优惠审核
                        {
                            ICouponCheckApplyList couponApplyList = WinnerHIS.Analyse.Decision.DAL.Interface.DALHelper.DALManager.CreateCouponCheckApplyList();
                            couponApplyList.Session = Common.ContextHelper.Session;
                            ICouponCheckApply couponApply = couponApplyList.GetEntity(adv.AdviceCode, adv.ItemCode);
                            if (couponApply != null)
                            {
                                couponApply.Refresh();
                                couponApply.Delete();
                            }
                        }
                        adv.RestoreUpdatePK();
                        deleteNum = adv.Delete();

                        if (deleteNum == 0)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                    }
                    else if (obj is ICheckAdvice)
                    {
                        ICheckAdvice adv = obj as ICheckAdvice;
                        int Original = (int)adv.AdvState;
                        adv.Refresh();
                        if (Original != (int)adv.AdvState)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                        if (Original == 2)
                        {
                            XtraMessageBox.Show("已收费的医嘱无法再进行修改！");
                            return;
                        }
                        if (Original == 3)
                        {
                            XtraMessageBox.Show("已经执的医嘱无法再进行修改！");
                            return;
                        }
                        //删除备份
                        //if (adv.AdvState != AdviceState.NotSave)
                        //{
                        //    DrugAdviceBack(adv.ID, "Check");
                        //}
                        adv.RestoreUpdatePK();
                        deleteNum = adv.Delete();

                        if (deleteNum == 0)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                    }
                }

            }
            #region 如果是一组的，被删除一个，取消其他组
            DataTable table = this.gridControlAdvice.DataSource as DataTable;
            if (row["GroupNo"].ToString() != "")
            {
                foreach (DataRow dr in table.Rows)
                {
                    if (row["GroupNo"].ToString() == dr["GroupNo"].ToString())
                    {
                        #region 修改Object对象
                        object objectAdvice = dr["Object"];
                        if (objectAdvice is IDrugAdvice)
                        {
                            IDrugAdvice drug = objectAdvice as IDrugAdvice;
                            drug.GroupNoSymbol = "";
                            drug.GroupNo = 0;
                            dr["Object"] = drug;

                        }
                        else if (objectAdvice is ICureAdvice)
                        {
                            ICureAdvice cure = objectAdvice as ICureAdvice;
                            cure.GroupNoSymbol = "";
                            cure.GroupNo = 0;
                            dr["Object"] = cure;
                        }
                        else if (objectAdvice is ICheckAdvice)
                        {
                            ICheckAdvice check = objectAdvice as ICheckAdvice;
                            check.GroupNoSymbol = "";
                            check.GroupNo = 0;
                            dr["Object"] = check;

                        }
                        dr["GroupNoSymbol"] = "";
                        dr["GroupNo"] = "";
                        #endregion
                    }
                }

            }
            #endregion
            tb.Rows.Remove(row);
            Binding();
        }
        #region 删除生成备份记录
        public void DrugAdviceBack(int AdviceID, string SType)
        {
            try
            {

                string Sql = "";
                if (SType == "Drug")
                {
                    Sql = "insert into HIS.HIS.CM_DRUGADVICE_Delete select * from HIS.his.CM_DRUGADVICE where ID=" + AdviceID;
                }
                else if (SType == "Cure")
                {
                    Sql = "insert into HIS.HIS.CM_CUREADVICE_Delete select * from HIS.his.CM_CUREADVICE where ID=" + AdviceID;
                }
                else if (SType == "Check")
                {
                    Sql = "insert into HIS.HIS.CM_CHECKADVICE_Delete select * from HIS.his.CM_CHECKADVICE where ID=" + AdviceID;
                }

                IConnection dbcn = WinnerHIS.Common.ContextHelper.Context.Container.GetComponentInstances(typeof(IConnection))[0] as IConnection;
                dbcn.CreateAccessor().Execute(Sql.ToString());

                BackRecord(AdviceID, SType);
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常：" + ex.ToString());
            }
        }
        public void BackRecord(int AdviceID, string SType)
        {
            string Sql = "insert his.his.CM_Advice_Delete values(" + AdviceID + ",'" + SType + "',GETDATE(),'" + WinnerHIS.Common.ContextHelper.Account.LoginID + "','门诊病历')";
            IConnection dbcn = WinnerHIS.Common.ContextHelper.Context.Container.GetComponentInstances(typeof(IConnection))[0] as IConnection;
            dbcn.CreateAccessor().Execute(Sql.ToString());
        }


        #endregion

        /// <summary>
        /// 退费申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            List<string> AdviceList = new List<string>();
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            if (row["AdvState"].ToString() == "1" && row["AdvState"].ToString() == "-1")
            {
                XtraMessageBox.Show("请选择已经收费过的处方！");
                return;
            }
            Advice.DAL.Interface.IAdvice adv = (Advice.DAL.Interface.IAdvice)row["Object"];
            if (adv.Creator != Common.ContextHelper.Employee.EmployeeID)
            {
                XtraMessageBox.Show("该处方的开立医生是【" + Common.DataConvertHelper.GetEmpName(adv.Creator) + "】,请选择自己开立处方！");
                return;
            }
            AdviceList.Add(row["AdviceCode"].ToString());
            if (_NewWorkstation.currentReg.PatientType != 1000401)
            {
                #region 医保的
                StringBuilder str = new StringBuilder();
                str.Append(" select CREDENCEID from HIS.his.CM_COST where ");
                str.Append(" BILLCODE in(select BILLCODE from HIS.his.CM_COST where  CREDENCEID='" + AdviceList[0] + "')");
                str.Append("  group by CREDENCEID");

                IList list = WinnerHIS.Common.ContextHelper.ExeSqlList(str.ToString());
                foreach (string obj in list)
                {
                    AdviceList.Add(obj);
                }
                #endregion
            }
            //RefundFrom From = new RefundFrom(AdviceList, _NewWorkstation.currentReg);
            //if (From.ShowDialog(this.ParentForm) == DialogResult.OK)
            //{

            //}
        }

        private WinnerSoft.Report.DAL.Interface.IReportEx report;
        protected WinnerSoft.Report.DAL.Interface.IReportEx Report
        {
            get
            {
                if (this.report == null)
                {
                    this.report = WinnerSoft.Report.DAL.Interface.DALHelper.DALManager.CreateReportEx();
                    this.report.Session = WinnerHIS.Common.ContextHelper.Session;
                }

                return this.report;
            }
        }
        private WinnerSoft.Report.Controls.RDLViewDialog printForm;
        protected WinnerSoft.Report.Controls.RDLViewDialog PrintForm
        {
            get
            {
                if (this.printForm == null)
                {
                    this.printForm = new WinnerSoft.Report.Controls.RDLViewDialog();
                }

                return this.printForm;
            }
        }
        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            if (row != null)
            {
                toolStripMenuItem1.Enabled = true;
                toolStripMenuItem2.Enabled = true;
                toolStripMenuItem3.Enabled = true;
                toolStripMenuItem6.Visible = false;
                object obj = row["Object"] as object;
                if (obj is IDrugAdvice)
                {
                    toolStripMenuItem6.Visible = true;
                    toolStripMenuItem6.Enabled = true;

                }
                toolStripMenuItem5.Enabled = true;
                toolStripMenuItem5.Visible = true;
            }
            else
            {
                toolStripMenuItem1.Enabled = false;
                toolStripMenuItem2.Enabled = false;
                toolStripMenuItem3.Enabled = false;
                toolStripMenuItem5.Enabled = false;
                toolStripMenuItem6.Enabled = false;
            }

        }
        #region 治疗申请单
        /// <summary>
        /// 治疗单,You can select mutiply advice codes and print them together;
        /// According different prescribing code and excute department
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreatmentListTool_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<ICureAdvice>> advDict = new Dictionary<string, List<ICureAdvice>>();
            #region 获取选中行
            string advKey = string.Empty;
            foreach (DataRow dr in (gridControlAdvice.DataSource as DataTable).Rows)
            {
                string advCode = "";
                string msg = string.Empty;
                object obj = dr["Object"] as object;
                if (!VeryfyAdvStatus(dr, ref advCode, ref msg))
                {
                    if (!string.IsNullOrEmpty(msg))
                        XtraMessageBox.Show(msg);
                    continue;
                }
                ICureAdvice adv = obj as ICureAdvice;
                advKey = advCode + "-" + adv.ExecDept.ToString();
                if (!advDict.ContainsKey(advKey))
                {
                    List<ICureAdvice> advList = new List<ICureAdvice>();
                    advList.Add(adv);
                    advDict.Add(advKey, advList);
                }
                else
                {
                    List<ICureAdvice> advlist = advDict[advKey];
                    advlist.Add(adv);
                }
            }
            if (advDict.Count == 0)
            {
                DataRow row1 = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
                string advCode = "";
                string msg = string.Empty;
                object obj = row1["Object"] as object;
                if (!VeryfyAdvStatus(row1, ref advCode, ref msg))
                {
                    if (!string.IsNullOrEmpty(msg))
                        XtraMessageBox.Show(msg);
                    return;
                }
                ICureAdvice adv = obj as ICureAdvice;
                List<ICureAdvice> advList = new List<ICureAdvice>();
                advList.Add(adv);
                advDict.Add(advCode + "-" + adv.ExecDept.ToString(), advList);
            }

            #endregion
            foreach (var adKey in advDict)
            {
                #region 打印
                ICureadviceReportList preList = DALHelper.ClinicDAL.CreateCureadviceReportList();
                preList.Session = WinnerHIS.Common.ContextHelper.Session;
                preList.GetCureadviceReportList(currentCode, patInfo, adKey.Value, WinnerHIS.Common.ContextHelper.Employee.Name);

                this.Report.Name = "门诊治疗单";
                this.Report.Refresh();

                if (Report.Exists)
                {
                    this.PrintForm.Report = this.Report;
                    this.PrintForm.DataObject = preList;
                    this.PrintForm.Print(false);
                }
                #endregion
            }
        }

        private bool VeryfyAdvStatus(DataRow dr, ref string advCode, ref string msg)
        {
            object obj = dr["Object"] as object;
            if (Convert.ToBoolean(dr["CheckObj"].ToString()))
            {
                #region 验证
                if (obj is IDrugAdvice)
                {
                    IDrugAdvice advice = obj as IDrugAdvice;
                    msg = "[" + advice.DrugName + "]药品处方不能打印申请单！";
                    return false;
                }
                else if (obj is ICureAdvice)
                {
                    ICureAdvice Advice = obj as ICureAdvice;
                    advCode = Advice.AdviceCode;
                    return true;
                }
                else if (obj is ICheckAdvice)
                {
                    ICheckAdvice advice = obj as ICheckAdvice;
                    msg = "[" + advice.ItemName + "]检查不能打印诊疗申请单！";
                    return false;
                }
                else
                {
                    msg = "请选择已经审核的处方进行打印！";
                    return false;
                }
                #endregion

            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检验报告单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LisSingleTool_Click(object sender, EventArgs e)
        {
            //BaoGaoDanNew o = new BaoGaoDanNew();
            //o.RegCode = _NewWorkstation.currentReg;
            //o.Initialize();
            //o.Show();
        }


        /// <summary>
        /// 打印  检验科申请单  or  检查申请单 的通用方法
        /// </summary>
        /// <param name="ReportName"></param>
        public void CheckApplyCurrency()
        {
            try
            {
                #region 打印
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//鼠标为忙碌状态
                if (patInfo == null)
                {
                    patInfo = DALHelper.ClinicDAL.CreatePatientDetail();
                    patInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                }

                patInfo.OrgCode = _NewWorkstation.currentReg.OrgCode;
                patInfo.RegCode = _NewWorkstation.currentReg.Code;
                patInfo.Refresh();
                if (!patInfo.Exists)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                    XtraMessageBox.Show("请选中病人！");
                    return;
                }
                DataTable dt = (gridControlAdvice.DataSource as DataTable).Clone();
                #region 获取选中行
                foreach (DataRow Row in (gridControlAdvice.DataSource as DataTable).Rows)
                {
                    if (Convert.ToBoolean(Row["CheckObj"].ToString()))
                    {
                        DataRow NewRos = dt.NewRow();
                        NewRos.ItemArray = Row.ItemArray;
                        dt.Rows.Add(NewRos);

                    }
                }
                #endregion

                //是否是右击选中
                bool checkFocuse = false;
                if (dt.Rows.Count == 0)
                {
                    DataRow FocuseRow = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
                    DataRow NewRos = dt.NewRow();
                    NewRos.ItemArray = FocuseRow.ItemArray;
                    dt.Rows.Add(NewRos);

                    checkFocuse = true;
                }
                DataRow row = dt.Rows[0];
                string adviceCode = "";
                int exeDept = 0;
                #region 验证
                object obj = row["Object"] as object;
                if (!(obj is ICheckAdvice))
                {
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                    XtraMessageBox.Show("请选择检查项目！");
                    return;
                }
                adviceCode = (obj as ICheckAdvice).AdviceCode;
                exeDept = (obj as ICheckAdvice).ExecDept;
                #endregion

                #region 添加列
                DataTable sumTable = new DataTable();
                sumTable.Columns.Add("Project");
                sumTable.Columns.Add("Number");
                sumTable.Columns.Add("Unit");
                sumTable.Columns.Add("Remarks");
                sumTable.Columns.Add("BuWei");
                sumTable.Columns.Add("Mudi");
                sumTable.Columns.Add("Price");
                sumTable.Columns.Add("Description");
                sumTable.Columns.Add("xh");
                #endregion
                Decimal cash = 0.00M;
                DataTable dtAdvice;
                if (checkFocuse)//如果全部打印
                {
                    dtAdvice = (this.gridControlAdvice.DataSource as DataTable).Copy();
                }
                else
                {
                    dtAdvice = dt;
                }
                string reportName = string.Empty;//报表名称
                Dictionary<int, DataTable> deptRepList = new Dictionary<int, DataTable>();//科室报表--datatable
                Dictionary<int, Decimal> deptCashList = new Dictionary<int, Decimal>();//科室报表-金额
                #region 重新排序,按照机器绑定
                IItemInfoEx itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
                itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                #endregion 
                int xh = 1;
                foreach (DataRow dr in dtAdvice.Rows)
                {
                    #region 添加行
                    if (dr["Object"] is ICheckAdvice)
                    {
                        if (dr["AdviceCode"].ToString() == adviceCode || !checkFocuse)
                        {
                            ICheckAdvice chkAdv = dr["Object"] as ICheckAdvice;
                            if (chkAdv.ItemCode.IndexOf("AP") > -1)//如果是组套
                            {
                                if (deptRepList.ContainsKey(chkAdv.ExecDept))//如果已有
                                {
                                    DataTable deptTable = deptRepList[chkAdv.ExecDept];
                                    DataRow rowTmp = deptTable.NewRow();
                                    rowTmp["xh"] = xh;
                                    rowTmp["Project"] = chkAdv.ItemName;
                                    rowTmp["Unit"] = chkAdv.Unit;
                                    rowTmp["Number"] = chkAdv.Number;
                                    rowTmp["Remarks"] = chkAdv.Remarks;
                                    rowTmp["BuWei"] = chkAdv.BuWei;
                                    rowTmp["Mudi"] = chkAdv.MuDi;
                                    rowTmp["Price"] = chkAdv.Price.ToString("F2");
                                    deptTable.Rows.Add(rowTmp);
                                    deptCashList[chkAdv.ExecDept] += chkAdv.Cash;
                                }
                                else//如果新建
                                {
                                    DataTable deptTable = sumTable.Clone();

                                    DataRow rowTmp = deptTable.NewRow();
                                    rowTmp["xh"] = xh;
                                    rowTmp["Project"] = chkAdv.ItemName;
                                    rowTmp["Unit"] = chkAdv.Unit;
                                    rowTmp["Number"] = chkAdv.Number;
                                    rowTmp["Remarks"] = chkAdv.Remarks;
                                    rowTmp["Mudi"] = chkAdv.MuDi;
                                    rowTmp["BuWei"] = chkAdv.BuWei;
                                    rowTmp["Price"] = chkAdv.Price.ToString("F2");
                                    deptTable.Rows.Add(rowTmp);
                                    deptRepList.Add(chkAdv.ExecDept, deptTable);
                                    deptCashList.Add(chkAdv.ExecDept, chkAdv.Cash);
                                }
                            }
                            else //如果是明细
                            {
                                itemInfo.Code = chkAdv.ItemCode;
                                itemInfo.Refresh();

                                if (deptRepList.ContainsKey(chkAdv.ExecDept))//如果已有
                                {
                                    DataTable deptTable = deptRepList[chkAdv.ExecDept];
                                    DataRow rowTmp = deptTable.NewRow();
                                    rowTmp["xh"] = xh;
                                    rowTmp["Project"] = chkAdv.ItemName;
                                    rowTmp["Unit"] = chkAdv.Unit;
                                    rowTmp["Number"] = chkAdv.Number;
                                    rowTmp["Remarks"] = chkAdv.Remarks;
                                    rowTmp["Mudi"] = chkAdv.MuDi;
                                    rowTmp["BuWei"] = chkAdv.BuWei;
                                    rowTmp["Price"] = chkAdv.Price.ToString("F2");
                                    rowTmp["Description"] = itemInfo.Remarks;
                                    deptTable.Rows.Add(rowTmp);
                                    deptCashList[chkAdv.ExecDept] += chkAdv.Cash;
                                }
                                else//如果新建
                                {
                                    DataTable deptTable = sumTable.Clone();
                                    DataRow rowTmp = deptTable.NewRow();
                                    rowTmp["xh"] = xh;
                                    rowTmp["Project"] = chkAdv.ItemName;
                                    rowTmp["Unit"] = chkAdv.Unit;
                                    rowTmp["Number"] = chkAdv.Number;
                                    rowTmp["Remarks"] = chkAdv.Remarks;
                                    rowTmp["Mudi"] = chkAdv.MuDi;
                                    rowTmp["BuWei"] = chkAdv.BuWei;
                                    rowTmp["Price"] = chkAdv.Price.ToString("F2");
                                    rowTmp["Description"] = itemInfo.Remarks;
                                    deptTable.Rows.Add(rowTmp);
                                    deptRepList.Add(chkAdv.ExecDept, deptTable);
                                    deptCashList.Add(chkAdv.ExecDept, chkAdv.Cash);
                                }
                            }
                            xh++;
                        }
                    }
                    #endregion
                }
                foreach (var dept in deptRepList)
                {
                    if (!reqDeptList.ContainsKey(dept.Key.ToString()))
                        MessageBox.Show("没有配置科室[" + Common.DataConvertHelper.GetDeptName(dept.Key) + "]申请单!");
                    PrintingMethod.CheckApply(currentCode, patInfo, dept.Value, dept.Key, deptCashList[dept.Key], reqDeptList[dept.Key.ToString()].ToString());
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
            }
        }



        /// <summary>
        /// 检验科申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolApplyRadiation_Click(object sender, EventArgs e)
        {
            CheckApplyCurrency();
        }

        /// <summary>
        /// DR检查申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DR_Tool_Click(object sender, EventArgs e)
        {
            IPatientDetail detail = DALHelper.ClinicDAL.CreatePatientDetail();
            detail.Session = WinnerHIS.Common.ContextHelper.Session;
            detail.OrgCode = _NewWorkstation.currentReg.OrgCode;
            detail.RegCode = _NewWorkstation.currentReg.Code;
            detail.Refresh();
            if (!detail.Exists)
            {
                XtraMessageBox.Show("请选中病人！");
                return;
            }
            PrintingMethod.DRApply(detail, "DR申请单");
        }
        /// <summary>
        /// 骨龄申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoneAgeTool_Click(object sender, EventArgs e)
        {
            IPatientDetail detail = DALHelper.ClinicDAL.CreatePatientDetail();
            detail.Session = WinnerHIS.Common.ContextHelper.Session;
            detail.OrgCode = _NewWorkstation.currentReg.OrgCode;
            detail.RegCode = _NewWorkstation.currentReg.Code;
            detail.Refresh();
            if (!detail.Exists)
            {
                XtraMessageBox.Show("请选中病人！");
                return;
            }
            PrintingMethod.BoneAge(detail, _NewWorkstation.currentReg, "骨龄申请单");
        }
        /// <summary>
        /// MRI申请单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolMRI_Click(object sender, EventArgs e)
        {
            try
            {
                #region 打印
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//鼠标为忙碌状态
                if (patInfo == null)
                {
                    patInfo = DALHelper.ClinicDAL.CreatePatientDetail();
                    patInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                }

                patInfo.OrgCode = _NewWorkstation.currentReg.OrgCode;
                patInfo.RegCode = _NewWorkstation.currentReg.Code;
                patInfo.Refresh();
                if (!patInfo.Exists)
                {
                    XtraMessageBox.Show("请选中病人！");
                    return;
                }
                DataTable dt = (gridControlAdvice.DataSource as DataTable).Clone();
                #region 获取选中行
                foreach (DataRow Row in (gridControlAdvice.DataSource as DataTable).Rows)
                {
                    if (Convert.ToBoolean(Row["CheckObj"].ToString()))
                    {
                        DataRow NewRos = dt.NewRow();
                        NewRos.ItemArray = Row.ItemArray;
                        dt.Rows.Add(NewRos);

                    }
                }
                #endregion

                //是否是右击选中
                bool checkFocuse = false;
                if (dt.Rows.Count == 0)
                {
                    DataRow FocuseRow = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
                    DataRow NewRos = dt.NewRow();
                    NewRos.ItemArray = FocuseRow.ItemArray;
                    dt.Rows.Add(NewRos);

                    checkFocuse = true;
                }
                DataRow row = dt.Rows[0];
                string adviceCode = "";
                int exeDept = 0;
                #region 验证
                object obj = row["Object"] as object;
                if (!(obj is ICheckAdvice))
                {
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                    XtraMessageBox.Show("请选择检查项目！");
                    return;
                }
                adviceCode = (obj as ICheckAdvice).AdviceCode;
                exeDept = (obj as ICheckAdvice).ExecDept;
                #endregion

                #region 添加列
                DataTable sumTable = new DataTable();
                sumTable.Columns.Add("Project");
                sumTable.Columns.Add("Number");
                sumTable.Columns.Add("Unit");
                sumTable.Columns.Add("Remarks");
                sumTable.Columns.Add("BuWei");
                sumTable.Columns.Add("Mudi");
                sumTable.Columns.Add("Price");
                sumTable.Columns.Add("Description");
                #endregion
                DataTable dtAdvice;
                if (checkFocuse)//如果全部打印
                {
                    dtAdvice = (this.gridControlAdvice.DataSource as DataTable).Copy();
                }
                else
                {
                    dtAdvice = dt;
                }
                DataTable deptTable = sumTable.Clone();

                decimal sumcash = 0;
                #region 重新排序,按照机器绑定
                IItemInfoEx itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
                itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                #endregion 
                foreach (DataRow dr in dtAdvice.Rows)
                {
                    #region 添加行
                    if (dr["Object"] is ICheckAdvice)
                    {
                        if (dr["AdviceCode"].ToString() == adviceCode || !checkFocuse)
                        {
                            ICheckAdvice chkAdv = dr["Object"] as ICheckAdvice;
                            if (chkAdv.ExecDept != 210288)//放射科
                            {
                                continue;
                            }
                            itemInfo.Code = chkAdv.ItemCode;
                            itemInfo.Refresh();
                            DataRow rowTmp = deptTable.NewRow();
                            rowTmp["Project"] = chkAdv.ItemName;
                            rowTmp["Unit"] = chkAdv.Unit;
                            rowTmp["Number"] = chkAdv.Number;
                            rowTmp["Remarks"] = chkAdv.Remarks;
                            rowTmp["BuWei"] = chkAdv.BuWei;
                            rowTmp["Mudi"] = chkAdv.MuDi;
                            rowTmp["Price"] = chkAdv.Price.ToString("F2");
                            rowTmp["Description"] = itemInfo.Remarks;
                            deptTable.Rows.Add(rowTmp);
                            sumcash += chkAdv.Cash;
                        }
                    }
                    #endregion
                }
                if (deptTable.Rows.Count > 0)
                {
                    PrintingMethod.MRIApply(currentCode, patInfo, deptTable, 210288, sumcash);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
            }
        }
        #endregion

        /// <summary>
        /// 内容复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolCopy_Click(object sender, EventArgs e)
        {
            List<object> AdviceList = new List<object>();

            //string str = "";
            DataTable SumTable = this.gridControlAdvice.DataSource as DataTable;
            foreach (DataRow dr in SumTable.Rows)
            {
                if (Convert.ToBoolean(dr["CheckObj"]))
                {
                    object obj = dr["Object"] as object;
                    AdviceList.Add(obj);
                }
            }
            if (AdviceList.Count == 0)//没有勾选那就是右击
            {
                DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
                object obj = row["Object"] as object;
                AdviceList.Add(obj);
            }
            string str = "";
            foreach (object obj in AdviceList)
            {
                var index = gridViewAdvice.GetFocusedDataSourceRowIndex();

                if (obj is IDrugAdvice)
                {
                    IDrugAdvice Advice = obj as IDrugAdvice;
                    //if (Advice.Frequence == 0 && Advice.Method == 0) //中药的
                    if (Advice.Method == 0) //中药的
                    {
                        str += Advice.DrugName + Advice.SmallUnit + " " + Advice.Dosage + Advice.DosageUnit + " " + " " + Advice.Pack + "付 "
                                              + Advice.Memo + "\r\n";
                    }
                    else
                    {
                        if (Advice.Remarks != "")
                        {
                            str += Advice.DrugName + " " + Advice.Spec + " " + Advice.Number + Advice.SmallUnit + " " + ReturnMethodNo(Advice.Method) + " " + Advice.Remarks + "\r\n";
                        }
                        else
                        {
                            string Mename = gridViewAdvice.GetRowCellValue(index, "Mode").ToString();


                            str += Advice.DrugName + " " + Advice.Spec + " " + Advice.Number + Advice.SmallUnit + " " + Advice.Dosage + Advice.DosageUnit + " "
                                            + ReturnMethod(Advice.Method) + " " + ReturnFrequency(Advice.Frequence) + "\r\n";
                        }
                    }

                }
                else if (obj is ICureAdvice)
                {
                    ICureAdvice Advice = obj as ICureAdvice;
                    str += Advice.ItemName + " " + Advice.Number + Advice.Unit;
                }
                else if (obj is ICheckAdvice)
                {
                    ICheckAdvice Advice = obj as ICheckAdvice;
                    str += Advice.ItemName + " " + Advice.Number + Advice.Unit;
                }

            }
            CopyData(str);
            XtraMessageBox.Show(this, "内容已经复制成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 根据编码返回频率
        /// </summary>
        /// <returns></returns>
        public string ReturnFrequencyNo(int FrequencyCode)
        {
            if (FrequencyCode == 0)
            {
                return "";
            }
            IDrugFrequency freq = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateDrugFrequency();
            freq.Session = WinnerHIS.Common.ContextHelper.Session;
            freq.Code = FrequencyCode;
            freq.Refresh();
            if (freq != null)
            {
                return freq.Symbol;
            }
            else
            {
                return "";
            }

        }
        /// <summary>
        /// 返回用药方式
        /// </summary>
        /// <param name="Method"></param>
        /// <returns></returns>
        public string ReturnMethodNo(int Method)
        {
            if (Method == 0)
            {
                return "";
            }
            IDrugMethod methodTemp = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateDrugMethod();
            methodTemp.Session = WinnerHIS.Common.ContextHelper.Session;
            methodTemp.Code = Method;
            methodTemp.Refresh();
            if (methodTemp != null)
            {
                return methodTemp.Symbol;
            }
            else
            {
                return "";
            }
        }
        public bool CopyData(string data)
        {
            try
            {
                bool result = false;
                if (string.IsNullOrEmpty(data))
                    return false;
                else
                {
                    System.Windows.Forms.Clipboard.SetDataObject(data.ToString(), true, 3, 100);
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("复制失败：剪贴板被其他线程或应用程序占用。\n" + ex.Message);
                return false;
            }

        }
        #endregion

        #region 处方签
        private void toolStripMenuPrintAdv_Click(object sender, EventArgs e)
        {
            PrescriptionLabel();
        }
        /// <summary>
        /// 打印处方
        /// </summary>
        /// <param name="AStype"></param>
        public void PrescriptionLabel()
        {
            try
            {
                #region 打印
                WinnerHIS.Drug.DrugTable.DAL.Interface.IPrescriptionList preList = DALHelper.DrugTableDAL.CreatePrescriptionList();
                preList.Session = WinnerHIS.Common.ContextHelper.Session;

                if (patInfo == null)
                {
                    patInfo = DALHelper.ClinicDAL.CreatePatientDetail();
                    patInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                    patInfo.OrgCode = _NewWorkstation.currentReg.OrgCode;
                    patInfo.RegCode = _NewWorkstation.currentReg.Code;
                    patInfo.Refresh();
                }

                if (!patInfo.Exists)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                    XtraMessageBox.Show("请选中病人！");
                    return;
                }

                int drugType = (int)Common.DrugTypeEnum.ChineseHerbal;
                string advType = "202105131";//普通处方
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//鼠标为忙碌状态



                DataRow FocuseRow = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
                DataTable dt = gridControlAdvice.DataSource as DataTable;
                List<IDrugAdvice> DrugList = new List<IDrugAdvice>();
                DataTable dtPrescription = new DataTable();
                DataTable dtPrescription_temp = new DataTable();

                if (drugAdvList == null)
                {
                    drugAdvList = DALHelper.ClinicDAL.CreateDrugAdviceList();
                    drugAdvList.Session = CacheHelper.Session;
                }
                drugAdvList.Rows.Clear();

                foreach (DataRow dr in dt.Rows)
                {
                    object obj = dr["Object"] as object;
                    if (FocuseRow["AdviceCode"].ToString() == dr["AdviceCode"].ToString() && obj is IDrugAdvice)
                    {
                        IDrugAdvice drugAdvTmp = obj as IDrugAdvice;
                        advType = drugAdvTmp.PrescriptionType == string.Empty ? "202105131" : drugAdvTmp.PrescriptionType;
                        DrugList.Add(drugAdvTmp);
                        drugAdvList.AddEntity(drugAdvTmp);
                        drugType = drugAdvTmp.Type;
                    }
                }
                if (DrugList.Count == 0)
                {
                    XtraMessageBox.Show("请选择药品医嘱！");
                    return;
                }
                if (drugType == (int)Common.DrugTypeEnum.ChineseHerbal)//如果中草药
                {
                    #region 中药
                    this.Report.Name = "门诊处方(草药)";
                    preList.GetPrescription2(drugAdvList, patInfo, WinnerHIS.Common.ContextHelper.Employee.Name);
                    dtPrescription = preList.DataTable;
                    dtPrescription_temp = dtPrescription.Copy();

                    while ((dtPrescription.Rows.Count % this.prescriptionCDrugNumber) > 0)
                    {
                        //  根据list个数 补充为整数，控制打印长度  只改了西药的处方签

                        DataRow dr = dtPrescription.NewRow();
                        dr["DrugName"] = "";
                        dr["Spec"] = "";
                        dr["Dosage"] = "";
                        dr["Method"] = "";
                        dr["Frequence"] = "";
                        dr["Number"] = 0;
                        dtPrescription.Rows.Add(dr);
                    }
                    #endregion
                }
                else
                {
                    #region 西药
                    preList.GetPrescription(drugAdvList, patInfo, WinnerHIS.Common.ContextHelper.Employee.Name);

                    dtPrescription = preList.DataTable;

                    dtPrescription_temp = dtPrescription.Copy();

                    this.Report.Name = Common.DataConvertHelper.GetGbCodeName(int.Parse(advType));
                    bool bakSql = true;
                    while ((dtPrescription.Rows.Count % this.prescriptionWDrugNumber) > 0)
                    {
                        //  根据list个数 补充为整数，控制打印长度  只改了西药的处方签
                        DataRow dr = dtPrescription.NewRow();

                        if (bakSql)
                        {
                            dr["DrugName"] = "以下空白";
                            bakSql = false;
                        }
                        else
                        {
                            dr["DrugName"] = "";
                        }
                        dr["Spec"] = "";
                        dr["Dosage"] = "";
                        dr["Method"] = "";
                        dr["Frequence"] = "";
                        dr["Number"] = 0;
                        dtPrescription.Rows.Add(dr);
                    }
                    #endregion

                }

                this.Report.Refresh();

                if (Report.Exists)
                {
                    if (drugType == (int)DrugTypeEnum.ChineseHerbal)//中药
                    {
                        //i 代表页码   
                        #region 中药
                        for (int i = 0; i < (dtPrescription.Rows.Count / this.prescriptionCDrugNumber); i++)
                        {
                            dtPrescription_temp.Rows.Clear();//临时表存放要打印的数据
                            dtPrescription_temp = dtPrescription.Copy();

                            int num = dtPrescription.Rows.Count;
                            foreach (DataRow pres in dtPrescription.Rows) //循环提取某一页内的数据
                            {
                                num--;
                                if ((num < (i + 1) * prescriptionCDrugNumber) && (num >= i * prescriptionCDrugNumber))
                                {
                                    // dtPrescription_temp.Rows.Add(pres); ADD 有问题 不能加载另外一个表的数据，所以用removeat
                                }
                                else
                                {
                                    dtPrescription_temp.Rows.RemoveAt(num);
                                }
                            }
                            this.PrintForm.Report = this.Report;
                            this.PrintForm.DataTableObject = dtPrescription_temp;
                            this.PrintForm.Print(); //this.PrintForm.ShowDialog(this.ParentForm);
                        }
                        #endregion

                    }
                    else
                    {
                        //i 代表页码   
                        #region 西药
                        if (advType == "202105131" || advType == "202105131" || advType == "202105132" || advType == "202105133")
                        {
                            for (int i = 0; i < (dtPrescription.Rows.Count / this.prescriptionWDrugNumber); i++)
                            {
                                dtPrescription_temp.Rows.Clear();//临时表存放要打印的数据
                                dtPrescription_temp = dtPrescription.Copy();

                                int num = dtPrescription.Rows.Count;
                                foreach (DataRow pres in dtPrescription.Rows) //循环提取某一页内的数据
                                {
                                    num--;
                                    if ((num < (i + 1) * prescriptionWDrugNumber) && (num >= i * prescriptionWDrugNumber))
                                    {
                                        // dtPrescription_temp.Rows.Add(pres); ADD 有问题 不能加载另外一个表的数据，所以用removeat
                                    }
                                    else
                                    {
                                        dtPrescription_temp.Rows.RemoveAt(num);
                                    }
                                }
                                this.PrintForm.Report = this.Report;
                                this.PrintForm.DataTableObject = dtPrescription_temp;
                                //this.PrintForm.ShowDialog(this.ParentForm);
                                this.PrintForm.Print();
                            }
                        }
                        else
                        {
                            #region 如果是精麻类
                            DataTable SumTable = new DataTable();
                            SumTable.Columns.Add("No");
                            SumTable.Columns.Add("AdviceCode");
                            SumTable.Columns.Add("DrugName");
                            SumTable.Columns.Add("Spec");
                            SumTable.Columns.Add("Number");
                            SumTable.Columns.Add("Price", typeof(decimal));
                            SumTable.Columns.Add("Content");
                            SumTable.Columns.Add("Doctor");
                            SumTable.Columns.Add("Amount", typeof(decimal));
                            int i = 1;
                            foreach (IDrugAdvice obj in DrugList)
                            {
                                DataRow Row = SumTable.NewRow();
                                Row["No"] = i + ".";
                                Row["DrugName"] = obj.DrugName;
                                Row["AdviceCode"] = obj.AdviceCode;
                                Row["Spec"] = obj.Spec;
                                Row["Number"] = obj.Number + obj.SmallUnit;
                                Row["Price"] = obj.Price;
                                Row["Amount"] = obj.Price * obj.Number;
                                Row["Doctor"] = obj.Creator;
                                Row["Content"] = "Sig：" + obj.Dosage + obj.DosageUnit + " " + ReturnMethod(obj.Method) + " " + ReturnFrequency(obj.Frequence) + obj.Remarks;
                                SumTable.Rows.Add(Row);
                                i++;
                            }
                            PrintingMethod.PrescriptionMental(SumTable, Common.DataConvertHelper.GetGbCodeName(int.Parse(advType)), patInfo, true);
                            #endregion
                        }
                        #endregion
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
            }
        }
        #endregion

        IDrugAdvice advice;
        private void 药品执行单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 长期执行单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLong_Click(object sender, EventArgs e)
        {
            PriceExecute(0);
        }
        /// <summary>
        /// 临时执行单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTem_Click(object sender, EventArgs e)
        {
            PriceExecute(1);
        }

        void PriceExecute(int flag)
        {
            string HosName = WinnerHIS.Common.AppSettingHelper.Instance.GetHospitalName();
            DataTable SumTable = new DataTable();
            SumTable.Columns.Add("HosName", typeof(string));//医院名称
            SumTable.Columns.Add("PName", typeof(string));//姓名
            SumTable.Columns.Add("Age", typeof(string));//年龄
            SumTable.Columns.Add("Sex", typeof(string));//性别
            SumTable.Columns.Add("Dept", typeof(string));///住院病区
            SumTable.Columns.Add("BedNo", typeof(string));//床号
            SumTable.Columns.Add("Pnumber", typeof(string));//门诊号
            SumTable.Columns.Add("CreationTime", typeof(string));//开立时间
            SumTable.Columns.Add("Creator", typeof(string));//开立医生
            SumTable.Columns.Add("Nurse", typeof(string));//执行护士
            SumTable.Columns.Add("ItemName", typeof(string));//内容
            SumTable.Columns.Add("StopTime", typeof(string));//停止时间
            SumTable.Columns.Add("ICDName", typeof(string));//诊断
            DataTable tb = this.gridControlAdvice.DataSource as DataTable;

            //DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            #region 全部医嘱
            List<int> zuhaoarry = new List<int>();
            DataTable dbcom = SumTable.Clone();
            dbcom.Columns.Add("zuhao");
            for (int j = 0; j < tb.Rows.Count; j++)
            {
                DataRow r = tb.Rows[j];
                if (Convert.ToBoolean(r["CheckObj"]))
                {
                    DataRow dr = dbcom.NewRow();
                    DataRow row = this.gridViewAdvice.GetDataRow(j);
                    object obj = row["Object"] as object;
                    advice = obj as IDrugAdvice;
                    if (advice == null)
                    {
                        continue;
                    }
                    else
                    {
                        //if ((int)advice.AdvState == 1 || (int)advice.AdvState == 2 || (int)advice.AdvState == 3 || (int)advice.AdvState == 4 || (int)advice.AdvState == 8)
                        //{
                        IRegCode currentRegCode = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreateRegCode();
                        currentRegCode.Session = WinnerHIS.Common.ContextHelper.Session;
                        currentRegCode.Code = advice.RegCode;//门诊号
                        currentRegCode.OrgCode = WinnerHIS.Common.ContextHelper.Account.OrgCode;
                        currentRegCode.Refresh();
                        dr["HosName"] = HosName;
                        dr["PName"] = advice.Name;
                        dr["Age"] = WinnerHIS.Common.ContextHelper.GetAge(currentRegCode.Birthday, currentRegCode.EventTime);
                        dr["Sex"] = currentRegCode.Sex;
                        dr["Dept"] = WinnerHIS.Common.DataConvertHelper.GetDeptName(advice.DeptID);
                        dr["BedNo"] = "";
                        dr["Pnumber"] = advice.RegCode;
                        dr["CreationTime"] = advice.CreateTime;
                        dr["Creator"] = WinnerHIS.Common.DataConvertHelper.GetEmpName(advice.Creator);

                        //dr["Nurse"] = advice.Execter == 0 ? "" : DataConvertHelper.GetEmpName(advice.Execter);
                        //药品名称 剂量  用药评率  滴速
                        if (advice.DripSpeed == 0)
                        {
                            dr["ItemName"] = advice.DrugName.ToString() + " | " + advice.Dosage.ToString() + advice.DosageUnit.ToString() + " | " + WinnerHIS.Common.DataConvertHelper.GetGbCodeName(advice.Method) + " | " + ReturnFrequency(advice.Frequence) + "\n";
                        }
                        else
                        {
                            dr["ItemName"] = advice.DrugName.ToString() + " | " + advice.Dosage.ToString() + advice.DosageUnit.ToString() + " | " + WinnerHIS.Common.DataConvertHelper.GetGbCodeName(advice.Method) + " | " + ReturnFrequency(advice.Frequence) + " |滴速:" + advice.DripSpeed + "ml/h \n";
                        }

                        dr["StopTime"] = "";
                        #region 分组
                        int groupon = advice.GroupNo;
                        if (groupon == 0)
                        {
                            if (zuhaoarry.Count > 0)
                            {
                                groupon = zuhaoarry.Max() + 1;
                                dr["zuhao"] = groupon;
                            }
                            else
                            {
                                groupon = 0;
                                dr["zuhao"] = groupon;
                            }
                        }
                        else
                        {
                            groupon = advice.GroupNo;
                            dr["zuhao"] = groupon;
                        }
                        #endregion
                        dbcom.Rows.Add(dr);
                        if (!zuhaoarry.Contains(groupon))
                        {
                            zuhaoarry.Add(groupon);
                        }
                        //adviceList.Rows.Add(advice); 
                        // }
                    }
                }

            }

            if (advice == null)
            {
                MessageBox.Show("只限药品");
            }
            else
            {
                foreach (int p in zuhaoarry)
                {
                    DataRow dr = SumTable.NewRow();
                    DataView dv = dbcom.DefaultView;
                    dv.RowFilter = "zuhao='" + p + "'";
                    DataTable dvtb = dv.ToTable();
                    foreach (DataRow item in dvtb.Rows)
                    {
                        dr["HosName"] = item["HosName"];
                        dr["PName"] = item["PName"];
                        dr["Age"] = this.txtAge.Text;
                        dr["Sex"] = item["Sex"].ToString() == "1000951" ? "男" : "女";
                        dr["Dept"] = item["Dept"];
                        dr["BedNo"] = item["BedNo"];
                        dr["Pnumber"] = item["Pnumber"];
                        dr["CreationTime"] = item["CreationTime"];
                        dr["Creator"] = item["Creator"];
                        dr["Nurse"] = item["Nurse"];
                        if (Convert.ToInt32(item["zuhao"]) == p)
                        {
                            dr["ItemName"] = dr["ItemName"].ToString() + "\r\n" + item["ItemName"].ToString();
                        }
                        else
                        {
                            dr["ItemName"] = item["ItemName"];
                        }
                        dr["StopTime"] = item["StopTime"];

                    }
                    SumTable.Rows.Add(dr);
                }
                IPatientDetail patient = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreatePatientDetail();
                patient.Session = Common.ContextHelper.Session;
                patient.RegCode = advice.RegCode;
                patient.OrgCode = advice.OrgCode;
                patient.Refresh();
                if (SumTable.Rows.Count <= 6)
                {
                    if (flag == 1)
                    {

                        PrintingMethod.NExecutionSheet(SumTable, patient, "新执行单");
                    }
                    else
                    {
                        PrintingMethod.ExecutionSheet(SumTable, "执行单");
                    }

                }
                else
                {
                    DataTable PrintingTable = SumTable.Clone();
                    for (int s = 0; s < SumTable.Rows.Count; s++)
                    {

                        DataRow dr = PrintingTable.NewRow();
                        dr.ItemArray = SumTable.Rows[s].ItemArray;
                        PrintingTable.Rows.Add(dr);

                        if (PrintingTable.Rows.Count == 6)
                        {
                            if (flag == 1)
                            {
                                PrintingMethod.NExecutionSheet(SumTable, patient, "新执行单");
                            }
                            else
                            {
                                PrintingMethod.ExecutionSheet(SumTable, "执行单");
                            }
                            PrintingTable.Rows.Clear();
                        }
                        else
                        {
                            if (s == SumTable.Rows.Count - 1)
                            {
                                if (flag == 1)
                                {
                                    PrintingMethod.NExecutionSheet(SumTable, patient, "新执行单");
                                }
                                else
                                {
                                    PrintingMethod.ExecutionSheet(SumTable, "执行单");
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }

        private void btnInjec_Click(object sender, EventArgs e)
        {
            DataTable SumTable = new DataTable();
            SumTable.Columns.Add("PName", typeof(string));//姓名
            SumTable.Columns.Add("Age", typeof(string));//年龄
            SumTable.Columns.Add("Sex", typeof(string));//性别
            SumTable.Columns.Add("RegCode", typeof(string));//门诊号
            SumTable.Columns.Add("CreateTime", typeof(string));//开立时间
            SumTable.Columns.Add("DrugName", typeof(string));//药品
            SumTable.Columns.Add("Dosage", typeof(string));//剂量
            SumTable.Columns.Add("Use", typeof(string));//用法
            SumTable.Columns.Add("Frequency", typeof(string));//频率
            SumTable.Columns.Add("Creator", typeof(string));//开立医生
            SumTable.Columns.Add("Nurse", typeof(string));//执行护士
            SumTable.Columns.Add("ExecDate", typeof(string));//执行时间
            SumTable.Columns.Add("Spec", typeof(string));//规格
            SumTable.Columns.Add("Remark", typeof(string));//规格
            SumTable.Columns.Add("Group", typeof(string));//组

            if (gridViewAdvice.RowCount > 0)
            {
                IPatientDetail patInfo = DALHelper.ClinicDAL.CreatePatientDetail();
                patInfo.Session = WinnerHIS.Common.ContextHelper.Session;
                patInfo.OrgCode = _NewWorkstation.currentReg.OrgCode;
                patInfo.RegCode = _NewWorkstation.currentReg.Code;
                patInfo.Refresh();
                foreach (DataRow dr in (gridControlAdvice.DataSource as DataTable).Rows)
                {
                    if (Boolean.Parse(dr["CheckObj"].ToString()) == true)
                    {
                        object obj = dr["Object"] as object;
                        IDrugAdvice drugadvice = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreateDrugAdvice();
                        drugadvice = obj as IDrugAdvice;
                        if (drugadvice == null)
                        {
                            continue;
                        }
                        IRegCode currentRegCode = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreateRegCode();
                        currentRegCode.Session = WinnerHIS.Common.ContextHelper.Session;
                        currentRegCode.Code = drugadvice.RegCode;//门诊号
                        currentRegCode.OrgCode = WinnerHIS.Common.ContextHelper.Account.OrgCode;
                        currentRegCode.Refresh();
                        DataRow row = this.gridViewAdvice.GetDataRow(1);
                        DataRow drdrug = SumTable.NewRow();
                        drdrug["PName"] = drugadvice.Name;
                        drdrug["Age"] = WinnerHIS.Common.ContextHelper.GetAge(currentRegCode.Birthday, currentRegCode.EventTime);
                        drdrug["Sex"] = currentRegCode.Sex;
                        drdrug["RegCode"] = drugadvice.RegCode;
                        drdrug["CreateTime"] = dr["CreationTime"];
                        drdrug["DrugName"] = dr["Content"];
                        drdrug["Dosage"] = dr["Dosage"];
                        drdrug["Spec"] = dr["Spec"];
                        drdrug["Use"] = dr["Mode"];
                        drdrug["Frequency"] = dr["Frequency"];
                        drdrug["Remark"] = dr["Remarks"];
                        drdrug["Creator"] = WinnerHIS.Common.DataConvertHelper.GetEmpName(drugadvice.Creator);
                        drdrug["Nurse"] = "";
                        drdrug["ExecDate"] = "";
                        drdrug["Group"] = dr["GroupNoSymbol"];
                        SumTable.Rows.Add(drdrug);
                    }
                }
                if (SumTable.Rows.Count > 0)
                {
                    PrintingMethod.InjectionPrint(SumTable, patInfo, "注射卡");
                    SumTable.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("仅限药品");
                }

            }
        }

        private void CheckApply_Click(object sender, EventArgs e)
        {

        }

        private void btnCopyAdvice_Click(object sender, EventArgs e)
        {
            copyAdviceList.Clear();
            List<object> AdviceList = new List<object>();

            DataTable SumTable = this.gridControlAdvice.DataSource as DataTable;
            if (SumTable.Rows.Count == 0)
            {
                return;
            }
            foreach (DataRow dr in SumTable.Rows)
            {
                if (Convert.ToBoolean(dr["CheckObj"]))
                {
                    object obj = dr["Object"] as object;
                    AdviceList.Add(obj);
                }
            }
            if (AdviceList.Count == 0)//没有勾选那就是右击
            {
                DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);

                object obj = row["Object"] as object;
                AdviceList.Add(obj);
            }
            copyAdviceList = AdviceList;
            XtraMessageBox.Show(this, "处方已复制在剪贴板！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnPasteAdvice_Click(object sender, EventArgs e)
        {
            if (copyAdviceList.Count == 0)
            {
                XtraMessageBox.Show(this, "剪贴板没有复制的处方！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DateTime ctime = WinnerHIS.Common.ContextHelper.CurrentTime;
            foreach (object obj in copyAdviceList)
            {
                if (obj is IDrugAdvice)
                {
                    IDrugAdvice advicecopy = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreateDrugAdvice();
                   // advicecopy.Session = _NewWorkstation.Session;
                    IDrugAdvice adviceOld = obj as IDrugAdvice;
                    for (int i = 0; i < advicecopy.Propertys.Count; i++)
                    {
                        WinnerSoft.Data.ORM.Property prop = advicecopy.GetProperty(i);
                        if (advicecopy.ContainsProperty(prop.Name))
                        {
                            advicecopy[prop.Name] = adviceOld[i];
                        }
                    }
                    advicecopy.Name = currentCode.Name;
                    advicecopy.RegCode = currentCode.Code;
                    advicecopy.Patient = currentCode.Patient;
                    advicecopy.CreateTime = ctime;
                    advicecopy.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
                    advicecopy.CheckTime = ctime;
                    advicecopy.Checker = adviceOld.Creator;
                    advicecopy.ExecTime = DateTime.Parse("1900-01-01");
                    advicecopy.Executer = 0;
                    advicecopy.AdviceCode = "";
                    advicecopy.ID = 0;
                    advicecopy.AdvState = AdviceState.NotSave;
                    advicecopy.BillCode = null;
                    DrugTableAdd(advicecopy);
                }
                else if (obj is ICureAdvice)
                {
                    ICureAdvice advicecopy = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreateCureAdvice();
                   // advicecopy.Session = _NewWorkstation.Session;
                    ICureAdvice adviceOld = obj as ICureAdvice;
                    for (int i = 0; i < advicecopy.Propertys.Count; i++)
                    {
                        WinnerSoft.Data.ORM.Property prop = advicecopy.GetProperty(i);

                        if (advicecopy.ContainsProperty(prop.Name))
                        {
                            advicecopy[prop.Name] = adviceOld[i];
                        }
                    }
                    advicecopy.Name = currentCode.Name;
                    advicecopy.RegCode = currentCode.Code;
                    advicecopy.Patient = currentCode.Patient;
                    advicecopy.CreateTime = ctime;
                    advicecopy.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
                    advicecopy.CheckTime = ctime;
                    advicecopy.Checker = adviceOld.Creator;
                    advicecopy.ExecTime = DateTime.Parse("1900-01-01");
                    advicecopy.Executer = 0;
                    advicecopy.AdviceCode = "";
                    advicecopy.ID = 0;
                    advicecopy.AdvState = AdviceState.NotSave;
                    CureTableAdd(advicecopy);
                }
                else if (obj is ICheckAdvice)
                {
                    ICheckAdvice advicecopy = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreateCheckAdvice();
                  //  advicecopy.Session = _NewWorkstation.Session;
                    ICheckAdvice adviceOld = obj as ICheckAdvice;
                    for (int i = 0; i < advicecopy.Propertys.Count; i++)
                    {
                        WinnerSoft.Data.ORM.Property prop = advicecopy.GetProperty(i);

                        if (advicecopy.ContainsProperty(prop.Name))
                        {
                            advicecopy[prop.Name] = adviceOld[i];
                        }
                    }
                    advicecopy.Name = currentCode.Name;
                    advicecopy.RegCode = currentCode.Code;
                    advicecopy.Patient = currentCode.Patient;
                    advicecopy.CreateTime = ctime;
                    advicecopy.Creator = WinnerHIS.Common.ContextHelper.Employee.EmployeeID;
                    advicecopy.CheckTime = ctime;
                    advicecopy.Checker = adviceOld.Creator;
                    advicecopy.ExecTime = DateTime.Parse("1900-01-01");
                    advicecopy.Executer = 0;
                    advicecopy.AdviceCode = "";
                    advicecopy.ID = 0;
                    advicecopy.AdvState = AdviceState.NotSave;
                    CheckTableAdd(advicecopy);
                }

            }
        }

        private void gridViewAdvice_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            DataRow dr = gridViewAdvice.GetDataRow(e.RowHandle);
            if (e.Column.Name == "Dosage")
            {
                string dosageNum = dr["Dosage"].ToString();
                object obj = dr["Object"];
                if (!Regex.IsMatch(dosageNum, @"^[+-]?\d*[.]?\d*$"))
                {
                    MessageBox.Show("剂量需要输入数字，请确认！");
                }
                if (Convert.ToInt32(dr["AdvState"]) == (int)AdviceState.NotSave)
                {
                    if (obj is IDrugAdvice)
                    {
                        IDrugAdvice drgAdv = obj as IDrugAdvice;
                        if (drgAdv.Type == (int)DrugTypeEnum.ChineseHerbal)
                        {
                            drgAdv.Number = int.Parse(dosageNum);
                        }
                        drgAdv.Dosage = dosageNum;
                    }
                }
                else if (Convert.ToInt32(dr["AdvState"]) == (int)AdviceState.IsAuditing)
                {
                    if (obj is IDrugAdvice)
                    {
                        IDrugAdvice drgAdv = obj as IDrugAdvice;
                        if (drgAdv.Type == (int)DrugTypeEnum.ChineseHerbal)
                        {
                            drgAdv.Number = int.Parse(dosageNum);
                        }
                        drgAdv.Dosage = dosageNum;

                        drgAdv.RestoreUpdatePK();
                        int updateNum = drgAdv.Update();
                        if (updateNum == 0)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                    }
                }
            }
            if (e.Column.Name == "Frequency")
            {
                string frequency = dr["Frequency"].ToString();

                if (Convert.ToInt32(dr["AdvState"]) == (int)AdviceState.NotSave)
                {
                    object objectAdvice = dr["Object"];
                    if (objectAdvice is IDrugAdvice)
                    {
                        IDrugAdvice drgAdv = objectAdvice as IDrugAdvice;
                        drgAdv.Frequence = int.Parse(frequency);
                    }
                    else if (objectAdvice is ICureAdvice)
                    {
                        ICureAdvice drgAdv = objectAdvice as ICureAdvice;
                        drgAdv.Frequency = int.Parse(frequency);
                    }
                }
                else if (Convert.ToInt32(dr["AdvState"]) == (int)AdviceState.IsAuditing)
                {
                    object objectAdvice = dr["Object"];
                    if (objectAdvice is IDrugAdvice)
                    {
                        IDrugAdvice drgAdv = objectAdvice as IDrugAdvice;
                        drgAdv.Refresh();
                        drgAdv.Frequence = int.Parse(frequency);

                        drgAdv.RestoreUpdatePK();
                        int updateNum = drgAdv.Update();
                        if (updateNum == 0)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                    }
                    else if (objectAdvice is ICureAdvice)
                    {
                        ICureAdvice drgAdv = objectAdvice as ICureAdvice;
                        drgAdv.Refresh();
                        drgAdv.Frequency = int.Parse(frequency);

                        drgAdv.RestoreUpdatePK();
                        int updateNum = drgAdv.Update();
                        if (updateNum == 0)
                        {
                            XtraMessageBox.Show("医嘱状态存在修改,请即使刷新！");
                            SumTableRefresh(currentCode, patInfo, isUseCard, regValidDays, true);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 复制处方内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiCopy_Click(object sender, EventArgs e)
        {

            //西药
            string drg = "药物：";
            //诊疗
            string cure = "治疗：";
            //检查
            string check = "检查：";
            sumTable = this.gridControlAdvice.DataSource as DataTable;
            foreach (DataRow dr in sumTable.Rows)
            {
                if (Convert.ToDecimal(dr["Price"]) < 0)
                {
                    continue;
                }

                object objectAdvice = dr["Object"];
                if (objectAdvice is IDrugAdvice)
                {

                    IDrugAdvice obj = objectAdvice as IDrugAdvice;
                    drg += "\n";
                    drg += "        " + obj.DrugName + " " + obj.Dosage + obj.DosageUnit
                        + " " + Common.DataConvertHelper.GetGbCodeName(Convert.ToInt32(dr["Frequency"])) + "  " + dr["Days"] + "天  "
                        + obj.Number + obj.SmallUnit;


                }
                else if (objectAdvice is ICureAdvice)
                {
                    ICureAdvice obj = objectAdvice as ICureAdvice;

                    cure += "\n";
                    cure += "        " + obj.ItemName + " " + obj.Number + obj.Unit;

                }
                else if (objectAdvice is ICheckAdvice)
                {
                    ICheckAdvice obj = objectAdvice as ICheckAdvice;
                    check += "\n";
                    check += "        " + obj.ItemName + " " + obj.Number + obj.Unit;

                }
            }
            string data = "";
            if (drg != "药物：")
            {
                data += drg + "\n";
            }
            if (cure != "治疗：")
            {
                data += cure + "\n";
            }
            if (check != "检查：")
            {
                data += check + "\n";
            }
            CopyData(data);
        }


    }
}

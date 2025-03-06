using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WinnerSoft.Windows.Controls;

using WinnerHIS.Integral.DataCenter.DAL.Interface;
using WinnerHIS.Integral.Personnel.DAL.Interface;
using WinnerHIS.Clinic.DAL.Interface;
using WinnerHIS.Common;
using HPSoft.FrameWork;
using HPSoft.FrameWork.WinForm;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils;
using SmartHIS.Clinic.DAL.Interface;
using WinnerHIS.Advice;
using System.Diagnostics;
using WinnerHIS.Drug.Dict.DAL.Interface;
using System.Collections;
using DevExpress.XtraTreeList;
using WinnerSoft.Report.Interface;
using DevExpress.XtraEditors;
using WinnerHIS.Analyse.Decision.DAL.Interface;
using HIS.Clinic.ClinicCase.UI;
using WinnerSoft.Data.Access;
using System.Reflection;
using Radiate.Interface.Clinic;
using DevExpress.XtraBars;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HIS.Clinic.DoctorWorkstation;
using WinFormsApp1;
using WinnerHIS.Clinic;
using WinnerSoft.Sessions;

namespace WinnerHIS.Diagnosis.Clinic.DoctorWorkstation.UI
{
    public partial class Workstation : UserControl//: WinnerHIS.Common.BaseExplorerControl
    {
        private ISession Session;
        /// <summary>
        /// 门诊病历
        /// </summary>
        MedicalRecords _MedicalRecords;

        /// <summary>
        /// 处方开立
        /// </summary>
        public PrescribingUser _PrescribingUser;

        /// <summary>
        /// 历史处方
        /// </summary>
        HistoricalPrescription _HistoricalPrescription;

        /// <summary>
        /// 内部私有成员，门诊挂号信息列表。
        /// </summary>
        public IRegCodeList codeList;

        /// <summary>
        /// 内部私有成员，列表显示的诊期类型。
        /// </summary>
        private MedicakCycleType cycleType = MedicakCycleType.Waiting;

        /// <summary>
        /// 选中的人
        /// </summary>
        public IRegCode currentReg = null;


        /// <summary>
        /// 西医诊断
        /// </summary>
        public IICDList WesternMedicine = null;
        /// <summary>
        /// 中医诊断
        /// </summary>
        public IICDList ChineseIcd = null;

        public DataTable dt;
        /// <summary>
        /// 门诊优惠申请
        /// </summary>
        public bool cmCouponCheckFlag;

        //就诊卡数据
        private WinnerMIS.Integral.DataCenter.DAL.Interface.IPeople ipeopleList = null;

        /// <summary>
        /// 病人汇总表
        /// </summary>
        WinnerHIS.Integral.DataCenter.DAL.Interface.IPatient patient;

        /// <summary>
        /// 门诊病人表
        /// </summary>
        IPatientDetail detail;

        /// <summary>
        /// 是否启用一卡通
        /// </summary>
        bool enableHISCard = false;

        /// <summary>
        /// 是否启用诊间支付
        /// </summary>
        bool enableVisitingPay = false;
        /// <summary>
        /// 在集中观察的医生
        /// </summary>
        IList FocusDept;

        /// <summary>
        /// 最大金额
        /// </summary>
        decimal adviceMoney;

        /// <summary>
        /// 限制是否查看跨科人开关
        /// </summary>
        bool restrictedSee;

        /// <summary>
        /// 病历质控时间(等于0就行不启用)
        /// </summary>
        public int RecordDateTime;

        /// <summary>
        /// 挂号有效天数
        /// </summary>
        public int regValidDays = 0;
        /// <summary>
        /// 处方判断方法
        /// </summary>
        public string advVerify;
        /// <summary>
        /// 门诊配置Json集合
        /// </summary>
        public JObject cMJsonList;



        /// <summary>
        /// 处方编码集合
        /// </summary>
        Dictionary<string, string> advCodeList = new Dictionary<string, string>();
        Dictionary<string, string> mjDrugadvCodeList = new Dictionary<string, string>();

        /// <summary>
        /// 当前库存
        /// </summary>
        WinnerHIS.Drug.DrugStore.DAL.Interface.IDrugStoreExLetList storeList;
        public Workstation()
        {
            InitializeComponent();
        }
        //#region 重写 ExplorerControl 属性/方法

        ///// <summary>
        ///// 重写Windows 扩展 ExplorerControl 属性Description。
        ///// </summary>
        //public override string Description
        //{
        //    get
        //    {
        //        return "门诊医生工作台";
        //    }
        //}

        ///// <summary>
        ///// 重写Windows 扩展 ExplorerControl 属性Guid属性。
        ///// </summary>
        //public override Guid Guid
        //{
        //    get
        //    {
        //        return new System.Guid("2AC615FE-C3B3-4D3E-AE81-3386E46F56D6");
        //    }
        //}

        //public override string Group
        //{
        //    get
        //    {
        //        return "门诊业务";
        //    }
        //}

        ///// <summary>
        ///// 重写Windows 扩展 ExplorerControl 属性ObjectName属性。
        ///// </summary>
        //public override string ObjectName
        //{
        //    get
        //    {
        //        return "门诊医生工作台";
        //    }
        //}

        ///// <summary>
        ///// 重写Windows 扩展 ExplorerControl 属性ObjectName属性。
        ///// </summary>
        //public override string ModuleName
        //{
        //    get
        //    {
        //        return "门诊工作台";
        //    }
        //}

        ///// <summary>
        ///// 重写Windows 扩展 IS.Windows.UI.Forms.IModuleForm 属性Run方法。
        ///// </summary>
        ///// <param name="parameters">运行参数。</param>
        //public override IPlugIn Run(params object[] parameters)
        //{
        //    if (this.Account.OriginalID == "")
        //    {
        //        MessageBox.Show("对不起，当前账户没有有效的员工原型信息，无法进行业务操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return null;
        //    }
        //    SetWaitDialogCaption("界面正在初始化...");

        //    this.Initialize();
        //    HideWaitDialog();//隐藏等待窗口
        //    IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
        //    return plg;
        //}

        //#endregion                

        /// <summary>
        /// 初始化。
        /// </summary>
        public void Initialize()
        {
            ClinicHelper.cmJsonList = (JObject)JsonConvert.DeserializeObject(WinnerHIS.Common.AppSettingHelper.Instance.GetAppValue("CM_SwitchJson"));
            FocusDept = WinnerHIS.Common.ContextHelper.ExeSqlList("select EMPID from BaseData.dbo.EMPLOYEE where DEPTID=" + AppSettingHelper.Instance.GetAppValue("Bservation"));
            //获取星期几
            int week = (int)ContextHelper.ServerTime.GetCurrentTime().Date.DayOfWeek;
            if (week == 6 || week == 0)//周六或者周日
            {
                RecordDateTime = Convert.ToInt32(AppSettingHelper.Instance.GetAppValue("RecordDateTime"));
            }
            else
            {
                RecordDateTime = Convert.ToInt32(AppSettingHelper.Instance.GetAppValue("RecordWithin"));
            }
            string couponFlag = AppSettingHelper.Instance.GetAppValue("__HIS_CM", "CouponCheckFlag");
            cmCouponCheckFlag = couponFlag.Split('-')[0] == "1";//门诊优惠申请  0/关闭 1/启用

            regValidDays = Convert.ToInt32(AppSettingHelper.Instance.GetAppValue("RegCodeValidityDays"));

            this.searchControlPatient.Client = this.TreeListPatient;//设置搜索绑定
            TreeListPatient.OptionsBehavior.EnableFiltering = true;//开启过滤功能
            TreeListPatient.OptionsFilter.FilterMode = FilterMode.Smart;//过滤模式

            adviceMoney = Convert.ToDecimal(AppSettingHelper.Instance.GetAppValue("AdviceMaxMoney"));

            restrictedSee = AppSettingHelper.Instance.GetAppValue("DoctorAcrossDeptLimit") == "1";//限制查看跨科病人

            if (restrictedSee)
            {
                this.panelControlTop.Visible = false;
            }

            advVerify = AppSettingHelper.Instance.GetAppValue("AdviceMethods");

            this.codeList = DALHelper.ClinicDAL.CreateRegCodeList();
            this.codeList.Session = this.Session;
            //PanelContent
            enableHISCard = AppSettingHelper.Instance.GetAppValue("AIOCS") == "1";
            enableVisitingPay = AppSettingHelper.Instance.GetAppValue("VisitingBalanceFlag") == "1";
            storeList = WinnerHIS.Drug.DrugStore.DAL.Interface.DALHelper.DALManager.CreateDrugStoreExLetList();
            storeList.Session = WinnerHIS.Common.ContextHelper.Session;

            Kaibar.EditValue = ContextHelper.ServerTime.GetCurrentTime().Date.ToString("yyyy-MM-dd");
            Endbar.EditValue = ContextHelper.ServerTime.GetCurrentTime().Date.ToString("yyyy-MM-dd");

            #region 界面填充
            this._MedicalRecords = new MedicalRecords() { Dock = DockStyle.Fill };
            this.PanelContent.Controls.Add(this._MedicalRecords);
            _MedicalRecords.Visible = true;

            this._PrescribingUser = new PrescribingUser(this) { Dock = DockStyle.Fill };
            this.PanelContent.Controls.Add(this._PrescribingUser);
            PanelContent.AutoScroll = true;
            PanelContent.VerticalScroll.Value = PanelContent.VerticalScroll.Minimum;
            _PrescribingUser.Visible = false;

            this._HistoricalPrescription = new HistoricalPrescription(this) { Dock = DockStyle.Fill };
            this.PanelContent.Controls.Add(this._HistoricalPrescription);
            _HistoricalPrescription.Visible = false;

            #region 动态加载barBtn和UI 
            // 2024/12/18 zdq 不需要加载 多余的
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(" select a.Id,b.ModuleGuid,SortCode,MName,Dll,SpaceName,TechnologyType,Describe,EnableState");
            //stringBuilder.Append(" from BaseData.dbo.PlugDetail a  join BaseData.dbo.PlugModule b on a.ModuleGuid=b.ModuleGuid ");
            //stringBuilder.Append($" where a.ParentGuid='{this.Guid}' and EnableState=0");
            //stringBuilder.Append(" order by SortCode asc");
            //DataTable barBtnDt = ContextHelper.ExeSqlDataTable(stringBuilder.ToString());
            //foreach (DataRow row in barBtnDt.Rows)
            //{
            //    //TODO: 动态加载barBtn和UI
            //    string dll = row["Dll"].ToString();
            //    string spaceName = row["SpaceName"].ToString();

            //    LoadModule(dll, spaceName);
            //}
            #endregion


            #endregion
            TreeNodeAdd();
            _MedicalRecords.ApplyRegCodeBinding();
        }

        #region 反射加载barBtn按钮

        /// <summary>
        /// barButtonk控件字典
        /// </summary>
        public Dictionary<string, BarButtonItem> barButtonList = new Dictionary<string, BarButtonItem>();

        /// <summary>
        /// UI字典
        /// </summary>
        public Dictionary<string, UserControl> UserControlList = new Dictionary<string, UserControl>();

        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="dll">dll名称</param>
        /// <param name="namespaceName">命名空间</param>
        public void LoadModule(string dll, string namespaceName)
        {

            //利用dll的路径加载,同时将此程序集所依赖的程序集加载进来,需后辍名.dll
            Assembly assembly = System.Reflection.Assembly.LoadFrom(dll);
            Type userType = assembly.GetType(namespaceName);

            IBarButton barButton = (IBarButton)Activator.CreateInstance(userType);
            BarButtonItem bar = SetBarManager.GetBarButton(barButton);
            string name = bar.Name;
            BarManagerAdd(barButton.BarName, bar);


            UserControl myControl = (UserControl)assembly.CreateInstance(namespaceName);
            myControl.Visible = true;
            myControl.Dock = DockStyle.Fill;
            this.PanelContent.Controls.Add(myControl);
            UserControlList.Add(barButton.BarName, myControl);

            bar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtn_ItemClick);
        }

        /// <summary>
        ///  barBtn点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DevExpress.XtraBars.ItemClickEventArgs args = e;
            if (args == null)
            {
                return;
            }
            ShowHideUI(args.Item.Name);
        }

        /// <summary>
        /// BarManager 里面添加 barBtn
        /// </summary>
        /// <param name="BarName"></param>
        /// <param name="bar"></param>
        public void BarManagerAdd(string BarName, BarButtonItem bar)
        {
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bar });
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(bar, true) });

            barButtonList.Add(BarName, bar);


        }


        /// <summary>
        /// 显隐空间
        /// </summary>
        /// <param name="itemName"></param>
        public void ShowHideUI(string itemName)
        {
            switch (itemName)
            {
                //门诊病历
                case "btnMedical":
                    _MedicalRecords.Visible = true;
                    _PrescribingUser.Visible = false;
                    _HistoricalPrescription.Visible = false;
                    break;
                //今日处方
                case "btnPrescribing":
                    _MedicalRecords.Visible = false;
                    _PrescribingUser.Visible = true;
                    _HistoricalPrescription.Visible = false;
                    break;
                //历史处方
                case "btnBrowse":
                    _MedicalRecords.Visible = false;
                    _PrescribingUser.Visible = false;
                    _HistoricalPrescription.Visible = true;
                    break;
                default:
                    _MedicalRecords.Visible = false;
                    _PrescribingUser.Visible = false;
                    _HistoricalPrescription.Visible = false;
                    break;
            }
            SetControlHide(itemName);
        }

        /// <summary>
        /// 显示或者隐藏，反射的UI
        /// </summary>
        /// <param name="itemName"></param>
        public void SetControlHide(string itemName)
        {
            foreach (var dic in UserControlList)
            {
                dic.Value.Visible = dic.Key == itemName;
            }
        }
        #endregion

        #region 树节点相关
        /// <summary>
        /// 绑定树节点
        /// </summary>
        public void TreeNodeAdd()
        {
            this.TreeListPatient.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TreeListPatient_FocusedNodeChanged);
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//鼠标为忙碌状态
                LoadScreen.ShowWaitForm();
                LoadScreen.SetWaitFormCaption("加载中");

                #region 查询
                DateTime Kai = Convert.ToDateTime(Kaibar.EditValue).Date;
                DateTime End = Convert.ToDateTime(Endbar.EditValue).Date;

                //重写绑定,初始化，并且清空
                EnableOrNot(false);
                currentReg = null;
                _MedicalRecords.CheckregCode = null;
                _MedicalRecords.ClearInput();

                Dictionary<int, IRegCodeEX> regList = new Dictionary<int, IRegCodeEX>();

                string seachKey = "";
                if (barEditItem2.EditValue != null)
                {
                    seachKey = barEditItem2.EditValue.ToString();
                }

                //标题
                string Title = "门诊病人";
                List<TreeNodeObj> list = new List<TreeNodeObj>();
                list.Add(new TreeNodeObj() { Id = 1, ParentId = 1, Name = Title });

                int empCode = cbMy.Checked ? ContextHelper.Employee.EmployeeID : 0;

                //是否跨科
                bool KuaKe = chkKuaKe.Checked;

                //if (this.cycleType == MedicakCycleType.Waiting)
                //{
                //    this.codeList.GetWaitingRegCodeList(ContextHelper.Employee.DepartmentID, empCode, 0, seachKey, KuaKe, 0, Kai, End);
                //}
                //else
                //{
                //    this.codeList.GetWaitingRegCodeList(ContextHelper.Employee.DepartmentID, empCode, 0, seachKey, KuaKe, 1, Kai, End);
                //}
                List<IRegCode> regCodes = new List<IRegCode>();

                if (this.cycleType == MedicakCycleType.Waiting)
                {
                    #region 接入Oracle



                    DataTable Oracledt = null;
                    string message = string.Empty;

                    string sql = $"SELECT * FROM cm_regcode  WHERE eventtime BETWEEN TO_DATE('{Kai.ToString("yyyy-MM-dd HH:mm")}', 'yyyy-MM-dd HH24:MI') AND TO_DATE('{End.ToString("yyyy-MM-dd 23:59")}', 'yyyy-MM-dd HH24:MI') ";
                    int ii = 0;
                    Oracledt = OracleHelpher.SelectSql(sql, ref message);
                    try
                    {
                        if (Oracledt == null || Oracledt.Rows.Count == 0)
                        {
                            return;
                        }
                        else
                        {
                            for (int i = 0; i < Oracledt.Rows.Count; i++)
                            {
                                ii = i;
                                IRegCodeEX regCode = DALHelper.ClinicDAL.CreateRegCodeEX();
                                regCode.ID = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["ID"].ToString()) ? "0" : Oracledt.Rows[i]["ID"].ToString());
                                regCode.Code = Oracledt.Rows[i]["CODE"].ToString();
                                regCode.CardID = Oracledt.Rows[i]["CARDID"].ToString();
                                regCode.InsureCode = Oracledt.Rows[i]["INSURECODE"].ToString();
                                regCode.Patient = Oracledt.Rows[i]["PATIENT"].ToString();
                                regCode.Name = Oracledt.Rows[i]["NAME"].ToString();
                                regCode.Sex = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["SEX"].ToString()) ? "0" : Oracledt.Rows[i]["SEX"].ToString());
                                regCode.Age = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["AGE"].ToString()) ? "0" : Oracledt.Rows[i]["AGE"].ToString());
                                regCode.Birthday = Convert.ToDateTime(string.IsNullOrEmpty(Oracledt.Rows[i]["BIRTHDAY"].ToString()) ? null : Oracledt.Rows[i]["BIRTHDAY"].ToString());
                                regCode.Dept = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["DEPT"].ToString()) ? "0" : Oracledt.Rows[i]["DEPT"].ToString());
                                regCode.Doctor = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["DOCTOR"].ToString()) ? "0" : Oracledt.Rows[i]["DOCTOR"].ToString());
                                regCode.Type = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["TYPE"].ToString()) ? "0" : Oracledt.Rows[i]["TYPE"].ToString());
                                regCode.Origin = RegOrigin.Normal;
                                regCode.BespeakTime = Convert.ToDateTime(string.IsNullOrEmpty(Oracledt.Rows[i]["BESPEAKTIME"].ToString()) ? null : Oracledt.Rows[i]["BESPEAKTIME"].ToString());
                                regCode.ItemCode = Oracledt.Rows[i]["ITEMCODE"].ToString();
                                regCode.ItemName = Oracledt.Rows[i]["ITEMNAME"].ToString();
                                regCode.Cash = Convert.ToDecimal(string.IsNullOrEmpty(Oracledt.Rows[i]["CASH"].ToString()) ? "0" : Oracledt.Rows[i]["CASH"].ToString());
                                regCode.Attribute = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["ATTRIBUTEL"].ToString()) ? "0" : Oracledt.Rows[i]["ATTRIBUTEL"].ToString());
                                string OPERATOR = Oracledt.Rows[i]["OPERATOR"].ToString();
                                int IntOPERATOR = 0;
                                int.TryParse(OPERATOR, out IntOPERATOR);
                                regCode.Operator = IntOPERATOR;
                                regCode.EventTime = Convert.ToDateTime(string.IsNullOrEmpty(Oracledt.Rows[i]["EVENTTIME"].ToString()) ? null : Oracledt.Rows[i]["EVENTTIME"].ToString());
                                regCode.PatientType = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["PATIENTTYPE"].ToString()) ? "0" : Oracledt.Rows[i]["PATIENTTYPE"].ToString());
                                regCode.MedicalCategories = Oracledt.Rows[i]["MedicalCategories"].ToString();
                                regCode.Disease = Oracledt.Rows[i]["Disease"].ToString();
                                regCode.TransFlag = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["TransFlag"].ToString()) ? "0" : Oracledt.Rows[i]["TransFlag"].ToString());
                                regCode.OrgCode = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["OrgCode"].ToString()) ? "0" : Oracledt.Rows[i]["OrgCode"].ToString());
                                regCode.AGETYPE = Oracledt.Rows[i]["AGETYPE"].ToString();
                                regCode.INVOICECODE = Oracledt.Rows[i]["INVOICECODE"].ToString();
                                regCode.ActiveType = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["ACTIVETYPE"].ToString()) ? "0" : Oracledt.Rows[i]["ACTIVETYPE"].ToString());
                                regCode.reExamFlag = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["ReExamFlag"].ToString()) ? "0" : Oracledt.Rows[i]["ReExamFlag"].ToString());
                                regCode.ContractCode = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["ContractCode"].ToString()) ? "0" : Oracledt.Rows[i]["ContractCode"].ToString());
                                regCode.sourceType = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["SOURCETYPE"].ToString()) ? "0" : Oracledt.Rows[i]["SOURCETYPE"].ToString());
                                regCode.RegType = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["RegType"].ToString()) ? "0" : Oracledt.Rows[i]["RegType"].ToString());
                                regCode.ClinicDoc = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["ClinicDoc"].ToString()) ? "0" : Oracledt.Rows[i]["ClinicDoc"].ToString());
                                regCode.Remark = Oracledt.Rows[i]["Remark"].ToString();
                                regCode.Identity = Oracledt.Rows[i]["Identity"].ToString();
                                regCode.FaBingTime = Convert.ToDateTime(string.IsNullOrEmpty(Oracledt.Rows[i]["FaBingTime"].ToString()) ? null : Oracledt.Rows[i]["FaBingTime"].ToString());
                                regCode.ChuZhengTime = Convert.ToDateTime(string.IsNullOrEmpty(Oracledt.Rows[i]["ChuZhengTime"].ToString()) ? null : Oracledt.Rows[i]["ChuZhengTime"].ToString());
                                regCode.FuZhengTime = Convert.ToDateTime(string.IsNullOrEmpty(Oracledt.Rows[i]["FuZhengTime"].ToString()) ? null : Oracledt.Rows[i]["FuZhengTime"].ToString());
                                regCode.ZhiYe = Oracledt.Rows[i]["ZhiYe"].ToString();
                                regCode.Phone = Oracledt.Rows[i]["Phone"].ToString();
                                regCode.Contacts = Oracledt.Rows[i]["Contacts"].ToString();
                                regCode.ContactsPhone = Oracledt.Rows[i]["ContactsPhone"].ToString();
                                regCode.Relationship = Oracledt.Rows[i]["Relationship"].ToString();
                                regCode.BloodMin = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["BloodMin"].ToString()) ? "0" : Oracledt.Rows[i]["BloodMin"].ToString());
                                regCode.BloodMan = Convert.ToInt32(string.IsNullOrEmpty(Oracledt.Rows[i]["BloodMan"].ToString()) ? "0" : Oracledt.Rows[i]["BloodMan"].ToString());
                                regCode.familyor = Oracledt.Rows[i]["familyor"].ToString();
                                regCode.FatheRrise = Oracledt.Rows[i]["FatheRrise"].ToString();
                                regCode.MotherRrise = Oracledt.Rows[i]["MotherRrise"].ToString();
                                regCode.AgencyTelephone = Oracledt.Rows[i]["AgencyTelephone"].ToString();
                                regCode.PWeight = Oracledt.Rows[i]["PWeight"].ToString();
                                regCode.Address = Oracledt.Rows[i]["Address"].ToString();
                                regCodes.Add(regCode);
                            }
                        }
                    }
                    catch (Exception err)
                    {

                        throw;
                    }
                    #endregion

                }
                else
                {

                }



                int Subscript = 2;
                WinnerHIS.Clinic.DAL.Interface.IPatientDetail patientdetail = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreatePatientDetail();
                patientdetail.Session = this.Session;


                #region 遍历添加行
                foreach (IRegCodeEX code in regCodes)
                {

                    LoadScreen.SetWaitFormDescription(code.Name + "......");

                    patientdetail.RegCode = code.Code;
                    patientdetail.OrgCode = WinnerHIS.Common.ContextHelper.Employee.OrgCode;
                    patientdetail.Refresh();

                    string svalue = code.Name;
                    if (!cbMy.Checked)
                    {
                        svalue += "[" + DataConvertHelper.GetEmpName(code.Doctor) + "]";
                    }
                    svalue += "[" + (code.reExamFlag == 0 ? "初" : "复") + "][" + code.EventTime.ToString("yyyy-MM-dd") + "]";
                    if (FocusDept.Contains(code.Doctor))
                    {
                        svalue += "[集中]";
                    }
                    else
                    {
                        svalue += "[门诊]";
                    }
                    //if (patientdetail.ICDNAME != "")
                    //{
                    //    svalue += "[" + patientdetail.ICDNAME + "]";
                    //}
                    TreeNodeObj nodePat = new TreeNodeObj() { Id = Subscript, ParentId = 1, Name = svalue };
                    list.Add(nodePat);
                    regList.Add(Subscript, code);
                    Subscript++;
                }
                #endregion
                // 绑定TreeList
                TreeListPatient.DataSource = list;
                TreeListPatient.KeyFieldName = "Id";
                TreeListPatient.ParentFieldName = "ParentId";
                TreeListPatient.Columns[0].Caption = "病人列表";
                TreeListPatient.OptionsBehavior.Editable = false;
                TreeListPatient.SelectImageList = TrieeListImage;
                TreeListPatient.ExpandAll();
                #region 设置图标,Tag
                int listIndex = 0;
                foreach (TreeListNode root in TreeListPatient.Nodes)
                {
                    root.ImageIndex = 0;
                    root.SelectImageIndex = 0;
                    foreach (TreeListNode child in root.Nodes)
                    {
                        listIndex = Convert.ToInt32(child["Id"]);
                        IRegCodeEX Patient = regList[listIndex];
                        child.Tag = Patient;

                        child.SelectImageIndex = 3;
                        if (Patient.Sex == 1000951)
                        {
                            child.ImageIndex = 1;
                        }
                        else
                        {
                            child.ImageIndex = 2;
                        }
                    }
                }
                #endregion
                // 展开节点
                ShowHideUI("btnMedical");
                #endregion
            }
            catch (Exception exc)
            {

                // MessageShow("在查询数据过程中发生错误：" + exc.Message, HPSoft.Core.CustomMessageBoxKind.WarningOk);
            }
            finally
            {
                LoadScreen.CloseWaitForm();
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
            }
            this.TreeListPatient.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TreeListPatient_FocusedNodeChanged);
        }
        /// <summary>
        /// 提示框
        /// </summary>
        /// <param name="strString"></param>
        /// <returns></returns>
        public DialogResult Confirm(string strString)
        {
            return DevExpress.XtraEditors.XtraMessageBox.Show(strString, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
        /// <summary>
        /// 查看是否有未保存的医嘱
        /// </summary>
        /// <returns></returns>
        public int NotSaved()
        {
            int NotCount = 0;
            DataTable SumTable = _PrescribingUser.gridControlAdvice.DataSource as DataTable;
            foreach (DataRow row in SumTable.Rows)
            {
                string AdvState = row["AdvState"].ToString();
                if (AdvState == "-1")
                {
                    NotCount++;
                }
            }
            return NotCount;
        }
        /// <summary>
        /// 选择患者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeListPatient_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            _MedicalRecords.ClearInput();
            if (e.Node.ParentNode != null && !e.Node.HasChildren)
            {
                if (NotSaved() > 0)
                {
                    if (this.Confirm("有未保存的处方,是否保存?") == DialogResult.OK)
                    {
                        btnSave_Click(null, null);
                    }
                }
                try
                {
                    LoadScreen.ShowWaitForm();
                    LoadScreen.SetWaitFormCaption("加载中");
                    LoadScreen.SetWaitFormDescription("正在获取【病人信息和处方】");
                    IRegCodeEX reg = e.Node.Tag as IRegCodeEX;
                    currentReg = reg;
                    EnableOrNot(true);

                    detail = _MedicalRecords.LoadPatientList(reg, RecordDateTime);
                    bool sf = _MedicalRecords.inilRegCode();
                    _PrescribingUser.SumTableRefresh(reg, detail, enableHISCard, regValidDays, true);
                    _HistoricalPrescription.SumTableRefresh();
                    btnSave.Enabled = sf;


                    //TODO: 动态UserControl赋值，加载数据
                    foreach (var dic in UserControlList)
                    {
                        UserControl control = dic.Value;

                        IBarButton barButton = control as IBarButton;
                        switch (barButton.BarName)
                        {
                            case "EvaluateBar":
                                if (detail.Exists)
                                {
                                    barButton.InData = detail.Identity;
                                }
                                break;
                            default:
                                barButton.InData = reg;
                                break;

                        }

                        barButton.LoadData();

                    }
                    _MedicalRecords.TreeNodeAdd();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("异常：" + ex.ToString());
                }
                finally
                {
                    LoadScreen.CloseWaitForm();
                    this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                }
            }

        }

        #endregion

        #region 更改筛选条件
        /// <summary>
        /// 候诊患者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paWait_Click(object sender, EventArgs e)
        {
            this.pcComplete.Active = false;
            this.paWait.Active = true;

            this.cycleType = MedicakCycleType.Waiting;
            TreeNodeAdd();


        }

        /// <summary>
        /// 已就诊患者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pcComplete_Click(object sender, EventArgs e)
        {
            this.pcComplete.Active = true;
            this.paWait.Active = false;
            this.cycleType = MedicakCycleType.Complete;
            TreeNodeAdd();
        }

        /// <summary>
        /// 分管病人是否选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbMy_CheckedChanged(object sender, EventArgs e)
        {
            TreeNodeAdd();
        }
        /// <summary>
        /// 跨科病人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkKuaKe_CheckedChanged(object sender, EventArgs e)
        {

            TreeNodeAdd();


        }

        /// <summary>
        /// 按钮是否可用
        /// </summary>
        /// <param name="sf"></param>
        public void EnableOrNot(bool sf)
        {
            //打印
            BtPrintMedical.Enabled = sf;
            //保存
            btnSave.Enabled = sf;

            btnMedical.Enabled = sf;
            btnPrescribing.Enabled = sf;
            btnBrowse.Enabled = sf;

            //TODO: 动态barButton 是否可用
            foreach (var dic in barButtonList)
            {
                dic.Value.Enabled = sf;
            }

        }
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeNodeAdd();
        }
        #endregion


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

            LoadScreen.ShowWaitForm();
            LoadScreen.SetWaitFormCaption("正在保存数据...");
            if (advVerify != string.Empty)
            {
                DataTable SumTable = _PrescribingUser.gridControlAdvice.DataSource as DataTable;
                if (SumTable.Rows.Count > 0)
                {
                    #region 处方判断 
                    string[] Method = advVerify.Split(',');
                    string information = "";
                    foreach (string obj in Method)
                    {
                        switch (obj)
                        {
                            case "1":
                                if (PrescriptionVerification.DrugMoreFive(SumTable, ref information))
                                {
                                    LoadScreen.CloseWaitForm();
                                    //    MessageShow(information);
                                    return;
                                }
                                break;
                            case "2":
                                if (PrescriptionVerification.Prescription_1W(SumTable, ref information, adviceMoney))
                                {
                                    LoadScreen.CloseWaitForm();
                                    //MessageShow(information);
                                    return;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                    if (PrescriptionVerification.IdenticalItem(SumTable, ref information))
                    {
                        LoadScreen.CloseWaitForm();
                        //  MessageShow(information);
                        return;
                    }
                    #endregion

                    #region 刷新处方状态，判断是否追加
                    if (PrescriptionVerification.AppendAdvice(SumTable, ref information))
                    {
                        LoadScreen.CloseWaitForm();
                        //  MessageShow(information);
                        return;
                    }
                    #endregion
                }
            }


            if (!_MedicalRecords.ValidInput())
            {
                LoadScreen.CloseWaitForm();
                return;
            }
            //currentReg.Refresh();

            //if (this.currentReg.Exists)
            //{
            //    if (this.currentReg.Code.Length == 0x00)
            //    {
            //        LoadScreen.CloseWaitForm();
            //        MessageBox.Show(this, "请输入卡号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    if (this.currentReg.Attribute == 0x01)
            //    {
            //        LoadScreen.CloseWaitForm();
            //        MessageBox.Show(this, "请确认该门诊号是否已经退号!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //}
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.btnSave.Enabled = false;

            //是否开立处方
            int TransFlag = currentReg.TransFlag;
            this.SaveData();
            try
            {
                //    this.DbConnection.CreateAccessor().TransactionExecute(new WinnerSoft.Data.Access.TransactionHandler(this.InternalSave));
                _PrescribingUser.SumTableRefresh(currentReg, detail, enableHISCard, regValidDays, true);
            }
            catch (Exception ex)
            {
                LoadScreen.CloseWaitForm();
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                this.btnSave.Enabled = true;
                return;
            }

            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSave.Enabled = true;

            LoadScreen.CloseWaitForm();
            MessageBox.Show("已保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (TransFlag == 0 && _MedicalRecords.Visible)//跳转处方开立界面
            {
                ShowHideUI("btnPrescribing");
            }
        }


        protected void SaveData()
        {
            #region 回写就诊卡数据
            this.ipeopleList = WinnerMIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreatePeople();
            this.ipeopleList.Session = this.Session;
            this.ipeopleList.Cardcode = _MedicalRecords.textBoxCard.Text.Trim();
            this.ipeopleList.Refresh();
            if (!this.ipeopleList.Exists)
            {
                this.ipeopleList.Cardcode = _MedicalRecords.textBoxCard.Text.Trim();
            }
            this.ipeopleList.Patienttypecode = this.currentReg.PatientType.ToString(); //病人类型编号
            ipeopleList.orgcode = ContextHelper.Account.OrgCode.ToString();
            this.ipeopleList.Idcode = _MedicalRecords.txtIdentity.Text.Trim(); //身份证号
            this.ipeopleList.Manname = _MedicalRecords.TextName.Text; //姓名
            this.ipeopleList.Birthday = Convert.ToDateTime(_MedicalRecords.dtpBirthday.Text);
            this.ipeopleList.Sex = _MedicalRecords.cmbSex.Text;
            //this.ipeopleList.EmergencyRelation = _MedicalRecords.NameOfParent.Text.Trim(); //紧急联系亲属
            this.ipeopleList.Mobitelphone = _MedicalRecords.txtTelephone.Text.Trim();
            this.ipeopleList.Manaddress = _MedicalRecords.tbAddress.Text.Trim();  //地址
            #endregion

            #region 创建病人信息（PATIENT）
            patient = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreatePatient();
            patient.Session = this.Session;
            patient.Code = this.currentReg.Patient;
            patient.OrgCode = ContextHelper.Account.OrgCode;
            patient.Refresh();


            patient.CardID = String.IsNullOrEmpty(_MedicalRecords.textBoxCard.Text.Trim()) ? patient.CardID : _MedicalRecords.textBoxCard.Text.Trim();
            patient.Identity = String.IsNullOrEmpty(_MedicalRecords.txtIdentity.Text.Trim()) ? patient.Identity : _MedicalRecords.txtIdentity.Text.Trim();
            patient.Name = _MedicalRecords.TextName.Text;
            patient.Attribute = 0x01;
            patient.InsureCode = currentReg.InsureCode;
            patient.Birthday = Convert.ToDateTime(_MedicalRecords.dtpBirthday.EditValue);
            patient.Sex = currentReg.Sex;
            patient.Address = _MedicalRecords.tbAddress.Text;
            patient.Phone = _MedicalRecords.txtTelephone.Text;
            patient.PostalCode = string.Empty;
            patient.Organ = string.Empty;

            #endregion

            #region 保存挂号记录(CM_REGCODE)

            currentReg.IsBacked = false;

            //就诊卡号
            currentReg.CardID = _MedicalRecords.textBoxCard.Text.Trim();
            //姓名
            currentReg.Name = _MedicalRecords.TextName.Text;

            //性别
            this.currentReg.Sex = _MedicalRecords.cmbSex.SelectedIndex == 0 ? 1000951 : 1000952;
            //病人类型
            this.currentReg.PatientType = Convert.ToInt32(_MedicalRecords.comboBoxPatientType.EditValue);

            //出生日期
            currentReg.Birthday = Convert.ToDateTime(_MedicalRecords.dtpBirthday.EditValue);


            //detail = DALHelper.ClinicDAL.CreatePatientDetail();
            //detail.Session = ContextHelper.Session;
            //detail.RegCode = currentReg.Code;
            //detail.OrgCode = ContextHelper.Employee.OrgCode;
            //detail.Refresh();
            //if (!this.detail.Exists)
            //{
            //    MessageBox.Show("该门诊号不存在!");
            //    return;
            //}

            if (currentReg.TransFlag == 0)
            {
                detail.RecordTime = ContextHelper.ServerTime.GetCurrentTime();
                detail.RecordPlan = RecordDateTime;
            }
            //是否就诊
            currentReg.TransFlag = 1;
            //currentReg.Tag= _MedicalRecords.mzcontent.rbZhenDuan.Text;//给开立处方界面
            #endregion

            _MedicalRecords.SaveDetail(ref detail, currentReg, patient);

            #region CRM系统
            if (AppSettingHelper.Instance.GetAppValue("CRMFla") == "1")
            {
                string[] diagnosis = detail.MedicineIcd.Split('#');
                if (diagnosis != null && diagnosis.Length >= 2)
                {
                    string sql = $"update CRM_Data.crm.PatientInfo set DiagnosticCode='{diagnosis[0]}',DiagnosticName='{diagnosis[1]}' where CODE='{this.patient.Code}'";
                    ContextHelper.ExeSql(sql);
                }

            }
            #endregion

        }
        internal int InternalSave(WinnerSoft.Data.Access.IAccessor accessor)
        {
            int iAff = 0;

            //就诊卡
            //if (this.ipeopleList.Changed)
            //{
            //    this.ipeopleList.DataAccessor = accessor.Connection;
            //    this.ipeopleList.Save();
            //}

            ////病人信息表
            //if (this.patient.Changed)
            //{
            //    this.patient.DataAccessor = accessor.Connection;
            //    this.patient.Save();
            //}

            ////挂号信息表
            //if (this.currentReg.Changed)
            //{
            //    this.currentReg.DataAccessor = accessor.Connection;
            //    this.currentReg.Save();
            //}

            ////门诊病人信息表
            //if (this.detail.Changed)
            //{
            //    this.detail.DataAccessor = accessor.Connection;
            //    this.detail.Save();
            //}

            #region 保存医嘱信息
            advCodeList.Clear();
            mjDrugadvCodeList.Clear();
            string advCode = string.Empty;
            DateTime NowDate = ContextHelper.ServerTime.GetCurrentTime();
            DataTable SumTable = _PrescribingUser.gridControlAdvice.DataSource as DataTable;
            foreach (DataRow dr in SumTable.Rows)
            {
                object objectAdvice = dr["Object"];
                if (objectAdvice is IDrugAdvice)
                {
                    IDrugAdvice advice = objectAdvice as IDrugAdvice;
                    advice.Session = this.Session;
                    advice.Patient = currentReg.Patient;
                    advice.RegCode = currentReg.Code;
                    advice.Name = currentReg.Name;
                    advice.CardID = currentReg.CardID;
                    if (advice.AdvState == AdviceState.NotSave)//新增
                    {
                        advice.ID = advice.GetMaxID();
                        //麻精处方
                        if (advice.PrescriptionType == ((int)Common.AdviceTypeEnum.Mental1).ToString() ||
                            advice.PrescriptionType == ((int)Common.AdviceTypeEnum.Anaesthesia).ToString())
                        {
                            if (mjDrugadvCodeList.ContainsKey(advice.AdviceCode))
                            {
                                advice.AdviceCode = mjDrugadvCodeList[advice.AdviceCode];
                            }
                            else
                            {
                                advCode = GetMaxMJAdviceCode();//麻精处方专属编码
                                mjDrugadvCodeList.Add(advice.AdviceCode, advCode);
                                advice.AdviceCode = advCode;
                            }
                        }
                        else
                        {
                            if (advCodeList.ContainsKey(advice.AdviceCode))
                            {
                                advice.AdviceCode = advCodeList[advice.AdviceCode];
                            }
                            else
                            {
                                advCode = GetMaxAdviceCode();
                                advCodeList.Add(advice.AdviceCode, advCode);
                                advice.AdviceCode = advCode;
                            }
                        }


                        advice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                        advice.CheckTime = NowDate;
                        advice.AdvState = AdviceState.IsAuditing;
                        advice.Insert();
                    }
                }
                else if (objectAdvice is ICureAdvice)
                {
                    ICureAdvice advice = objectAdvice as ICureAdvice;
                    advice.Session = this.Session;
                    advice.Patient = currentReg.Patient;
                    advice.RegCode = currentReg.Code;
                    advice.Name = currentReg.Name;
                    advice.CardID = currentReg.CardID;

                    if (advice.AdvState == AdviceState.NotSave)//新增
                    {
                        advice.ID = advice.GetMaxID();
                        if (advCodeList.ContainsKey(advice.AdviceCode))
                        {
                            advice.AdviceCode = advCodeList[advice.AdviceCode];
                        }
                        else
                        {
                            advCode = GetMaxAdviceCode();
                            advCodeList.Add(advice.AdviceCode, advCode);
                            advice.AdviceCode = advCode;
                        }
                        if (advice.Cash < 0 && cmCouponCheckFlag)//门诊优惠申请启用则插入优惠申请表
                        {
                            ICouponCheckApply couponApply = WinnerHIS.Analyse.Decision.DAL.Interface.DALHelper.DALManager.CreateCouponCheckApply();
                            couponApply.Session = ContextHelper.Session;
                            couponApply.CardID = currentReg.CardID;
                            couponApply.PatientNo = currentReg.Code;
                            couponApply.ItemCode = advice.ItemCode;
                            couponApply.ItemName = advice.ItemName;
                            couponApply.PName = currentReg.Name;
                            couponApply.AdviceCode = advice.AdviceCode;
                            couponApply.Unit = advice.Unit;
                            couponApply.Price = advice.Price;
                            couponApply.Number = advice.Number;
                            couponApply.Cash = advice.Cash;
                            couponApply.Status = 0;//未审核
                            couponApply.InType = 0;//门诊
                            couponApply.CreateTime = ContextHelper.CurrentTime;
                            couponApply.Creator = ContextHelper.Employee.EmployeeID;
                            couponApply.Insert();
                        }
                        advice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                        advice.CheckTime = NowDate;
                        advice.AdvState = AdviceState.IsAuditing;
                        advice.Insert();
                    }
                }
                else if (objectAdvice is ICheckAdvice)
                {
                    ICheckAdvice advice = objectAdvice as ICheckAdvice;
                    advice.Session = this.Session;

                    advice.Patient = currentReg.Patient;
                    advice.RegCode = currentReg.Code;
                    advice.Name = currentReg.Name;
                    advice.CARDID = currentReg.CardID;

                    if (advice.AdvState == AdviceState.NotSave)//新增
                    {
                        advice.ID = advice.GetMaxID();
                        if (advCodeList.ContainsKey(advice.AdviceCode))
                        {
                            advice.AdviceCode = advCodeList[advice.AdviceCode];
                        }
                        else
                        {
                            advCode = GetMaxAdviceCode();
                            advCodeList.Add(advice.AdviceCode, advCode);
                            advice.AdviceCode = advCode;
                        }
                        advice.Checker = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                        advice.CheckTime = NowDate;
                        advice.AdvState = AdviceState.IsAuditing;
                        advice.Insert();
                    }
                }
            }
            #endregion

            iAff++;
            return iAff;
        }
        public string GetMaxAdviceCode()
        {
            IDrugAdvice advice = DALHelper.ClinicDAL.CreateDrugAdvice();
            advice.Session = ContextHelper.Session;
            return advice.GetMaxAdviceCode();
        }
        public string GetMaxMJAdviceCode()
        {
            IDrugAdvice advice = DALHelper.ClinicDAL.CreateDrugAdvice();
            advice.Session = ContextHelper.Session;
            return advice.MaxMJAdviceCode();
        }



        /// <summary>
        /// 打印门诊病历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentReg == null)
                {
                    MessageBox.Show("请选中列表的就诊病人再点击打印!");
                    return;
                }
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//鼠标为忙碌状态
                //detail = DALHelper.ClinicDAL.CreatePatientDetail();
                //detail.Session = Common.ContextHelper.Session;
                //detail.RegCode = this.currentReg.Code;
                //detail.OrgCode = ContextHelper.Employee.OrgCode;
                //detail.Refresh();
                //if (!detail.Exists)
                //{
                //    XtraMessageBox.Show(this, "病人信息不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                //    return;
                //}

                //this.ipeopleList = WinnerMIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreatePeople();
                //this.ipeopleList.Session = this.Session;
                //this.ipeopleList.Cardcode = _MedicalRecords.textBoxCard.Text.Trim();
                //this.ipeopleList.Refresh();
                //if (!this.ipeopleList.Exists)
                //{
                //    XtraMessageBox.Show(this, "IPeople表信息不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
                //    return;
                //}
                _MedicalRecords.PrintCase(ipeopleList);
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
        /// 显示患者列表字体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeListPatient_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            TreeNodeObj row = TreeListPatient.GetDataRecordByNode(e.Node) as TreeNodeObj;
            if (e.Node.Tag == null) return;
            if (e.Node.Tag is IRegCode)
            {
                IRegCode Reg = e.Node.Tag as IRegCode;
                if (Reg.reExamFlag == 1)
                {
                    //e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.Blue;
                    //e.Appearance.Font = new Font(e.Appearance.Font, e.Appearance.Font.Style | FontStyle.Italic);//斜体
                }
            }

        }

        private void Kaibar_EditValueChanged(object sender, EventArgs e)
        {
            TreeNodeAdd();
        }



        /// <summary>
        /// 科室报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemReport_Click(object sender, EventArgs e)
        {
            int reportType;
            string reportURL = string.Empty;
            if (this.pcComplete.Active)
            {
                TreeListNode patNode = TreeListPatient.FocusedNode;
                if (patNode.Tag == null) return;

                IRegCode reg = patNode.Tag as IRegCode;
                currentReg = reg;

                StringBuilder sb = new StringBuilder();
                //sb.Append("SELECT REPORTNAME FROM HIS.DBO.YiJiKeShiReport WHERE EDEPTID=" + ContextHelper.Employee.DepartmentID + " AND ORGCODE=" + ContextHelper.Employee.OrgCode);
                sb.Append("SELECT * FROM HIS.DBO.YiJiKeShiReport WHERE ORGCODE=" + ContextHelper.Employee.OrgCode);

                IConnection dbcn = WinnerSoft.Context.ContextHelper.GetContext().Container.GetComponentInstances(typeof(IConnection))[0] as IConnection;
                DataTable table2 = (System.Data.DataTable)dbcn.CreateAccessor().Query(sb.ToString(), WinnerSoft.Data.Access.ResultType.DataTable);
                foreach (DataRow dr in table2.Rows)
                {
                    reportType = Convert.ToInt32(dr["InterfaceType"]);
                    reportURL = dr["URL"].ToString();
                    if (reportURL != string.Empty)
                    {

                    }
                    break;
                }
            }
        }

        private void pcComplete_Load(object sender, EventArgs e)
        {

        }
    }
}

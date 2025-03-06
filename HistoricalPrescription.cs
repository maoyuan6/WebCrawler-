using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using WinnerHIS.Clinic.DAL.Interface;
using WinnerHIS.Integral.DataCenter.DAL.Interface;
using WinnerHIS.Advice;
using WinnerHIS.Common;
using WinnerHIS.Drug.DrugStore.DAL.Interface;
using System.Collections;
using WinnerSoft.Data.Access;
using WinnerSoft.Report.Interface;
using WinnerHIS.Drug.Dict.DAL.Interface;
using DevExpress.XtraPrinting;
using WinnerHIS.Material.Dict.DAL.Interface;

namespace WinnerHIS.Diagnosis.Clinic.DoctorWorkstation.UI
{
    public partial class HistoricalPrescription : DevExpress.XtraEditors.XtraUserControl
    {
        Workstation _NewWorkstation;

        /// <summary>
        /// 医嘱绑定总的集合
        /// </summary>
        public DataTable SumTable;

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
        /// 诊疗项目
        /// </summary>
        WinnerHIS.Integral.DataCenter.DAL.Interface.IItemInfoEx itemInfo;

        public HistoricalPrescription(Workstation NewWorkstation)
        {
            InitializeComponent();
            storeList = WinnerHIS.Drug.DrugStore.DAL.Interface.DALHelper.DALManager.CreateDrugStoreExLetList();
            storeList.Session = WinnerHIS.Common.ContextHelper.Session;

            mstoreList = WinnerHIS.Material.MaterialStore.DAL.Interface.DALHelper.DALManager.CreateMaterialStoreExLetList();
            mstoreList.Session = WinnerHIS.Common.ContextHelper.Session;


            _NewWorkstation = NewWorkstation;

            TableColumnsAdd();
            Startbar.EditValue = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            Endbar.EditValue = DateTime.Now.ToString("yyyy-MM-dd");

            itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
            itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;

        }
        #region 绑定
        /// <summary>
        /// 表格添加列
        /// </summary>
        public void TableColumnsAdd()
        {
            #region 生成列
            SumTable = new DataTable();
            SumTable.Columns.Add("ID", typeof(Int32));
            SumTable.Columns.Add("No");
            SumTable.Columns.Add("RegCode");
            SumTable.Columns.Add("AdviceCode");
            SumTable.Columns.Add("AdvState");
            SumTable.Columns.Add("Code");
            SumTable.Columns.Add("Content");
            SumTable.Columns.Add("Mode");
            SumTable.Columns.Add("Days");
            SumTable.Columns.Add("Spec");
            SumTable.Columns.Add("Unit");
            SumTable.Columns.Add("Price");
            SumTable.Columns.Add("Number");
            SumTable.Columns.Add("Cash");
            SumTable.Columns.Add("CheckTime");
            SumTable.Columns.Add("CreationTime", typeof(DateTime));
            SumTable.Columns.Add("CreationDept");
            SumTable.Columns.Add("CreationDoc");
            SumTable.Columns.Add("ExeDept");
            SumTable.Columns.Add("GroupNo");
            SumTable.Columns.Add("GroupNoSymbol");
            SumTable.Columns.Add("Remarks");
            SumTable.Columns.Add("CheckObj", typeof(bool));
            SumTable.Columns.Add("Object", typeof(object));
            SumTable.Columns.Add("AState");
            SumTable.Columns.Add("Dosage");
            #endregion
        }

        public void SumTableRefresh()
        {
            DateTime startDate = Convert.ToDateTime(_NewWorkstation.currentReg.EventTime.ToShortDateString());
            DateTime endDate = Convert.ToDateTime(WinnerHIS.Common.ContextHelper.ServerTime.GetCurrentTime().ToShortDateString());
            TimeSpan spDate = endDate.Subtract(startDate);
            bool enableFlag = true;
            if (_NewWorkstation.regValidDays!=-1)
            {
                enableFlag = (_NewWorkstation.regValidDays - spDate.Days) >= 0 ? true : false;
            }
            else
            {
                if (_NewWorkstation.currentReg.reExamFlag == 0)//初诊默认当天有效号
                {
                    enableFlag = (0 - spDate.Days) >= 0 ? true : false;
                }
            }
            this.CopyBar.Enabled = enableFlag;
            this.CopyTool.Enabled = enableFlag;
            this.CopyAllTool.Enabled = enableFlag;

            Dictionary<string, decimal> AdviceDict = new Dictionary<string, decimal>();
            SumTable.Rows.Clear();
            //#region 药品
            //WinnerHIS.Clinic.DAL.Interface.IDrugAdviceList mzadviceList = DALHelper.ClinicDAL.CreateDrugAdviceList();
            //mzadviceList.Session = CacheHelper.Session;
            //mzadviceList.GetPatientList(_NewWorkstation.currentReg.Patient);
            ////mzadviceList.GetDrugList(_NewWorkstation.CheckReg.Code);
            //foreach (IDrugAdvice drug in mzadviceList.Rows)
            //{
            //    if (drug.AdviceCode.Trim() != "")
            //    {
            //        DrugTableAdd(drug);
            //        if (AdviceDict.ContainsKey(drug.AdviceCode))
            //        {
            //            AdviceDict[drug.AdviceCode] = AdviceDict[drug.AdviceCode] + drug.Cash;
            //        }
            //        else
            //        {
            //            AdviceDict.Add(drug.AdviceCode, drug.Cash);
            //        }
            //    }
            //}
            //#endregion

            //#region 诊疗
            //ICureAdviceList mzcureList = DALHelper.ClinicDAL.CreateCureAdviceList();
            //mzcureList.Session = CacheHelper.Session;
            //mzcureList.GetPatientList(_NewWorkstation.currentReg.Patient);
            ////mzcureList.GetCureList(_NewWorkstation.CheckReg.Code);
            //foreach (ICureAdvice cure in mzcureList.Rows)
            //{
            //    CureTableAdd(cure);
            //    if (AdviceDict.ContainsKey(cure.AdviceCode))
            //    {
            //        AdviceDict[cure.AdviceCode] = AdviceDict[cure.AdviceCode] + cure.Cash;
            //    }
            //    else
            //    {
            //        AdviceDict.Add(cure.AdviceCode, cure.Cash);
            //    }
            //}
            //#endregion

            //#region 检查
            //ICheckAdviceList mzcheckList = DALHelper.ClinicDAL.CreateCheckAdviceList();
            //mzcheckList.Session = CacheHelper.Session;
            //mzcheckList.GetPatientList(_NewWorkstation.currentReg.Patient);
            ////mzcheckList.GetCheckList(_NewWorkstation.CheckReg.Code);
            //foreach (ICheckAdvice check in mzcheckList.Rows)
            //{
            //    CheckTableAdd(check);
            //    if (AdviceDict.ContainsKey(check.AdviceCode))
            //    {
            //        AdviceDict[check.AdviceCode] = AdviceDict[check.AdviceCode] + check.Cash;
            //    }
            //    else
            //    {
            //        AdviceDict.Add(check.AdviceCode, check.Cash);
            //    }
            //}
            //#endregion


            Binding(AdviceDict);


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
        public void Binding(Dictionary<string, decimal> AdviceDict)
        {
            foreach (DataRow dr in SumTable.Rows)
            {
                dr["ADVICECODE"] = dr["ADVICECODE"] + "（" + AdviceDict[dr["ADVICECODE"].ToString()].ToString("F2") + ")";
            }

            SumTable.DefaultView.Sort = "CreationTime asc";//倒序
            gridViewAdvice.Columns["CheckTime"].GroupIndex = 0;
            gridViewAdvice.Columns["AdviceCode"].GroupIndex = 1;
            gridViewAdvice.OptionsBehavior.AutoExpandAllGroups = true;
            gridControlAdvice.DataSource = SumTable;
        }


        /// <summary>
        /// 药品
        /// </summary>
        /// <param name="Advice"></param>
        public void DrugTableAdd(IDrugAdvice Advice)
        {
            if (Advice.CreateTime >= Convert.ToDateTime(Startbar.EditValue.ToString()) && Advice.CreateTime < Convert.ToDateTime(Endbar.EditValue.ToString()).AddDays(1))
            {
                DataRow newRow = SumTable.NewRow();
                newRow["ID"] = Advice.ID;
                newRow["AdviceCode"] = Advice.AdviceCode.Trim();
                newRow["RegCode"] = Advice.RegCode;
                newRow["AdvState"] = (int)Advice.AdvState;
                newRow["Code"] = Advice.DrugCode;
                newRow["Content"] = Advice.DrugName;
                if (Advice.Method == 0 && Advice.Frequence == 0)
                {
                    newRow["Mode"] = Advice.Memo;
                    newRow["Days"] = Advice.Days + "/" + Advice.Pack;
                    newRow["Number"] = int.Parse(Advice.Dosage) * Advice.Pack;
                    newRow["Unit"] = Advice.SmallUnit;
                }
                else
                {
                    newRow["Mode"] = ReturnMethod(Advice.Method) + " " + ReturnFrequency(Advice.Frequence);
                    newRow["Days"] = Advice.Days;

                    if (Advice.Number % Advice.PackRule == 0)
                    {
                        newRow["Number"] = Advice.Number / Advice.PackRule;
                        newRow["Unit"] = Advice.BigUnit;
                    }
                    else
                    {
                        newRow["Number"] = Advice.Number;
                        newRow["Unit"] = Advice.SmallUnit;
                    }
                }
                newRow["Dosage"] = Advice.Dosage + Advice.DosageUnit;
                newRow["Spec"] = Advice.Spec;
                newRow["Price"] = Advice.Price;
                newRow["Cash"] = Advice.Cash;
                newRow["CheckTime"] = Advice.CheckTime.ToString("yyyy-MM-dd");
                newRow["CreationTime"] = Advice.CreateTime;
                newRow["CreationDept"] = DataConvertHelper.GetDeptName(Advice.DeptID);
                newRow["CreationDoc"] = DataConvertHelper.GetEmpName(Advice.Creator);
                newRow["ExeDept"] = DataConvertHelper.GetDeptName(Advice.ExecDept);
                newRow["GroupNo"] = Advice.GroupNo == 0 ? "" : Advice.GroupNo.ToString();
                newRow["GroupNoSymbol"] = Advice.GroupNoSymbol;
                newRow["Remarks"] = Advice.Remarks;
                newRow["Object"] = Advice;
                newRow["CheckObj"] = false;
                newRow["AState"] = (int)Advice.AdvState;
                SumTable.Rows.Add(newRow);
            }
            
        }
        /// <summary>
        /// 诊疗
        /// </summary>
        /// <param name="Advice"></param>
        public void CureTableAdd(ICureAdvice Advice)
        {
            if (Advice.CreateTime >= Convert.ToDateTime(Startbar.EditValue.ToString()) && Advice.CreateTime < Convert.ToDateTime(Endbar.EditValue.ToString()).AddDays(1))
            {
                DataRow newRow = SumTable.NewRow();
                newRow["ID"] = Advice.ID;
                newRow["AdviceCode"] = Advice.AdviceCode.Trim();
                newRow["RegCode"] = Advice.RegCode;
                newRow["AdvState"] = (int)Advice.AdvState;
                newRow["Code"] = Advice.ItemCode;
                newRow["Content"] = Advice.ItemName;
                newRow["Mode"] = ReturnFrequency(Advice.Frequency);
                newRow["Days"] = Advice.Days;
                newRow["Spec"] = "--";
                newRow["Unit"] = Advice.Unit;
                newRow["Price"] = Advice.Price;
                newRow["Number"] = Advice.Number;
                newRow["Cash"] = Advice.Cash;
                newRow["Number"] = Advice.Number;
                newRow["CheckTime"] = Advice.CheckTime.ToString("yyyy-MM-dd");
                newRow["CreationTime"] = Advice.CreateTime;
                newRow["CreationDept"] = DataConvertHelper.GetDeptName(Advice.DeptID);
                newRow["CreationDoc"] = DataConvertHelper.GetEmpName(Advice.Creator);
                newRow["ExeDept"] = DataConvertHelper.GetDeptName(Advice.ExecDept);
                newRow["GroupNo"] = Advice.GroupNo == 0 ? "" : Advice.GroupNo.ToString();
                newRow["GroupNoSymbol"] = Advice.GroupNoSymbol;
                newRow["Remarks"] = Advice.Remarks;
                newRow["Object"] = Advice;
                newRow["CheckObj"] = false;
                newRow["AState"] = (int)Advice.AdvState;
                SumTable.Rows.Add(newRow);
            }
           

        }
        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="Advice"></param>
        public void CheckTableAdd(ICheckAdvice Advice)
        {
            if (Advice.CreateTime >= Convert.ToDateTime(Startbar.EditValue.ToString()) && Advice.CreateTime < Convert.ToDateTime(Endbar.EditValue.ToString()).AddDays(1))
            {
                DataRow newRow = SumTable.NewRow();
                newRow["ID"] = Advice.ID;
                newRow["AdviceCode"] = Advice.AdviceCode.Trim();
                newRow["RegCode"] = Advice.RegCode;
                newRow["AdvState"] = (int)Advice.AdvState;
                newRow["Code"] = Advice.ItemCode;
                newRow["Content"] = Advice.ItemName;
                newRow["Mode"] = "--";
                newRow["Spec"] = "--";
                newRow["Days"] = "--";
                newRow["Unit"] = Advice.Unit;
                newRow["Price"] = Advice.Price;
                newRow["Number"] = Advice.Number;
                newRow["Cash"] = Advice.Cash;
                newRow["Number"] = Advice.Number;
                newRow["CheckTime"] = Advice.CheckTime.ToString("yyyy-MM-dd");
                newRow["CreationTime"] = Advice.CreateTime;
                newRow["CreationDept"] = DataConvertHelper.GetDeptName(Advice.DeptID);
                newRow["CreationDoc"] = DataConvertHelper.GetEmpName(Advice.Creator);
                newRow["ExeDept"] = DataConvertHelper.GetDeptName(Advice.ExecDept);
                newRow["GroupNo"] = Advice.GroupNo == 0 ? "" : Advice.GroupNo.ToString();
                newRow["GroupNoSymbol"] = Advice.GroupNoSymbol;
                newRow["Remarks"] = Advice.Remarks;
                newRow["Object"] = Advice;
                newRow["CheckObj"] = false;
                newRow["AState"] = (int)Advice.AdvState;
                SumTable.Rows.Add(newRow);
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
            IDrugFrequency freq = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateDrugFrequency();
            freq.Session = WinnerHIS.Common.ContextHelper.Session;
            freq.Code = FrequencyCode;
            freq.Refresh();
            if (freq != null)
            {
                return freq.Name;
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
        public string ReturnMethod(int Method)
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
                return methodTemp.Name;
            }
            else
            {
                return "";
            }
        }

       
        #endregion
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selectbar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SumTable.Rows.Clear();
            gridControlRecord.DataSource = null;
            SumTableRefresh();
           
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Printingbar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
            DevExpress.XtraPrinting.PrintableComponentLink link = new DevExpress.XtraPrinting.PrintableComponentLink(ps);
            ps.Links.Add(link);

            //这里可以是可打印的部件
            link.Component = gridControlAdvice;
            //标题
            string _PrintHeader = $"门诊处方({_NewWorkstation.currentReg.Name})";
            link.PaperKind = System.Drawing.Printing.PaperKind.A4;
            // 设置纸张方向   true:横向   false:竖向
            link.Landscape = true;
            PageHeaderFooter phf = link.PageHeaderFooter as PageHeaderFooter; phf.Header.Content.Clear();
            phf.Header.Content.AddRange(new string[] { "", _PrintHeader, "" });
            phf.Header.Font = new System.Drawing.Font("宋体", 14, System.Drawing.FontStyle.Bold); 
            //phf.Header.LineAlignment = BrickAlignment.Center;
            link.CreateDocument(); //建立文档
            ps.PreviewFormEx.Show();//预览

            Cursor.Current = Cursors.Arrow;
        }

        private void gridViewAdvice_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow dr = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            if (dr != null)
            {
                if (dr["Object"] is ICureAdvice)
                {
                    ICureAdviceSendList SendList = WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager.CreateCureAdviceSendList();
                    SendList.Session = WinnerHIS.Common.ContextHelper.Session;
                    SendList.GetSendList(Convert.ToInt32(dr["ID"].ToString()));
                    gridControlRecord.DataSource = SendList.DataTable;
                }
                else
                {
                    gridControlRecord.DataSource = null;
                }
              
            }
        }

        private void lv_sendrecord_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "ExecDept")
            {
                e.DisplayText = DataConvertHelper.GetDeptName(int.Parse(e.Value.ToString()));
            }
            if (e.Column.FieldName == "ExecUter")
            {
                e.DisplayText = DataConvertHelper.GetEmpName(int.Parse(e.Value.ToString()));
            }
            if (e.Column.FieldName == "AState")
            {
                switch (e.DisplayText)
                {
                    case "0":
                        e.DisplayText = "已执行";
                        break;
                    case "1":
                        e.DisplayText = "已作废";
                        break;
                }
            }

        }

        private void repositoryItemCheckEdit6_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit chEdit = sender as DevExpress.XtraEditors.CheckEdit;
            DataRow dr = gridViewAdvice.GetFocusedDataRow();
            dr["CheckObj"] = chEdit.Checked;
        }
        
        /// <summary>
        /// 处方复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyBar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable tb = this.gridControlAdvice.DataSource as DataTable;
            int ExeCount = 0;
            DateTime Now = ContextHelper.ServerTime.GetCurrentTime();
            string msg = string.Empty;
            bool storeFlag = false;

            itemInfo = WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager.CreateItemInfoEx();
            itemInfo.Session = WinnerHIS.Common.ContextHelper.Session;
            foreach (DataRow dr in tb.Rows)
            {
                if (!Convert.ToBoolean(dr["CheckObj"]))
                {
                    continue;
 
                }
                object objectAdvice = dr["Object"];
                if (objectAdvice is IDrugAdvice)
                {
                    IDrugAdvice drug = objectAdvice as IDrugAdvice;

                    #region 库存判断
                    if (drug.Type != (int)DrugTypeEnum.Material)//如果非耗材
                    {
                        storeFlag = storeList.GetDrugNumFlagByCode(drug.ExecDept, drug.DrugCode, drug.Price, drug.Number * drug.Pack, ref msg);

                        if (!storeFlag)
                        {
                            MessageBox.Show(this, "药品编号:[" + drug.DrugCode + "]" + drug.DrugName + "(¥" + drug.Price.ToString() + ")库存不足," + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            continue;
                        }
                        IDrugDict drugDict = WinnerHIS.Common.DataConvertHelper.GetDrugDict(drug.DrugCode);
                        if (drugDict.Status == 1)
                        {
                            XtraMessageBox.Show("该药品[" + drugDict.DrugName + "]已经被禁用，请选择其他药品！");
                            continue;
                        }
                    }
                    else //如果耗材
                    {
                        storeFlag = mstoreList.GetDrugNumFlagByCode(drug.ExecDept, drug.DrugCode, drug.Price, drug.Number * drug.Pack, ref msg);

                        if (!storeFlag)
                        {
                            MessageBox.Show(this, "耗材编号:[" + drug.DrugCode + "]" + drug.DrugName + "(¥" + drug.Price.ToString() + ")库存不足," + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            continue;
                        }
                        IMaterialDict dict = DataConvertHelper.GetMaterialDict(drug.DrugCode);//判断耗材字典是否禁用
                        if (dict.Status == 1)
                        {
                            XtraMessageBox.Show("该耗材[" + dict.MaterialName + "]已经被禁用，请选择其他药品！");
                            continue;
                        }
                    }
                    
                    #endregion
                    #region 1
                    //ID
                    drug.ID = drug.GetMaxID();
                    //状态
                    drug.AdvState = AdviceState.NotSave;
                    //创建医生
                    drug.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                    //部门
                    drug.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                    //开立时间
                    drug.CheckTime = Convert.ToDateTime("1900-01-01");
                    drug.Checker = 0;
                    //创建时间
                    drug.CreateTime = Now;
                    drug.AdviceCode = "";
                    _NewWorkstation._PrescribingUser.DrugTableAdd(drug);
                    Now=Now.AddSeconds(1);
                    #endregion
                }
                else if (objectAdvice is ICureAdvice)
                {
                    #region 2
                    ICureAdvice cure = objectAdvice as ICureAdvice;

                    itemInfo.Code = cure.ItemCode;
                    itemInfo.Refresh();

                    if (itemInfo.Attribute == 2)
                    {
                        XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
                        continue ;
                    }
                    //ID
                    cure.ID = cure.GetMaxID();
                    //状态
                    cure.AdvState = AdviceState.NotSave;
                    //创建医生
                    cure.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                    //部门
                    cure.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                    //开立时间
                    cure.CheckTime = Convert.ToDateTime("1900-01-01");
                    cure.Checker = 0;
                    //创建时间
                    cure.CreateTime = ContextHelper.ServerTime.GetCurrentTime();
                    cure.AdviceCode = "";
                    _NewWorkstation._PrescribingUser.CureTableAdd(cure);
                    Now = Now.AddSeconds(1);
                    #endregion

                }
                else if (objectAdvice is ICheckAdvice)
                {
                    #region 3
                    ICheckAdvice check = objectAdvice as ICheckAdvice;

                    itemInfo.Code = check.ItemCode;
                    itemInfo.Refresh();

                    if (itemInfo.Attribute == 2)
                    {
                        XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
                        continue;
                    }

                    //ID
                    check.ID = check.GetMaxID();
                    //状态
                    check.AdvState = AdviceState.NotSave;
                    //创建医生
                    check.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                    //部门
                    check.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                    //开立时间
                    check.CheckTime = Convert.ToDateTime("1900-01-01");
                    check.Checker = 0;
                    //创建时间
                    check.CreateTime = ContextHelper.ServerTime.GetCurrentTime();
                    check.AdviceCode = "";
                    _NewWorkstation._PrescribingUser.CheckTableAdd(check);
                    Now = Now.AddSeconds(1);
                    #endregion

                }
                ExeCount++;
            }

            if (ExeCount == 0)
            {
                XtraMessageBox.Show("请勾选！");
            }
            else
            {
                XtraMessageBox.Show("复制成功！");
                AllCheck(false);
                _NewWorkstation._PrescribingUser.Binding();
                _NewWorkstation.ShowHideUI("btnPrescribing");
            }

        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit chEdit = sender as DevExpress.XtraEditors.CheckEdit;
            AllCheck(chEdit.Checked);
        }

        /// <summary>
        /// 删除处方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteTool_Click(object sender, EventArgs e)
        {
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            object objectAdvice = row["Object"];
            if (objectAdvice is IDrugAdvice)
            {
                IDrugAdvice drug = objectAdvice as IDrugAdvice;
                drug.Refresh();
                if ((int)drug.AdvState!=1)
                {
                    XtraMessageBox.Show("只能删除已审核的医嘱！");
                    Selectbar_ItemClick(null,null);
                    return;
                }
                DrugAdviceBack(drug.ID, "Drug");
                drug.Delete();
                
            }
            else if (objectAdvice is ICureAdvice)
            {
                ICureAdvice cure = objectAdvice as ICureAdvice;
                cure.Refresh();
                if ((int)cure.AdvState != 1)
                {
                    XtraMessageBox.Show("只能删除已审核的医嘱！");
                    Selectbar_ItemClick(null, null);
                    return;
                }
                DrugAdviceBack(cure.ID, "Cure");
                cure.Delete();
            }
            else if (objectAdvice is ICheckAdvice)
            {
                ICheckAdvice check = objectAdvice as ICheckAdvice;
                check.Refresh();
                if ((int)check.AdvState != 1)
                {
                    XtraMessageBox.Show("只能删除已审核的医嘱！");
                    Selectbar_ItemClick(null, null);
                    return;
                }
                DrugAdviceBack(check.ID, "Check");
                check.Delete();
            }
            XtraMessageBox.Show("操作成功！");
            SumTableRefresh();
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
            string Sql = "insert his.his.CM_Advice_Delete values(" + AdviceID + ",'" + SType + "',GETDATE(),'" + WinnerHIS.Common.ContextHelper.Account.LoginID + "','历史处方')";
            IConnection dbcn = WinnerHIS.Common.ContextHelper.Context.Container.GetComponentInstances(typeof(IConnection))[0] as IConnection;
            dbcn.CreateAccessor().Execute(Sql.ToString());
        }


        #endregion

        /// <summary>
        /// 复制处方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyTool_Click(object sender, EventArgs e)
        {
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            DateTime Now = ContextHelper.ServerTime.GetCurrentTime();
            object objectAdvice = row["Object"];
            string msg = string.Empty;
            bool storeFlag = false;
            if (objectAdvice is IDrugAdvice)
            {
                IDrugAdvice drug = objectAdvice as IDrugAdvice;
                #region 库存判断
                storeFlag = storeList.GetDrugNumFlagByCode(drug.ExecDept, drug.DrugCode, drug.Price, drug.Number * drug.Pack, ref msg);

                if (!storeFlag)
                {
                    MessageBox.Show(this, "药品编号:[" + drug.DrugCode + "]" + drug.DrugName + "(¥" + drug.Price.ToString() + ")库存不足," + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                IDrugDict drugDict = WinnerHIS.Common.DataConvertHelper.GetDrugDict(drug.DrugCode);
                if (drugDict.Status == 1)
                {
                    XtraMessageBox.Show("该药品[" + drugDict.DrugName + "]已经被禁用，请选择其他药品！");
                    return;
                }
                #endregion
                #region 1
                //ID
                drug.ID = drug.GetMaxID();
                //状态
                drug.AdvState = AdviceState.NotSave;
                //创建医生
                drug.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                //部门
                drug.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                //开立时间
                drug.CheckTime = Convert.ToDateTime("1900-01-01");
                drug.Checker = 0;
                //创建时间
                drug.CreateTime = Now;
                drug.AdviceCode = "";
                drug.BillCode = null;
                _NewWorkstation._PrescribingUser.DrugTableAdd(drug);
                #endregion

            }
            else if (objectAdvice is ICureAdvice)
            {
                #region 2
                ICureAdvice cure = objectAdvice as ICureAdvice;

                itemInfo.Code = cure.ItemCode;
                itemInfo.Refresh();

                if (itemInfo.Attribute == 2)
                {
                    XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
                    return;
                }

                //ID
                cure.ID = cure.GetMaxID();
                //状态
                cure.AdvState = AdviceState.NotSave;
                //创建医生
                cure.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                //部门
                cure.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                //开立时间
                cure.CheckTime = Convert.ToDateTime("1900-01-01");
                cure.Checker = 0;
                //创建时间
                cure.CreateTime = ContextHelper.ServerTime.GetCurrentTime();
                cure.AdviceCode = "";
                _NewWorkstation._PrescribingUser.CureTableAdd(cure);
                #endregion
            }
            else if (objectAdvice is ICheckAdvice)
            {
                #region 3
                ICheckAdvice check = objectAdvice as ICheckAdvice;
                itemInfo.Code = check.ItemCode;
                itemInfo.Refresh();

                if (itemInfo.Attribute == 2)
                {
                    XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
                    return;
                }
                //ID
                check.ID = check.GetMaxID();
                //状态
                check.AdvState = AdviceState.NotSave;
                //创建医生
                check.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                //部门
                check.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                //开立时间
                check.CheckTime = Convert.ToDateTime("1900-01-01");
                check.Checker = 0;
                //创建时间
                check.CreateTime = ContextHelper.ServerTime.GetCurrentTime();
                check.AdviceCode = "";
                _NewWorkstation._PrescribingUser.CheckTableAdd(check);
                #endregion
            }
            XtraMessageBox.Show("复制成功！");
            _NewWorkstation._PrescribingUser.Binding();
            _NewWorkstation.ShowHideUI("btnPrescribing");
        }
        private void CopyAllTool_Click(object sender, EventArgs e)
        {
            string AdviceCode = "";
            #region 获取处方号
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            DateTime Now = ContextHelper.ServerTime.GetCurrentTime();
            object objectAdvice = row["Object"];
            if (objectAdvice is IDrugAdvice)
            {
                AdviceCode = (objectAdvice as IDrugAdvice).AdviceCode;
            }
            else if (objectAdvice is ICureAdvice)
            {
                AdviceCode = (objectAdvice as ICureAdvice).AdviceCode;
            }
            else if (objectAdvice is ICheckAdvice)
            {
                AdviceCode = (objectAdvice as ICheckAdvice).AdviceCode;
            }
            #endregion
            DataTable tb = this.gridControlAdvice.DataSource as DataTable;
            foreach (DataRow dr in tb.Rows)
            {
                #region 复制处方
                object Advice = dr["Object"];
                if (Advice is IDrugAdvice)
                {
                    IDrugAdvice drug = Advice as IDrugAdvice;
                     string msg = string.Empty;
                     bool storeFlag = false;
                    if (drug.AdviceCode != AdviceCode)
                    {
                        continue;
                    }
                    #region 库存判断
                    storeFlag = storeList.GetDrugNumFlagByCode(drug.ExecDept, drug.DrugCode, drug.Price, drug.Number * drug.Pack, ref msg);

                    if (!storeFlag)
                    {
                        MessageBox.Show(this, "药品编号:[" + drug.DrugCode + "]" + drug.DrugName + "(¥" + drug.Price.ToString() + ")库存不足," + msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    IDrugDict drugDict = WinnerHIS.Common.DataConvertHelper.GetDrugDict(drug.DrugCode);
                    if (drugDict.Status == 1)
                    {
                        XtraMessageBox.Show("该药品[" + drugDict.DrugName + "]已经被禁用，请选择其他药品！");
                        continue;
                    }
                    #endregion
                    #region 1
                    //ID
                    drug.ID = drug.GetMaxID();
                    //状态
                    drug.AdvState = AdviceState.NotSave;
                    //创建医生
                    drug.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                    //部门
                    drug.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                    //开立时间
                    drug.CheckTime = Convert.ToDateTime("1900-01-01");
                    drug.Checker = 0;
                    //创建时间
                    drug.CreateTime = Now;
                    drug.AdviceCode = "";
                    drug.BillCode = null;
                    _NewWorkstation._PrescribingUser.DrugTableAdd(drug);
                    Now = Now.AddSeconds(1);
                    #endregion
                }
                else if (Advice is ICureAdvice)
                {
                    ICureAdvice cure = Advice as ICureAdvice;
                    if (cure.AdviceCode != AdviceCode)
                    {
                        continue;
                    }
                    itemInfo.Code = cure.ItemCode;
                    itemInfo.Refresh();

                    if (itemInfo.Attribute == 2)
                    {
                        XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
                        continue;
                    }
                 
                    #region 2

                    //ID
                    cure.ID = cure.GetMaxID();
                    //状态
                    cure.AdvState = AdviceState.NotSave;
                    //创建医生
                    cure.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                    //部门
                    cure.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                    //开立时间
                    cure.CheckTime = Convert.ToDateTime("1900-01-01");
                    cure.Checker = 0;
                    //创建时间
                    cure.CreateTime = ContextHelper.ServerTime.GetCurrentTime();
                    cure.AdviceCode = "";
                    _NewWorkstation._PrescribingUser.CureTableAdd(cure);
                    Now = Now.AddSeconds(1);
                    #endregion
                }
                else if (Advice is ICheckAdvice)
                {
                    ICheckAdvice check = Advice as ICheckAdvice;
                    if (check.AdviceCode != AdviceCode)
                    {
                        continue;
                    }
                    itemInfo.Code = check.ItemCode;
                    itemInfo.Refresh();

                    if (itemInfo.Attribute == 2)
                    {
                        XtraMessageBox.Show("该项目[" + itemInfo.Name + "]已经被禁用，请选择其他项目！");
                        continue;
                    }
                    #region 3
                    //ID
                    check.ID = check.GetMaxID();
                    //状态
                    check.AdvState = AdviceState.NotSave;
                    //创建医生
                    check.Creator = int.Parse(WinnerHIS.Common.ContextHelper.Account.LoginID);
                    //部门
                    check.DeptID = WinnerHIS.Common.ContextHelper.Account.DeptID;
                    //开立时间
                    check.CheckTime = Convert.ToDateTime("1900-01-01");
                    check.Checker = 0;
                    //创建时间
                    check.CreateTime = ContextHelper.ServerTime.GetCurrentTime();
                    check.AdviceCode = "";
                    _NewWorkstation._PrescribingUser.CheckTableAdd(check);
                    Now = Now.AddSeconds(1);
                    #endregion
                }
                #endregion
            }
            XtraMessageBox.Show("复制成功！");
            _NewWorkstation._PrescribingUser.Binding();
            _NewWorkstation.ShowHideUI("btnPrescribing");

        }
        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            if (row != null)
            {
                contextMenuStrip1.Enabled = true;
            }
            else
            {
                contextMenuStrip1.Enabled = false;
            }
        }

        private void gridViewAdvice_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "AState")
            {
                if (e.Value.ToString() == "1")//已审核
                {
                    e.DisplayText = "已审核";
                }
                else if (e.Value.ToString() == "2")//已收费
                {
                    e.DisplayText = "已收费";
                }
                else if (e.Value.ToString() == "3")//已执行
                {
                    e.DisplayText = "已执行";
                }
                else if (e.Value.ToString() == "4")//已退费
                {
                    e.DisplayText = "已退费";
                }
                else if (e.Value.ToString() == "8")//放弃检查
                {
                    e.DisplayText = "放弃检查";
                }
                else if (e.Value.ToString() == "5")//放弃检查
                {
                    e.DisplayText = "作废";
                }
            }
        }
        /// <summary>
        /// 退费申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Retreattool_Click(object sender, EventArgs e)
        {
            List<string> AdviceList = new List<string>();
            DataRow row = gridViewAdvice.GetDataRow(gridViewAdvice.FocusedRowHandle);
            if (row["AState"].ToString() == "1" && row["AState"].ToString() == "-1")
            {
                XtraMessageBox.Show("请选择已经收费过的处方！");
                return;
            }
            Advice.DAL.Interface.IAdvice adv = (Advice.DAL.Interface.IAdvice)row["Object"];
            if (adv.Creator != ContextHelper.Employee.EmployeeID)
            {
                XtraMessageBox.Show("该处方的开立医生是【"+ DataConvertHelper.GetEmpName(adv.Creator) + "】,请选择自己开立处方！");
                return;
            }
            IRegCode Reg = DALHelper.ClinicDAL.CreateRegCode();
            Reg.Session = CacheHelper.Session;
            Reg.Code = row["RegCode"].ToString();
            Reg.OrgCode = ContextHelper.Employee.OrgCode;
            Reg.Refresh();

            #region 自费
            object objectAdvice = row["Object"];
            if (objectAdvice is IDrugAdvice)
            {
                AdviceList.Add((objectAdvice as IDrugAdvice).AdviceCode);
            }
            else if (objectAdvice is ICureAdvice)
            {
                AdviceList.Add((objectAdvice as ICureAdvice).AdviceCode);
            }
            else if (objectAdvice is ICheckAdvice)
            {
                AdviceList.Add((objectAdvice as ICheckAdvice).AdviceCode);
            }
            #endregion

            if (_NewWorkstation.currentReg.PatientType != 1000401)
            {
                string AdviCode = AdviceList[0];
                AdviceList.Clear();
                #region 医保的
                StringBuilder str = new StringBuilder();
                str.Append(" select CREDENCEID from HIS.his.CM_COST where ");
                str.Append(" BILLCODE in(select BILLCODE from HIS.his.CM_COST where  CREDENCEID='" + AdviCode + "')");
                str.Append("  group by CREDENCEID");

                IList list = WinnerHIS.Common.ContextHelper.ExeSqlList(str.ToString());
                foreach (string obj in list)
                {
                    AdviceList.Add(obj);
                }
                #endregion
            }
            //RefundFrom From = new RefundFrom(AdviceList, Reg);
            //if (From.ShowDialog(this.ParentForm) == DialogResult.OK)
            //{

            //}
        }

        private void gridViewAdvice_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            DataRow dr = gridViewAdvice.GetDataRow(e.RowHandle);
            if (e.Column.FieldName == "AState")
            {
                if (dr["AState"].ToString() == "1")//已审核
                {
                    e.Appearance.ForeColor = System.Drawing.Color.DarkViolet;
                }
                else if (dr["AState"].ToString() == "2")//已收费
                {
                    e.Appearance.ForeColor = System.Drawing.Color.Green;
                }
                else if (dr["AState"].ToString() == "3")//已执行
                {
                    e.Appearance.ForeColor = System.Drawing.Color.Blue;
                }
                else if (dr["AState"].ToString() == "4")//已退费
                {
                    e.Appearance.ForeColor = System.Drawing.Color.Red;
                }
                else if (dr["AState"].ToString() == "8"  )//放弃检查
                {
                    e.Appearance.ForeColor = System.Drawing.Color.Red;
                }
                else if (dr["AState"].ToString() == "5" )//放弃检查
                {
                    e.Appearance.ForeColor = System.Drawing.Color.Red;
                }

            }
        }
        /// <summary>
        ///内容复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolCopy_Click(object sender, EventArgs e)
        {
            List<object> AdviceList = new List<object>();

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
                if (obj is IDrugAdvice)
                {
                    IDrugAdvice Advice = obj as IDrugAdvice;
                    if (Advice.Frequence == 0 && Advice.Method == 0) //中药的
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
                            str += Advice.DrugName + " " + Advice.Spec + " " + Advice.Number + Advice.SmallUnit + " " + Advice.Dosage + Advice.DosageUnit + " "
                                            + ReturnMethodNo(Advice.Method) + " " + ReturnFrequencyNo(Advice.Frequence) + "\r\n";
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

        /// <summary>
        /// 明细行号显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridviewRecord_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
             if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
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

        
    }
}

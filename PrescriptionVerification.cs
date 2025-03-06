using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WinnerHIS.Advice;
using WinnerHIS.Clinic.DAL.Interface;

namespace WinnerHIS.Diagnosis.Clinic.DoctorWorkstation.UI
{
    /// <summary>
    /// 处方验证
    /// </summary>
    public static class PrescriptionVerification
    {
        /// <summary>
        /// 一个处方不能有相同的项目
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static bool IdenticalItem(DataTable dt, ref string information)
        {
            List<string> DrugList = new List<string>();
            List<string> CureList = new List<string>();
            List<string> CheckList = new List<string>();
            Dictionary<int, decimal> dicDeptCash = new Dictionary<int, decimal>();
            information = "";
            foreach (DataRow dr in dt.Rows)
            {
                object obj = dr["Object"];
                if (obj is IDrugAdvice)
                {
                      IDrugAdvice Advice = obj as IDrugAdvice;
                      if (Advice.AdviceCode == "")
                      {
                          if (DrugList.Contains(Advice.DrugCode))
                          {
                              information = "同一处方不能有相同的2个药(" + Advice.DrugName + ")！";
                              return true;
                          }
                          else
                          {
                              DrugList.Add(Advice.DrugCode);
                          }
                      }
                    if (!dicDeptCash.ContainsKey(Advice.ExecDept))
                    {
                        dicDeptCash.Add(Advice.ExecDept, Advice.Cash);
                    }
                    else
                    {
                        dicDeptCash[Advice.ExecDept] += Advice.Cash;
                    }
                }
                else if (obj is ICureAdvice)
                {
                    ICureAdvice Advice = obj as ICureAdvice;
                    if (Advice.AdviceCode == "")
                    {
                        if (CureList.Contains(Advice.ItemCode))
                        {
                            information = "同一处方不能有相同的项目(" + Advice.ItemCode + ")！";
                            return true;
                        }
                        else
                        {
                            CureList.Add(Advice.ItemCode);
                        }
                        if (!dicDeptCash.ContainsKey(Advice.ExecDept))
                        {
                            dicDeptCash.Add(Advice.ExecDept, Advice.Cash);
                        }
                        else
                        {
                            dicDeptCash[Advice.ExecDept]+= Advice.Cash;
                        }
                    }
                }
                else if (obj is ICheckAdvice)
                {
                    ICheckAdvice Advice = obj as ICheckAdvice;
                    if (Advice.AdviceCode == "")
                    {
                        if (CheckList.Contains(Advice.ItemCode))
                        {
                            information = "同一处方不能有相同的项目(" + Advice.ItemCode + ")！";
                            return true;
                        }
                        else
                        {
                            CheckList.Add(Advice.ItemCode);
                        }
                    }
                    if (!dicDeptCash.ContainsKey(Advice.ExecDept))
                    {
                        dicDeptCash.Add(Advice.ExecDept, Advice.Cash);
                    }
                    else
                    {
                        dicDeptCash[Advice.ExecDept] += Advice.Cash;
                    }
                }
            }
            bool flag = false;
            if (ClinicHelper.cmJsonList!=null)
            {
                if (ClinicHelper.cmJsonList.Property("IsVerifyCMDeptCash")!=null)
                {
                    flag = ClinicHelper.cmJsonList["IsVerifyCMDeptCash"]["ItemSwitch"].ToString() == "true";
                }
            }
            if (flag)
            {
                foreach (KeyValuePair<int, decimal> item in dicDeptCash)
                {
                    if (item.Key == 0)
                        continue;
                    if (item.Value < 0)
                    {
                        information = $"该科室[{WinnerHIS.Common.DataConvertHelper.GetDeptName(item.Key)}]下总费用低于0！检查下优惠开立情况";
                        return true;
                    }
                }
            }
            return false;
             
        }

        /// <summary>
        /// 追加医嘱是否放到已收费
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static bool AppendAdvice(DataTable dt, ref string information)
        {
            Dictionary<string, string> advList = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                object advObj = dr["Object"];
                if (advObj is Advice.DAL.Interface.IAdvice)
                {
                    Advice.DAL.Interface.IAdvice adv = advObj as Advice.DAL.Interface.IAdvice;
                    if (adv.AdviceCode != "" && adv.AdvState == AdviceState.NotSave)//可能追加也可能保存过
                    {
                        advList.Add(adv.AdviceCode, adv.AdviceCode);
                        break;
                    }
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                object advObj = dr["Object"];
                if (advObj is Advice.DAL.Interface.IAdvice)
                {
                    Advice.DAL.Interface.IAdvice adv = advObj as Advice.DAL.Interface.IAdvice;
                    foreach (var advVar in advList)
                    {
                        if (advVar.Key == adv.AdviceCode&&adv.AdvState!=AdviceState.NotSave)
                        {
                            adv.Refresh();
                            if (adv.AdvState == AdviceState.IsCharge)
                            {
                                information = "不能追加在已经收费的处方组里，请新开处方！";
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 处方不能超过五种药/并且西药和中药只能开在一个处方   方法1
        /// </summary>
        /// <returns></returns>
        public static bool DrugMoreFive(DataTable dt, ref string information)
        {
            bool Chinese = false;
            bool Western = false;
            bool curePreference = false;
            Dictionary<string, int> Drug = new Dictionary<string, int>();
            int NoSave = 0;
            information = "";
            foreach (DataRow dr in dt.Rows)
            {
                object obj = dr["Object"];
                if (obj is IDrugAdvice)
                {
                    IDrugAdvice Advice = obj as IDrugAdvice;

                    if (Advice.AdviceCode == "")
                    {
                        #region  1
                        if (Advice.Frequence != 0 && Advice.Method != 0)
                        {
                            NoSave++;
                            if (NoSave > 5)
                            {
                                information = "处方不能超过五种药！";
                                return true;
                            }
                        }
                        #endregion
                        if (Advice.Type == 1000003)
                        {
                            Chinese = true;
                        }
                        else
                        {
                            Western = true;
                        }

                    }
                    else// 追加处方  判断
                    {
                        #region 判断
                        if (Advice.Frequence != 0 && Advice.Method != 0)
                        {
                            if (Drug.ContainsKey(Advice.AdviceCode))
                            {
                                Drug[Advice.AdviceCode] = Drug[Advice.AdviceCode] + 1;
                            }
                            else
                            {
                                Drug.Add(Advice.AdviceCode, 1);
                            }
                        }
                        #endregion
                    }
                    if (Advice.AdvState == AdviceState.NotSave)
                    {
                        bool enableMaterialStore = int.Parse(WinnerHIS.Common.AppSettingHelper.GetAppSettingsString("EnableMaterialStore")) == 1;
                        #region 库存+预售数量判断
                        if (enableMaterialStore&&Advice.Type== 1000008)
                        {
                            WinnerHIS.Material.MaterialStore.DAL.Interface.IMaterialStoreExLetList mstoreList = WinnerHIS.Material.MaterialStore.DAL.Interface.DALHelper.DALManager.CreateMaterialStoreExLetList();
                            mstoreList.Session = WinnerHIS.Common.ContextHelper.Session;

                            string StorID = Advice.ExecDept.ToString();

                            int Number = Advice.Number;
                            string msg = string.Empty;
                            bool storeFlag = mstoreList.GetDrugNumFlagByCode(Advice.ExecDept, Advice.DrugCode, Advice.Price, Number, ref msg);

                            if (!storeFlag)
                            {
                                information = msg;
                                return true;
                            }
                        }
                        else
                        {
                            WinnerHIS.Drug.DrugStore.DAL.Interface.IDrugStoreExLetList storeList = WinnerHIS.Drug.DrugStore.DAL.Interface.DALHelper.DALManager.CreateDrugStoreExLetList();
                            storeList.Session = WinnerHIS.Common.ContextHelper.Session;

                            string StorID = Advice.ExecDept.ToString();

                            int Number = Advice.Number;
                            if (Advice.Type == 1000003)//中药
                            {
                                Number = Advice.Number * Advice.Pack;
                            }
                            string msg = string.Empty;
                            bool storeFlag = storeList.GetDrugNumFlagByCode(int.Parse(StorID), Advice.DrugCode, Advice.Price, Number, ref msg);

                            if (!storeFlag)
                            {
                                information = msg;
                                return true;
                            }
                        }
                        #endregion
                    }
                }
                else if (obj is ICureAdvice)
                {
                    ICureAdvice cureAdv = obj as ICureAdvice;
                    if (cureAdv.AdvState == AdviceState.NotSave
                        && cureAdv.Cash < 0)
                    {
                        curePreference = true;
                    }
                }
            }
            foreach (string key in Drug.Keys)
            {
                if (Drug[key]>5)
                {
                    information = key + "处方不能超过五种药！";
                    return true;
                } 
            }
            if (Chinese && Western)
            {
                information = "中药和西药不能开在一张处方上！";
                return true;
            }
            if ((Chinese || Western)&&curePreference)
            {
                information = "药品处方中不能含有优惠项目！";
                return true;
            }
            return false;
        }
        /// <summary>
        /// 处方不能大于1W
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public static bool Prescription_1W(DataTable dt, ref string information,decimal money)
        {
            information = "";
            Dictionary<string, decimal> CashDic = new Dictionary<string, decimal>();
            foreach (DataRow dr in dt.Rows)
            {
                string AdviceCode = dr["AdviceCode"].ToString();
                if (AdviceCode == "")
                {
                    if (CashDic.ContainsKey("未保存"))
                    {
                        CashDic["未保存"] = CashDic["未保存"] + Convert.ToDecimal(dr["Cash"].ToString());
                    }
                    else
                    {
                        CashDic.Add("未保存", Convert.ToDecimal(dr["Cash"].ToString()));
                    }
                }
                else
                {
                    if (CashDic.ContainsKey(AdviceCode))
                    {
                        if (dr["AdvState"].ToString() == "-1")
                        {
                            CashDic[AdviceCode] = CashDic[AdviceCode] + Convert.ToDecimal(dr["Cash"].ToString());
                        }
                    }
                    else
                    {
                        if (dr["AdvState"].ToString() == "-1")
                        {
                            CashDic.Add(AdviceCode, Convert.ToDecimal(dr["Cash"].ToString()));
                        }
                        
                    }
                }

                
            }
            foreach (string key in CashDic.Keys)
            {
                if (CashDic[key] > money)
                {
                    information = key+"处方金额不能大于1W";
                    return true;
 
                }

            }
            return false;
        }


    }
}

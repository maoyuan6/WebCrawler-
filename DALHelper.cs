using System;
using System.Collections.Generic;
using System.Text;

namespace WinnerHIS.Diagnosis.Clinic.DoctorWorkstation.UI
{
    public class DALHelper
    {
        public static WinnerMIS.Integral.Template.DAL.Interface.IDALManager TemplateDALManager
        {
            get
            {
                return WinnerMIS.Integral.Template.DAL.Interface.DALHelper.DALManager;
            }
        }

        public static WinnerHIS.Drug.DrugTable.DAL.Interface.IDALManager DrugTableDAL
        {
            get
            {
                return WinnerHIS.Drug.DrugTable.DAL.Interface.DALHelper.DALManager;
            }
        }

        public static WinnerHIS.Integral.DataCenter.DAL.Interface.IDALManager DataCenterDAL
        {
            get
            {
                return WinnerHIS.Integral.DataCenter.DAL.Interface.DALHelper.DALManager;
            }
        }

        public static WinnerHIS.MTS.DAL.Interface.IDALManager MTSDAL
        {
            get
            {
                return WinnerHIS.MTS.DAL.Interface.DALHelper.DALManager;
            }
        }

        public static WinnerMIS.Integral.Template.DAL.Interface.IDALManager TemplateDAL
        {
            get
            {
                return WinnerMIS.Integral.Template.DAL.Interface.DALHelper.DALManager;
            }
        }

        public static WinnerHIS.Clinic.DAL.Interface.IDALManager ClinicDAL
        {
            get
            {
                return WinnerHIS.Clinic.DAL.Interface.DALHelper.DALManager;
            }
        }

        public static WinnerExplorer.Shell.DAL.IDALManager ExplorerDAL
        {
            get
            {
                return WinnerExplorer.Shell.DAL.DALHelper.DALManager;
            }
        }
        public static WinnerHIS.Integral.AIOCS.DAL.Interface.IDALManager AIOCSDAL
        {
            get
            {
                return WinnerHIS.Integral.AIOCS.DAL.Interface.DALHelper.DALManager;
            }
        }
    }
}

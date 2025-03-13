using Clinic.Case.Interface;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinnerHIS.Common;
using WinnerHIS.Integral.Personnel.DAL.Interface;

namespace Clinic.Case.Business
{
    public partial class TempletEdit : BaseModuleForm
    {
        bool isEdit;//是否编辑状态
        DapperHelper EMRContext = new DapperHelper("EMR");
        DapperHelper BaseDataContext = new DapperHelper("BaseData");
        //添加
        public TempletEdit(int deptid, string mrCode, int attr = 0)
        {
            InitializeComponent();
            LoadControl();
            isEdit = false;
            if (deptid != 0)
            {
                cmbDept.EditValue = deptid;
            }
            if (mrCode != "")
            {
                cmbType.EditValue = mrCode;
            }
            radioType.SelectedIndex = attr;
        }
        EmrTemplet emrtemplet;
        //修改
        public TempletEdit(EmrTemplet templet)
        {
            InitializeComponent();
            LoadControl();
            isEdit = true;
            emrtemplet = templet;
            txtCode.Text = templet.CreatorId.ToString();
            txtName.Text = templet.MrName;
            cmbType.EditValue = templet.MrClass;
            cmbDept.EditValue = templet.DeptId;
            txtTitle.Text = templet.FileName;
            checkShowTitle.Checked = templet.IsShowFileName == 1;
            radioType.SelectedIndex = templet.MrAttr ?? 0;
            foreach (CheckedListBoxItem item in checklist.Items)
            {
                switch (item.Value)
                {
                    case 0://首次病程
                        item.CheckState = templet.IsFirstDaily == 1 ? CheckState.Checked : CheckState.Unchecked;
                        break;
                    case 1://新页结束
                        item.CheckState = templet.NewPageEnd == 1 ? CheckState.Checked : CheckState.Unchecked;
                        break;
                    case 2://页面配置
                        item.CheckState = templet.IsConfigPageSize == 1 ? CheckState.Checked : CheckState.Unchecked;
                        break;
                    case 3://新页开始
                        item.CheckState = templet.NewPageFlag == 1 ? CheckState.Checked : CheckState.Unchecked;
                        break;
                    case 4://医患沟通
                        item.CheckState = templet.IsYiHuanGouTong == 1 ? CheckState.Checked : CheckState.Unchecked;
                        break;
                }
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cmbType.EditValue == null)
            {
                MessageBox.Show("请选择模板类型");
                return;
            }
            if (txtName.Text == string.Empty)
            {
                MessageBox.Show("请填写模板名称");
                return;
            }
            if (cmbDept.EditValue == null)
            {
                MessageBox.Show("请选择科室");
                return;
            }
            if (!isEdit)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            { 
                emrtemplet.MrName = txtName.Text;
                emrtemplet.MrClass = cmbType.EditValue.ToString();
                emrtemplet.FileName = txtTitle.Text;
                emrtemplet.DeptId = Convert.ToInt32(cmbDept.EditValue);
                emrtemplet.IsShowFileName = checkShowTitle.Checked ? 1 : 0;
                emrtemplet.MrAttr = radioType.SelectedIndex;
                foreach (CheckedListBoxItem item in checklist.Items)
                {
                    switch (item.Value)
                    {
                        case 0://首次病程
                            emrtemplet.IsFirstDaily = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                        case 1://新页结束
                            emrtemplet.NewPageEnd = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                        case 2://页面配置
                            emrtemplet.IsConfigPageSize = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                        case 3://新页开始
                            emrtemplet.NewPageFlag = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                        case 4://医患沟通
                            emrtemplet.IsYiHuanGouTong = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                    }
                }
                UpdateEmrTemplet(emrtemplet); 
                this.DialogResult = DialogResult.OK;
            }
        } 
        /// <summary>
        /// 更新EmrTemplet表中的数据
        /// </summary>
        /// <param name="emrTemplet">要更新的数据对象</param>
        public int UpdateEmrTemplet(EmrTemplet emrTemplet)
        {
            var sql = @"UPDATE [EMR].[EMRTEMPLET]
                    SET 
                        FileName = @FileName,
                        DeptId = @DeptId,
                        CreatorId = @CreatorId,
                        CreateDateTime = @CreateDateTime,
                        LastTime = @LastTime,
                        Permission = @Permission,
                        MrClass = @MrClass,
                        MrCode = @MrCode,
                        MrName = @MrName,
                        MrAttr = @MrAttr,
                        QcCode = @QcCode,
                        NewPageFlag = @NewPageFlag,
                        FileFlag = @FileFlag,
                        WriteTimes = @WriteTimes,
                        Code = @Code,
                        HospitalCode = @HospitalCode,
                        XmlDoc = @XmlDoc,
                        XmlDocNew = @XmlDocNew,
                        Py = @Py,
                        Wb = @Wb,
                        IsFirstDaily = @IsFirstDaily,
                        IsShowFileName = @IsShowFileName,
                        IsYiHuanGouTong = @IsYiHuanGouTong,
                        NewPageEnd = @NewPageEnd,
                        Valid = @Valid,
                        State = @State,
                        Auditor = @Auditor,
                        AuditDate = @AuditDate,
                        IsConfigPageSize = @IsConfigPageSize,
                        ZyZhenDuan = @ZyZhenDuan,
                        XyZhenDuan = @XyZhenDuan
                    WHERE TempletId = @TempletId";

            return EMRContext.Execute(sql, emrTemplet);
        }
        public void LoadControl()
        {
            IDICT_CATALOGList dictList = WinnerHIS.Clinic.Case.DAL.Interface.DALHelper.DALManager.CreateDICT_CATALOGList();
            dictList.Session = this.Session;
            dictList.GetAllList();
            cmbType.Properties.DataSource = dictList;

            IDepartmentList deptlist = WinnerHIS.Integral.Personnel.DAL.Interface.DALHelper.DALManager.CreateDepartmentList();
            deptlist.Session = this.Session;
            deptlist.GetCMClinicDept();
            cmbDept.Properties.DataSource = deptlist;
        }
    }
}

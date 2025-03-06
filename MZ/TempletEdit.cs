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
        //添加
        public TempletEdit(int deptid,string mrCode,int attr=0)
        {
            InitializeComponent();
            LoadControl();
            isEdit = false;
            if (deptid != 0)
            {
                cmbDept.EditValue = deptid;
            }
            if (mrCode!="")
            {
                cmbType.EditValue = mrCode;
            }
            radioType.SelectedIndex = attr;
        }
        IEMRTEMPLET emrtemplet;
        //修改
        public TempletEdit(IEMRTEMPLET templet)
        {
            InitializeComponent();
            LoadControl();
            isEdit = true;
            emrtemplet = templet;
            txtCode.Text = templet.CREATOR_ID.ToString();
            txtName.Text = templet.MR_NAME;
            cmbType.EditValue = templet.MR_CLASS;
            cmbDept.EditValue = templet.DEPT_ID;
            txtTitle.Text = templet.FILE_NAME;
            checkShowTitle.Checked = templet.ISSHOWFILENAME == 1;
            radioType.SelectedIndex = templet.MR_ATTR;
            foreach (CheckedListBoxItem item in checklist.Items)
            {
                switch (item.Value)
                {
                    case 0://首次病程
                        item.CheckState = templet.ISFIRSTDAILY == 1?CheckState.Checked:CheckState.Unchecked;
                        break;
                    case 1://新页结束
                        item.CheckState = templet.NEW_PAGE_END == 1 ? CheckState.Checked : CheckState.Unchecked;
                        break;
                    case 2://页面配置
                        item.CheckState = templet.ISCONFIGPAGESIZE == 1 ? CheckState.Checked : CheckState.Unchecked;
                        break;
                    case 3://新页开始
                        item.CheckState = templet.NEW_PAGE_FLAG == 1 ? CheckState.Checked : CheckState.Unchecked;
                        break;
                    case 4://医患沟通
                        item.CheckState = templet.ISYIHUANGOUTONG == 1 ? CheckState.Checked : CheckState.Unchecked;
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
                emrtemplet.Refresh();
                emrtemplet.MR_NAME = txtName.Text;
                emrtemplet.MR_CLASS = cmbType.EditValue.ToString();
                emrtemplet.FILE_NAME = txtTitle.Text;
                emrtemplet.DEPT_ID = Convert.ToInt32(cmbDept.EditValue);
                emrtemplet.ISSHOWFILENAME = checkShowTitle.Checked ? 1 : 0;
                emrtemplet.MR_ATTR = radioType.SelectedIndex;
                foreach (CheckedListBoxItem item in checklist.Items)
                {
                    switch (item.Value)
                    {
                        case 0://首次病程
                            emrtemplet.ISFIRSTDAILY =item.CheckState==CheckState.Checked? 1:0;
                            break;
                        case 1://新页结束
                            emrtemplet.NEW_PAGE_END = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                        case 2://页面配置
                            emrtemplet.ISCONFIGPAGESIZE = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                        case 3://新页开始
                            emrtemplet.NEW_PAGE_FLAG = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                        case 4://医患沟通
                            emrtemplet.ISYIHUANGOUTONG = item.CheckState == CheckState.Checked ? 1 : 0;
                            break;
                    }
                }
                emrtemplet.Update();
                this.DialogResult = DialogResult.OK;
            }
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

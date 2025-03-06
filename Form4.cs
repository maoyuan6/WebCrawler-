using HIS.Clinic.ClinicCase.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Extensions;

namespace WinFormsApp1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            MzCotextEditor mzCotextEditor = new MzCotextEditor() { isEdit = true };
            panel3.AddControl(mzCotextEditor);
        }
    }
}

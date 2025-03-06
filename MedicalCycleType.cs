using System;
using System.Collections.Generic;
using System.Text;

namespace WinnerHIS.Diagnosis.Clinic.DoctorWorkstation.UI
{
    /// <summary>
    /// 枚举病人类型列表。
    /// </summary>
    public enum MedicakCycleType : int
    {
        /// <summary>
        /// 待诊病人列表。
        /// </summary>
        Waiting = 0x00,

        /// <summary>
        /// 已诊病人列表。
        /// </summary>
        Complete = 0x01
    }
}

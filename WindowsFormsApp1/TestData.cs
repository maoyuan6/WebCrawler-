// TestData.cs
using System.Collections.Generic;

namespace MedicalSystem
{
    public static class TestData
    {
        public static List<Patient> GetPatients()
        {
            return new List<Patient>
            {
                new Patient {
                    VisitStatus = "结束就诊",
                    QueueNumber = 1,
                    CallStatus = "未叫号",
                    RecordStatus = "未提交",
                    Gender = "女",
                    Name = "嘻嘻嘻",
                    Age = 32,
                    Phone = "15029923231",
                    Diagnosis = "气管、支气管炎",
                    PaymentType = "门诊自费",
                    CardNumber = "X1001",
                    CaseNumber = "201983010",
                    RegisterType = "普通挂号"
                },
                new Patient {
                    VisitStatus = "就诊中",
                    QueueNumber = 2,
                    CallStatus = "未叫号",
                    RecordStatus = "已提交",
                    Gender = "男",
                    Name = "章do06",
                    Age = 47,
                    Phone = "18755367890",
                    Diagnosis = "孕产妇保健",
                    PaymentType = "门诊自费",
                    CardNumber = "202007200",
                    CaseNumber = "361jin",
                    RegisterType = "普通挂号"
                }
            };
        }
    }
}
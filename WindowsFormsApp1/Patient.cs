// Patient.cs
namespace MedicalSystem
{
    public class Patient
    {
        public string VisitStatus { get; set; }  // 就诊状态
        public int QueueNumber { get; set; }     // 号序
        public string CallStatus { get; set; }   // 叫号状态
        public string RecordStatus { get; set; }  // 病历状态
        public string Gender { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Diagnosis { get; set; }    // 诊断
        public string PaymentType { get; set; }  // 费别
        public string CardNumber { get; set; }   // 卡号
        public string CaseNumber { get; set; }   // 病历号
        public string RegisterType { get; set; } // 挂号类型
    }
}
using MSUi.Models;

namespace MSUi.Models
{
    public class DiagnosticData
    {
        public Patient Patient { get; set; }
        public string PatientName { get; set; }
        public string PatId { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int TriggerCount { get; set; }
        public List<string> FoundTriggers { get; set; }
        public string RiskLevel { get; set; }
    }
}
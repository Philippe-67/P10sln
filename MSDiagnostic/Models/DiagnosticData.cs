using Newtonsoft.Json;

namespace MSDiagnostic.Models
{
    public class DiagnosticData
    {
        public Patient Patient { get; set; }
        public List<Note> Notes { get; set; }
        [JsonProperty("age")]
        public int Age { get; set; }
        [JsonProperty("triggerCount")]
        public int TriggerCount { get; set; }
        [JsonProperty("foundTrigger")]
       
        public List<string> FoundTriggers { get; set; }
        [JsonProperty("riskLevel")]

        public string RiskLevel {  get; set; }
    }
}

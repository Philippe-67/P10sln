
using Microsoft.AspNetCore.Mvc;
using MSDiagnostic.Models;
using System.Text.RegularExpressions;

namespace MSDiagnosticService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticController : ControllerBase
    {
        private readonly HttpClient _patientClient;
        private readonly HttpClient _noteClient;

        public DiagnosticController(HttpClient httpClient)
        {
            _patientClient = httpClient;
            _patientClient.BaseAddress = new Uri("https://Localhost:7001");
            _noteClient = httpClient;
            _noteClient.BaseAddress = new Uri("https://Localhost:7001");
        }

        [HttpGet("patientsWithDiabetesRisk")]

        public async Task<IActionResult> GetPatientsWithDiabetesRisk()
        {
            try
            {
                HttpResponseMessage responsePatients = await _patientClient.GetAsync("api/Patient");
                responsePatients.EnsureSuccessStatusCode();
                var patients = await responsePatients.Content.ReadAsAsync<IEnumerable<Patient>>();

                List<DiagnosticData> diagnosticDatas = new List<DiagnosticData>();
                foreach (var patient in patients)
                {
                    int age = CalculateAge(patient.DateDeNaissance);

                    HttpResponseMessage responseNotes = await _noteClient.GetAsync($"api/Notes/{patient.Id}");
                    responseNotes.EnsureSuccessStatusCode();
                    var notes = await responseNotes.Content.ReadAsAsync<IEnumerable<Note>>();

                    var (triggerCount, foundTriggers) = CountTriggers(notes);

                    var riskLevel = DetermineDiabetesRiskLevel(new DiagnosticData { Patient = patient, Notes = (List<Note>)notes, Age = age, TriggerCount = triggerCount, FoundTriggers = foundTriggers });

                    diagnosticDatas.Add(new DiagnosticData { Patient = patient, Notes = (List<Note>)notes, Age = age, TriggerCount = triggerCount, FoundTriggers = foundTriggers, RiskLevel = riskLevel });
                }

                return Ok(diagnosticDatas);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, "Une erreur s'est produite lors de la récupération des informations : " + ex.Message);
            }
        }
       [HttpGet]
        public string DetermineDiabetesRiskLevel(DiagnosticData patientData)
        {
            int triggerCount = patientData.TriggerCount;
            int patientAge = patientData.Age;
            string patientGender = patientData.Patient.Genre;

            if (triggerCount <=1)
            {
                return "None";
            }
            else if ((patientGender == "M" || patientGender == "F") && (patientAge > 30) && (triggerCount >= 2 && triggerCount <= 5))
            {
                return "Borderline";
            }
            else if ((patientGender == "M" || patientGender == "F") && (patientAge > 30) && (triggerCount >= 6 && triggerCount <= 7))
            {
                return "Danger";
            }
            else if ((patientGender == "M" && patientAge < 30) && (triggerCount == 3))
            {
                return "Danger";
            }
            else if ((patientGender == "F" && patientAge < 30) && (triggerCount == 4))
            { 
                    return "Danger";
            }
            else if ((patientGender == "M" && patientAge < 30) && (triggerCount >= 5))
            {
                return "Early Onset";
            }
            else if ((patientGender == "F" && patientAge < 30) && (triggerCount >=7))
            {
                return "Early Onset";
            }
            else if ((patientGender == "M" || patientGender == "F") && (patientAge > 30) && (triggerCount >= 8 ))
            {
                return "Early Onset";
            }
                return "None";
            }


            private int CalculateAge(DateTime dateDeNaissance)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - dateDeNaissance.Year;
                if (dateDeNaissance > today.AddYears(-age)) age--;
                return age;
            }
           
            private (int count, List<string> foundTriggers) CountTriggers(IEnumerable<Note> notes)
            {
                string[] triggers = new string[] { "Hémoglobine A1C", "Microalbumine", "Taille", "Poids", "Fumeur", "Fumeuse", "Anormal", "Cholestérol", "Vertiges", "Rechute", "Réaction", "Anticorps" };

                var triggerCount = 0;
                var foundTriggers = new List<string>();

                foreach (var note in notes)
                {
                    string noteContent = note.Notes.ToLower();
                    foreach (var trigger in triggers)
                    {
                        string pattern = GetRegexPatternForTrigger(trigger);
                        if (Regex.IsMatch(noteContent, pattern))
                        {
                            if (!foundTriggers.Contains(trigger))
                            {
                                foundTriggers.Add(trigger);
                                triggerCount++;
                            }
                        }
                    }
                }

                return (triggerCount, foundTriggers);
            }

            private string GetRegexPatternForTrigger(string trigger)
            {
                switch (trigger)
                {
                    case "Fumeur":
                        return @"\bfum(eur|euse|er|ant)?\b";
                    case "Vertiges":
                        return @"\bvertige[s]?\b";
                    default:
                        return $@"\b{Regex.Escape(trigger.ToLower())}\w*";
                }
            }
        }
    }
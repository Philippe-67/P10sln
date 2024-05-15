//using Microsoft.AspNetCore.Mvc;
//using MSDiagnostic.Models;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace MSDiagnosticService.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DiagnosticController : ControllerBase
//    {
//        private readonly HttpClient _patientClient;
//        private readonly HttpClient _noteClient;

//        public DiagnosticController(HttpClient httpClient)
//        {

//            _patientClient = httpClient;
//            _patientClient.BaseAddress = new Uri("https://Localhost:7001");
//            _noteClient = httpClient;
//            _noteClient.BaseAddress = new Uri("https://Localhost:7001");

//        }


//        //    [HttpGet("patientsWithNotes")]

//        //    public async Task<IActionResult> GetPatientsWithNotes()
//        //    {
//        //        try
//        //        {
//        //            // Récupérer la liste des patients depuis MSPatient
//        //            HttpResponseMessage responsePatients = await _patientClient.GetAsync("api/Patient");
//        //            responsePatients.EnsureSuccessStatusCode(); // Gérer les erreurs si la requête échoue
//        //            var patients = await responsePatients.Content.ReadAsAsync<IEnumerable<Patient>>();

//        //            // Récupérer les notes associées aux patients depuis MSNote
//        //            List<DiagnosticData> diagnosticDatas = new List<DiagnosticData>();
//        //            foreach (var patient in patients)
//        //            {
//        //                int age = CalculateAge(patient.DateDeNaissance);

//        //                HttpResponseMessage responseNotes = await _noteClient.GetAsync($"api/Notes/{patient.Id}");
//        //                responseNotes.EnsureSuccessStatusCode(); // Gérer les erreurs si la requête échoue
//        //                var notes = await responseNotes.Content.ReadAsAsync<IEnumerable<Note>>();

//        //                int triggerCount = CountTriggers(notes);

//        //                diagnosticDatas.Add(new DiagnosticData { Patient = patient, Notes = (List<Note>)notes, Age=age , TriggerCount = triggerCount });

//        //            }

//        //            return Ok(diagnosticDatas);
//        //        }
//        //        catch (HttpRequestException ex)
//        //        {
//        //            return StatusCode(500, "Une erreur s'est produite lors de la récupération des informations : " + ex.Message);
//        //        }
//        //    }
//        [HttpGet("patientsWithNotes")]
//    public async Task<IActionResult> GetPatientsWithNotes()
//    {
//        try
//        {
//            // Récupérer la liste des patients depuis MSPatient
//            HttpResponseMessage responsePatients = await _patientClient.GetAsync("api/Patient");
//            responsePatients.EnsureSuccessStatusCode(); // Gérer les erreurs si la requête échoue
//            var patients = await responsePatients.Content.ReadAsAsync<IEnumerable<Patient>>();

//            // Récupérer les notes associées aux patients depuis MSNote
//            List<DiagnosticData> diagnosticDatas = new List<DiagnosticData>();
//            foreach (var patient in patients)
//            {
//                int age = CalculateAge(patient.DateDeNaissance);

//                HttpResponseMessage responseNotes = await _noteClient.GetAsync($"api/Notes/{patient.Id}");
//                responseNotes.EnsureSuccessStatusCode(); // Gérer les erreurs si la requête échoue
//                var notes = await responseNotes.Content.ReadAsAsync<IEnumerable<Note>>();

//                    var (triggerCount, foundTriggers) = foundTriggers(notes); // Appel de CountTriggers pour obtenir le décompte des déclencheurs et la liste des déclencheurs trouvés

//                    diagnosticDatas.Add(new DiagnosticData { Patient = patient, Notes = (List<Note>)notes, Age = age, TriggerCount = triggerCount, FoundTriggers = foundTriggers }); // Ajout de la liste des déclencheurs trouvés à DiagnosticData

//            }

//            return Ok(diagnosticDatas);
//        }
//        catch (HttpRequestException ex)
//        {
//            return StatusCode(500, "Une erreur s'est produite lors de la récupération des informations : " + ex.Message);
//        }
//    }
//    private int CalculateAge(DateTime dateDeNaissance)
//        {
//            DateTime today = DateTime.Today;
//            int age = today.Year - dateDeNaissance.Year;
//            if (dateDeNaissance > today.AddYears(-age)) age--;
//            return age;
//        }
//        //private int CountTriggers(IEnumerable<Note> notes)
//        //{
//        //    string[] triggers = new string[] { "Hémoglobine A1C", "Microalbumine", "Taille", "Poids", "Fumeur", "Fumeuse", "Anormal", "Cholestérol", "Vertiges", "Rechute", "Réaction", "Anticorps" };

//        //    int triggerCount = 0;
//        //    foreach (var note in notes)
//        //    {
//        //        string noteContent = note.Notes.ToLower();
//        //        foreach (var trigger in triggers)
//        //        {
//        //             if (note.Notes.Contains(trigger.ToLower()))
//        //            //string pattern = $@"\b{Regex.Escape(trigger)}\w*"; // Utiliser une expression régulière pour rechercher le terme déclencheur avec des variations
//        //            //if (Regex.IsMatch(noteContent, pattern))
//        //            {
//        //                triggerCount++;
//        //            }
//        //        }
//        //    }
//        //    return triggerCount;
//        //}
//        ////private int CountTriggers(IEnumerable<Note> notes)
//        ////{
//        ////   // string[] triggers = new string[] { "Hémoglobine A1C", "Microalbumine", "Taille", "Poids", "Fumeur", "Fumeuse", "Anormal", "Cholestérol", "Vertiges", "Rechute", "Réaction", "Anticorps" };
//        ////    string[] triggers = new string[] { "hémoglobine A1C", "microalbumine", "taille", "poids", "fumeur", "fumeuse", "anormal", "cholestérol", "vertiges", "rechute", "réaction", "Anticorps" };

//        ////    int triggerCount = 0;
//        ////    foreach (var note in notes)
//        ////    {
//        ////        string noteContent = note.Notes.ToLower();
//        ////        foreach (var trigger in triggers)
//        ////        {
//        ////            string pattern = $@"\b{Regex.Escape(trigger)}\w*"; // Construire l'expression régulière pour le terme déclencheur et ses dérivés
//        ////            if (Regex.IsMatch(noteContent, pattern))
//        ////            {
//        ////                triggerCount++;
//        ////            }
//        ////        }
//        ////    }
//        ////    return triggerCount;
//        ////}

//    }
//    private (int count, List<string> foundTriggers) CountTriggers(IEnumerable<Note> notes)
//    {
//        string[] triggers = new string[] { "Hémoglobine A1C", "Microalbumine", "Taille", "Poids", "Fumeur", "Fumeuse", "Anormal", "Cholestérol", "Vertiges", "Rechute", "Réaction", "Anticorps" };

//        var triggerCount = 0;
//        var foundTriggers = new List<string>();

//        foreach (var note in notes)
//        {
//            string noteContent = note.Notes.ToLower();
//            foreach (var trigger in triggers)
//            {
//                string pattern = $@"\b{Regex.Escape(trigger.ToLower())}\w*"; // Construire l'expression régulière pour le terme déclencheur et ses dérivés en minuscules
//                if (Regex.IsMatch(noteContent, pattern))
//                {
//                    if (!foundTriggers.Contains(trigger))
//                    {
//                        foundTriggers.Add(trigger);
//                        triggerCount++;
//                    }
//                }
//            }
//        }

//        return (triggerCount, foundTriggers);
//    }
//}
using Microsoft.AspNetCore.Mvc;
using MSDiagnostic.Models;
using System.Net.Http;
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

        [HttpGet("patientsWithNotes")]
        public async Task<IActionResult> GetPatientsWithNotes()
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

                    diagnosticDatas.Add(new DiagnosticData { Patient = patient, Notes = (List<Note>)notes, Age = age, TriggerCount = triggerCount, FoundTriggers = foundTriggers });
                }

                return Ok(diagnosticDatas);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, "Une erreur s'est produite lors de la récupération des informations : " + ex.Message);
            }
        }
        ////[HttpGet("patientsWithDiabetesRisk")]

        ////public async Task<IActionResult> GetPatientsWithDiabetesRisk()
        ////{
        ////    try
        ////    {
        ////        HttpResponseMessage responsePatients = await _patientClient.GetAsync("api/Patient");
        ////        responsePatients.EnsureSuccessStatusCode();
        ////        var patients = await responsePatients.Content.ReadAsAsync<IEnumerable<Patient>>();

        ////        List<DiagnosticData> diagnosticDatas = new List<DiagnosticData>();
        ////        foreach (var patient in patients)
        ////        {
        ////            int age = CalculateAge(patient.DateDeNaissance);

        ////            HttpResponseMessage responseNotes = await _noteClient.GetAsync($"api/Notes/{patient.Id}");
        ////            responseNotes.EnsureSuccessStatusCode();
        ////            var notes = await responseNotes.Content.ReadAsAsync<IEnumerable<Note>>();

        ////            var (triggerCount, foundTriggers) = CountTriggers(notes);

        ////            var riskLevel = DetermineDiabetesRiskLevel(new DiagnosticData { Patient = patient, Notes = (List<Note>)notes, Age = age, TriggerCount = triggerCount, FoundTriggers = foundTriggers });

        ////            diagnosticDatas.Add(new DiagnosticData { Patient = patient, Notes = (List<Note>)notes, Age = age, TriggerCount = triggerCount, FoundTriggers = foundTriggers, RiskLevel = riskLevel });
        ////        }

        ////        return Ok(diagnosticDatas);
        ////    }
        ////    catch (HttpRequestException ex)
        ////    {
        ////        return StatusCode(500, "Une erreur s'est produite lors de la récupération des informations : " + ex.Message);
        ////    }
        ////}
        ////public string DetermineDiabetesRiskLevel(DiagnosticData patientData)
        ////{
        ////    int triggerCount = patientData.TriggerCount;
        ////    int patientAge = patientData.Age;
        ////    string patientGender = patientData.Patient.Genre;

        ////    if (triggerCount == 0)
        ////    {
        ////        return "None";
        ////    }
        ////    else if (triggerCount >= 2 && triggerCount <= 5 && patientAge > 30)
        ////    {
        ////        return "Borderline";
        ////    }
        ////    else if (patientAge < 30)
        ////    {
        ////        if (patientGender == "M" && triggerCount >= 3)
        ////        {
        ////            return "In Danger";
        ////        }
        ////        else if (patientGender == "F" && triggerCount >= 4)
        ////        {
        ////            return "In Danger";
        ////        }
        ////    }
        ////    else
        ////    {
        ////        if (patientGender == "M" && triggerCount >= 6)
        ////        {
        ////            return "In Danger";
        ////        }
        ////        else if (patientGender == "F" && triggerCount >= 7)
        ////        {
        ////            return "In Danger";
        ////        }
        ////    }

        ////    if (patientAge < 30)
        ////    {
        ////        if (patientGender == "M" && triggerCount >= 5)
        ////        {
        ////            return "Early onset";
        ////        }
        ////        else if (patientGender == "F" && triggerCount >= 7)
        ////        {
        ////            return "Early onset";
        ////        }
        ////    }
        ////    else
        ////    {
        ////        if (patientGender == "M" && triggerCount >= 8)
        ////        {
        ////            return "Early onset";
        ////        }
        ////        else if (patientGender == "F" && triggerCount >= 8)
        ////        {
        ////            return "Early onset";
        ////        }
        ////    }

        ////    return "None";
        ////}
        private int CalculateAge(DateTime dateDeNaissance)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateDeNaissance.Year;
            if (dateDeNaissance > today.AddYears(-age)) age--;
            return age;
        }



        //private (int count, List<string> foundTriggers) CountTriggers(IEnumerable<Note> notes)
        //{
        //    string[] triggers = new string[] { "Hémoglobine A1C", "Microalbumine", "Taille", "Poids", "Fumeur", "Fumeuse", "Anormal", "Cholestérol", "Vertiges", "Rechute", "Réaction", "Anticorps" };

        //    var triggerCount = 0;
        //    var foundTriggers = new List<string>();

        //    foreach (var note in notes)
        //    {
        //        string noteContent = note.Notes.ToLower();
        //        foreach (var trigger in triggers)
        //        {
        //            string pattern = $@"\b{Regex.Escape(trigger.ToLower())}\w*";
        //            if (Regex.IsMatch(noteContent, pattern))
        //            {
        //                if (!foundTriggers.Contains(trigger))
        //                {
        //                    foundTriggers.Add(trigger);
        //                    triggerCount++;
        //                }
        //            }
        //        }
        //    }

        //    return (triggerCount, foundTriggers);
        //}
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
                // Ajoutez d'autres cas pour chaque terme avec son expression régulière spécifique
                default:
                    return $@"\b{Regex.Escape(trigger.ToLower())}\w*";
            }
        }
    }
}
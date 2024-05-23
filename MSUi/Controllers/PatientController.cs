using Microsoft.AspNetCore.Mvc;
using MSUi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;
namespace MSUi.Controllers
{
    public class PatientController : Controller
    {
        private readonly HttpClient _httpClient;
     //   private readonly HttpClient _diagnosticClient;

        public PatientController(HttpClient httpClient)
        {

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://Localhost:7001");
            //_diagnosticClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://Localhost:7001");

        }
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    HttpResponseMessage response = await _httpClient.GetAsync("/api/Patient");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string responseData = await response.Content.ReadAsStringAsync();
        //        var patients = JsonConvert.DeserializeObject<List<Patient>>(responseData);
        //        return View(patients);
        //    }
        //    else
        //    {
        //        return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}");
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/Patient");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<Patient>>(responseData);

                // Récupérer le risque au diabète pour chaque patient à partir de l'API MSDiagnostic
                foreach (var patient in patients)
                {
                    HttpResponseMessage responseDiagnostic = await _httpClient.GetAsync($"api/Diagnostic/patientDiagnostic/{patient.Id}");
                    if (responseDiagnostic.IsSuccessStatusCode)
                    {
                        string diagnosticData = await responseDiagnostic.Content.ReadAsStringAsync();
                        var diagnostic = JsonConvert.DeserializeObject<DiagnosticData>(diagnosticData);
                        patient.RiskLevel = diagnostic.RiskLevel; // Mettre à jour le risque au diabète pour le patient
                    }
                    else
                    {
                        // Gérer les erreurs lors de la récupération du risque au diabète
                        // par exemple, définir un risque par défaut ou gérer l'erreur autrement
                        patient.RiskLevel = "Non disponible";
                    }
                }

                return View(patients);
            }
            else
            {
                return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            // Récupérer les données du patient depuis l'API ou la source de données appropriée
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Patient/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var patient = JsonConvert.DeserializeObject<Patient>(json);

                return View(patient); // Retourner la vue avec les données du patient pour modification
            }
            else
            {
                return NotFound(); // Gérer le cas où le patient n'existe pas
            }
        }

        // [HttpPost] pour traiter la soumission du formulaire avec les modifications du patient
        [HttpPost]
        public async Task<IActionResult> Update(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient); // Retourner la vue avec les erreurs de validation
            }

            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PutAsync($"/api/Patient/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(); // Gérer le cas où le patient n'existe pas
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}. Détails : {errorMessage}");
            }
        }
     

        // [HttpDelete]
        public async Task<IActionResult> Delete(int id)
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/Patient/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}. Détails : {errorMessage}");
                }
            }
        }
    } 

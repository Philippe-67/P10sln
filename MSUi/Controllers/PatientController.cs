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


        }
        //  [Authorize(Roles = "organisateur")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/Patient");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<Patient>>(responseData);


                foreach (var patient in patients)
                {
                    HttpResponseMessage responseDiagnostic = await _httpClient.GetAsync($"api/Diagnostic/patientDiagnostic/{patient.Id}");
                    if (responseDiagnostic.IsSuccessStatusCode)
                    {
                        string diagnosticData = await responseDiagnostic.Content.ReadAsStringAsync();
                        var diagnostic = JsonConvert.DeserializeObject<DiagnosticData>(diagnosticData);
                        patient.RiskLevel = diagnostic.RiskLevel;
                    }
                    else
                    {

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
      
        [HttpPut]
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

       
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return View(patient); // Retourne la vue avec les erreurs de validation
            }

            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync("/api/Patient", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}. Détails : {errorMessage}");
            }
        }

       

        [HttpGet]
        public async Task<IActionResult> Delete( int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/Patient/{id}");

            if (response.IsSuccessStatusCode)
            {
                 return RedirectToAction("Index");
              //return Ok(supprimerPatient);

            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else
            {
                //string errorMessage = await response.Content.ReadAsStringAsync();
                return View("Error");
                //return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}. Détails : {errorMessage}");
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MSUi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
namespace MSUi.Controllers
{


    public class PatientController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<PatientController> _logger;
        
        public PatientController(HttpClient httpClient, 
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IHttpContextAccessor contextAccessor,
        ILogger<PatientController> logger)
        {

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://Localhost:7001");
            _userManager = userManager;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _logger = logger;

        }
        [Authorize(Roles = "organisateur,praticien")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Récupération du jeton JWT de la session HTTP stocké dans la méthode Login de AuthenticationController.cs
           var token = _contextAccessor.HttpContext.Session.GetString("token");

            // Ajouter le jeton JWT dans l'en-tête d'autorisation de votre HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("/api/Patient");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var patients = JsonConvert.DeserializeObject<List<Patient>>(responseData);
                
                if (User.IsInRole("praticien"))

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
        [Authorize(Roles = "organisateur")]
        [HttpGet]
        public async Task<IActionResult> detail(int id)
        {
            // Récupération du jeton JWT de la session HTTP stocké dans la méthode Login de AuthenticationController.cs
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            // Ajouter le jeton JWT dans l'en-tête d'autorisation de votre HttpClient
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/Patient/{id}");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var patient = JsonConvert.DeserializeObject<Patient>(responseData);


                //foreach (var patient in patients)
                //{
                //    HttpResponseMessage responseDiagnostic = await _httpClient.GetAsync($"api/Diagnostic/patientDiagnostic/{patient.Id}");
                //    if (responseDiagnostic.IsSuccessStatusCode)
                //    {
                //        string diagnosticData = await responseDiagnostic.Content.ReadAsStringAsync();
                //        var diagnostic = JsonConvert.DeserializeObject<DiagnosticData>(diagnosticData);
                //        patient.RiskLevel = diagnostic.RiskLevel;
                //    }
                //    else
                //    {

                //        patient.RiskLevel = "Non disponible";
                //    }
              //  }

                return View(patient);
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
      
        [HttpPost] //avant j'avais [HttpPut) mais les modf n'étaient pas enregistrées
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
                return RedirectToAction("Detail", new { id = id });
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

        [Authorize(Roles = "organisateur")]
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


        [Authorize(Roles = "organisateur")]
        [HttpGet]
        public async Task<IActionResult> Delete( int id)
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
                //string errorMessage = await response.Content.ReadAsStringAsync();
                return View("Error");
                //return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}. Détails : {errorMessage}");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MSUi.Models;
using Newtonsoft.Json;

namespace MSUi.Controllers
{
    public class DiagnosticController : Controller
    {
        private readonly HttpClient _httpClient;

        public DiagnosticController(HttpClient httpClient)
        {

            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://Localhost:7001");

        }
        [HttpGet("patientsWithDiabetesRisk")]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/api/Diagnostic/patientsWithDiabetesRisk");
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var risks = JsonConvert.DeserializeObject<List<DiagnosticData>>(responseData);
                return View(risks);
            }
            else
            {
                return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}");
            }
        }
            [HttpGet("patientsWithDiabetesRisk/{patId}")]
            public async Task<IActionResult> Index2()
            {
                HttpResponseMessage response = await _httpClient.GetAsync("/api/Diagnostic/patientsWithDiabetesRisk/{patId}");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var risks = JsonConvert.DeserializeObject<List<DiagnosticData>>(responseData);
                    return View(risks);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}");
                }
            }

    }
}

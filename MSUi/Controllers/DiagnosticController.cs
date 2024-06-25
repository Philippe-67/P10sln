using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSUi.Helpers;
using MSUi.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MSUi.Controllers
{
    public class DiagnosticController : Controller
    {
        private readonly HttpClient _httpClient;

        public DiagnosticController(HttpClient httpClient)
        {
            ///new Uri("https://Localhost:7001");--> _httpClient.BaseAddress = new Uri("http://gateway:80")-->_httpClient.BaseAddress = new Uri(UriHelpers.GATEWAY_URI);
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(UriHelpers.GATEWAY_URI);
        }

     //   [Authorize]
        [HttpGet("patientsWithDiabetesRisk")]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
    }      
}

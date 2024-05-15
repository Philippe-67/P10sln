using MSDiagnostic.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSDiagnostic.Services
{ 
public class PatientApiService
{
    private readonly HttpClient _httpClient;

    public PatientApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://Localhost/7001");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<Patient> GetPatientById(int Id)
    {
        var response = await _httpClient.GetAsync($"api/patient/{Id}");
        response.EnsureSuccessStatusCode(); // Gère les erreurs HTTP

        var patient = await response.Content.ReadAsAsync<Patient>(); // Désérialisation de la réponse JSON en objet PatientModel
        return patient;
    }
} }
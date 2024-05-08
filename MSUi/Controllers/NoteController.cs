using Microsoft.AspNetCore.Mvc;
using MSUi.Models;
using Newtonsoft.Json;

public class NoteController : Controller
{
    private readonly HttpClient _httpClient;

    public NoteController(HttpClient httpClient)
    {

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://Localhost:7001");

    }
    [HttpGet("Note/{patId}")]
    public async Task<IActionResult> Index(int patId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/Notes/{patId}"); 
        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<List<Note>>(responseData);
            return View(notes);
        }
        else
        {
            return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}");
        }
    }
}
   
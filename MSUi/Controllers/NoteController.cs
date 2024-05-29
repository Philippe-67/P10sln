using Microsoft.AspNetCore.Mvc;
using MSUi.Models;
using Newtonsoft.Json;
using System.IO;

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
            try
            {
                List<Note>? notes = JsonConvert.DeserializeObject<List<Note>>(responseData);

                return View(notes);
            }
            catch (JsonSerializationException)
            {
                return Content("Ce patient n'a pas de note");
            }
        }
        else
        {
            return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}");
        }
    }
    [HttpPost("Delete/{Id}")]
        public async  Task<IActionResult> SupprimerNote(string Id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/Notes/{Id}");
             return View("Index");
    }
}
   
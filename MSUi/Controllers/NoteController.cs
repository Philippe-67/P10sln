using Microsoft.AspNetCore.Mvc;
using MSUi.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

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
                var notes = JsonConvert.DeserializeObject<List<Note>>(responseData);
                if (notes.Any())
                {
                    return View(notes);
                }
                else
                {
                    return Content("Ce patient n'a pas de note");
                }
            }
            catch (JsonSerializationException)
            {
                return Content("Une erreur s'est produite lors de la désérialisation des données");
            }
        }
        else
        {
            return StatusCode((int)response.StatusCode, $"Erreur HTTP: {response.StatusCode}");
        }
    }

    


    [HttpGet]
    public async Task<IActionResult> Delete(string id, int PatId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/Notes/{id}");
        if (response.IsSuccessStatusCode)
        {
            //  return View(PatId);
            return RedirectToAction("Index", new { patId = PatId });
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
    //    [HttpGet]
    //   // public async Task<IActionResult> Update(int id)
    //    public async Task<IActionResult> Update(string Id, [FromBody] List<Note> updatedNotes)

    //{
    //    // Récupérer les données du patient depuis l'API ou la source de données appropriée
    //    HttpResponseMessage response = await _httpClient.GetAsync($"/api/Notes/{Id}");

    //        if (response.IsSuccessStatusCode)
    //        {
    //            var json = await response.Content.ReadAsStringAsync();
    //            var note = JsonConvert.DeserializeObject<Notes>(json);

    //            return View(note); // Retourner la vue avec les données du patient pour modification
    //        }
    //        else
    //        {
    //            return NotFound(); // Gérer le cas où le patient n'existe pas
    //        }
    //    }

    //}
}



   
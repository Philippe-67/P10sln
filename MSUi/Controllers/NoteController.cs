using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSUi.Helpers;
using MSUi.Models;
using Newtonsoft.Json;
using NuGet.Common;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

public class NoteController : Controller
{
    private readonly HttpClient _httpClient;

    public NoteController(HttpClient httpClient)
    {

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(UriHelpers.GATEWAY_URI);
    //    _httpClient.BaseAddress = new Uri("http://gateway:80");

    }
    public ActionResult VotreAction()
    {
        

        return View("Deux");
    }
  //  [Authorize(Roles = "praticien")]
    
    [HttpGet]
    public async Task<IActionResult> Index(int patId)
    {
        var token = Request.Cookies["jwtToken"];
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Token is missing");
        }
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(patId), Encoding.UTF8, "application/json");
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
    }

   // [Authorize(Roles = "praticien")]
    [HttpGet]
    public async Task<IActionResult> Delete(string id, int PatId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/api/Notes/{id}");
        if (response.IsSuccessStatusCode)
        {
            // return View("Index");
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
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Note newNote)
    {
        var content = new StringContent(JsonConvert.SerializeObject(newNote), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("/api/Notes", content);

        if (response.IsSuccessStatusCode)
        {
            // Renvoyer une vue ou rediriger vers une autre action si nécessaire
            return RedirectToAction("Index", new { patId = newNote.PatId });
        }
        else
        {
            // Gérer les erreurs en cas de réponse non valide
            return View("Error");
        }
    }
   
}



   
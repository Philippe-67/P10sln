
using MongoDB.Bson.IO;
using MSDiagnostic.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSDiagnostic.Services
{
    public class NoteApiService
    {
        private readonly HttpClient _httpClient;

        public NoteApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://Localhost:7001");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<Note> GetNoteByPatientId(int Id)
        {
            var response = await _httpClient.GetAsync($"api/note/{Id}");
            response.EnsureSuccessStatusCode(); // Gère les erreurs HTTP

            var note = await response.Content.ReadAsAsync<Note>(); // Désérialisation de la réponse JSON en objet NoteModel
            return note;



        }
    }
}

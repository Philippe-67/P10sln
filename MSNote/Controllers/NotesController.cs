using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSNote.Models;
using MSNote.Services;

namespace MSNote.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesService _notesService;

        public NotesController(NotesService notesService) =>
       _notesService = notesService;

        [HttpGet]
        public async Task<List<Note>> Get() =>
            await _notesService.GetAsync();

        
        [HttpGet("{patId}")]
        public async Task<ActionResult<List<Note>>> GetByPatId(int patId)
        {
            var notes = await _notesService.GetByPatIdAsync(patId);

            if (notes.Count == 0)
            {
                //return NotFound();
                return Content("ce patient n a pas de note");
            }

            return notes;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Note newNote)
        {
            await _notesService.CreateAsync(newNote);

            return CreatedAtAction(nameof(Get), new { id = newNote.Id }, newNote);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var note = await _notesService.GetAsync();
         


            if (note == null)
            {
                return NotFound();
            }

            await _notesService.RemoveAsync(Id);

            return NoContent();
        }
    }
}

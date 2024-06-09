using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MSNote.Models;
using MSNote.Services;
using System.Threading.Tasks;

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

            //if (notes.Count == 0)
            //{
            //    // return NotFound(new { message = "Aucune note enregistrée pour ce patient" });
            //    // return Content("ce patient n a pas de note");
            //    Note defaultNote = new Note
            //    {
            //        Notes = "Aucune note enregistrée"
            //        // Autres propriétés à initialiser si nécessaire
            //    };
         //       return new List<Note> { defaultNote };
         //   }
            return notes;
        }

        //[HttpPost]
        //public IActionResult Create([FromBody] Note note)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _notesService.Add(note);
        //        _notesService.SaveChanges();
        //        return Ok(note);
        //    }
        //    return BadRequest(ModelState);
        //}

        [HttpPost]
        public async Task<IActionResult> Post(Note newNote)
        {
            await _notesService.CreateAsync(newNote);

            return CreatedAtAction(nameof(Get), new { id = newNote.Id }, newNote);
        }

       // [HttpDelete("{Id}")]
        [HttpDelete("{Id:length(24)}")]
        public async Task<IActionResult> Delete(string Id)
        {
            var note = await _notesService.GetByIdAsync(Id);
            if (note == null)
            {
                return NotFound();
            }
            await _notesService.RemoveAsync(Id);
            return NoContent();
        }

        //    [HttpPut("{Id}")]
        //  //  public async Task<IActionResult> Update(string Id, Note updatedNotes);
        //        public async Task<IActionResult> Update(string Id, [FromBody] List<Note> updatedNotes)
        //    {
        //        var existingNote = await _notesService.GetByIdAsync(Id);

        //        if (existingNote == null)
        //        {
        //            return NotFound();
        //        }

        //        existingNote.Notes = updatedNotes;
        //       // existingNote.Notes= updatedNote.Notes;
        //        // Mettez à jour d'autres propriétés au besoin

        //        await _notesService.UpdateAsync(Id, existingNote);

        //        return NoContent();
        //    }
        //}
    }
}

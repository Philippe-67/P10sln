using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSPatient.Data;
using MSPatient.Models;
using System.Collections.Generic;
using System.Linq;

namespace MSPatient.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
  // [Authorize(Roles = "organisateur")]
    public class PatientController : ControllerBase
    {
        private readonly PatientDbContext _context;

        public PatientController(PatientDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetPatients()
        {
            var patients = _context.Patients.ToList();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }
        [HttpPost]
        public IActionResult Create([FromBody] Patient patient)
        {
            if (ModelState.IsValid)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return Ok(patient);
            }
            return BadRequest(ModelState);
        }


       
        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }
            _context.Entry(patient).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public IActionResult DeletePatient(int id)
        //{
        //    var patient = _context.Patients.Find(id);
        //    if (patient == null)
        //    {
        //        return NotFound();
        //    }
        //    _context.Patients.Remove(patient);
        //    _context.SaveChanges(); 
        //    return NoContent();
        //}
        [HttpDelete("{id}")]
       
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
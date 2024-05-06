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
        public ActionResult<Patient> AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges(); 
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
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

        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            _context.Patients.Remove(patient);
            _context.SaveChanges(); 
            return NoContent();
        }
    }
}
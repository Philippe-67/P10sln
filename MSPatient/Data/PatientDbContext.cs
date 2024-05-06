using Microsoft.EntityFrameworkCore;
using MSPatient.Models;

namespace MSPatient.Data
{
    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }

        internal void DeletePatient(int id)
        {
            throw new NotImplementedException();
        }

        internal object GetPatientById(int id)
        {
            throw new NotImplementedException();
        }

        internal void UpdatePatient(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}

//namespace MSDiagnostic.Services
//{
    
//    public class DiagnosticService : IDiagnosticService
//    {
//        private readonly IMSPatientService _msPatientService;
//        private readonly IMSNoteService _msNoteService;

//        public DiagnosticService(IMSPatientService msPatientService, IMSNoteService msNoteService)
//        {
//            _msPatientService = msPatientService;
//            _msNoteService = msNoteService;
//        }

//        public PatientDiagnosticData GetPatientDiagnosticData(int patientId)
//        {
//            var patient = _msPatientService.GetPatient(patientId);
//            var patientNotes = _msNoteService.GetPatientNotes(patientId);

//            var patientDiagnosticData = new PatientDiagnosticData
//            {
//                Patient = patient,
//                Notes = patientNotes
//            };

//            return patientDiagnosticData;
//        }
//    }
//}

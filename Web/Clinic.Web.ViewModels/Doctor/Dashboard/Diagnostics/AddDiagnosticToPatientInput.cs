namespace Clinic.Web.ViewModels.Doctor.Dashboard.Diagnostics
{
    public class AddDiagnosticToPatientInput
    {
        public string PatientId { get; set; }

        public string DiagnosticId { get; set; }

        public string ClinicId { get; set; }
    }
}

namespace Clinic.Data.Models.Hospital
{
    public class PatientDiagnostics
    {
        public string PatientId { get; set; }

        public virtual ApplicationUser Patient { get; set; }

        public string DiagnosticsId { get; set; }

        public virtual Diagnostics Diagnostics { get; set; }
    }
}

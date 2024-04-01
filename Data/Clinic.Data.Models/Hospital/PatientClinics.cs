namespace Clinic.Data.Models.Hospital
{
    public class PatientClinics
    {
        public string PatientId { get; set; }

        public virtual ApplicationUser Patient { get; set; }

        public string ClinicId { get; set; }

        public virtual Clinic Clinic { get; set; }
    }
}

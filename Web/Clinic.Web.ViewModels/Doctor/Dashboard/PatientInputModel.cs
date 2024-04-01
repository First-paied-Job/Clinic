namespace Clinic.Web.ViewModels.Doctor.Dashboard
{
    using System.ComponentModel.DataAnnotations;

    public class PatientInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string ClinicId { get; set; }
    }
}

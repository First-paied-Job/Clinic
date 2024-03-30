namespace Clinic.Web.ViewModels.Administration.Dashboard.Clinic
{
    using System.ComponentModel.DataAnnotations;

    public class AddDoctorToClinicInput
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string ClinicId { get; set; }
    }
}

namespace Clinic.Web.ViewModels.Administration.Dashboard
{
    using System.ComponentModel.DataAnnotations;

    public class DoctorInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

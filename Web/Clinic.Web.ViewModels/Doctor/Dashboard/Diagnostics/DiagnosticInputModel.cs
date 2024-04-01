namespace Clinic.Web.ViewModels.Doctor.Dashboard.Diagnostics
{
    using System.ComponentModel.DataAnnotations;

    public class DiagnosticInputModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]

        public string Description { get; set; }

        [Required]
        public string ClinicId { get; set; }

        public string CreatorId { get; set; }
    }
}

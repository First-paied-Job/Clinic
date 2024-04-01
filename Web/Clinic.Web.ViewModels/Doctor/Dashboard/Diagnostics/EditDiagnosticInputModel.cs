namespace Clinic.Web.ViewModels.Doctor.Dashboard.Diagnostics
{
    using System.ComponentModel.DataAnnotations;

    public class EditDiagnosticInputModel
    {
        public string DiagnosticId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

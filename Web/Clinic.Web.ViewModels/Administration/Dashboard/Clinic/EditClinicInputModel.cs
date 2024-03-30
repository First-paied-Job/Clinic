namespace Clinic.Web.ViewModels.Administration.Dashboard.Clinic
{
    using System.ComponentModel.DataAnnotations;

    public class EditClinicInputModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        public string ClinicId { get; set; }
    }
}

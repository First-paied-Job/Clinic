namespace Clinic.Web.ViewModels.Administration.Dashboard.Clinic
{
    using System.ComponentModel.DataAnnotations;

    public class ClinicInputModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        public string HospitalEmployerId { get; set; }
    }
}

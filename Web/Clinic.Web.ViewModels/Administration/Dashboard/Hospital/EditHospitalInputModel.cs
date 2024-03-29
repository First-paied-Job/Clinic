namespace Clinic.Web.ViewModels.Administration.Dashboard.Hospital
{
    using System.ComponentModel.DataAnnotations;

    public class EditHospitalInputModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string HospitalId { get; set; }
    }
}

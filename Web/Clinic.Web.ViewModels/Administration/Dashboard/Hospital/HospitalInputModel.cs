namespace Clinic.Web.ViewModels.Administration.Dashboard.Hospital
{
    using System.ComponentModel.DataAnnotations;

    public class HospitalInputModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
    }
}

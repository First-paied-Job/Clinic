namespace Clinic.Web.ViewModels.Administration.Dashboard.Hospital
{
    using System.Collections.Generic;

    public class HospitalViewModel
    {
        public string Name { get; set; }

        public string HospitalId { get; set; }

        public List<ClinicDTO> Clinics { get; set; }
    }
}

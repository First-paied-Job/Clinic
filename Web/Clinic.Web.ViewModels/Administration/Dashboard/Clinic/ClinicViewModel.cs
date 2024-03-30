namespace Clinic.Web.ViewModels.Administration.Dashboard.Clinic
{
    using System.Collections.Generic;

    public class ClinicViewModel
    {
        public string HospitalId { get; set; }

        public string ClinicId { get; set; }

        public string Name { get; set; }

        public List<DoctorDTO> Doctors { get; set; }
    }
}

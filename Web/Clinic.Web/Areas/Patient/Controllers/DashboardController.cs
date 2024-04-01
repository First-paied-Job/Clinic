namespace Clinic.Web.Areas.Patient.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Clinic.Services.Data.Contracts;
    using Clinic.Web.ViewModels.Patient.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : PatientController
    {
        private readonly IPatientService patientService;

        public DashboardController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var viemodel = await this.patientService.GetClinicsForPatientAsync(userId);
            return this.View(viemodel);
        }

        public async Task<IActionResult> DiagnosticsForPatient(string clinicId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            var viemodel = await this.patientService.GetDiagnosticForPatientInClinic(new DiagnosticInClinicInputModel { ClinicId = clinicId, PatientId = userId });
            return this.View(viemodel);
        }
    }
}

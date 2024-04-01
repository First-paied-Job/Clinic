namespace Clinic.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Clinic.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class StatisticsController : AdministrationController
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public IActionResult Statistics()
        {
            return this.View();
        }

        public async Task<IActionResult> Clinics()
        {
            var viewModel = await this.statisticsService.GetAllClinics();

            return this.View(viewModel);
        }

        public async Task<IActionResult> GetDoctorsInClinic(string clinicId)
        {
            var viewModel = await this.statisticsService.GetDoctorsInClinic(clinicId);

            return this.View(viewModel);
        }

        public async Task<IActionResult> GetPatientsInClinic(string clinicId)
        {
            var viewModel = await this.statisticsService.GetPatientsInClinic(clinicId);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Diagnostics()
        {
            var viewModel = await this.statisticsService.GetAllDiagnostics();

            return this.View(viewModel);
        }

        public async Task<IActionResult> DiagnosticsFromDoctor(string doctorId)
        {
            var viewModel = await this.statisticsService.GetDiagnosticsFromDoctor(doctorId);

            return this.View(viewModel);
        }

        public async Task<IActionResult> DiagnosticsForPatient(string patientId)
        {
            var viewModel = await this.statisticsService.GetDiagnosticsForPatient(patientId);

            return this.View(viewModel);
        }

        public async Task<IActionResult> DiagnosticsPatientList()
        {
            var viewModel = await this.statisticsService.GetPatientsWithDiagnostics();

            return this.View(viewModel);
        }
    }
}

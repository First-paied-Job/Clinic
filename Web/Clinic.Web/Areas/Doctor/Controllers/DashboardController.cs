namespace Clinic.Web.Areas.Doctor.Controllers
{
    using System.Threading.Tasks;

    using Clinic.Services.Data.Contracts;
    using Clinic.Web.ViewModels.Doctor.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : DoctorController
    {
        private readonly IDoctorService doctorService;

        public DashboardController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { };
            return this.View(viewModel);
        }

        public IActionResult AddPatient()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(PatientInputModel input)
        {
            try
            {
                await this.doctorService.AddPatientRoleToUser(input.Email);
            }
            catch (System.Exception e)
            {
                if (e.Message == "404, Resource not found")
                {
                    this.ModelState.AddModelError("noPatient", "There are no results recorded for the given email.");
                }
                else
                {
                    this.ModelState.AddModelError("noPatient", e.Message);
                }
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddPatient");
            }

            return this.Redirect("/");
        }

        public async Task<IActionResult> List()
        {
            var viewModel = await doctorService.GetPatientsAsync();
            return this.View(viewModel);
        }

        public async Task<IActionResult> RemovePatient(string userId)
        {
            await this.doctorService.RemovePatientRoleFromUser(userId);

            return this.Redirect("/");
        }
    }
}

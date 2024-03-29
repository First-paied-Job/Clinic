namespace Clinic.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Clinic.Services.Data.Contracts;
    using Clinic.Web.ViewModels.Administration.Dashboard;
    using Clinic.Web.ViewModels.Administration.Dashboard.Hospital;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly IAdministratorService administratorService;

        public DashboardController(IAdministratorService administratorService)
        {
            this.administratorService = administratorService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { };
            return this.View(viewModel);
        }

        public IActionResult AddDoctor()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorInputModel input)
        {
            try
            {
                await this.administratorService.AddDoctorRoleToUser(input.Email);
            }
            catch (System.Exception e)
            {
                if (e.Message == "404, Resource not found")
                {
                    this.ModelState.AddModelError("noDoctor", "There are no results recorded for the given email.");
                }
                else
                {
                    this.ModelState.AddModelError("noDoctor", e.Message);
                }
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddDoctor");
            }

            return this.Redirect("/");
        }

        public async Task<IActionResult> List()
        {
            var viewModel = await administratorService.GetDoctorsAsync();
            return this.View(viewModel);
        }

        public async Task<IActionResult> RemoveDoctor(string userId)
        {
            await this.administratorService.RemoveDoctorRoleFromUser(userId);

            return this.Redirect("/");
        }

        public IActionResult AddHospital()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHospital(HospitalInputModel input)
        {
            try
            {
                await this.administratorService.AddHospitalAsync(input);
            }
            catch (System.Exception e)
            {
                this.ModelState.AddModelError("noDoctor", e.Message);
            }

            if (!this.ModelState.IsValid)
            {
                return this.View("AddHospital");
            }

            return this.Redirect("/");
        }

        public async Task<IActionResult> RemoveHospital(string hospitalId)
        {
            await this.administratorService.RemoveHospitalAsync(hospitalId);

            return this.Redirect("/");
        }

        public async Task<IActionResult> HospitalList()
        {
            var viewModel = await this.administratorService.GetHospitalsAsync();
            return this.View(viewModel);
        }

        public async Task<IActionResult> EditHospital(string hospitalId)
        {
            var viewModel = await this.administratorService.GetHospitalEdit(hospitalId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditHospitalPost(EditHospitalInputModel input)
        {
            await this.administratorService.EditHospitalAsync(input);

            return this.Redirect("/Administration/Dashboard/HospitalList");
        }
    }
}

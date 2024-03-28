namespace Clinic.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Clinic.Services.Data.Contracts;
    using Clinic.Web.ViewModels.Administration.Dashboard;
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
            await this.administratorService.AddDoctorRoleToUser(input.Email);

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
    }
}

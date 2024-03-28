namespace Clinic.Web.Areas.Doctor.Controllers
{
    using Clinic.Web.ViewModels.Doctor.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : DoctorController
    {
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { };
            return this.View(viewModel);
        }
    }
}

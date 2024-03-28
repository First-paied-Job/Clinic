namespace Clinic.Web.Areas.Patient.Controllers
{
    using Clinic.Web.ViewModels.Patient.Dashboard;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : PatientController
    {
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { };
            return this.View(viewModel);
        }
    }
}

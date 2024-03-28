namespace Clinic.Web.Areas.Doctor.Controllers
{
    using Clinic.Common;
    using Clinic.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.ClinicDoctortRoleName)]
    [Area("Doctor")]
    public class DoctorController : BaseController
    {
    }
}

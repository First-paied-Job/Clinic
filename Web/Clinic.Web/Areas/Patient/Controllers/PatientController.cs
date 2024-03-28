namespace Clinic.Web.Areas.Patient.Controllers
{
    using Clinic.Common;
    using Clinic.Web.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.ClinicPatientRoleName)]
    [Area("Patient")]
    public class PatientController : BaseController
    {
    }
}

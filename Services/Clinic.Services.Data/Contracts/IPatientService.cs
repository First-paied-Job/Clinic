namespace Clinic.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Web.ViewModels.Patient.Dashboard;

    public interface IPatientService
    {
        public Task<ICollection<ClinicPatientView>> GetClinicsForPatientAsync(string patientId);

        public Task<ICollection<DiagnosticPatientView>> GetDiagnosticForPatientInClinic(DiagnosticInClinicInputModel input);
    }
}

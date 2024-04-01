namespace Clinic.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Web.ViewModels.Doctor.Dashboard;
    using Clinic.Web.ViewModels.Doctor.Dashboard.Diagnostics;

    public interface IDoctorService
    {
        public Task<IndexViewModel> GetDoctorsClinic(string userId);

        public Task AddPatientToClinic(PatientInputModel input);

        public Task<ICollection<PatientViewModel>> GetPatientsAsync(string clinicId);

        public Task RemovePatientFromClinic(PatientRemoveModel model);

        public Task RemovePatientRolesAsync(string userId);

        // Diagnostics

        public Task AddDiagnosticAsync(DiagnosticInputModel input);

        public Task RemoveDiagnosticAsync(string diagnosticId);

        public Task<ICollection<DiagnosticViewModel>> GetDiagnosticsInClinicAsync(string clinicId);

        public Task<DiagnosticEditViewModel> GetDiagnosticEditAsync(string diagnosticId);

        public Task EditDiagnosticAsync(EditDiagnosticInputModel input);

        public Task RemoveDiagnosticFromPatientAsync(RemoveDiagnosticFromPatientModel model);

        public Task AddDiagnosticToPatientAsync(AddDiagnosticToPatientInput input);

        public Task<ICollection<PatientDiagnosticView>> GetPatientDiagnosticsAsync(AvailableDiagnosticsInput input);

        public Task<ICollection<DiagnosticViewModel>> GetAvailableDiagnosticsForPatient(AvailableDiagnosticsInput input);

        // Справки за служители, клиенти, изследвания.
    }
}

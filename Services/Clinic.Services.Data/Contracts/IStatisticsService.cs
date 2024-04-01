namespace Clinic.Services.Data.Contracts
{
    using Clinic.Web.ViewModels.Administration.Statistics;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStatisticsService
    {

        // Clinics
        public Task<ICollection<StatisticClinicViewModel>> GetAllClinics();

        public Task<ICollection<StatisticDoctorViewModel>> GetDoctorsInClinic(string clinicId);

        public Task<ICollection<StatisticPatientViewModel>> GetPatientsInClinic(string clinicId);

        public Task<ICollection<StatisticDiagnosticViewModel>> GetAllDiagnostics();

        public Task<ICollection<StatisticDiagnosticViewModel>> GetDiagnosticsFromDoctor(string doctorId);

        public Task<ICollection<StatisticDiagnosticPatientViewModel>> GetDiagnosticsForPatient(string patientId);

        public Task<ICollection<StatisticPatientViewModel>> GetPatientsWithDiagnostics();

        // Diagnostics
    }
}

namespace Clinic.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Web.ViewModels.Doctor.Dashboard;

    public interface IDoctorService
    {
        public Task AddPatientRoleToUser(string email);

        public Task<ICollection<PatientViewModel>> GetPatientsAsync();

        public Task RemovePatientRoleFromUser(string userId);

        // Справки за служители, клиенти, изследвания.
    }
}

namespace Clinic.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Web.ViewModels.Administration.Dashboard;
    using Clinic.Web.ViewModels.Administration.Dashboard.Hospital;

    public interface IAdministratorService
    {
        public Task AddDoctorRoleToUser(string email);

        public Task<ICollection<DoctorViewModel>> GetDoctorsAsync();

        public Task RemoveDoctorRoleFromUser(string userId);

        public Task AddHospitalAsync(HospitalInputModel input);

        public Task RemoveHospitalAsync(string hospitalId);

        public Task<ICollection<HospitalViewModel>> GetHospitalsAsync();

        public Task EditHospitalAsync(EditHospitalInputModel input);

        public Task<EditHospitalViewModel> GetHospitalEdit(string id);

        // Справки за служители, клиенти, изследвания.
    }
}

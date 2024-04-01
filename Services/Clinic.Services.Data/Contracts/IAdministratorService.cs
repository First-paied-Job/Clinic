namespace Clinic.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Web.ViewModels.Administration.Dashboard;
    using Clinic.Web.ViewModels.Administration.Dashboard.Clinic;
    using Clinic.Web.ViewModels.Administration.Dashboard.Hospital;

    public interface IAdministratorService
    {

        // Doctor
        public Task AddDoctorRoleToUser(string email);

        public Task<ICollection<DoctorViewModel>> GetDoctorsAsync();

        public Task RemoveDoctorRoleFromUser(string userId);

        // Hospital
        public Task AddHospitalAsync(HospitalInputModel input);

        public Task RemoveHospitalAsync(string hospitalId);

        public Task<ICollection<HospitalViewModel>> GetHospitalsAsync();

        public Task EditHospitalAsync(EditHospitalInputModel input);

        public Task<EditHospitalViewModel> GetHospitalEdit(string id);

        // Clinic
        public Task AddClinicToHospitalAsync(ClinicInputModel input);

        public Task RemoveClinicAsync(string clinicId);

        public Task<ICollection<ClinicViewModel>> GetClinicsInHospitalAsync(string hospitalId);

        public Task EditClinicAsync(EditClinicInputModel input);

        public Task<EditClinicViewModel> GetClinicEdit(string id);

        public Task AddDoctorToClinic(AddDoctorToClinicInput input);

        public Task RemoveDoctorFromClinic(string doctorId);

        // Справки за служители, клиенти, изследвания.
    }
}

namespace Clinic.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Web.ViewModels.Administration.Dashboard;

    public interface IAdministratorService
    {
        public Task AddDoctorRoleToUser(string email);

        public Task<ICollection<DoctorViewModel>> GetDoctorsAsync();

        public Task RemoveDoctorRoleFromUser(string userId);


        // Справки за служители, клиенти, изследвания.
    }
}

namespace Clinic.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Clinic.Common;
    using Clinic.Data;
    using Clinic.Data.Models.Hospital;
    using Clinic.Services.Data.Contracts;
    using Clinic.Web.ViewModels.Administration.Dashboard;
    using Clinic.Web.ViewModels.Administration.Dashboard.Hospital;
    using Microsoft.EntityFrameworkCore;

    public class AdministratorService : IAdministratorService
    {
        private readonly ApplicationDbContext db;

        public AdministratorService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddDoctorRoleToUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email", "The given email is invalid!");
            }

            var user = this.db.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                throw new ArgumentNullException("email", "There is no user with the given email!");
            }

            var doctorRole = this.db.Roles.FirstOrDefault(r => r.Name == GlobalConstants.ClinicDoctortRoleName);

            if (doctorRole == null)
            {
                throw new InvalidOperationException($"There is no role with the name \"{GlobalConstants.ClinicDoctortRoleName}\"!");
            }

            this.db.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = doctorRole.Id,
                UserId = user.Id,
            });

            await this.db.SaveChangesAsync();
        }

        public async Task AddHospitalAsync(HospitalInputModel input)
        {
            var check = await this.db.Hospitals.FirstOrDefaultAsync(h => h.Name == input.Name);

            if (check != null)
            {
                throw new ArgumentException("There is already a Hospital with this name!");
            }

            var hospital = new Hospital
            {
                Name = input.Name,
            };

            await this.db.Hospitals.AddAsync(hospital);

            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<DoctorViewModel>> GetDoctorsAsync()
        {
            var viewModel = new List<DoctorViewModel>();

            var doctorRole = await this.db.Roles.FirstOrDefaultAsync(r => r.Name == GlobalConstants.ClinicDoctortRoleName);
            var doctorsIds = await this.db.UserRoles.Where(ur => ur.RoleId == doctorRole.Id).ToListAsync();
            foreach (var pair in doctorsIds)
            {
                var doctor = await this.db.Users.Where(u => u.Id == pair.UserId)
                    .Select(u => new DoctorViewModel
                    {
                        DoctorId = u.Id,
                        Name = u.Name,
                        Email = u.Email
                    })
                    .FirstOrDefaultAsync();

                viewModel.Add(doctor);
            }

            return viewModel;
        }

        public async Task<ICollection<HospitalViewModel>> GetHospitalsAsync()
        {
            var hospitals = await this.db.Hospitals
                .Select(h => new HospitalViewModel
                {
                    HospitalId = h.HospitalId,
                    Name = h.Name,
                    Clinics = h.Clincs.Select(c => new ClinicDTO
                    {
                        ClinicId = c.ClinicId,
                        Name = c.Name,
                    }).ToList(),
                })
                .ToListAsync();

            return hospitals;
        }

        public async Task RemoveDoctorRoleFromUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("userId", "The given userId is invalid!");
            }

            var user = this.db.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("userId", "There is no user with the given userId!");
            }

            var doctorRole = this.db.Roles.FirstOrDefault(r => r.Name == GlobalConstants.ClinicDoctortRoleName);

            if (doctorRole == null)
            {
                throw new InvalidOperationException($"There is no role with the name \"{GlobalConstants.ClinicDoctortRoleName}\"!");
            }

            this.db.UserRoles.Remove(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = doctorRole.Id,
                UserId = user.Id,
            });

            await this.db.SaveChangesAsync();
        }

        public async Task RemoveHospitalAsync(string hospitalId)
        {
            var hospital = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            if (hospital == null)
            {
                throw new ArgumentException("This hospital does not exist!");
            }

            this.db.Hospitals.Remove(hospital);

            await this.db.SaveChangesAsync();
        }

        public async Task EditHospitalAsync(EditHospitalInputModel input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "The given input is null!");
            }

            var hospital = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == input.HospitalId);

            if (hospital == null)
            {
                throw new InvalidOperationException("No hospital found with this id!");
            }

            hospital.Name = input.Name;

            await this.db.SaveChangesAsync();
        }

        public async Task<EditHospitalViewModel> GetHospitalEdit(string id)
        {
            var hospitalDb = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == id);

            if (hospitalDb == null)
            {
                throw new ArgumentNullException("hospitalId", "No hospital found with this id!");
            }

            return new EditHospitalViewModel()
            {
               HospitalId = hospitalDb.HospitalId,
               Name = hospitalDb.Name,
            };
        }
    }
}

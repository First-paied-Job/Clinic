using Clinic.Data;
using Clinic.Services.Data.Contracts;
using System.Linq;
using System;
using System.Threading.Tasks;
using Clinic.Common;
using System.Collections.Generic;
using Clinic.Web.ViewModels.Administration.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Services.Data
{
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
    }
}

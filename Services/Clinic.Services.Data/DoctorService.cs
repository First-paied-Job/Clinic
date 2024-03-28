using Clinic.Data;
using Clinic.Services.Data.Contracts;
using System.Linq;
using System;
using System.Threading.Tasks;
using Clinic.Common;
using System.Collections.Generic;
using Clinic.Web.ViewModels.Administration.Dashboard;
using Microsoft.EntityFrameworkCore;
using Clinic.Web.ViewModels.Doctor.Dashboard;

namespace Clinic.Services.Data
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext db;

        public DoctorService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddPatientRoleToUser(string email)
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

            var doctorRole = this.db.Roles.FirstOrDefault(r => r.Name == GlobalConstants.ClinicPatientRoleName);

            if (doctorRole == null)
            {
                throw new InvalidOperationException($"There is no role with the name \"{GlobalConstants.ClinicPatientRoleName}\"!");
            }

            this.db.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = doctorRole.Id,
                UserId = user.Id,
            });

            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<PatientViewModel>> GetPatientsAsync()
        {
            var viewModel = new List<PatientViewModel>();

            var patientRole = await this.db.Roles.FirstOrDefaultAsync(r => r.Name == GlobalConstants.ClinicPatientRoleName);
            var patientIds = await this.db.UserRoles.Where(ur => ur.RoleId == patientRole.Id).ToListAsync();
            foreach (var pair in patientIds)
            {
                var patient = await this.db.Users.Where(u => u.Id == pair.UserId)
                    .Select(u => new PatientViewModel
                    {
                        PatientId = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                    })
                    .FirstOrDefaultAsync();

                viewModel.Add(patient);
            }

            return viewModel;
        }

        public async Task RemovePatientRoleFromUser(string userId)
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

            var patientRole = this.db.Roles.FirstOrDefault(r => r.Name == GlobalConstants.ClinicPatientRoleName);

            if (patientRole == null)
            {
                throw new InvalidOperationException($"There is no role with the name \"{GlobalConstants.ClinicPatientRoleName}\"!");
            }

            this.db.UserRoles.Remove(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = patientRole.Id,
                UserId = user.Id,
            });

            await this.db.SaveChangesAsync();
        }
    }
}

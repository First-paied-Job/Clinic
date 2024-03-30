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
    using Clinic.Web.ViewModels.Administration.Dashboard.Clinic;
    using Clinic.Web.ViewModels.Administration.Dashboard.Hospital;
    using Microsoft.EntityFrameworkCore;

    public class AdministratorService : IAdministratorService
    {
        private readonly ApplicationDbContext db;

        public AdministratorService(ApplicationDbContext db)
        {
            this.db = db;
        }

        // Doctor
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

            var userRole = new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
            {
                RoleId = doctorRole.Id,
                UserId = user.Id,
            };

            user.Roles.Add(userRole);

            this.db.Users.Update(user);
            await this.db.UserRoles.AddAsync(userRole);
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

        // Hospital
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

        public async Task RemoveHospitalAsync(string hospitalId)
        {
            var hospital = await this.db.Hospitals.OrderBy(h => h.Name).Include(h => h.Clincs).FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

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

        // Clinic
        public async Task AddClinicToHospitalAsync(ClinicInputModel input)
        {
            var employer = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == input.HospitalEmployerId);
            if (employer == null)
            {
                throw new ArgumentException("No Hospital with the given name exists!");
            }

            var check = await this.db.Clincs.FirstOrDefaultAsync(c => c.Name == input.Name);
            if (check != null)
            {
                throw new ArgumentException("This clinic already exists!");
            }

            var clinic = new Clinic
            {
                Name = input.Name,
                HospitalEmployerId = employer.HospitalId,
                HospitalEmployer = employer,
            };

            await this.db.Clincs.AddAsync(clinic);
            employer.Clincs.Add(clinic);

            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<ClinicViewModel>> GetClinicsInHospitalAsync(string hospitalId)
        {
            var hospital = await this.db.Hospitals
                .FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            var doctorRole = await this.db.Roles.FirstOrDefaultAsync(r => r.Name == GlobalConstants.ClinicDoctortRoleName);
            var doctors = await (
                from userroles in this.db.UserRoles
                join user in this.db.Users on userroles.UserId equals user.Id
                where userroles.RoleId == doctorRole.Id
                select new DoctorDTO
                {
                    DoctorId = user.Id,
                    Name = user.Email,
                }).ToListAsync();

            var clinics = hospital.Clincs
                .Select(c => new ClinicViewModel
                {
                    ClinicId = c.ClinicId,
                    Name = c.Name,
                    Doctors = doctors,
                }).ToList();
            return clinics;
        }

        public async Task RemoveClinicAsync(string clinicId)
        {
            var clinic = await this.db.Clincs.FirstOrDefaultAsync(h => h.ClinicId == clinicId);
            if (clinic == null)
            {
                throw new ArgumentException("This hospital does not exist!");
            }

            var hospitalEmployer = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == clinic.HospitalEmployerId);

            hospitalEmployer.Clincs.Remove(clinic);
            this.db.Clincs.Remove(clinic);
            await this.db.SaveChangesAsync();
        }

        public async Task EditClinicAsync(EditClinicInputModel input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "The given input is null!");
            }

            var clinic = await this.db.Clincs.FirstOrDefaultAsync(c => c.ClinicId == input.ClinicId);

            if (clinic == null)
            {
                throw new InvalidOperationException("No clinic found with this id!");
            }

            clinic.Name = input.Name;

            await this.db.SaveChangesAsync();
        }

        public async Task<EditClinicViewModel> GetClinicEdit(string id)
        {
            var clinicDb = await this.db.Clincs.FirstOrDefaultAsync(c => c.ClinicId == id);

            if (clinicDb == null)
            {
                throw new ArgumentNullException("clinicId", "No clinic found with this id!");
            }

            return new EditClinicViewModel
            {
                ClinicId = clinicDb.ClinicId,
                Name = clinicDb.Name,
            };
        }
    }
}

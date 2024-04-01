namespace Clinic.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
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
        private readonly IDoctorService doctorService;

        public AdministratorService(ApplicationDbContext db, IDoctorService doctorService)
        {
            this.db = db;
            this.doctorService = doctorService;
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

            await this.RemoveDoctorFromClinic(userId);

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
            var hospital = await this.db.Hospitals.Include(h => h.Clincs).ThenInclude(c => c.People).Include(c => c.Clincs).ThenInclude(c => c.Diagnostics).FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            if (hospital == null)
            {
                throw new ArgumentException("This hospital does not exist!");
            }

            foreach (var clinic in hospital.Clincs)
            {
                await this.CheckPatients(clinic.ClinicId);
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
                .Include(h => h.Clincs)
                .ThenInclude(c => c.People)
                .FirstOrDefaultAsync(h => h.HospitalId == hospitalId);

            var clinics = hospital.Clincs
                .Select(c => new ClinicViewModel
                {
                    HospitalId = hospital.HospitalId,
                    ClinicId = c.ClinicId,
                    Name = c.Name,
                    Doctors = c.People
                        .Where(p => p.ClinicId == c.ClinicId)
                        .Select(p => new DoctorDTO
                        {
                            ClinicId = p.ClinicId,
                            DoctorId = p.Id,
                            Name = p.Email,
                        })
                        .ToList(),
                }).ToList();
            return clinics;
        }

        public async Task RemoveClinicAsync(string clinicId)
        {
            var clinic = await this.db.Clincs.Include(c => c.People).Include(c => c.Diagnostics).FirstOrDefaultAsync(h => h.ClinicId == clinicId);
            if (clinic == null)
            {
                throw new ArgumentException("This hospital does not exist!");
            }

            var hospitalEmployer = await this.db.Hospitals.FirstOrDefaultAsync(h => h.HospitalId == clinic.HospitalEmployerId);

            hospitalEmployer.Clincs.Remove(clinic);

            await this.CheckPatients(clinic.ClinicId);

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
                HospitalId = clinicDb.HospitalEmployerId,
            };
        }

        public async Task AddDoctorToClinic(AddDoctorToClinicInput input)
        {
            var doctor = await this.db.Users.FirstOrDefaultAsync(u => u.Email == input.Email);

            if (doctor == null)
            {
                throw new ArgumentException("The given person is not in our system.");
            }

            var doctorRole = await this.db.Roles.FirstOrDefaultAsync(r => r.Name == GlobalConstants.ClinicDoctortRoleName);

            var checkIfDoctor = await this.db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == doctor.Id && ur.RoleId == doctorRole.Id);

            if (checkIfDoctor == null)
            {
                throw new ArgumentException("The given person is not a doctor.");
            }

            if (doctor.ClinicId != null)
            {
                throw new ArgumentException("The given doctor already is in another clinic.");
            }

            var clinic = await this.db.Clincs.FirstOrDefaultAsync(c => c.ClinicId == input.ClinicId);

            if (clinic == null)
            {
                throw new ArgumentException("The given clinic does not exist.");
            }

            clinic.People.Add(doctor);
            doctor.Clinic = clinic;
            doctor.ClinicId = clinic.ClinicId;
            await this.db.SaveChangesAsync();
        }

        public async Task RemoveDoctorFromClinic(string doctorId)
        {
            var doctor = await this.db.Users.FirstOrDefaultAsync(u => u.Id == doctorId);

            if (doctor == null)
            {
                throw new ArgumentException("This doctor does not exist!");
            }

            var clinic = await this.db.Clincs
                .Include(c => c.People)
                .FirstOrDefaultAsync(c => c.ClinicId == doctor.ClinicId);

            if (clinic == null)
            {
                throw new ArgumentException("This doctor is not in this clinic!");
            }

            doctor.ClinicId = null;
            doctor.Clinic = null;
            clinic.People.Remove(doctor);
            await this.db.SaveChangesAsync();
        }

        public async Task CheckPatients(string clinicId)
        {
            var patientClinics = await this.db.PatientClinics.Where(pc => pc.ClinicId == clinicId).ToListAsync();

            this.db.PatientClinics.RemoveRange(patientClinics);
            await this.db.SaveChangesAsync();

            foreach (var pair in patientClinics)
            {
                await this.doctorService.RemovePatientRolesAsync(pair.PatientId);
            }
        }
    }
}

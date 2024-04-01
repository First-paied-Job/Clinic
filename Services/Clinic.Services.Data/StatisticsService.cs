namespace Clinic.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Clinic.Common;
    using Clinic.Data;
    using Clinic.Data.Models.Hospital;
    using Clinic.Services.Data.Contracts;
    using Clinic.Web.ViewModels.Administration.Statistics;
    using Microsoft.EntityFrameworkCore;

    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext db;

        public StatisticsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<ICollection<StatisticClinicViewModel>> GetAllClinics()
        {
            var hospitals = await this.db.Hospitals
                .Include(h => h.Clincs)
                .ToListAsync();

            var viewModel = new List<StatisticClinicViewModel>();

            foreach (var hospital in hospitals)
            {
                var clinics = hospital.Clincs.Select(c => new StatisticClinicViewModel()
                {
                    HospitalName = hospital.Name,
                    ClinicId = c.ClinicId,
                    Name = c.Name,
                })
                .ToList();

                viewModel.AddRange(clinics);
            }

            return viewModel;
        }

        public async Task<ICollection<StatisticDoctorViewModel>> GetDoctorsInClinic(string clinicId)
        {
            return await this.db.Users.Where(u => u.ClinicId == clinicId).Select(u => new StatisticDoctorViewModel()
            {
                DoctorId = u.Id,
                DoctorEmail = u.Email,
            }).ToListAsync();
        }

        public async Task<ICollection<StatisticPatientViewModel>> GetPatientsInClinic(string clinicId)
        {
            var viewModel = new List<StatisticPatientViewModel>();

            var pc = await this.db.PatientClinics.Where(pc => pc.ClinicId == clinicId).ToListAsync();

            foreach (var patientId in pc)
            {
                var patient = await this.db.Users.FirstOrDefaultAsync(u => u.Id == patientId.PatientId);

                viewModel.Add(new StatisticPatientViewModel()
                {
                    PatientId = patient.Id,
                    PatientEmail = patient.Email,
                });
            }

            return viewModel;
        }

        public async Task<ICollection<StatisticDiagnosticViewModel>> GetAllDiagnostics()
        {
            var viewModel = new List<StatisticDiagnosticViewModel>();

            var diagnostics = await this.db.Diagnostics
                .OrderBy(d => d.CreatorId)
                .ToListAsync();

            if (diagnostics.Count <= 0)
            {
                return viewModel;
            }

            var creator = await this.db.Users.FirstOrDefaultAsync(u => u.Id == diagnostics[0].CreatorId);

            foreach (var diagnostic in diagnostics)
            {
                if (diagnostic.CreatorId == creator.Id)
                {
                    viewModel.Add(new StatisticDiagnosticViewModel()
                    {
                        CreatorId = diagnostic.CreatorId,
                        CreatorName = creator.Email,
                        DiagnosticId = diagnostic.DiagnosticsId,
                        Name = diagnostic.Name,
                        Description = diagnostic.Description,
                    });
                }
                else
                {
                    creator = await this.db.Users.FirstOrDefaultAsync(u => u.Id == diagnostic.CreatorId);
                    viewModel.Add(new StatisticDiagnosticViewModel()
                    {
                        CreatorId = diagnostic.CreatorId,
                        CreatorName = creator.Email,
                        DiagnosticId = diagnostic.DiagnosticsId,
                        Name = diagnostic.Name,
                        Description = diagnostic.Description,
                    });
                }
            }

            return viewModel;
        }

        public async Task<ICollection<StatisticDiagnosticPatientViewModel>> GetDiagnosticsForPatient(string patientId)
        {
            var viewModel = new List<StatisticDiagnosticPatientViewModel>();

            var pd = await this.db.PatientDiagnostics.Where(pd => pd.PatientId == patientId).ToListAsync();

            foreach (var diagnosticsId in pd)
            {
                var diagnostic = await this.db.Diagnostics.FirstOrDefaultAsync(d => d.DiagnosticsId == diagnosticsId.DiagnosticsId);
                var patient = await this.db.Users.FirstOrDefaultAsync(u => u.Id == diagnosticsId.PatientId);

                viewModel.Add(new StatisticDiagnosticPatientViewModel()
                {
                    PatientId = patient.Id,
                    PatientName = patient.Email,
                    Name = diagnostic.Name,
                    Description = diagnostic.Description,
                    DiagnosticId = diagnostic.DiagnosticsId,
                });
            }

            return viewModel;
        }

        public async Task<ICollection<StatisticDiagnosticViewModel>> GetDiagnosticsFromDoctor(string doctorId)
        {
            var doctor = await this.db.Users.FirstOrDefaultAsync(u => u.Id == doctorId);

            return await this.db.Diagnostics.Where(d => d.CreatorId == doctorId).Select(d => new StatisticDiagnosticViewModel()
            {
                CreatorName = doctor.Email,
                CreatorId = d.CreatorId,
                Name = d.Name,
                Description = d.Description,
                DiagnosticId = d.DiagnosticsId,
            }).ToListAsync();
        }

        public async Task<ICollection<StatisticPatientViewModel>> GetPatientsWithDiagnostics()
        {
            var viewModel = new List<StatisticPatientViewModel>();

            var pds = await this.db.PatientDiagnostics.OrderBy(pd => pd.PatientId).ToListAsync();

            if (pds.Count <= 0)
            {
                return viewModel;
            }

            var patient = await this.db.Users.FirstOrDefaultAsync(u => u.Id == pds.First().PatientId);
            viewModel.Add(new StatisticPatientViewModel()
            {
                PatientId = patient.Id,
                PatientEmail = patient.Email,
            });

            foreach (var pd in pds)
            {
                if (pd.PatientId != patient.Id)
                {
                    patient = await this.db.Users.FirstOrDefaultAsync(u => u.Id == pd.PatientId);
                    viewModel.Add(new StatisticPatientViewModel()
                    {
                        PatientId = patient.Id,
                        PatientEmail = patient.Email,
                    });
                }
            }

            return viewModel;
        }
    }
}

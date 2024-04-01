// ReSharper disable VirtualMemberCallInConstructor
namespace Clinic.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Clinic.Data.Common.Models;
    using Clinic.Data.Models.Hospital;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.PatientDiagnostics = new HashSet<PatientDiagnostics>();
            this.PatientClinics = new HashSet<PatientClinics>();
        }

        public string Name { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual Clinic Clinic { get; set; }

        public string ClinicId { get; set; }

        public virtual ICollection<PatientClinics> PatientClinics { get; set; }

        public virtual ICollection<PatientDiagnostics> PatientDiagnostics { get; set; }
    }
}

namespace Clinic.Data.Models.Hospital
{
    using System;
    using System.Collections.Generic;

    public class Clinic
    {
        public Clinic()
        {
            this.ClinicId = Guid.NewGuid().ToString();
            this.Services = new HashSet<Service>();
            this.People = new HashSet<ApplicationUser>();
            this.Diagnostics = new HashSet<Diagnostics>();
        }

        public string ClinicId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> People { get; set; }

        public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<Diagnostics> Diagnostics { get; set; }

        public virtual Hospital HospitalEmployer { get; set; }

        public string HospitalEmployerId { get; set; }
    }
}

namespace Clinic.Data.Models.Hospital
{
    using System;
    using System.Collections.Generic;

    public class Hospital
    {
        public Hospital()
        {
            this.HospitalId = Guid.NewGuid().ToString();
            this.Clincs = new HashSet<Clinic>();
        }

        public string HospitalId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Clinic> Clincs { get; set; }
    }
}

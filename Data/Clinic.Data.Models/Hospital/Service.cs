namespace Clinic.Data.Models.Hospital
{
    using System;

    public class Service
    {
        public Service()
        {
            this.ServiceId = Guid.NewGuid().ToString();
        }

        public string ServiceId { get; set; }

        public virtual Clinic Clinic { get; set; }

        public string ClinicId { get; set; }
    }
}

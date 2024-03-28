namespace Clinic.Data.Models.Hospital
{
    using System;

    public class Diagnostics
    {
        public Diagnostics()
        {
            this.DiagnosticsId = Guid.NewGuid().ToString();
        }

        public string DiagnosticsId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Clinic Clinic { get; set; }

        public string ClinicId { get; set; }
    }
}

namespace Clinic.Data.Models.Hospital
{
    using System;
    using System.Collections.Generic;

    public class Diagnostics
    {
        public Diagnostics()
        {
            this.DiagnosticsId = Guid.NewGuid().ToString();
            this.PatientDiagnostics = new HashSet<PatientDiagnostics>();
        }

        public string DiagnosticsId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual Clinic Clinic { get; set; }

        public string ClinicId { get; set; }

        public virtual ICollection<PatientDiagnostics> PatientDiagnostics { get; set; }

        public string CreatorId { get; set; }
    }
}

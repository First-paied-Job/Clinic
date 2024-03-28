using System;

namespace Clinic.Data.Models.Hospital
{
    public class Patient : ApplicationUser
    {
        public Patient()
        {
            this.PatientId = Guid.NewGuid().ToString();
        }

        public string PatientId { get; set; }


    }
}

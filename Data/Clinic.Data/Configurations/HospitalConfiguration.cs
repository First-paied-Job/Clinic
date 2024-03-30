namespace Clinic.Data.Configurations
{
    using Clinic.Data.Models.Hospital;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
    {
        public void Configure(EntityTypeBuilder<Hospital> builder)
        {
            builder
                .HasMany(e => e.Clincs)
                .WithOne(e => e.HospitalEmployer)
                .HasForeignKey(e => e.HospitalEmployerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

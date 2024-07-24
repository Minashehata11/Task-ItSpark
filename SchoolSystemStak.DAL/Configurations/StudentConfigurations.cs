using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolSystemStak.DAL.Models;
using SchoolSystemStak.DAL.Models.Identity;

namespace SchoolSystemStak.DAL.Configurations
{
    public class StudentConfigurations : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasOne(s=>s.Class).WithMany().HasForeignKey(s => s.ClassId);
            builder.HasOne(s => s.ApplicationUser).WithOne().HasForeignKey<Student>(s => s.ApplicationUserId).OnDelete(DeleteBehavior.SetNull); 
            builder.Property(o => o.Gender).HasConversion(status => status.ToString(), status => ((Gender)Enum.Parse(typeof(Gender), status)));
            builder.Property(o => o.City).HasConversion(status => status.ToString(), status => ((City)Enum.Parse(typeof(City), status)));

        }
    }
}

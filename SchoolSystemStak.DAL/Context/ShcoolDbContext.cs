using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolSystemStak.DAL.Models;
using SchoolSystemStak.DAL.Models.Identity;
using System.Reflection;

namespace SchoolSystemStak.DAL.Context
{
    public class ShcoolDbContext : IdentityDbContext<ApplicationUser>
    {
        public ShcoolDbContext(DbContextOptions<ShcoolDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
    }
}

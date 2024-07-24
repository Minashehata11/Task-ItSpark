using Microsoft.AspNetCore.Identity;
using SchoolSystemStak.DAL.Models.Identity;

namespace SchoolSystemStak.DAL.SeedingData
{
    public static class SeedAdmin
    {
        public async static Task CreateUser(UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "Admin@Gmail.com",
                    UserName = "Mina",
                    PhoneNumber = "01225666903",
                };
                await userManager.CreateAsync(user, "P@ssW0rd");
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}

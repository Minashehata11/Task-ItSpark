using Microsoft.AspNetCore.Identity;

namespace SchoolSystemStak.DAL.SeedingData
{
    public static class SeedRoles
    {
        public async static Task AddRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                string[] rolesToAdd = { "Admin" };

                foreach (var role in rolesToAdd)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }
    }
}

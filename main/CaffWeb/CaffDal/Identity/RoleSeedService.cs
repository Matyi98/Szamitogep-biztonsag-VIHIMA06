using Microsoft.AspNetCore.Identity;

namespace CaffDal.Identity
{
    public class RoleSeedService
    {
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public RoleSeedService(RoleManager<IdentityRole<int>> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task SeedRoleAsync()
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = Roles.Admin });
            }
        }

    }
}

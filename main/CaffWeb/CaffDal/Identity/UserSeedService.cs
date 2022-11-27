using CaffDal.Entities;
using Microsoft.AspNetCore.Identity;

namespace CaffDal.Identity
{
    public class UserSeedService
    {
        private readonly UserManager<User> userManager;

        public UserSeedService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task SeedUserAsync()
        {
            if ((await userManager.GetUsersInRoleAsync(Roles.Admin)).Any())
                return;

            var user = new User {
                Email = "tboy.ultak@ciff.caff",
                UserName = "admin",
                CustomName = "Tigris",
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, "$Alma123");
            var roleResult = await userManager.AddToRoleAsync(user, Roles.Admin);

            /*if (!createResult.Succeeded || !roleResult.Succeeded) {
                throw new ApplicationException("Admin account clould not be created:" +
                        String.Join(", ", createResult.Errors.Concat(roleResult.Errors).Select(e => e.Description)));
            }*/
        }

    }
}

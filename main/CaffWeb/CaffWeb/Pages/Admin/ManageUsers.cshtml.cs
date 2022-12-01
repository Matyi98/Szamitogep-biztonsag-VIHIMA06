using CaffDal.Entities;
using CaffDal.Identity;
using CaffDal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaffWeb.Pages.Admin
{
    public class ManageUsersModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<ManageUsersModel> logger;
        private readonly ICaffFacade caffFacade;

        public ManageUsersModel(UserManager<User> userManager, ILogger<ManageUsersModel> logger, ICaffFacade caffFacade)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.caffFacade = caffFacade;
        }

        public class UserInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public bool IsAdmin { get; set; }
            public bool IsActivated { get; set; }

            public string Roles
            {
                get
                {
                    string ret = "";
                    if (IsActivated)
                    {
                        ret += "User";
                        if (IsAdmin)
                            ret += ", ";
                    }

                    if (IsAdmin)
                    {
                        ret += "Admin";
                    }

                    return ret;
                }
            }
        }

        [BindProperty]
        public int EditedId { get; set; }

        public IList<UserInfo> AllUsers { get; set; }
        public async Task OnGetAsync()
        {
            await LoadModel();
        }

        public async Task LoadModel()
        {
            AllUsers = new List<UserInfo>();
            foreach (var item in userManager.Users.ToList())
            {
                var userInfo = new UserInfo();
                userInfo.IsAdmin = await userManager.IsInRoleAsync(item, Roles.Admin);
                userInfo.IsActivated = item.EmailConfirmed;
                userInfo.Id = item.Id;
                userInfo.Name = item.UserName;
                userInfo.Email = item.Email;
                AllUsers.Add(userInfo);
            }
        }

        public async Task<IActionResult> OnPostPromoteAsync()
        {
            if (ModelState.IsValid)
            {
                var user = userManager.Users.Single(c => c.Id == EditedId);
                if (await userManager.IsInRoleAsync(user, Roles.Admin))
                {
                    await userManager.RemoveFromRoleAsync(user, Roles.Admin);
                    logger.LogInformation("An admin has removed Moderator role from: {NominatedId} at {Time}",
                        user.Id, DateTime.UtcNow);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                    logger.LogInformation("An admin has granted Moderator role to: {NominatedId} at {Time}",
                        user.Id, DateTime.UtcNow);
                }
                return new RedirectToPageResult("/Admin/ManageUsers");
            }
            await LoadModel();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (ModelState.IsValid)
            {
                await caffFacade.DeleteUser(EditedId);
                return new RedirectToPageResult("/Admin/ManageUsers");
            }
            await LoadModel();
            return Page();
        }

    }
}

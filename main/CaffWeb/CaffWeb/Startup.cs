using CaffDal;
using CaffDal.Entities;
using CaffDal.Identity;
using CaffDal.ParserWrapper;
using CaffDal.Services;
using CaffWeb.Email;
using CaffWeb.Mock;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace CaffWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddDbContext<CaffDbContext>(
                o => o.UseSqlServer(
                    Configuration.GetConnectionString(nameof(CaffDbContext)), b => b.MigrationsAssembly("CaffDal")
                    ));

            services.AddIdentity<User, IdentityRole<int>>(options => {
                options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<CaffDbContext>()
                .AddDefaultTokenProviders();


            services.AddAuthorization(options => {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Roles.Admin));
                options.AddPolicy("RequireAuthenticated", policy => policy.RequireAuthenticatedUser());
            });

            services.Configure<CookiePolicyOptions>(options => {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            services.ConfigureApplicationCookie(options => {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
            });

            services.Configure<CaffParserConfig>(Configuration.GetSection("CaffParserConfig"));

            services.Configure<MailSettings>(
               Configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<RoleSeedService>()
                    .AddScoped<UserSeedService>();

            services.AddScoped<ICaffFacade, CaffFacadeImpl>();

            services.AddControllers();
            services.AddRazorPages(options => {
                options.Conventions.AuthorizeFolder("/Admin", "RequireAdminRole");
                options.Conventions.AuthorizePage("/Upload", "RequireAuthenticated");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/_Basic/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}

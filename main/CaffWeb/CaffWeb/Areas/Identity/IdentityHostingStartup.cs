using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CaffWeb.Areas.Identity.IdentityHostingStartup))]
namespace CaffWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(NetCoreApp.Areas.Identity.IdentityHostingStartup))]
namespace NetCoreApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}

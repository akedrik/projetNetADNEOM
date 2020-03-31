using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NetCoreApp.Core.Interfaces.Logging;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Core.Interfaces.Services;
using NetCoreApp.Core.Services;
using NetCoreApp.Infrastructure.Data;
using NetCoreApp.Infrastructure.Data.Repositories;
using NetCoreApp.Infrastructure.Logging;
using Serilog;
using System;

namespace NetCoreApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // use in-memory database
            services.AddDbContext<NetCoreAppContext>(c =>
                c.UseInMemoryDatabase("NetCoreApp"));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<ICategorieService, CategorieService>();
            services.AddScoped<ICategorieRepository, CategorieRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.AddHttpClient();
            services.AddControllers();
            services.AddRazorPages(
                options =>
                {
                    options.Conventions.AddPageRoute("/Home/Index", "");
                })
                .AddSessionStateTempDataProvider();

            services.AddSession();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCoreApp API", Version = "v1" });
            });
            services.AddLogging(configure => configure.AddSerilog());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                app.UseHsts();
                
                Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"Logs/log-error-{DateTime.Now.ToShortDateString().Replace('/', '-')}.log",
                Serilog.Events.LogEventLevel.Error)
                .CreateLogger();
            }

            app.UseStatusCodePagesWithRedirects("/Shared/StatusCode?code={0}");

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreApp API V1");
            });
        }
    }
}

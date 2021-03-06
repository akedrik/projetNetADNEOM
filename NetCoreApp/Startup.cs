using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.EmailSender;
using NetCoreApp.Core.Interfaces.Logging;
using NetCoreApp.Core.Interfaces.Repositories;
using NetCoreApp.Core.Interfaces.Services;
using NetCoreApp.Core.Interfaces.Services.Pages;
using NetCoreApp.Core.Services;
using NetCoreApp.Infrastructure.Data;
using NetCoreApp.Infrastructure.Data.Repositories;
using NetCoreApp.Infrastructure.EmailSender;
using NetCoreApp.Infrastructure.Identity;
using NetCoreApp.Infrastructure.Logging;
using NetCoreApp.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace NetCoreApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // use in-memory database
            services.AddDbContext<NetCoreAppContext>(c =>
                 c.UseInMemoryDatabase("NetCoreApp"));

            services.AddDbContext<AppIdentityDbContext>(c =>
                 c.UseInMemoryDatabase("NetCoreAppAppIdentity"));

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            //Use SQL SERVER
            services.AddDbContext<NetCoreAppContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("NetCoreAppConnection")));

            services.AddDbContext<AppIdentityDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("NetCoreAppConnection")));

            ConfigureServices(services);
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCookieSettings(services);
            CreateIdentityIfNotCreated(services);

            services.AddScoped<ICategorieService, CategorieService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICategorieRepository, CategorieRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICategoriePageService, CategoriePageService>();
            services.AddScoped<IArticlePageService, ArticlePageService>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddHttpClient();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddHttpContextAccessor();

            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Resources";
            });

            services.AddRazorPages(
                options =>
                {
                    options.Conventions.AddPageRoute("/Home/Index", "");
                })
                .AddSessionStateTempDataProvider()
               // .AddViewLocalization(opts => { opts.ResourcesPath = "Resources"; })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo> {
                    new CultureInfo("en"),
                    new CultureInfo("fr")
                  };

                opts.DefaultRequestCulture = new RequestCulture("fr");
                // Formatting numbers, dates, etc.
                opts.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                opts.SupportedUICultures = supportedCultures;
            });
            services.AddAuthentication()
               .AddFacebook(facebookOptions =>
               {
                   facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                   facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                   facebookOptions.Events.OnRemoteFailure = ftx =>
                   {
                       ftx.Response.Redirect("/");
                       ftx.HandleResponse();
                       return Task.FromResult(0);
                   };
               });
            services.AddSession();
            services.AddServerSideBlazor();

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
                app.UseDatabaseErrorPage();
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

            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCoreApp API V1");
            });
        }

        private static void CreateIdentityIfNotCreated(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var existingUserManager = scope.ServiceProvider
                    .GetService<UserManager<ApplicationUser>>();
                if (existingUserManager == null)
                {
                    services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddDefaultUI()
                        .AddEntityFrameworkStores<AppIdentityDbContext>()
                                        .AddDefaultTokenProviders();
                }
            }
        }

        private static void ConfigureCookieSettings(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                };
            });
        }
    }
}

using FirmwareServer.Breadcrumb;
using FirmwareServer.EntityLayer;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace FirmwareServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            IsPasswordProtected = !string.IsNullOrEmpty(configuration.GetValue("FirmwareServer:Password", string.Empty));
        }

        public IConfiguration Configuration { get; }

        public bool IsPasswordProtected { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Configuration.GetValue<bool>("FirmwareServer:IsRunningInContainer"))
            {
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Configuration.GetValue<string>("FirmwareServer:AppData"), "keys")));
            }

            services.AddSingleton(Configuration);

            services.Configure<FirmwareServerConfiguration>(Configuration.GetSection("FirmwareServer"));

            var dataSource = Path.GetFullPath(Path.Combine(Configuration.GetValue<string>("FirmwareServer:AppData"), "FirmwareServer.db"));

            Console.WriteLine($"Data source={dataSource}");
            services.AddDbContext<Database>(options => options.UseSqlite($"Data source={dataSource}"));

            if (IsPasswordProtected)
            {
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            }

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    if (IsPasswordProtected)
                    {
                        options.Conventions.AuthorizeFolder("/");
                        options.Conventions.AllowAnonymousToFolder("/Account");
                        options.Conventions.AllowAnonymousToPage("/StatusCode");
                    }

                    options.Conventions.ConfigureFilter(new BreadcrumbPageFilter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHangfire(x => x.UseMemoryStorage());

            //services.AddScoped<IDatabaseServices, DatabaseServices>();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
            , IHostingEnvironment env
            , IServiceProvider serviceProvider)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                ForwardLimit = null,
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

            app.UseStaticFiles();

            if (IsPasswordProtected)
            {
                app.UseAuthentication();
            }

            // Hangfire
            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                Activator = new HangfireActivator(serviceProvider),
                WorkerCount = 1,
            });
            app.UseHangfireDashboard();

            app.UseMvc();

            //RecurringJob.AddOrUpdate<IDatabaseServices>(x => x.Vacuum(), Cron.Weekly);
            //RecurringJob.AddOrUpdate<IDatabaseServices>(x => x.Backup(), Cron.Daily);
        }
    }
}

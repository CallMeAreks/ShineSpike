using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using ShineSpike.Services;
using ShineSpike.Services.Backup;
using ShineSpike.Utils;
using WebMarkupMin.AspNetCore3;
using WebMarkupMin.Core;

namespace ShineSpike
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IPostService, FilePostService>();
            services.AddSingleton<ILoginService, LoginService>();
            services.AddScoped<IFormFileUploadService, FormFileUploadService>();
            services.AddScoped<IBackupService, ZipFileBackupService>();
            services.Configure<SiteSettings>(Configuration.GetSection("site"));
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            // Output caching
            services.AddMvc(opts => opts.CacheProfiles.Add(Constants.CacheProfile, new CacheProfile { Duration = 3600 }));

            // Markup minimization
            services.AddWebMarkupMin(opts => {
                opts.AllowMinificationInDevelopmentEnvironment = true;
                opts.DisablePoweredByHttpHeaders = true;
            })
            .AddHtmlMinification(opts => {
                opts.MinificationSettings.RemoveOptionalEndTags = false;
                opts.MinificationSettings.WhitespaceMinificationMode = WhitespaceMinificationMode.Safe;
            });

            // Cookie authentication.
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(opts => {
                        opts.LoginPath = "/account/login";
                        opts.LogoutPath = "/account/logout";
                    });
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Markup minimization
            app.UseWebMarkupMin();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

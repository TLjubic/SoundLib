using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace SoundLib
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<SoundLibDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SoundLibDbContext")));

            services.AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<SoundLibDbContext>();

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "843426356068-c9ef5beadgm5v3p2biv5upergq2r0sn7.apps.googleusercontent.com";
                googleOptions.ClientSecret = "k7Yu7vt22aX_8PYRaJAp-ele";
                googleOptions.SaveTokens = false;

            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseAuthentication();

            var supportedCultures = new[]
            {
                new CultureInfo("hr"),
                new CultureInfo("en-US"),
                new CultureInfo("hu")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("hr"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "album",
                    template: "Albums",
                    defaults: new { controller = "Album", action = "Index" });

                routes.MapRoute(
                    name: "album-details",
                    template: "AlbumDetails/{id?}",
                    defaults: new { controller = "Album", action = "Details" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Soundlib}/{action=Index}/{id?}");
            });
        }
    }
}

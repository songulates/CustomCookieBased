using CustomCookieCore.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCookieCore
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
           
            services.AddDbContext<CookieContext>(opt =>
            {
                opt.UseSqlServer("server=DESKTOP-1TE9E4I;database=CookieDb;integrated security=true;");
            });
            //COOKÝE CONFIGURATION
            //delegetion opt ile
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   .AddCookie(opt => {
       opt.Cookie.Name = "CustomCookie";
       opt.Cookie.HttpOnly = true;
       //http only javascrptle ilgili cookinin çekilmesini engelliyordu.
       //samesite ile cookie paylaþýma kapanýyordu.
       opt.Cookie.SameSite = SameSiteMode.Strict;
       opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
       //cokieyi 10gün boyunca sakla
       opt.ExpireTimeSpan = TimeSpan.FromDays(10);
       opt.LoginPath = new PathString("/Home/SignIn");
       opt.LogoutPath = new PathString("/Home/LogOut");
       opt.AccessDeniedPath = new PathString("/Home/AccesDenied");
   });
            services.AddControllersWithViews();
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
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //sonra kullanýcý giriþ ve çýkýþlarýný yapalým

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=SignIn}/{id?}");
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using AldeiaParental.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AldeiaParental.Models;
using AldeiaParental.Areas.Identity;

namespace AldeiaParental
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<AldeiaParentalUser, AldeiaParentalRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityErrorDescriber>(); ;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministradorPolicy",
                                    policy => policy.RequireRole("Administrador")
                                    );
                options.AddPolicy("CuidadorPolicy",
                                    policy => policy.RequireRole("Cuidador")
                                    );
                options.AddPolicy("ClientePolicy",
                                    policy => policy.RequireRole("Cliente")
                                    );
            }
            );

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Regions", "AdministradorPolicy");
                options.Conventions.AuthorizeFolder("/Services", "AdministradorPolicy");
                options.Conventions.AuthorizeFolder("/Users", "AdministradorPolicy");
                options.Conventions.AuthorizeFolder("/CheckDocuments", "AdministradorPolicy");
                options.Conventions.AuthorizeFolder("/ListServiceLocations", "AdministradorPolicy");
                options.Conventions.AuthorizeFolder("/ServiceLocations", "CuidadorPolicy");
                options.Conventions.AuthorizeFolder("/CaregiverServices", "CuidadorPolicy");
                options.Conventions.AuthorizeFolder("/FindCaregivers", "ClientePolicy");
                options.Conventions.AuthorizeFolder("/CustomerServices", "ClientePolicy");
            });

            services.AddAuthentication().AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                            Configuration.GetSection("Authentication:Google");
                            options.ClientId = googleAuthNSection["ClientId"];
                            options.ClientSecret = googleAuthNSection["ClientSecret"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AldeiaParental.Data.ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            //ensure database is created
            context.Database.Migrate();
        }
    }
}

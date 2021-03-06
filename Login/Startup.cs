using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Login.AutherizationRequirements;
using Login.Controllers;
using Login.CustomPolicyProvider;
using Login.Models;
using Login.Transformer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Login
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
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth",config =>
                {
                    config.Cookie.Name = "Grandmas.Cookie";
                    config.LoginPath = "/Home/Authenticate";
                });
            services.AddAuthorization(config =>
            {
                // var defualtAuthBuilder = new AuthorizationPolicyBuilder();
                //var defualtAuthPolicy = defualtAuthBuilder
                //.RequireAuthenticatedUser()
                // .RequireClaim(ClaimTypes.DateOfBirth)
                // .Build();
                // config.DefaultPolicy = defualtAuthPolicy;


                //config.AddPolicy("Claim.DOB", PolicyBuilder =>
                //  {
                //      PolicyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                //  });

                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

                config.AddPolicy("Claim.DOB", PolicyBuilder =>
                {
                    PolicyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                });
            });
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            #region # Extras #
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, SecurityLevelHandler>();
            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();
            #endregion

            services.AddControllersWithViews(config =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                    .RequireAuthenticatedUser()
                    .Build();
            }
            );
            services.Configure<ReCAPTCHASettings>(Configuration.GetSection("GooglereCAPTCHA"));
            services.AddTransient<GooglereCaptchaService>();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            // who are you?
            app.UseAuthentication();

            // are you allowed?
            app.UseAuthorization();

            /* app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
             });*/
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

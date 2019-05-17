using ATL_WebUI.Data;
using ATL_WebUI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;

namespace ATL_WebUI
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
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = "cookie";
            //    options.DefaultChallengeScheme = "oidc";
            //})
            //.AddCookie("cookie")
            //.AddOpenIdConnect("oidc", options =>
            //{
            //    options.Authority = "http://localhost:5005";
            //    options.RequireHttpsMetadata = false;
            //    options.ClientId = "atl.web";
            //    options.SignInScheme = "cookie";
            //    options.ResponseType = "id_token";              
            //});

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("CloudConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddHttpClient("neo4j", c => c.BaseAddress = new Uri(Configuration["mapApiUrl"]))
                .AddTypedClient(c => RestService.For<INeo4jApiClient>(c));
            services.AddHttpClient("sql", c => c.BaseAddress = new Uri(Configuration["shipmentApiUrl"]))
                .AddTypedClient(c => RestService.For<IShipmentApiClient>(c)); ;

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseExceptionHandler("/404");
                //app.Use(async (ctx, next) =>
                //{
                //    await next();

                //    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                //    {
                //        //Re-execute the request so the user gets the error page
                //        string originalPath = ctx.Request.Path.Value;
                //        ctx.Items["originalPath"] = originalPath;
                //        ctx.Request.Path = "/404";
                //        await next();
                //    }
                //});
            }
            else
            {
                app.UseExceptionHandler("/404");
                app.Use(async (ctx, next) =>
                {
                    await next();

                    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                    {
                        //Re-execute the request so the user gets the error page
                        string originalPath = ctx.Request.Path.Value;
                        ctx.Items["originalPath"] = originalPath;
                        ctx.Request.Path = "/404";
                        await next();
                    }
                });
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

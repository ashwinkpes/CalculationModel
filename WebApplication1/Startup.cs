using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace WebApplication1
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

            var connection = @"Server=(localdb)\mssqllocaldb;Database=AbbRelcare;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<AbbRelCareContext>
                (options => options.UseSqlServer(connection));

            services.AddScoped(typeof(CalculationEngine));

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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var seedData = new SeedData
                {
                    Assets = JsonConvert.DeserializeObject<List<Asset>>(File.ReadAllText("seed" + Path.DirectorySeparatorChar + "Asset.json")),
                    SubSystems = JsonConvert.DeserializeObject<List<SubSystem>>(File.ReadAllText("seed" + Path.DirectorySeparatorChar + "Subsystem.json")),
                    Characteristics = JsonConvert.DeserializeObject<List<Characteristic>>(File.ReadAllText("seed" + Path.DirectorySeparatorChar + "Characteristic.json")),
                   
                };

                serviceScope.ServiceProvider.GetService<AbbRelCareContext>().EnsureSeedData(seedData);
            }
        }
    }
}

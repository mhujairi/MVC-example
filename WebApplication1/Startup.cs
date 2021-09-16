using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.IO;

using WebApplication1.Business;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Validation;

namespace WebApplication1
{
    public class Startup
    {
        private string partsDataFilePath = @"~/App_Data/partData.json";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IValidator<Part>, PartValidator>();

            services.AddSingleton<IRepository<string,Part>, FilePartsRepository>(
                serviceProvider =>
                {
                    return new FilePartsRepository(
                        partsDataFilePath,
                        serviceProvider.GetService<IValidator<Part>>()
                    );
                }
            );

            services.AddScoped<IPartsProvider, PartsProvider>();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Parts}/{action=Index}/{id?}");
            });
            partsDataFilePath = MapPath(env, partsDataFilePath);
        }


        private string MapPath(IWebHostEnvironment env,string partsDataFilePath)
        {
            if (Path.IsPathRooted(partsDataFilePath))
                return partsDataFilePath;

           return Path.GetFullPath(Path.Combine(env.WebRootPath, partsDataFilePath).Replace("~", ".."));
        }
    }
}

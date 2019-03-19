using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PeopleSearch
{
    public class Startup
    {
        private const string DefaultDbConnection = "PeopleSearch";
        private readonly PeopleSearchSettings settings;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            settings = BuildSettings(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(settings);
            services.AddSingleton(new DataGenerator());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var connection = Configuration
                .GetSection("ConnectionStrings")
                .GetValue<string>(DefaultDbConnection);
            services.AddDbContext<PersonContext>(options => options.UseSqlServer(connection));
            services.AddScoped<PersonRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private static PeopleSearchSettings BuildSettings(IConfiguration configuration)
        {
            var settings = new PeopleSearchSettings();
            configuration.Bind("PeopleSearchSettings", settings);
            return settings;
        }
    }
}

using System;
using MediaServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MediaServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Lese den Pfad zu den Mediadaten aus der Konfigurationsdatei
            var mediaDirectory = Configuration.GetSection("MediaSettings:MediaDirectory").Value;

            // Überprüfe, ob der Pfad nicht null ist
            if (string.IsNullOrEmpty(mediaDirectory))
            {
                throw new InvalidOperationException("Media directory path is not configured.");
            }

            services.AddSingleton(new MediaService(mediaDirectory));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
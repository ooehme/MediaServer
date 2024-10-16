using System;
using System.Threading;
using System.Threading.Tasks;
using MediaServer.Data;
using MediaServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MediaServer.Services
{
    public class IndexingService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public IndexingService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // This service doesn't do anything on startup
            await Task.CompletedTask;
        }

        public async Task StartIndexingAsync()
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var mediaService = scope.ServiceProvider.GetRequiredService<MediaService>();

            await mediaService.IndexMediaFilesAsync(dbContext);
            await mediaService.IndexVideoFilesAsync(dbContext);
        }
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq; // IMPORTANT
using WebApplication1.Models;

namespace AppTests1
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Supprimer l'ancien DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<IncidentsDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Ajouter un DbContext avec BD de test
                services.AddDbContext<IncidentsDbContext>(options =>
                    options.UseSqlServer( "Server=(localdb)\\mssqllocaldb;Database=IncidentDb_Test;Trusted_Connection=True;TrustServerCertificate=True;" )
                );

                // Construire le provider
                var sp = services.BuildServiceProvider();

                // Initialiser la BD
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<IncidentsDbContext>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class IncidentsDbContext : DbContext
    {
        public IncidentsDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Incident> Incidents { get; set; }

    }
}

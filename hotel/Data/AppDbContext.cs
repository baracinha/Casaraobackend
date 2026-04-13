using hotel.Controllers;
using Microsoft.EntityFrameworkCore;
using hotel.Models;

namespace hotel.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<utilizador> utilizador { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Manufactor.API.Data.Entites;

namespace Manufactor.API.Data
{
    public class ManufactureContext : DbContext
    {
        public ManufactureContext(DbContextOptions<ManufactureContext> options) : base(options)
        {

        }

        public DbSet<Products> Products { get; set; }
    }
}

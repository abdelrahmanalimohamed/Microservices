using Microsoft.EntityFrameworkCore;
using SalesBusiness.API.Data.Entities;

namespace SalesBusiness.API.Data
{
    public class SalesBusinessContext : DbContext
    {
        public SalesBusinessContext(DbContextOptions<SalesBusinessContext> options) : base(options)
        {

        }

        public DbSet<Orders> Orders { get; set; }

        public DbSet<Products> Products { get; set; }
    }
}

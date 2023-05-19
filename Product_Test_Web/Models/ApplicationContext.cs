using Microsoft.EntityFrameworkCore;

namespace Product_Test_Web.Models
{
    public class ApplicationContext : DbContext
    {
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Product_Test1_DB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                
        public DbSet<Product> Product { get; set; } = null!;

        public DbSet<Category> Category { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}

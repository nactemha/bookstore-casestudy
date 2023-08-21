using ecommerce.data;
using ecommerce.models;
using ecommerce.service;
using Microsoft.EntityFrameworkCore;
namespace ecommerce.data.postgre
{
    public class AppDbContext : DbContext
    {
        private readonly ISettings _settings;
        public AppDbContext(ISettings settings)
        {
            _settings = settings;
        }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_settings.PostgreConnectionString());
        }

    }
}
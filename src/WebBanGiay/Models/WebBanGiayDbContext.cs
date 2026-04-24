using System.Data.Entity;

namespace WebBanGiay.Models
{
    public class WebBanGiayDbContext : DbContext
    {
        public WebBanGiayDbContext() : base("name=WebBanGiayConnection")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
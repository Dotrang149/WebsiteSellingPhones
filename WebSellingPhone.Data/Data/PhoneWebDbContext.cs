using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.Data
{
    public class PhoneWebDbContext : IdentityDbContext<Users, Role, Guid>
    {
        public PhoneWebDbContext(DbContextOptions<PhoneWebDbContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=TRANGDO;database=Sang04;Trusted_connection=true;TrustServerCertificate=true");
        //    }
        //}


        public DbSet<Users> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>()
                .HasMany<Order>(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserOrderId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasMany<RefreshToken>(u => u.RefreshTokens)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Promotion>()
                .HasMany<Product>(pm => pm.Products)
                .WithOne(pr => pr.Promotion)
                .HasForeignKey(pr => pr.PromotionProductId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Brand>()
                .HasMany<Product>(b => b.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandProductId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<OrderDetail>().HasKey(po => new { po.OrderId, po.ProductId });
            modelBuilder.Entity<OrderDetail>()
                .HasOne<Product>(po => po.Products)
                .WithMany(p => p.ProductOrders)
                .HasForeignKey(po => po.ProductId);
            modelBuilder.Entity<OrderDetail>()
                .HasOne<Order>(po => po.Order)
                .WithMany(o => o.ProductOrders)
                .HasForeignKey(po => po.OrderId);

            modelBuilder.Entity<Review>().HasKey(r => new { r.ProductId, r.UserId });
            modelBuilder.Entity<Review>()
                .HasOne<Product>(r => r.Products)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);
            modelBuilder.Entity<Order>()
       .Property(o => o.TotalAmount)
       .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

        }
    }
}

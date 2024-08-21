using Microsoft.EntityFrameworkCore;
using TestDbMock.Models;

namespace TestDbMock.Data
{
    public class PhoneWebDbContext : DbContext
    {
        public DbSet<Users> Users {  get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<ProductCart> ProductsCart { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ProductOrder> ProductOrder { get; set; }

        public PhoneWebDbContext(DbContextOptions<PhoneWebDbContext> option) : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasOne<Cart>(u => u.Cart)
                .WithOne(c => c.Users)
                .HasForeignKey<Cart>(c => c.UserCartId);

            modelBuilder.Entity<Users>()
                .HasMany<Order>(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserOrderId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>()
                .HasMany<Review>(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserReviewId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Promotion>()
                .HasMany<Product>(pm  => pm.Products)
                .WithOne(pr => pr.Promotion)
                .HasForeignKey(pr => pr.PromotionProductId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Brand>()
                .HasMany<Product>(b => b.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandProductId).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProductOrder>().HasKey(po => new {po.OrderId, po.ProductId});
            modelBuilder.Entity<ProductOrder>()
                .HasOne<Product>(po => po.Products)
                .WithMany(p => p.ProductOrders)
                .HasForeignKey(po => po.ProductId);
            modelBuilder.Entity<ProductOrder>()
                .HasOne<Order>(po => po.Order)
                .WithMany(o => o.ProductOrders)
                .HasForeignKey(po => po.OrderId);

            modelBuilder.Entity<ProductCart>().HasKey(pc => new { pc.ProductId, pc.CartId });
            modelBuilder.Entity<ProductCart>()
                .HasOne(pc => pc.Cart)
                .WithMany(c => c.ProductCarts)
                .HasForeignKey(pc => pc.CartId);
            modelBuilder.Entity<ProductCart>()
                .HasOne(pc => pc.Products)
                .WithMany(p => p.ProductCarts)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductReview>().HasKey(pr => new {pr.ProductId, pr.ReviewId});
            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Review)
                .WithMany(r => r.ProductReviews)
                .HasForeignKey(pr => pr.ReviewId);
            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Products)
                .WithMany(p => p.ProductReviews)
                .HasForeignKey(pr => pr.ProductId);
        }
    }
}

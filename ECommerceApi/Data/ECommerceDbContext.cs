using ECommerceApi.Models; // Modellerimizi kullanabilmek için ekliyoruz
using Microsoft.EntityFrameworkCore; // DbContext için gerekli

namespace ECommerceApi.Data
{
    public class ECommerceDbContext : DbContext
    {
        // DbContext'in yapılandırma seçeneklerini alması için constructor
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        {
        }

        // Her bir model sınıfımız için bir DbSet tanımlıyoruz.
        // Bu, Entity Framework'ün bu modelleri veritabanındaki tablolara eşleştirmesini sağlar.
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // Modeller arasındaki ilişkileri ve kısıtlamaları daha detaylı yapılandırmak için OnModelCreating metodunu kullanırız.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product ve Category arasındaki bire-çok ilişkiyi belirtiyoruz.
            // Bir Category'nin birçok Product'ı olabilir (HasMany).
            // Bir Product'ın bir Category'si olmalı (HasOne).
            // Product'taki CategoryId Foreign Key'dir (HasForeignKey).
            // Category silindiğinde ilişkili Product'lar silinmez (SetNull yerine NoAction veya Restrict tercih edilebilir).
            // Cascade Delete'i manuel olarak yönetmek genellikle daha güvenlidir.
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategori silindiğinde ürünlerin silinmesini engelleriz.

            // User ve CartItem arasındaki ilişki
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silindiğinde sepet öğeleri de silinsin.

            // CartItem ve Product arasındaki ilişki
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Ürün silindiğinde sepet öğeleri silinmesin (önce sepetten çıkarılmalı).

            // User ve Order arasındaki ilişki
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde siparişler silinmesin.

            // Order ve OrderItem arasındaki ilişki
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Sipariş silindiğinde sipariş öğeleri de silinsin.

            // OrderItem ve Product arasındaki ilişki
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Ürün silindiğinde sipariş öğeleri silinmesin.

            // Benzersiz kısıtlamalar (örneğin kullanıcı adı veya e-posta benzersiz olmalı)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
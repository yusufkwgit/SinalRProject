using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Concrete
{
    public class SignalRContext: IdentityDbContext<AppUser, AppRole, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-IGOSQTA\\SQLEXPRESS01;initial Catalog=SignalRDb;integrated Security=true;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // BU SATIR ŞART!

            // Konsoldaki o Decimal uyarılarını kapatmak için şu örnekleri ekleyebilirsin:
            modelBuilder.Entity<Basket>().Property(x => x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Basket>().Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
            // Diğer uyarı verenleri de (Product.Price vb.) bu şekilde ekle dostum.
        }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrderDetails{ get; set; }
        public DbSet<MoneyCase> MoneyCases{ get; set; }
        public DbSet<MenuTable>MenuTables{ get; set; }
        public DbSet<Slider>Sliders{ get; set; }
        public DbSet<Basket>Baskets{ get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}

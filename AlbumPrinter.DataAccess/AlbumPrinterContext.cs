using AlbumPrinter.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AlbumPrinter.DataAccess
{
    [ExcludeFromCodeCoverage]
    public partial class AlbumPrinterContext : DbContext
    {
        public AlbumPrinterContext()
        {
        }

        public AlbumPrinterContext(DbContextOptions<AlbumPrinterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderProductType> OrderProductTypes { get; set; } = null!;
        public virtual DbSet<ProductType> ProductTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).ValueGeneratedNever();
            });

            modelBuilder.Entity<OrderProductType>(entity =>
            {
                entity.HasKey(e => e.OrderProductId)
                    .HasName("Pk_OrderProductType_OrderProductId");

                entity.ToTable("OrderProductType");

                entity.Property(e => e.OrderProductId).ValueGeneratedNever();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderProductTypes)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_Orders_OrderProductType_OrderId");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.OrderProductTypes)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fk_ProductTypes_OrderProductType_ProductTypeId");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.BinWidth).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductTypeDescription).HasMaxLength(150);

                entity.Property(e => e.ProductTypeName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

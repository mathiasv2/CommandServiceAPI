using System;
using System.Reflection;
using CommandeServiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandeServiceAPI.Database
{
    public class CommandDbContext : DbContext
    {
        public CommandDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Command> Commands { get; set; }
        public virtual DbSet<Command_Product> Command_Products { get; set; }
        public virtual DbSet<ProductCache> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Command>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Quantity)
                    .IsRequired();
            });

            modelBuilder.Entity<ProductCache>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.ProductId)
                    .IsRequired();

                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.ProductPriceAtOrder)
                    .IsRequired();

                entity.HasIndex(e => e.ProductId)
                    .IsUnique();
            });

            modelBuilder.Entity<Command_Product>(entity =>
            {
                entity.HasKey(cp => new { cp.CommandId, cp.ProductCacheId });

                entity.Property(e => e.QuantityOrdered)
                    .IsRequired();

                entity.HasOne(cp => cp.Command)
                    .WithMany(c => c.Command_Products)
                    .HasForeignKey(cp => cp.CommandId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cp => cp.ProductCache)
                    .WithMany(pc => pc.Command_Products)
                    .HasForeignKey(cp => cp.ProductCacheId)
                    .OnDelete(DeleteBehavior.Restrict); 

                entity.ToTable("Command_Products");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}


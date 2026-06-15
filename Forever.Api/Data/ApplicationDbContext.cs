
using Forever.Api.Models;
using Forever.Api.Models.Product;
using Forever.Api.Models.RefreshToken;
using Forever.Api.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AuthDemo.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       public DbSet<Users> Users { get; set; }
       public DbSet<RefreshToken> RefreshTokens { get; set; } 
       public DbSet<Product> Products {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            base.OnModelCreating(modelBuilder);
        
        }
    }
}

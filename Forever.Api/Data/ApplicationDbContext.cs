
using Forever.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
       
       public DbSet<Category> Categories { get; set; } 
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            base.OnModelCreating(modelBuilder);
        
        }
    }
}

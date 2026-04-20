using AuthDemo.Api.DTOs;
using AuthDemo.Api.Models;
using Forever.Api.Models;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
        }
    }
}

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zdimk.DataAccess.Configuration;
using Zdimk.Domain.Entities;

namespace Zdimk.DataAccess
{
    public class ZdimkDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Comment> Comments { get; set; }
            
        public ZdimkDbContext(DbContextOptions<ZdimkDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
    }
}
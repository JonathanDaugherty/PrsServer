using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrsServer.Models;

namespace PrsServer.Data
{
    public class PrsDbContext : DbContext {
        public PrsDbContext(DbContextOptions<PrsDbContext> options)
            : base(options) {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Request> Requests { get; set; }


        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>(e => {
                e.HasIndex(u => u.Username).IsUnique(true);
            });
            builder.Entity<Vendor>(v => {
                v.HasIndex(c => c.Code).IsUnique(true);
            });
            builder.Entity<Product>(p => {
                p.HasIndex(n => n.PartNbr).IsUnique(true);
            });
        }  

        public DbSet<PrsServer.Models.Product> Product { get; set; }

        public DbSet<PrsServer.Models.Request> Request { get; set; }

        public DbSet<PrsServer.Models.RequestLine> RequestLine { get; set; }
    }
}
        


        

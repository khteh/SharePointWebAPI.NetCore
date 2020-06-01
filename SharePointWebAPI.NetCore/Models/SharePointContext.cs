using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharePointWebAPI.NetCore.Models
{
    public class SharePointContext : DbContext
    {
        public SharePointContext(DbContextOptions<SharePointContext> options)
            : base(options)
        { }
        public DbSet<SharePointParam> SharePointItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<SharePointParam>(ConfigureSharePointParam);

        public void ConfigureSharePointParam(EntityTypeBuilder<SharePointParam> builder)
        {
            builder.HasNoKey();
        }
    }
}
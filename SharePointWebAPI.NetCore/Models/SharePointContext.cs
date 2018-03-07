using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.NetCore.Models
{
    public class SharePointContext : DbContext
    {
        public SharePointContext(DbContextOptions<SharePointContext> options)
            : base(options)
        { }
        public DbSet<SharePointParam> SharePointItems { get; set; }
    }
}
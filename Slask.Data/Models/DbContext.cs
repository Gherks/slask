using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slask.Data.Models
{
    public class SlaskContext : DbContext
    {
        public SlaskContext(DbContextOptions<SlaskContext> options)
            : base(options)
        { }
        public DbSet<>  { get; set; }
        public DbSet<>  { get; set; }
    }
}

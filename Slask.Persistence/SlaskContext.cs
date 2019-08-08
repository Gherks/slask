﻿using Microsoft.EntityFrameworkCore;
using Slask.Domain;

namespace Slask.Persistence
{
    public class SlaskContext : DbContext
    {
        public SlaskContext()
        {
        }

        public SlaskContext(DbContextOptions options)
            : base(options)
        {
        }

        public SlaskContext(DbContextOptions<SlaskContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Database = SlaskDB; Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>().Ignore(player => player.Player1);
            modelBuilder.Entity<Match>().Ignore(player => player.Player2);
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

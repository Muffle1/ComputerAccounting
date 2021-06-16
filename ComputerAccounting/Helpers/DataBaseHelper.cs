using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class DataBaseHelper : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<ComputerConfig> ComputerConfigs { get; set; }
        public DbSet<Computer> Computers { get; set; }

        public DataBaseHelper()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Computer>().HasOne(c => c.Cabinet).WithMany(c => c.Computers).OnDelete(DeleteBehavior.ClientCascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Computer Accounting\\CADataBase.db";
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CADataBase.db");
        }
    }
}

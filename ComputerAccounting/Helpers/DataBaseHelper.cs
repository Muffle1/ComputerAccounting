using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class DataBaseHelper : DbContext, IDisposable
    {
        public DbSet<User> Users { get; set; }

        public DataBaseHelper()
        {
            Database.EnsureCreated();
            //InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await Task.Run(() => Database.EnsureCreatedAsync());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CADataBase.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}

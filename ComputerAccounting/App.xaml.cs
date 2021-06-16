using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Windows;

namespace ComputerAccounting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Computer Accounting"))
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Computer Accounting");

            using var db = new DataBaseHelper();
            db.Database.Migrate();
        }
    }
}

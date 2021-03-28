using Microsoft.EntityFrameworkCore;
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

            using var db = new DataBaseHelper();
            db.Database.Migrate();
        }
    }
}

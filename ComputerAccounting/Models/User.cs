using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace ComputerAccounting
{
    public enum Role
    {
        [Description("Техник")]
        Engineer = 1,
        [Description("Заведующий лабораторией")]
        LaboratoryHead
    }

    public class User
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        [NotMapped]
        public int SymbolCount { get; private set; }

        private string _password;
        public string Password
        {
            get => _password;

            set
            {
                _password = Hash(value);
                SymbolCount = value.Length;
            }
        }

        public Role Role { get; set; } = Role.Engineer;

        public string Hash(string input) =>
            Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)));
    }
}

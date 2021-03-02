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

    public class User : BaseModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }

        private string _password;
        public string Password
        {
            get => _password;
            set => _password = Hash(value);
        }

        public Role Role { get; set; } = Role.Engineer;

        public User Clone()
        {
            return new User { UserId = this.UserId, Login = this.Login, Role = this.Role };
        }

        public void SaveWithoutHash(string password) =>
            _password = password;
    }
}

using System.ComponentModel;

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
        public string Password { get; set; }
        public Role Role { get; set; } = Role.Engineer;
    }
}

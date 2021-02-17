using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class SecondSideMenuViewModel : BaseViewModel
    {
        public delegate void ErrorHandler(string errorMessage);
        public event ErrorHandler ShowError;
        public User User { get; set; }

        public IEnumerable<string> Roles
        {
            get => EnumHelper.GetEnumDescriptions(new Role().GetType());
        }

        public SecondSideMenuViewModel(User user)
        {
            User = user;
        }

        private RelayCommand _saveUserInfoCommand;
        public RelayCommand SaveUserInfoCommand
        {
            get
            {
                return _saveUserInfoCommand ??= new RelayCommand(async o =>
                {
                    
                });
            }
        }

        private async Task<bool> CheckUser()
        {
            User.ClearErrors(nameof(User.Login));
            User.ClearErrors("Pass");
            ShowError("");

            if ((User.Login == null) || (User.Login.Length <= 5))
                User.AddError(nameof(User.Login), "Логин должен быть больше 6 символов.");

            if ((User.Password == null) || (User.SymbolCount <= 5))
            {
                ShowError("Пароль должен быть больше 6 символов.");
                User.AddError("Pass", "Пароль должен быть больше 6 символов.");
            }

            if (User.HasErrors) return false;

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                if (!db.Users.Any(u => u.Login == User.Login))
                {
                    db.Users.Add(User);
                    db.SaveChanges();

                    return true;
                }
                else
                    User.AddError(nameof(User.Login), "Пользователь с таким логином уже есть.");

                return false;
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class SecondSideMenuViewModel : BaseViewModel
    {
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
                    await CheckUser((int)o);
                        
                });
            }
        }

        private RelayCommand _exitCommand;
        public RelayCommand ExitCommand
        {
            get
            {
                return _exitCommand ??= new RelayCommand(o =>
                {
                    OnViewSwitched(new AuthenticationManagerViewModel(), NameView.Manager);
                });
            }
        }

        private async Task<bool> CheckUser(int symbolCount)
        {
            User.ClearErrors(nameof(User.Login));
            User.ClearErrors(nameof(User.Password));

            if ((User.Login == null) || (User.Login.Length <= 5))
                User.AddError(nameof(User.Login), "Логин должен быть больше 6 символов.");
            else
            {
                return await Task.Run(() =>
                {
                    using DataBaseHelper db = new DataBaseHelper();
                    User user = db.Users.Single(u => u.UserId == User.UserId);

                    if (user != null)
                    {
                        user.Login = User.Login;
                        db.SaveChanges();

                        return true;
                    }

                    return false;
                });
            }

            if ((User.Password == null) || (symbolCount <= 5))
                User.AddError(nameof(User.Password), "Пароль должен быть больше 6 символов.");
            else
            {
                return await Task.Run(() =>
                {
                    using DataBaseHelper db = new DataBaseHelper();
                    User user = db.Users.Single(u => u.UserId == User.UserId);

                    if (user != null)
                    {
                        user.Password = User.Password;
                        db.SaveChanges();

                        return true;
                    }

                    return false;
                });
            }

            if (User.HasErrors) return false;

            return false;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerAccounting
{
    public class AuthorizationViewModel : BaseViewModel
    {
        public AuthorizationViewModel()
        {
            User = new User();
        }

        public User User { get; set; }

        private RelayCommand _registrationPageCommand;
        public RelayCommand RegistrationPageCommand
        {
            get
            {
                return _registrationPageCommand ??= new RelayCommand(o =>
                {
                    OnViewSwitched(new RegistrationViewModel(), NameView.Page);
                });
            }
        }

        private RelayCommand _mainPageCommand;
        public RelayCommand MainPageCommand
        {
            get
            {
                return _mainPageCommand ??= new RelayCommand(async o =>
                {
                    if (await CheckUserAsync((int)o))
                    {
                        MainWindowViewModel.CurrentUser = User;
                        OnViewSwitched(new MainManagerViewModel(), NameView.Manager);
                    }
                });
            }
        }

        private async Task<bool> CheckUserAsync(int symbolCount)
        {
            User.ClearErrors();

            if ((User.Login == null) || (User.Login.Length <= 5))
                User.AddError(nameof(User.Login), "Логин должен быть больше 6 символов.");

            if ((User.Password == null) || (symbolCount <= 5))
                User.AddError(nameof(User.Password), "Пароль должен быть больше 6 символов.");

            if (User.HasErrors) return false;

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                if (db.Users.Any(u => (u.Login == User.Login) && (u.Password == User.Password)))
                {
                    User user = db.Users.Single(u => (u.Login == User.Login) && (u.Password == User.Password));
                    User = user.Clone();
                    return true;
                }
                else
                    User.AddError(nameof(User.Login), "Неверно введён логин или пароль");

                return false;
            });
        }
    }
}

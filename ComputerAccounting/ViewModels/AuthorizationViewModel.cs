using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerAccounting
{
    public class AuthorizationViewModel : BaseViewModel
    {
        public delegate void ErrorHandler(string errorMessage);
        public event ErrorHandler ShowError;

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
                    if (await CheckUser())
                        OnViewSwitched(new MainManagerViewModel(), NameView.Manager);                  
                });
            }
        }

        private async Task<bool> CheckUser()
        {
            User.ClearErrors(nameof(User.Login));
            User.ClearErrors("Pass");

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
                if (db.Users.Any(u => (u.Login == User.Login) && (u.Password == User.Password)))
                    return true;
                else
                    User.AddError(nameof(User.Login), "Неверно введён логин или пароль");

                return false;
            });
        }
    }
}

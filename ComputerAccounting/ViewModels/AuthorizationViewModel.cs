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
        private int SymbolCount;

        private User _user;

        public AuthorizationViewModel()
        {
            User = new User();
        }

        public User User
        {
            get => _user;
            set => _user = value;
        }

        public string Login
        {
            get => _user.Login;

            set
            {
                _user.Login = value;
                ClearErrors("Login");
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get => _user.Password;

            set
            {
                _user.Password = Hash(value);
                SymbolCount = value.Length;
                ShowError("");
                ClearErrors("Pass");
                OnPropertyChanged("Password");
            }
        }

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
                    if (await CheckData())
                        OnViewSwitched(new MainManagerViewModel(), NameView.Manager);                  
                });
            }
        }

        public async Task<bool> CheckData()
        {
            ClearErrors("Login");
            ClearErrors("Pass");

            if ((Login == null) || (Login.Length <= 5))
                AddError("Login", "Логин должен быть больше 6 символов.");

            if ((Password == null) || (SymbolCount <= 5))
            {
                ShowError("Пароль должен быть больше 6 символов.");
                AddError("Pass", "Пароль должен быть больше 6 символов.");
            }

            if (HasErrors) return false;

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                if (db.Users.Any(u => (u.Login == Login) && (u.Password == Password)))
                    return true;
                else
                    AddError("Login", "Неверно введён логин или пароль");

                return false;
            });
        }
    }
}

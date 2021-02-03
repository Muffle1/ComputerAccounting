using ComputerAccounting.Commands;
using ComputerAccounting.Helpers;
using ComputerAccounting.Models;
using ComputerAccounting.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerAccounting.ViewModels
{
    public class AuthorizationViewModel : BaseViewModel, IPageSwitcher
    {
        public delegate void ErrorHandler(string errorMessage);
        public event ErrorHandler ShowError;

        public event PageHandler SwitchPage;
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
                _user.Password = value;
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
                    SwitchPage?.Invoke(this, new PageEventArgs(new RegistrationViewModel(), NameView.Page));
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
                        MessageBox.Show("Успех!");                   
                });
            }
        }

        public async Task<bool> CheckData()
        {
            ClearErrors("Login");
            ClearErrors("Pass");

            if ((_user.Login == null) || (_user.Login.Length <= 5))
                AddError("Login", "Логин должен быть больше 6 символов.");

            if ((_user.Password == null) || (_user.Password.Length <= 5))
            {
                ShowError("Пароль должен быть больше 6 символов.");
                AddError("Pass", "Пароль должен быть больше 6 символов.");
            }

            if (HasErrors) return false;

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                if (db.Users.Any(u => (u.Login == _user.Login) && (u.Password == _user.Password)))
                    return true;
                else
                    AddError("Login", "Неверно введён логин или пароль");

                return false;
            });
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class RegistrationViewModel : BaseViewModel
    {
        public delegate void ErrorHandler(string errorMessage);
        public event ErrorHandler ShowError;
        private int SymbolCount;

        public RegistrationViewModel()
        {
            User = new User();
        }

        private User _user;
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

        public string SelectedRole
        {
            get => EnumHelper.GetEnumDescription(_user.Role);

            set
            {
                _user.Role = EnumHelper.GetValueFromDescription<Role>(value);
                OnPropertyChanged("SelectedRole");
            }
        }

        public IEnumerable<string> Roles
        {
            get => EnumHelper.GetEnumDescriptions(new Role().GetType());
            set => OnPropertyChanged("Roles");
        }

        private RelayCommand _authorizationPageCommand;
        public RelayCommand AuthorizationPageCommand
        {
            get
            {
                return _authorizationPageCommand ??= new RelayCommand(async o =>
                    {
                        if (await CheckData())
                            OnViewSwitched(new AuthorizationViewModel(), NameView.Page);
                    });
            }
        }

        private RelayCommand _backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand ??= new RelayCommand(o =>
                {
                    OnViewSwitched(new AuthorizationViewModel(), NameView.Page);
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
                if (!db.Users.Any(u => u.Login == Login))
                {
                    db.Users.Add(_user);
                    db.SaveChanges();

                    return true;
                }
                else
                    AddError("Login", "Пользователь с таким логином уже есть.");

                return false;
            });

        }
    }
}

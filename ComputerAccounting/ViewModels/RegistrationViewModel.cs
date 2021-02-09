using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class RegistrationViewModel : BaseViewModel
    {
        public delegate void ErrorHandler(string errorMessage);
        public event ErrorHandler ShowError;

        public RegistrationViewModel()
        {
            User = new User();
        }

        public User User { get; set; }

        public IEnumerable<string> Roles
        {
            get => EnumHelper.GetEnumDescriptions(new Role().GetType());
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
            ClearErrors("User.Login");
            ClearErrors("Pass");
            ShowError("");

            if ((User.Login == null) || (User.Login.Length <= 5))
                AddError("User.Login", "Логин должен быть больше 6 символов.");

            if ((User.Password == null) || (User.SymbolCount <= 5))
            {
                ShowError("Пароль должен быть больше 6 символов.");
                AddError("Pass", "Пароль должен быть больше 6 символов.");
            }

            if (HasErrors) return false;

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
                    AddError("User.Login", "Пользователь с таким логином уже есть.");

                return false;
            });

        }
    }
}

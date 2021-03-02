using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class RegistrationViewModel : BaseViewModel
    {
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
                        if (await CheckUserAsync((int)o))
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

        private async Task<bool> CheckUserAsync(int symbolCount)
        {
            User.ClearErrors(nameof(User.Login));
            User.ClearErrors(nameof(User.Password));

            if ((User.Login == null) || (User.Login.Length <= 5))
                User.AddError(nameof(User.Login), "Логин должен быть больше 6 символов.");

            if ((User.Password == null) || (symbolCount <= 5))
                User.AddError(nameof(User.Password), "Пароль должен быть больше 6 символов.");

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

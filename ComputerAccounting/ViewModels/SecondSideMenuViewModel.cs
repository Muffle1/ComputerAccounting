using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class SecondSideMenuViewModel : BaseViewModel
    {
        private User _user;
        public User User
        {
            get => _user;
            set => SetValue(ref _user, value, nameof(User));
        }

        private string _login;
        public string Login
        {
            get => _login;
            set => SetValue(ref _login, value, nameof(Login));
        }

        public IEnumerable<string> Roles
        {
            get => EnumHelper.GetEnumDescriptions(new Role().GetType());
        }

        private ProfileViewModel _profileViewModel;
        public ProfileViewModel ProfileViewModel
        {
            get => _profileViewModel;
            set => SetValue(ref _profileViewModel, value, nameof(ProfileViewModel));
        }

        public SecondSideMenuViewModel()
        {
            User = MainWindowViewModel.CurrentUser.Clone();
            Login = MainWindowViewModel.CurrentUser.Login;
            User.Login = "";
        }

        private RelayCommand _saveUserInfoCommand;
        public RelayCommand SaveUserInfoCommand
        {
            get => _saveUserInfoCommand ??= new RelayCommand(async o => await CheckUserAsync((int)o));
        }

        private RelayCommand _openControlCommand;
        public RelayCommand OpenControlCommand
        {
            get => _openControlCommand ??= new RelayCommand(o => OpenControl(o.ToString()));
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

        private void OpenControl(string controlName)
        {
            if (ProfileViewModel == null)
                OnViewSwitched(ProfileViewModel = new ProfileViewModel(), NameView.Page);

            if ((controlName == "ComputerConfigControl") && (ProfileViewModel.ComputerConfigControl == null))
                ProfileViewModel.ComputerConfigControl = new ComputerConfigViewModel();
            else if ((controlName == "ComputerConfigControl") && (ProfileViewModel.ComputerConfigControl != null))
                ProfileViewModel.ComputerConfigControl = null;

            if ((controlName == "LaboratoryInfoControl") && (ProfileViewModel.LaboratoryInfoControl == null))
                ProfileViewModel.LaboratoryInfoControl = new LaboratoryInfoViewModel();
            else if ((controlName == "LaboratoryInfoControl") && (ProfileViewModel.LaboratoryInfoControl != null))
                ProfileViewModel.LaboratoryInfoControl = null;

            OnPropertyChanged(nameof(ProfileViewModel));
        }

        private async Task<bool> CheckUserAsync(int symbolCount)
        {
            User.ClearErrors();

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                User user = db.Users.Single(u => u.UserId == User.UserId);

                if (user != null)
                {
                    if ((!db.Users.Any(u => u.Login == User.Login)) && (user.Login != User.Login) && (!string.IsNullOrEmpty(User.Login)))
                    {
                        if (User.Login.Length <= 5)
                            User.AddError(nameof(User.Login), "Логин должен быть больше 6 символов.");
                        else
                            user.Login = Login = MainWindowViewModel.CurrentUser.Login = User.Login;
                    }
                    else if ((user.Login == User.Login) && (!string.IsNullOrEmpty(User.Login)))
                        User.AddError(nameof(User.Login), "Логин не должен совпадать с предыдущим логином.");
                    else if (db.Users.Any(u => u.Login == User.Login))
                        User.AddError(nameof(User.Login), "Пользователь с таким логином уже есть.");

                    if ((user.Password != User.Password) && (!string.IsNullOrEmpty(User.Password)))
                    {
                        if (symbolCount <= 5)
                            User.AddError(nameof(User.Password), "Пароль должен быть больше 6 символов.");
                        else
                            user.SaveWithoutHash(User.Password);
                    }
                    else if ((user.Password == User.Password) && (!string.IsNullOrEmpty(User.Password)))
                        User.AddError(nameof(User.Password), "Пароль не должен совпадать с предыдущим паролем.");

                    if (user.Role != User.Role)
                        user.Role = MainWindowViewModel.CurrentUser.Role = User.Role;

                    if (User.HasErrors) return false;

                    User = new User() { UserId = User.UserId, Role = User.Role };
                    OnPropertyChanged(nameof(User.Login));
                    db.SaveChanges();

                    return true;
                }

                return false;
            });
        }
    }
}

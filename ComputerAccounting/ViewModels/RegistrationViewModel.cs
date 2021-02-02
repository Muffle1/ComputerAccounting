using ComputerAccounting.Commands;
using ComputerAccounting.Helpers;
using ComputerAccounting.Models;
using ComputerAccounting.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ComputerAccounting.ViewModels
{
    public class RegistrationViewModel : BaseViewModel, IPageSwitcher
    {
        private readonly DataBaseHelper _context = new DataBaseHelper();
        public event PageHandler SwitchPage;
        private User _user;

        public RegistrationViewModel()
        {
            User = new User();
        }

        public User User
        {
            get => _user;

            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        public string Login
        {
            get => _user.Login;

            set
            {
                _user.Login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get => _user.Password;

            set
            {
                _user.Password = value;
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

        private RelayCommand _switchPageCommand;
        public RelayCommand SwitchPageCommand
        {
            get
            {
                return _switchPageCommand ??= new RelayCommand(o =>
                    {
                        
                    });
            }
        }
    }
}

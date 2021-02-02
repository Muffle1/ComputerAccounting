using ComputerAccounting.Commands;
using ComputerAccounting.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ComputerAccounting.ViewModels
{
    internal class AuthorizationViewModel : IPageSwitcher
    {
        public event PageHandler SwitchPage;

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand ??
                    (_AddCommand = new RelayCommand(o =>
                    {
                        SwitchPage(this, new PageEventArgs(new RegistrationViewModel()));
                    }));
            }
        }

        internal AuthorizationViewModel()
        {

        }
    }
}

using ComputerAccounting.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;

namespace ComputerAccounting.ViewModels
{
    class AuthenticationManagerViewModel : BaseViewModel
    {
        private IPageSwitcher _currentPage;
        public IPageSwitcher CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public AuthenticationManagerViewModel()
        {
            LoadPage(new AuthorizationViewModel());
        }

        public void LoadPage(IPageSwitcher viewModel)
        {
            viewModel.SwitchPage += Context_SwitchPage;
            CurrentPage = viewModel;
        }

        private void Context_SwitchPage(object sender, PageEventArgs e)
        {
            if (e.NameView == NameView.Page)
                LoadPage(e.PageToLoad);
        }
    }
}

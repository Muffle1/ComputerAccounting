using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;

namespace ComputerAccounting
{
    class AuthenticationManagerViewModel : BaseViewModel
    {
        private IViewSwitcher _currentPage;
        public IViewSwitcher CurrentPage
        {
            get => _currentPage;
            set => SetValue(ref _currentPage, value, nameof(CurrentPage));
        }

        public AuthenticationManagerViewModel()
        {
            LoadPage(new AuthorizationViewModel());
        }

        public void LoadPage(IViewSwitcher viewModel)
        {
            viewModel.SwitchView += ViewModel_SwitchView;
            CurrentPage = viewModel;
        }

        private void ViewModel_SwitchView(object sender, ViewEventArgs e)
        {
            if (e.NameView == NameView.Page)
                LoadPage(e.ViewToLoad);

            if (e.NameView == NameView.Manager)
                OnViewSwitched(e.ViewToLoad, NameView.Manager);
        }
    }
}

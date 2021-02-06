using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    class MainManagerViewModel : BaseViewModel
    {
        private IViewSwitcher _sideMenu;
        public IViewSwitcher SideMenu
        {
            get => _sideMenu;
            set
            {
                _sideMenu = value;
                OnPropertyChanged("SideMenu");
            }
        }

        private IViewSwitcher _currentPage;
        public IViewSwitcher CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        public MainManagerViewModel()
        {
            //LoadView(new AuthorizationViewModel(), NameView.Page);
        }

        public void LoadView(IViewSwitcher viewModel, NameView nameView)
        {
            viewModel.SwitchView += ViewModel_SwitchView;

            if (nameView == NameView.Control) SideMenu = viewModel;
            if (nameView == NameView.Page) CurrentPage = viewModel;
        }

        private void ViewModel_SwitchView(object sender, ViewEventArgs e)
        {
            if (e.NameView == NameView.Manager)
                OnViewSwitched(e.ViewToLoad, NameView.Manager);
            else
                LoadView(e.ViewToLoad, e.NameView);
        }
    }
}

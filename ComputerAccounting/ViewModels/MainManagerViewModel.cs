using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    public enum NameSideMenu
    {
        FirstMenu = 1,
        SecondMenu
    }

    public class MainManagerViewModel : BaseViewModel
    {
        private IViewSwitcher _sideMenu;
        public IViewSwitcher SideMenu
        {
            get => _sideMenu;
            set
            {
                _sideMenu = value;
                OnPropertyChanged(nameof(SideMenu));
            }
        }

        private IViewSwitcher _currentPage;
        public IViewSwitcher CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private NameSideMenu _nameSideMenu;
        public NameSideMenu NameSideMenu
        {
            get => _nameSideMenu;

            set
            {
                _nameSideMenu = value;
                OnPropertyChanged(nameof(NameSideMenu));
            }
        }

        public User User { get; set; }

        public MainManagerViewModel(User user)
        {
            User = user;
            NameSideMenu = NameSideMenu.FirstMenu;
            LoadView(new FirstSideMenuViewModel(), NameView.Control);
        }

        public void LoadView(IViewSwitcher viewModel, NameView nameView)
        {
            if (viewModel != null)
            {
                viewModel.SwitchView += ViewModel_SwitchView;

                if (nameView == NameView.Control) SideMenu = viewModel;
                if (nameView == NameView.Page) CurrentPage = viewModel;
            }
        }

        private void ViewModel_SwitchView(object sender, ViewEventArgs e)
        {
            if (e.NameView == NameView.Manager)
                OnViewSwitched(e.ViewToLoad, NameView.Manager);
            else
                LoadView(e.ViewToLoad, e.NameView);
        }

        private RelayCommand _switchControlCommand;
        public RelayCommand SwitchControlCommand
        {
            get
            {
                return _switchControlCommand ??= new RelayCommand(o =>
                {
                    if ((NameSideMenu)Enum.Parse(typeof(NameSideMenu), o.ToString()) == NameSideMenu.FirstMenu)
                    {
                        NameSideMenu = NameSideMenu.FirstMenu;
                        LoadView(new FirstSideMenuViewModel(), NameView.Control);
                    }

                    if ((NameSideMenu)Enum.Parse(typeof(NameSideMenu), o.ToString()) == NameSideMenu.SecondMenu)
                    {
                        NameSideMenu = NameSideMenu.SecondMenu;
                        LoadView(new SecondSideMenuViewModel(User), NameView.Control);
                    }
                });
            }
        }
    }
}

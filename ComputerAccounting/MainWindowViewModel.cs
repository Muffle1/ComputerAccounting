﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    class MainWindowViewModel : BaseViewModel
    {
        private IViewSwitcher _currentManager;
        public IViewSwitcher CurrentManager
        {
            get => _currentManager;
            set
            {
                _currentManager = value;
                OnPropertyChanged("CurrentManager");
            }
        }

        public MainWindowViewModel()
        {
            LoadManager(new MainManagerViewModel());
            //LoadManager(new AuthenticationManagerViewModel());
        }

        public void LoadManager(IViewSwitcher viewModel)
        {
            viewModel.SwitchView += ViewModel_SwitchPage;
            CurrentManager = viewModel;
        }

        private void ViewModel_SwitchPage(object sender, ViewEventArgs e)
        {
            LoadManager(e.ViewToLoad);
        }
    }
}

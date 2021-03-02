using System;
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
            set => SetValue(ref _currentManager, value, nameof(CurrentManager));
        }

        public MainWindowViewModel()
        {
            //LoadManager(new MainManagerViewModel(new User()));
            LoadManager(new AuthenticationManagerViewModel());
        }

        public void LoadManager(IViewSwitcher viewModel)
        {
            viewModel.SwitchView += ViewModel_SwitchView;
            CurrentManager = viewModel;
        }

        private void ViewModel_SwitchView(object sender, ViewEventArgs e)
        {
            LoadManager(e.ViewToLoad);
        }
    }
}

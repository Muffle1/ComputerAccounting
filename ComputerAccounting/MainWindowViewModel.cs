using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static User CurrentUser;

        private IViewSwitcher _currentManager;
        public IViewSwitcher CurrentManager
        {
            get => _currentManager;
            set => SetValue(ref _currentManager, value, nameof(CurrentManager));
        }

        public MainWindowViewModel()
        {
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

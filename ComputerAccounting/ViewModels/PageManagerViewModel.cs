using ComputerAccounting.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;

namespace ComputerAccounting.ViewModels
{
    class PageManagerViewModel : INotifyPropertyChanged
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

        public PageManagerViewModel()
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
            LoadPage(e.PageToLoad);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

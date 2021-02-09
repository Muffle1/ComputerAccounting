using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ComputerAccounting
{
    public class Cabinet
    {
        public string Icon { get; set; }
        public string Title { get; set; }
    }

    public class FirstSideMenuViewModel : BaseViewModel
    {

        public FirstSideMenuViewModel()
        {
            Cabinets = new ObservableCollection<Cabinet>();

            Cabinets.Add(new Cabinet
            {
                Icon = "\uE7BE",
                Title = "Cabinet 1"
            });
        }

        private ObservableCollection<Cabinet> _cabinets;
        public ObservableCollection<Cabinet> Cabinets
        {
            get => _cabinets;

            set
            {
                _cabinets = value;
                OnPropertyChanged("Cabinets");
            }
        }

        private RelayCommand _testCommand;
        public RelayCommand TestCommand
        {
            get
            {
                return _testCommand ??= new RelayCommand(async o =>
                {
                    
                });
            }
        }
    }
}

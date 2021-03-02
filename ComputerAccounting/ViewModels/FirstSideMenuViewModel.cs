using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerAccounting
{
    public class FirstSideMenuViewModel : BaseViewModel
    {
        private DataBaseHelper _db;

        private Cabinet _cabinet;
        public Cabinet Cabinet
        {
            get => _cabinet;
            set => SetValue(ref _cabinet, value, nameof(Cabinet));
        }

        private Cabinet _selectedCabinet;
        public Cabinet SelectedCabinet
        {
            get => _selectedCabinet;
            set => SetValue(ref _selectedCabinet, value, nameof(SelectedCabinet));
        }

        private ObservableCollection<Cabinet> _cabinets;
        public ObservableCollection<Cabinet> Cabinets
        {
            get => _cabinets;
            set => SetValue(ref _cabinets, value, nameof(Cabinets));
        }

        private RelayCommand _addCabinetCommand;
        public RelayCommand AddCabinetCommand
        {
            get
            {
                return _addCabinetCommand ??= new RelayCommand(async o =>
                {
                    if (await CheckCabinetAsync())
                        Cabinet = new Cabinet();
                });
            }
        }

        private RelayCommand _deleteCabinetCommand;
        public RelayCommand DeleteCabinetCommand
        {
            get
            {
                return _deleteCabinetCommand ??= new RelayCommand(o =>
                {
                    _db.Remove(_db.Cabinets.Single(x => x.CabinetId == Convert.ToInt32(o)));
                    _db.SaveChanges();
                });
            }
        }

        public FirstSideMenuViewModel()
        {
            Cabinet = new Cabinet();
            LoadCabinetsAsync();
            PropertyChanged += FirstSideMenuViewModel_PropertyChanged;
        }

        private async void LoadCabinetsAsync()
        {
            await Task.Run(() =>
            {
                _db = new DataBaseHelper();
                _db.Cabinets.LoadAsync();
                Cabinets = _db.Cabinets.Local.ToObservableCollection();
            });
        }

        private void FirstSideMenuViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCabinet))
            {

            }
        }

        private async Task<bool> CheckCabinetAsync()
        {
            Cabinet.ClearErrors(nameof(Cabinet.Title));

            if (Cabinet.Title == null)
                Cabinet.AddError(nameof(Cabinet.Title), "Введите название.");

            if (Cabinet.HasErrors) return false;

            return await Task.Run(() =>
            {
                if (!_db.Cabinets.Any(c => c.Title == Cabinet.Title))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _db.Cabinets.Add(Cabinet);
                        _db.SaveChanges();
                    });

                    return true;
                }
                else
                    Cabinet.AddError(nameof(Cabinet.Title), "Такой кабинет уже есть.");

                return false;
            });
        }
    }
}

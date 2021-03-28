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

        private List<Cabinet> _cabinets;
        public List<Cabinet> Cabinets
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
                    using DataBaseHelper db = new DataBaseHelper();
                    db.Remove(db.Cabinets.Single(x => x.CabinetId == Convert.ToInt32(o)));
                    db.SaveChanges();
                    Cabinets = db.Cabinets.ToList().OrderBy(c => c.GetCabinetNumber()).ToList();
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
                using DataBaseHelper db = new DataBaseHelper();
                Cabinets = db.Cabinets.ToList().OrderBy(c => c.GetCabinetNumber()).ToList();
            });
        }

        private void FirstSideMenuViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedCabinet))
            {
                // TODO: Смена окна кабинета
            }
        }

        private async Task<bool> CheckCabinetAsync()
        {
            Cabinet.ClearErrors();

            if (Cabinet.Title == null)
                Cabinet.AddError(nameof(Cabinet.Title), "Введите название.");
            else if (!Cabinet.ContainsNumber())
                Cabinet.AddError(nameof(Cabinet.Title), "Название должно оканчиваться на число.");
            else if (Cabinets.Any(c => c.GetCabinetNumber() == Cabinet.GetCabinetNumber()))
                Cabinet.AddError(nameof(Cabinet.Title), "Кабинет с таким номером уже есть.");
            if (Cabinet.HasErrors) return false;

            return await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                if (!db.Cabinets.Any(c => c.Title == Cabinet.Title))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        db.Cabinets.Add(Cabinet);
                        db.SaveChanges();
                        Cabinets = db.Cabinets.ToList().OrderBy(c => c.GetCabinetNumber()).ToList();
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

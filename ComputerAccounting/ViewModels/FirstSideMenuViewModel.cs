using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerAccounting
{
    public class FirstSideMenuViewModel : BaseViewModel
    {
        private DataBaseHelper _db;

        public FirstSideMenuViewModel()
        {
            Cabinet = new Cabinet();

            _db = new DataBaseHelper();
            _db.Cabinets.Load();
            Cabinets = new ObservableCollection<Cabinet>(_db.Cabinets.Local);
        }

        private Cabinet _cabinet;
        public Cabinet Cabinet 
        {
            get => _cabinet;
            set
            {
                _cabinet = value;
                OnPropertyChanged(nameof(Cabinet));
            }
        }

        private ObservableCollection<Cabinet> _cabinets;
        public ObservableCollection<Cabinet> Cabinets
        {
            get => _cabinets;

            set
            {
                _cabinets = value;
                OnPropertyChanged(nameof(Cabinets));
            }
        }

        private RelayCommand _addCabinetCommand;
        public RelayCommand AddCabinetCommand
        {
            get
            {
                return _addCabinetCommand ??= new RelayCommand(async o =>
                {
                    if (await CheckCabinet())
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
                    Cabinet removableCabinet = new Cabinet() { Title = o.ToString() };
                    _db.Cabinets.Remove(removableCabinet);
                });
            }
        }

        private async Task<bool> CheckCabinet()
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

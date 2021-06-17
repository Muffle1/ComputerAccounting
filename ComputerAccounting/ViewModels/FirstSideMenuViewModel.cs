using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ComputerAccounting
{
    public class FirstSideMenuViewModel : BaseViewModel
    {
        public static CancellationTokenSource CancellationTokenSource;
        private CancellationToken Token;

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
                    {
                        using DataBaseHelper db = new DataBaseHelper();

                        if (Cabinets.First().GetCabinetNumber() > Cabinet.GetCabinetNumber())
                            Cabinets.Insert(0, Cabinet);
                        else if (Cabinets.First().GetCabinetNumber() < Cabinet.GetCabinetNumber())
                        {
                            int i = 0;
                            foreach (var cabinet in Cabinets)
                            {
                                if (cabinet.GetCabinetNumber() < Cabinet.GetCabinetNumber())
                                    i++;
                                else
                                {
                                    Cabinets.Insert(i, Cabinet);
                                    break;
                                }
                            }
                        }
                        else
                            Cabinets.Add(Cabinet);
                        Cabinet = new Cabinet();
                    }
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
                    Cabinet cabinet = db.Cabinets.Include(c => c.Computers).Single(c => c.CabinetId == Convert.ToInt32(o));
                    db.Remove(cabinet);
                    db.SaveChanges();
                    Cabinets.Remove(Cabinets.SingleOrDefault(c => c.CabinetId == Convert.ToInt32(o)));
                });
            }
        }

        public FirstSideMenuViewModel()
        {
            Cabinet = new Cabinet();
            Cabinets = new ObservableCollection<Cabinet>();
            CancellationTokenSource = new CancellationTokenSource();
            Token = CancellationTokenSource.Token;
            LoadCabinetsAsync();
            PropertyChanged += FirstSideMenuViewModel_PropertyChanged;
        }

        private async void LoadCabinetsAsync()
        {
            await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();

                int i = 0, cabinetsCount = db.Cabinets.Count();
                while (true)
                {
                    if (Token.IsCancellationRequested)
                        break;

                    Cabinet[] cabinets = db.Cabinets.ToList().OrderBy(c => c.GetCabinetNumber()).Skip(i).Take(5).ToArray();
                    foreach (var cabinet in cabinets)
                    {
                        if (Token.IsCancellationRequested)
                            break;
                        Application.Current?.Dispatcher.Invoke(() => Cabinets.Add(cabinet));
                    }

                    i += 5;

                    if (i > cabinetsCount)
                        break;
                }
            });
        }

        private void FirstSideMenuViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((SelectedCabinet != null) && (e.PropertyName == nameof(SelectedCabinet)))
            {
                CabinetViewModel cabinetViewModel = new CabinetViewModel(SelectedCabinet);
                OnViewSwitched(cabinetViewModel, NameView.Page);
                cabinetViewModel.UpdateCabinet += CabinetViewModel_UpdateCabinet;
            }
        }

        private void CabinetViewModel_UpdateCabinet() =>
            LoadCabinetsAsync();

        private async Task<bool> CheckCabinetAsync()
        {
            return await Task.Run(() =>
            {
                Cabinet.ClearErrors();

                if (string.IsNullOrEmpty(Cabinet.Title))
                    Cabinet.AddError(nameof(Cabinet.Title), "Введите название.");
                else if (!Cabinet.ContainsNumber())
                    Cabinet.AddError(nameof(Cabinet.Title), "Название должно оканчиваться на число.");
                else if (Cabinets.Any(c => c.GetCabinetNumber() == Cabinet.GetCabinetNumber()))
                    Cabinet.AddError(nameof(Cabinet.Title), "Кабинет с таким номером уже есть.");

                if (Cabinet.HasErrors) return false;

                using DataBaseHelper db = new DataBaseHelper();
                if (!db.Cabinets.Any(c => c.Title == Cabinet.Title))
                {
                    db.Cabinets.Add(Cabinet);
                    db.SaveChanges();

                    return true;
                }
                else
                    Cabinet.AddError(nameof(Cabinet.Title), "Такой кабинет уже есть.");

                return false;
            });
        }
    }
}

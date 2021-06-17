using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class CabinetViewModel : BaseViewModel
    {
        public delegate void CabinetHandler();
        public event CabinetHandler UpdateCabinet;

        public delegate void SoftHandler(int computerId);
        public event SoftHandler OpenSoftsList;

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetValue(ref _isEditing, value, nameof(IsEditing));
        }

        private Cabinet _cabinet;
        public Cabinet Cabinet
        {
            get => _cabinet;
            set => SetValue(ref _cabinet, value, nameof(Cabinet));
        }

        private Computer _computer;
        public Computer Computer
        {
            get => _computer;
            set => SetValue(ref _computer, value, nameof(Computer));
        }

        private List<Computer> _computers;
        public List<Computer> Computers
        {
            get => _computers;
            set => SetValue(ref _computers, value, nameof(Computers));
        }

        private Soft _soft;
        public Soft Soft
        {
            get => _soft;
            set => SetValue(ref _soft, value, nameof(Soft));
        }

        private ObservableCollection<Soft> _softs;
        public ObservableCollection<Soft> Softs
        {
            get => _softs;
            set => SetValue(ref _softs, value, nameof(Softs));
        }

        private List<ComputerConfig> _computerConfigs;
        public List<ComputerConfig> ComputerConfigs
        {
            get => _computerConfigs;
            set => SetValue(ref _computerConfigs, value, nameof(ComputerConfigs));
        }

        public CabinetViewModel(Cabinet selectedCabinet)
        {
            IsEditing = false;
            Cabinet = selectedCabinet;
            LoadComputersAsync();
            using DataBaseHelper db = new DataBaseHelper();
            Computer = new Computer();
            Soft = new Soft();
            Softs = new ObservableCollection<Soft>();
        }

        private async void LoadComputersAsync()
        {
            await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                Computers = db.Computers.Where(c => c.Cabinet == Cabinet).ToList();
            });
        }

        private RelayCommand _editCabinetCommand;
        public RelayCommand EditCabinetCommand
        {
            get
            {
                return _editCabinetCommand ??= new RelayCommand(async o =>
                {
                    if (await CheckCabinetAsync())
                    {
                        using DataBaseHelper db = new DataBaseHelper();
                        UpdateCabinet?.Invoke();
                    }
                });
            }
        }

        private async Task<bool> CheckCabinetAsync()
        {
            return await Task.Run(() =>
            {
                Cabinet.ClearErrors();
                using DataBaseHelper db = new DataBaseHelper();

                if (string.IsNullOrEmpty(Cabinet.Title))
                    Cabinet.AddError(nameof(Cabinet.Title), "Введите название.");
                else if (!Cabinet.ContainsNumber())
                    Cabinet.AddError(nameof(Cabinet.Title), "Название должно оканчиваться на число.");
                else if ((db.Cabinets.SingleOrDefault(c => c.CabinetId == Cabinet.CabinetId).GetCabinetNumber() != Cabinet.GetCabinetNumber())
                    && (db.Cabinets.ToList().Any(c => c.GetCabinetNumber() == Cabinet.GetCabinetNumber())))
                    Cabinet.AddError(nameof(Cabinet.Title), "Кабинет с таким номером уже есть.");

                if (Cabinet.HasErrors) return false;

                db.Cabinets.SingleOrDefault(c => c.CabinetId == Cabinet.CabinetId).Title = Cabinet.Title;
                db.SaveChanges();
                return true;
            });
        }

        private RelayCommand _addComputerCommand;
        public RelayCommand AddComputerCommand
        {
            get
            {
                return _addComputerCommand ??= new RelayCommand(o =>
                {
                    if (IsComputerCorrect())
                    {
                        using DataBaseHelper db = new DataBaseHelper();
                        if (!IsEditing)
                        {
                            Computer.CreateInventoryNumber(Cabinet.GetCabinetNumber(), (db.Computers.Where(c => c.Cabinet == Cabinet).Count() + 1).ToString());
                            Computer.Softs = string.Join(", ", Softs);
                            Computer.Cabinet = db.Cabinets.SingleOrDefault(c => c.CabinetId == Cabinet.CabinetId);
                            db.Computers.Add(Computer);
                        }
                        else
                        {
                            Computer.Softs = string.Join(", ", Softs);
                            db.Computers.Update(Computer);
                            IsEditing = false;
                        }
                        db.SaveChanges();

                        Computer = new Computer();
                        Softs = new ObservableCollection<Soft>();
                        Computers = db.Computers.Where(c => c.Cabinet == Cabinet).ToList();
                    }
                });
            }
        }

        private bool IsComputerCorrect()
        {
            Computer.ClearErrors();
            Soft.ClearErrors();

            if (string.IsNullOrEmpty(Computer.CPUName))
                Computer.AddError(nameof(Computer.CPUName), "Наименование процессора необходимо заполнить");
            if (string.IsNullOrEmpty(Computer.GPUName))
                Computer.AddError(nameof(Computer.GPUName), "Наименование видеокарты необходимо заполнить");
            if (string.IsNullOrEmpty(Computer.RAMName))
                Computer.AddError(nameof(Computer.RAMName), "Наименование ОЗУ необходимо заполнить");
            if (Computer.RAMCapacity == null)
                Computer.AddError(nameof(Computer.RAMCapacity), "Объём ОЗУ необходимо заполнить");
            if (string.IsNullOrEmpty(Computer.DiskName))
                Computer.AddError(nameof(Computer.DiskName), "Наименование ЖД необходимо заполнить");
            if (Computer.DiskCapacity == null)
                Computer.AddError(nameof(Computer.DiskCapacity), "Объём ЖД необходимо заполнить");
            if (Softs.Count == 0)
                Soft.AddError(nameof(Soft.SoftName), "Необходимо добавить хотя бы одну программу");

            if ((Computer.HasErrors) || (Soft.HasErrors))
                return false;
            else
                return true;
        }

        private RelayCommand _editComputerCommand;
        public RelayCommand EditComputerCommand
        {
            get
            {
                return _editComputerCommand ??= new RelayCommand(o =>
                {
                    IsEditing = true;
                    Computer = Computers.SingleOrDefault(c => c.ComputerId == Convert.ToInt32(o));
                    Softs.Clear();
                    foreach (var soft in ((string)Computer.Softs).Split(", "))
                        Softs.Add(new Soft() { SoftName = soft });
                });
            }
        }

        private RelayCommand _deleteComputerCommand;
        public RelayCommand DeleteComputerCommand
        {
            get
            {
                return _deleteComputerCommand ??= new RelayCommand(o =>
                {
                    using DataBaseHelper db = new DataBaseHelper();
                    db.Remove(db.Computers.Single(x => x.ComputerId == Convert.ToInt32(o)));
                    db.SaveChanges();
                    Computers = db.Computers.Where(c => c.Cabinet == Cabinet).ToList();
                    Computer = new Computer();
                    Softs = new ObservableCollection<Soft>();
                    Soft = new Soft();
                    IsEditing = false;
                });
            }
        }

        private RelayCommand _addSoftCommand;
        public RelayCommand AddSoftCommand
        {
            get
            {
                return _addSoftCommand ??= new RelayCommand(o =>
                {
                    Soft.ClearErrors();

                    if (string.IsNullOrEmpty(Soft.SoftName))
                        Soft.AddError(nameof(Soft.SoftName), "Название программы необходимо заполнить");
                    if (Softs.Any(s => s.SoftName == Soft.SoftName))
                        Soft.AddError(nameof(Soft.SoftName), "Программа с таким именем уже существует");

                    if (!Soft.HasErrors)
                    {
                        Softs.Add(Soft);
                        Soft = new Soft();
                    }
                });
            }
        }

        private RelayCommand _deleteSoftCommand;
        public RelayCommand DeleteSoftCommand
        {
            get
            {
                return _deleteSoftCommand ??= new RelayCommand(o =>
                {
                    Softs.Remove((Soft)o);
                });
            }
        }

        private RelayCommand _useComputerConfigCommand;
        public RelayCommand UseComputerConfigCommand
        {
            get
            {
                return _useComputerConfigCommand ??= new RelayCommand(o =>
                {
                    using DataBaseHelper db = new DataBaseHelper();
                    ComputerConfigs = db.ComputerConfigs.ToList();
                });
            }
        }

        private RelayCommand _useConfigCommand;
        public RelayCommand UseConfigCommand
        {
            get
            {
                return _useConfigCommand ??= new RelayCommand(o =>
                {
                    if (o != null)
                    {
                        ComputerConfig computerConfig = o as ComputerConfig;
                        Computer = new Computer()
                        {
                            CPUName = computerConfig.CPUName,
                            GPUName = computerConfig.GPUName,
                            RAMName = computerConfig.RAMName,
                            RAMCapacity = computerConfig.RAMCapacity,
                            DiskName = computerConfig.DiskName,
                            DiskCapacity = computerConfig.DiskCapacity
                        };
                    }
                });
            }
        }

        private RelayCommand _softsViewCommand;
        public RelayCommand SoftsViewCommand
        {
            get
            {
                return _softsViewCommand ??= new RelayCommand(o =>
                {
                    if (o is int)
                        OpenSoftsList?.Invoke(Convert.ToInt32(o));
                });
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class ComputerConfigViewModel : BaseViewModel
    {
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => SetValue(ref _isEditing, value, nameof(IsEditing));
        }

        private ComputerConfig _computerConfig;
        public ComputerConfig ComputerConfig
        {
            get => _computerConfig;
            set => SetValue(ref _computerConfig, value, nameof(ComputerConfig));
        }

        private List<ComputerConfig> _computerConfigs;
        public List<ComputerConfig> ComputerConfigs
        {
            get => _computerConfigs;
            set => SetValue(ref _computerConfigs, value, nameof(ComputerConfigs));
        }

        public ComputerConfigViewModel()
        {
            ComputerConfig = new ComputerConfig();
            LoadConfigsAsync();
            IsEditing = false;
        }

        private async void LoadConfigsAsync()
        {
            await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                ComputerConfigs = db.ComputerConfigs.ToList();
            });
        }

        private RelayCommand _addComputerConfigCommand;
        public RelayCommand AddComputerConfigCommand
        {
            get
            {
                return _addComputerConfigCommand ??= new RelayCommand(o =>
                {
                    if (IsConfigCorrect())
                    {
                        using DataBaseHelper db = new DataBaseHelper();
                        db.ComputerConfigs.Add(ComputerConfig);
                        ComputerConfig = new ComputerConfig();
                        db.SaveChanges();
                        ComputerConfigs = db.ComputerConfigs.ToList();
                    }
                });
            }
        }

        private bool IsConfigCorrect()
        {
            ComputerConfig.ClearErrors();
            using DataBaseHelper db = new DataBaseHelper();

            if (string.IsNullOrEmpty(ComputerConfig.ConfigName))
                ComputerConfig.AddError(nameof(ComputerConfig.ConfigName), "Наименование конфигурации необходимо заполнить");
            else if (db.ComputerConfigs.Any(c => c.ConfigName == ComputerConfig.ConfigName))
                ComputerConfig.AddError(nameof(ComputerConfig.ConfigName), "Конфигурация с таким именем уже существует");
            if (string.IsNullOrEmpty(ComputerConfig.CPUName))
                ComputerConfig.AddError(nameof(ComputerConfig.CPUName), "Наименование процессора необходимо заполнить");
            if (string.IsNullOrEmpty(ComputerConfig.GPUName))
                ComputerConfig.AddError(nameof(ComputerConfig.GPUName), "Наименование видеокарты необходимо заполнить");
            if (string.IsNullOrEmpty(ComputerConfig.RAMName))
                ComputerConfig.AddError(nameof(ComputerConfig.RAMName), "Наименование ОЗУ необходимо заполнить");
            if (ComputerConfig.RAMCapacity == null)
                ComputerConfig.AddError(nameof(ComputerConfig.RAMCapacity), "Объём ОЗУ необходимо заполнить");
            if (string.IsNullOrEmpty(ComputerConfig.DiskName))
                ComputerConfig.AddError(nameof(ComputerConfig.DiskName), "Наименование ЖД необходимо заполнить");
            if (ComputerConfig.DiskCapacity == null)
                ComputerConfig.AddError(nameof(ComputerConfig.DiskCapacity), "Объём ЖД необходимо заполнить");

            if (ComputerConfig.HasErrors)
                return false;
            else
                return true;
        }

        private RelayCommand _editConfigCommand;
        public RelayCommand EditConfigCommand
        {
            get
            {
                return _editConfigCommand ??= new RelayCommand(o =>
                {
                    IsEditing = true;
                    ComputerConfig = new ComputerConfig();
                });
            }
        }

        private RelayCommand _deleteConfigCommand;
        public RelayCommand DeleteConfigCommand
        {
            get
            {
                return _deleteConfigCommand ??= new RelayCommand(o =>
                {
                    using DataBaseHelper db = new DataBaseHelper();
                    db.Remove(db.ComputerConfigs.Single(x => x.ComputerConfigId == Convert.ToInt32(o)));
                    db.SaveChanges();
                    ComputerConfigs = db.ComputerConfigs.ToList();
                });
            }
        }
    }
}

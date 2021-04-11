using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerAccounting
{
    public class LaboratoryInfoViewModel : BaseViewModel
    {
        private List<Cabinet> _cabinets;
        public List<Cabinet> Cabinets
        {
            get => _cabinets;
            set => SetValue(ref _cabinets, value, nameof(Cabinets));
        }

        private Cabinet _selectedCabinet;
        public Cabinet SelectedCabinet
        {
            get => _selectedCabinet;
            set => SetValue(ref _selectedCabinet, value, nameof(SelectedCabinet));
        }

        public LaboratoryInfoViewModel()
        {
            using DataBaseHelper db = new DataBaseHelper();
            LoadCabinetsAsync();
        }

        private async void LoadCabinetsAsync()
        {
            await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                Cabinets = db.Cabinets.ToList().OrderBy(c => c.GetCabinetNumber()).ToList();
            });
        }

        private RelayCommand _createDocumentCommand;
        public RelayCommand CreateDocumentCommand
        {
            get
            {
                return _createDocumentCommand ??= new RelayCommand(o =>
                {
                    if (SelectedCabinet != null)
                    {

                    }
                });
            }
        }
    }
}

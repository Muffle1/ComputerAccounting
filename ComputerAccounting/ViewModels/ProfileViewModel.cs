using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    public class ProfileViewModel : BaseViewModel
    {
        private IViewSwitcher _computerConfigControl;
        public IViewSwitcher ComputerConfigControl
        {
            get => _computerConfigControl;
            set => SetValue(ref _computerConfigControl, value, nameof(ComputerConfigControl));
        }

        private IViewSwitcher _laboratoryInfoControl;
        public IViewSwitcher LaboratoryInfoControl
        {
            get => _laboratoryInfoControl;
            set => SetValue(ref _laboratoryInfoControl, value, nameof(LaboratoryInfoControl));
        }

        private IViewSwitcher _fullInfoControl;
        public IViewSwitcher FullInfoControl
        {
            get => _fullInfoControl;
            set => SetValue(ref _fullInfoControl, value, nameof(FullInfoControl));
        }
    }
}

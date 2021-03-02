using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ComputerAccounting
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IViewSwitcher
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event ViewHandler SwitchView;

        public void SetValue<T>(ref T property, T value, string propertyName)
        {
            property = value;
            OnPropertyChanged(propertyName);
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public void OnViewSwitched(IViewSwitcher view, NameView nameView) =>
            SwitchView?.Invoke(this, new ViewEventArgs(view, nameView));
    }
}

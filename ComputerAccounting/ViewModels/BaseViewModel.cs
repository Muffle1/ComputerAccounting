using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace ComputerAccounting
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo, IViewSwitcher
    {
        public readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public BaseViewModel()
        {

        }

        public bool HasErrors => _propertyErrors.Any();

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public event ViewHandler SwitchView;

        public IEnumerable GetErrors(string propertyName) => _propertyErrors.GetValueOrDefault(propertyName, null);

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
                _propertyErrors.Add(propertyName, new List<string>());

            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void ClearErrors(string propertyName)
        {
            if (_propertyErrors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        public string Hash(string input) =>
            Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input)));

        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public void OnErrorsChanged(string propertyName) =>
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        public void OnViewSwitched(IViewSwitcher view, NameView nameView) =>
            SwitchView?.Invoke(this, new ViewEventArgs(view, nameView));
    }
}

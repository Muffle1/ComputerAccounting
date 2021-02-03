using ComputerAccounting.ViewModels;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerAccounting.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : UserControl
    {
        public AuthorizationPage()
        {
            InitializeComponent();
            DataContextChanged += AuthorizationPage_DataContextChanged;
        }

        private void AuthorizationPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as AuthorizationPage).DataContext is AuthorizationViewModel authorizationViewModel)
                authorizationViewModel.ShowError += AuthorizationViewModel_ShowError;
        }

        private void AuthorizationViewModel_ShowError(string errorMessage)
        {
            Border border = Password.PasswordField.Template.FindName("border", Password.PasswordField) as Border;

            if (errorMessage != "")
            {
                border.BorderBrush = new SolidColorBrush(Colors.Red);
                Password.Error.Text = errorMessage;
                Password.Error.Visibility = Visibility.Visible;
            }
            else
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#107a2e"));
                Password.Error.Text = errorMessage;
                Password.Error.Visibility = Visibility.Collapsed;
            }
        }
    }
}

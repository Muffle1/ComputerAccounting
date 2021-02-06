using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ComputerAccounting
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : UserControl
    {
        public RegistrationPage()
        {
            InitializeComponent();
            DataContextChanged += RegistrationPage_DataContextChanged;
        }

        private void RegistrationPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as RegistrationPage).DataContext is RegistrationViewModel registrationViewModel)
                registrationViewModel.ShowError += RegistrationViewModel_ShowError;
        }

        private void RegistrationViewModel_ShowError(string errorMessage)
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

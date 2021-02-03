﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ComputerAccounting.Components
{
    /// <summary>
    /// Логика взаимодействия для BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        private bool _isPasswordChanging;

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
                passwordBox.UpdatePassword();
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
                PasswordField.Password = Password;
        }

        private void PasswordField_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = PasswordField.Password;

            if (Password.Length == 0)
                (PasswordField.Template.FindName("Placeholder", PasswordField) as TextBlock).Visibility = Visibility.Visible;
            else
                (PasswordField.Template.FindName("Placeholder", PasswordField) as TextBlock).Visibility = Visibility.Collapsed;

            _isPasswordChanging = false;
        }
    }
}

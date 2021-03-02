using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ComputerAccounting
{
    public class BindablePasswordBox
    {
        private static bool _updating = false;

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
                typeof(string),
                typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(string.Empty, OnPasswordChanged));

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength",
                typeof(int),
                typeof(BindablePasswordBox));

        public static string GetPassword(DependencyObject d) => (string)d.GetValue(PasswordProperty);
        public static int GetPasswordLength(DependencyObject d) => (int)d.GetValue(PasswordLengthProperty);

        public static void SetPassword(DependencyObject d, string value) =>
            d.SetValue(PasswordProperty, value);
        public static void SetPasswordLength(DependencyObject d, int value) =>
            d.SetValue(PasswordLengthProperty, value);

        private static void OnPasswordChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordBox password = d as PasswordBox;

            if (password != null)
                password.PasswordChanged += new RoutedEventHandler(PasswordChanged);

            if (e.NewValue != null)
            {
                if (!_updating)
                    password.Password = e.NewValue.ToString();
            }
            else
                password.Password = string.Empty;


        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox password = sender as PasswordBox;

            _updating = true;

            SetPassword(password, password.Password);
            SetPasswordLength(password, password.Password.Length);

            _updating = false;
        }
    }
}

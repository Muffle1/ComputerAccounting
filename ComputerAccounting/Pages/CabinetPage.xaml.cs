using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComputerAccounting
{
    /// <summary>
    /// Логика взаимодействия для CabinetPage.xaml
    /// </summary>
    public partial class CabinetPage : UserControl
    {
        public CabinetPage()
        {
            InitializeComponent();
            DataContextChanged += CabinetPage_DataContextChanged;
        }

        private void CabinetPage_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext != null)
                (DataContext as CabinetViewModel).OpenSoftsList += CabinetPage_OpenSoftsList;
        }

        private void CabinetPage_OpenSoftsList(int computerId)
        {
            ComputersList.SelectedItem = ComputersList.Items.Cast<Computer>().SingleOrDefault(c => c.ComputerId == computerId);
            if (SoftPopup.Visibility == Visibility.Visible)
                SoftPopup.Visibility = Visibility.Collapsed;
            else
                SoftPopup.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ConfigPopup.Visibility == Visibility.Visible)
                ConfigPopup.Visibility = Visibility.Collapsed;
            else
                ConfigPopup.Visibility = Visibility.Visible;
        }

        private void ConfigPopup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ConfigPopup.Visibility == Visibility.Visible)
                ConfigPopup.Visibility = Visibility.Collapsed;

            if (SoftPopup.Visibility == Visibility.Visible)
                SoftPopup.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ConfigsList.SelectedItem != null)
                ConfigPopup.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ComputersList.SelectedItem = null;
            if (SoftPopup.Visibility == Visibility.Visible)
                SoftPopup.Visibility = Visibility.Collapsed;
            else
                SoftPopup.Visibility = Visibility.Visible;
        }
    }
}

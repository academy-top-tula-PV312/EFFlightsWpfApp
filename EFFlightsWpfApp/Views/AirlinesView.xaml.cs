using EFFlightsWpfApp.Model;
using EFFlightsWpfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EFFlightsWpfApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AirlinesView.xaml
    /// </summary>
    public partial class AirlinesView : Window
    {
        AirlineViewModel airlineViewModel = new();
        bool add;
        bool edit;
        public AirlinesView()
        {
            InitializeComponent();

            DataContext = airlineViewModel;
            gridAirlinesForm.IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            airlineViewModel.Dispose();
        }

        private void btnAirlineAdd_Click(object sender, RoutedEventArgs e)
        {
            if (add || edit) return;
            
            add = true;
            gridAirlinesForm.IsEnabled = true;

            Binding bindingTitle = new();
            bindingTitle.Mode = BindingMode.TwoWay;
            bindingTitle.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            bindingTitle.Source = airlineViewModel.AirlineNew;
            bindingTitle.Path = new PropertyPath("Title");
            airlineTitleTextBox.SetBinding(TextBox.TextProperty, bindingTitle);


            Binding bindingDescript = new();
            bindingDescript.Mode = BindingMode.TwoWay;
            bindingDescript.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            bindingDescript.Source = airlineViewModel.AirlineNew;
            bindingDescript.Path = new PropertyPath("Description");
            airlineDescriptionTextBox.SetBinding(TextBox.TextProperty, bindingDescript);

            Binding bindingActivity = new();
            bindingActivity.Mode = BindingMode.TwoWay;
            bindingActivity.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            bindingActivity.Source = airlineViewModel.AirlineNew;
            bindingActivity.Path = new PropertyPath("Activity");
            airlineActivityCheckBox.SetBinding(CheckBox.IsCheckedProperty, bindingActivity);


        }

        private void btnAirlineSave_Click(object sender, RoutedEventArgs e)
        {
            if(add)
            {
                if(!String.IsNullOrEmpty(airlineTitleTextBox.Text))
                {
                    Airline airline = new Airline()
                    {
                        Title = airlineTitleTextBox.Text,
                        City = citiesComboBox.SelectedItem as City,
                        Description = airlineDescriptionTextBox.Text,
                        Activity = airlineActivityCheckBox.IsChecked
                    };
                    (DataContext as AirlineViewModel).AddAirlineCommand
                                                     .Execute(airline);
                }
            }
        }
    }
}

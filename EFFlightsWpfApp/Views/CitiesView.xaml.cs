using EFFlightsWpfApp.Model;
using EFFlightsWpfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace EFFlightsWpfApp
{
    /// <summary>
    /// Логика взаимодействия для CitiesView.xaml
    /// </summary>
    public partial class CitiesView : Window
    {
        bool edit = false;
        bool add = false;

        

        public CitiesView()
        {
            InitializeComponent();


            DataContext = new CityViewModel();

            citiesListBox.DisplayMemberPath = "Title";

            stackCityTitle.Visibility = Visibility.Hidden;
        }

        private void btnCityAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!edit && !add)
            {
                add = true;
                stackCityTitle.Visibility = Visibility.Visible;
                cityTitleTextBox.Text = "";
            }
        }

        private void btnCityEdit_Click(object sender, RoutedEventArgs e)
        {
            if(citiesListBox.SelectedItem is null) return;

            if(!edit || !add)
            {
                edit = true;
                stackCityTitle.Visibility = Visibility.Visible;
                cityTitleTextBox.Text = (citiesListBox.SelectedItem as City).Title;
            }
        }

        private void btnCityDelete_Click(object sender, RoutedEventArgs e)
        {
            if(citiesListBox.SelectedItem is null) return;
            if (!edit || !add)
            {
                (DataContext as CityViewModel).DeleteCityCommand
                                              .Execute(citiesListBox.SelectedItem);
            }
        }

        private void btnCityTitleSave_Click(object sender, RoutedEventArgs e)
        {
            if(add)
            {
                if (cityTitleTextBox.Text.Trim() != "")
                {
                    (DataContext as CityViewModel).AddCityCommand
                                                  .Execute(new City() { Title = cityTitleTextBox.Text});
                    
                }
                add = false;
            }

            if(edit)
            {
                if(citiesListBox.SelectedItem is not null && !String.IsNullOrEmpty(cityTitleTextBox.Text.Trim()))
                {
                    (DataContext as CityViewModel).EditCityCommand
                                                  .Execute(
                                                    new City()
                                                    {
                                                        Id = (citiesListBox.SelectedItem as City).Id,
                                                        Title = cityTitleTextBox.Text
                                                    });
                    citiesListBox.Items.Refresh();
                }
                
                edit = false;
            }

            stackCityTitle.Visibility = Visibility.Hidden;  
        }

        private void btnCityTitleCancel_Click(object sender, RoutedEventArgs e)
        {
            cityTitleTextBox.Text = "";
            stackCityTitle.Visibility = Visibility.Hidden;
            edit = add = false;
        }
    }
}

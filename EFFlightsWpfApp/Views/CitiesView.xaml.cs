using EFFlightsWpfApp.Model;
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
        ObservableCollection<City> cities;
        bool edit = false;
        bool add = false;
        public CitiesView()
        {
            InitializeComponent();


            using (AirFlightsDbContext context = new())
            {
                cities = new(context.Cities.ToList());
            }

            citiesListBox.ItemsSource = cities;
            citiesListBox.DisplayMemberPath = "Title";

            stackCityTitle.Visibility = Visibility.Hidden;
        }

        private void btnCityAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!edit && !add)
            {
                add = true;
                stackCityTitle.Visibility = Visibility.Visible;
            }
        }

        private void btnCityEdit_Click(object sender, RoutedEventArgs e)
        {
            if(!edit && !add)
            {
                edit = true;
                stackCityTitle.Visibility = Visibility.Visible;
                cityTitleTextBox.Text = (citiesListBox.SelectedItem as City).Title;
            }
        }

        private void btnCityDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCityTitleSave_Click(object sender, RoutedEventArgs e)
        {
            if(add)
            {
                if (cityTitleTextBox.Text.Trim() != "")
                {
                    City city = new() { Title = cityTitleTextBox.Text };
                    using (AirFlightsDbContext context = new())
                    {
                        cities.Add(city);
                        context.Cities.Add(city);
                        context.SaveChanges();
                    }
                }
                add = false;
            }

            if(edit)
            {
                if(citiesListBox.SelectedItem is not null)
                {
                    City cityList = citiesListBox.SelectedItem as City;
                    using (AirFlightsDbContext context = new())
                    {
                        City cityDb = context.Cities.Find(cityList.Id) as City;
                        cityList.Title = cityDb.Title = cityTitleTextBox.Text;
                        context.SaveChanges();
                    }
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

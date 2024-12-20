using EFFlightsWpfApp.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для AirportsView.xaml
    /// </summary>
    public partial class AirportsView : Window
    {
        bool add;
        bool edit;
        AirportsViewModel airportsViewModel = new();

        public AirportsView()
        {
            InitializeComponent();

            this.DataContext = airportsViewModel;
            gridAirportForm.IsEnabled = false;
            add = edit = false;
        }

        private void btnAirportAdd_Click(object sender, RoutedEventArgs e)
        {
            if (add || edit) return;

            add = true;
            gridAirportForm.IsEnabled = true;
            gridAirportsList.IsEnabled = false;
            airportsViewModel.AirportNew = new();
        }

        private void btnAirportEdit_Click(object sender, RoutedEventArgs e)
        {
            if (add || edit) return;

            edit = true;
            gridAirportForm.IsEnabled = true;
            gridAirportsList.IsEnabled = false;

            var airport = airportsViewModel.AirportSelect;
            airportsViewModel.AirportNew = new()
            {
                Id = airport.Id,
                Title = airport.Title,
                Description = airport.Description,
                ImageUrl = airport.ImageUrl,
                City = airport.City,
                CityId = airport.CityId,
            };
            //comboBoxCities.SelectedItem = airport.City;
            airportsViewModel.CitySelect = airportsViewModel.Cities
                                                            .FirstOrDefault(c => c.Id == airport.CityId);
        }

        private void btnAirportDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPhotoLoad_Click(object sender, RoutedEventArgs e)
        {
            string? filePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if(openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;

                airportsViewModel.AirportImage = filePath;


                //string fileName = filePath.Substring(filePath.LastIndexOf(@"\") + 1);

                //string appImagesPath = Directory.GetCurrentDirectory() + "/Images/Airports";
                //if(!Directory.Exists(appImagesPath))
                //    Directory.CreateDirectory(appImagesPath);

                //File.Copy(filePath, Directory.GetCurrentDirectory() + "/Images/Airports/" + fileName);
                //MessageBox.Show(Directory.GetCurrentDirectory() + @"\Images\Airports\" + fileName);
            }


        }

        private void btnAirportSave_Click(object sender, RoutedEventArgs e)
        {
            if(add)
            {
                //MessageBox.Show(airportsViewModel.AirportImage);
                airportsViewModel.AddAirportCommand.Execute(null);

                
            }

            add = edit = false;
            airportsViewModel.AirportNew = null;
            airportsViewModel.AirportImage = null;
            airportsViewModel.CitySelect = null;

            gridAirportForm.IsEnabled = false;
            gridAirportsList.IsEnabled = true;
        }

        private void btnAirportCancel_Click(object sender, RoutedEventArgs e)
        {
            add = edit = false;
            airportsViewModel.AirportNew = null;
            airportsViewModel.AirportImage = null;
            airportsViewModel.CitySelect = null;

            gridAirportForm.IsEnabled = false;
            gridAirportsList.IsEnabled = true;
        }
    }
}

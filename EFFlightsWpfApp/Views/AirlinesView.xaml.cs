using EFFlightsWpfApp.Model;
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
    /// Логика взаимодействия для AirlinesView.xaml
    /// </summary>
    public partial class AirlinesView : Window
    {
        AirlineViewModel airlineViewModel = new();
        bool add;
        bool edit;
        //byte[]? imageBuffer = null;
        public AirlinesView()
        {
            InitializeComponent();

            DataContext = airlineViewModel;
            gridAirlinesForm.IsEnabled = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //airlineViewModel.Dispose();
        }


        private void btnAirlineAdd_Click(object sender, RoutedEventArgs e)
        {
            if (add || edit) return;
            
            add = true;
            gridAirlinesForm.IsEnabled = true;
            gridAirlinesList.IsEnabled = false;
            airlineViewModel.AirlineNew = new();

            AirlineFormBinding(airlineViewModel.AirlineNew);
        }
        private void btnAirlineEdit_Click(object sender, RoutedEventArgs e)
        {
            if(add || edit) return;

            edit = true;
            gridAirlinesForm.IsEnabled = true;
            gridAirlinesList.IsEnabled = false;

            airlineViewModel.CitySelect = airlineViewModel.Cities
                                                          .FirstOrDefault(c => c.Id == airlineViewModel.AirlineSelect.CityId)!;

            airlineViewModel.AirlineNew = new()
            {
                Id = airlineViewModel.AirlineSelect.Id,
                Title = airlineViewModel.AirlineSelect.Title,
                City = airlineViewModel.AirlineSelect.City,
                CityId = airlineViewModel.AirlineSelect.CityId,
                Activity = airlineViewModel.AirlineSelect.Activity,
            };

            AirlineFormBinding(airlineViewModel.AirlineNew);

            byte[]? imageBuffer = airlineViewModel.AirlineSelect.Logotype;
            
            if (imageBuffer != null && imageBuffer.Length > 0)
                airlineLogotypeImage.Source = GetImageFromBytes(imageBuffer);
        }
        private void btnAirlineDelete_Click(object sender, RoutedEventArgs e)
        {
            if (airlineViewModel.AirlineSelect is not null)
                (DataContext as AirlineViewModel).DeleteAirlineCommand
                                                 .Execute(null);
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
                        Activity = airlineActivityCheckBox.IsChecked,
                        Logotype = airlineViewModel.AirlineNew.Logotype,
                    };
                    (DataContext as AirlineViewModel).AddAirlineCommand
                                                     .Execute(airline);
                    AirlineFormClear();
                    gridAirlinesForm.IsEnabled = false;
                    gridAirlinesList.IsEnabled = true;
                    add = false;
                }
            }

            if(edit)
            {
                if(!String.IsNullOrEmpty(airlineTitleTextBox.Text))
                {
                    airlineViewModel.AirlineNew.CityId = (citiesComboBox.SelectedItem as City)!.Id;
                    //airlineViewModel.AirlineNew.City = (citiesComboBox.SelectedItem as City);

                    (DataContext as AirlineViewModel).EditAirlineCommand
                                                     .Execute(null);

                    AirlineFormClear();
                    gridAirlinesForm.IsEnabled = false;
                    gridAirlinesList.IsEnabled = true;
                    edit = false;

                    airlinesDataGrid.Items.Refresh();
                }
            }
        }
        private void btnAirlineCancel_Click(object sender, RoutedEventArgs e)
        {
            if(edit)
            {
                BindingOperations.ClearBinding(airlineTitleTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(airlineDescriptionTextBox, TextBox.TextProperty);
                BindingOperations.ClearBinding(airlineActivityCheckBox, CheckBox.IsCheckedProperty);
            }
            AirlineFormClear();
            gridAirlinesForm.IsEnabled = false;
            gridAirlinesList.IsEnabled = true;
            add = false;
            edit = false;
        }
        private void btnAirlineLogotypeLoad_Click(object sender, RoutedEventArgs e)
        {
            string? fileName = null;
            long fileLength = 0;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                fileLength = (new FileInfo(fileName)).Length;
            }

            if (string.IsNullOrEmpty(fileName)) return;

            //airlineViewModel.LogoSource = "";
            airlineLogotypeImage.Source = null;

            byte[] imageBuffer;
            using (BinaryReader reader = new(File.Open(fileName, FileMode.Open)))
            {
                imageBuffer = reader.ReadBytes(((int)fileLength));
            }

            airlineViewModel.AirlineNew.Logotype = imageBuffer;

            airlineLogotypeImage.Source = GetImageFromBytes(imageBuffer);
            
        }


        private void AirlineFormClear()
        {
            airlineTitleTextBox.Text = String.Empty;
            citiesComboBox.SelectedItem = null;
            airlineDescriptionTextBox.Text = String.Empty;
            airlineActivityCheckBox.IsChecked = false;
            airlineLogotypeImage.Source = null;
        }

        private void AirlineFormBinding(Airline airline)
        {
            Binding bindingTitle = new();
            bindingTitle.Mode = BindingMode.TwoWay;
            bindingTitle.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            bindingTitle.Source = airline;
            bindingTitle.Path = new PropertyPath("Title");
            airlineTitleTextBox.SetBinding(TextBox.TextProperty, bindingTitle);

            Binding bindingDescript = new();
            bindingDescript.Mode = BindingMode.TwoWay;
            bindingDescript.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            bindingDescript.Source = airline;
            bindingDescript.Path = new PropertyPath("Description");
            airlineDescriptionTextBox.SetBinding(TextBox.TextProperty, bindingDescript);

            Binding bindingActivity = new();
            bindingActivity.Mode = BindingMode.TwoWay;
            bindingActivity.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            bindingActivity.Source = airline;
            bindingActivity.Path = new PropertyPath("Activity");
            airlineActivityCheckBox.SetBinding(CheckBox.IsCheckedProperty, bindingActivity);
        }

        private BitmapImage? GetImageFromBytes(byte[] imageBuffer)
        {
            if (imageBuffer is null || imageBuffer.Length == 0) return null;

            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream imageStream = new(imageBuffer))
            {
                imageStream.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource = imageStream;
                bitmapImage.EndInit();
            }
            bitmapImage.Freeze();

            return bitmapImage;
        }
    }
}

using EFFlightsWpfApp.Model;
using EFFlightsWpfApp.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EFFlightsWpfApp.ViewModels
{
    public class AirportsViewModel : INotifyPropertyChanged
    {
        private Airport? airportSelect;
        private Airport? airportNew;
        private City? citySelect;
        private string? airportImage;

        AirFlightsDbContext context = new AirFlightsDbContext();
        public ObservableCollection<Airport> Airports { get; set; }
        public Airport? AirportSelect
        {
            get => airportSelect;
            set
            {
                airportSelect = value;
                OnPropertyChanged();
            }
        }
        public Airport? AirportNew
        {
            get => airportNew;
            set
            {
                airportNew = value;
                OnPropertyChanged();
            }
        }
        public City? CitySelect
        {
            get => citySelect;
            set
            {
                citySelect = value;
                OnPropertyChanged();
            }
        }
        public string? AirportImage
        {
            get => airportImage;
            set
            {
                airportImage = value;
                OnPropertyChanged();
            }
        }
        public List<City> Cities { get; set; } = new();


        private FlightsCommand addAirportCommand;
        private FlightsCommand editAirportCommand;
        private FlightsCommand deleteAirportCommand;
        
        public FlightsCommand AddAirportCommand
        {
            get => addAirportCommand ??
                (addAirportCommand = new FlightsCommand(
                    _ =>
                    {
                        AirportNew.City = CitySelect;

                        if(!String.IsNullOrEmpty(AirportImage))
                        {
                            string fileName = AirportImage.Substring(AirportImage.LastIndexOf(@"\") + 1);
                            string appImagesPath = "/Images/Airports/";

                            if (!Directory.Exists(appImagesPath))
                                Directory.CreateDirectory(Directory.GetCurrentDirectory() + appImagesPath);

                            File.Copy(AirportImage, Directory.GetCurrentDirectory() + appImagesPath + fileName, true);

                            AirportNew.ImageUrl = fileName;
                        }

                        Airports.Add(AirportNew);

                        context.UpdateRange(Airports);
                        context.SaveChanges();
                        //
                    }));
        }

        public AirportsViewModel()
        {
            //AirFlightsDbContext context = new();
            
            Airports = new ObservableCollection<Airport>(context.Airports.Include(a => a.City));
            Cities = new List<City>(context.Cities);
            
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged is not null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

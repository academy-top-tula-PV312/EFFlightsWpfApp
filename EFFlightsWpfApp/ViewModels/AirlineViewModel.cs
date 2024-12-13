using EFFlightsWpfApp.Model;
using EFFlightsWpfApp.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EFFlightsWpfApp.ViewModels
{
    public class AirlineViewModel : INotifyPropertyChanged, IDisposable
    {
        private Airline airlineSelect;
        private Airline airlineNew = new();
        private City citySelect;

        private FlightsCommand addAirlineCommand;
        private FlightsCommand editAirlineCommand;
        private FlightsCommand deleteAirlineCommand;

        public ObservableCollection<Airline> Airlines { get; set; } = new();
        public List<City> Cities { get; set; } = new();
        public Airline AirlineSelect
        {
            get => airlineSelect;
            set
            {
                airlineSelect = value;
                OnPropertyChanged();
            }
        }
        public Airline AirlineNew
        {
            get => airlineNew;
            set
            {
                airlineNew = value;
                OnPropertyChanged();
            }
        }
        public City CitySelect
        {
            get => citySelect;
            set
            {
                citySelect = value;
                OnPropertyChanged();
            }
        }

        public AirlineViewModel()
        {
            using (AirFlightsDbContext context = new())
            {
                Cities = context.Cities.ToList();
                Airlines = new(context.Airlines.Include(a => a.City).ToList());
            }
        }

        public FlightsCommand AddAirlineCommand
        {
            get {
                return addAirlineCommand ??
                (addAirlineCommand = new FlightsCommand(
                    obj =>
                    {
                        Airline airline = obj as Airline;
                        Airlines.Add(airline);
                    }
                ));
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged is not null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            using (AirFlightsDbContext context = new())
            {
                context.Airlines.UpdateRange(Airlines);
                context.SaveChanges();
            }
        }
    }
}

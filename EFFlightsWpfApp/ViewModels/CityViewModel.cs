using EFFlightsWpfApp.Model;
using EFFlightsWpfApp.Types;
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
    public class CityViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<City> Cities { get; set; } = new();
        private City? citySelect;

        private FlightsCommand addCityCommand;
        private FlightsCommand editCityCommand;
        private FlightsCommand deleteCityCommand;
        public FlightsCommand AddCityCommand
        {
            get
            {
                return addCityCommand ??
                    (addCityCommand = new FlightsCommand(obj =>
                    {
                        Cities.Add(obj as City);
                        CitySelect = obj as City;

                        using (AirFlightsDbContext context = new())
                        {
                            var cities = context.Cities;
                            cities.Add(obj as City);
                            context.SaveChanges();
                        }
                    }));
            }
        }

        public FlightsCommand EditCityCommand
        {
            get
            {
                return editCityCommand ??
                    (editCityCommand = new FlightsCommand(obj =>
                    {
                        City city = obj as City;
                        Cities.FirstOrDefault(c => c.Id == city.Id).Title = city.Title;

                        using (AirFlightsDbContext context = new())
                        {
                            context.Cities.Find(city!.Id)!.Title = city.Title;
                            context.SaveChanges();
                        }                            
                    },
                    obj =>
                    {
                        return Cities.Count > 0 && citySelect is not null;
                    }));
                    
            }
        }

        public FlightsCommand DeleteCityCommand
        {
            get
            {
                return deleteCityCommand ??
                    (deleteCityCommand = new FlightsCommand(obj =>
                    {
                        if (obj is null) return;

                        City city = obj as City;
                        //int? id = obj as int?;
                        //var cityDelete = Cities.FirstOrDefault(c => c.Id == id);
                        Cities.Remove(city);

                        using(AirFlightsDbContext context = new())
                        {
                            context.Cities.Remove(context.Cities.Find(city.Id));
                            context.SaveChanges();
                        }
                    },
                    obj =>
                    {
                        return Cities.Count > 0 && citySelect is not null;
                    }));
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

        public CityViewModel()
        {
            using (AirFlightsDbContext context = new())
            {
                Cities = new(context.Cities.ToList());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged is not null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

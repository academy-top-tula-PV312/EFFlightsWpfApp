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
        private string logoSource;

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

        public string LogoSource
        {
            get => logoSource;
            set
            {
                logoSource = value;
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

                        using (AirFlightsDbContext context = new())
                        {
                            context.Airlines.UpdateRange(Airlines.ToArray());
                            context.SaveChanges();
                        }
                    }
                ));
            }
        }

        public FlightsCommand EditAirlineCommand
        {
            get
            {
                return editAirlineCommand ??
                    (editAirlineCommand = new FlightsCommand(
                        obj =>
                        {
                            Airline airline = Airlines.FirstOrDefault(a => a.Id == AirlineSelect.Id);

                            airline.Title = AirlineNew.Title;
                            airline.CityId = CitySelect.Id;
                            //airline.City = CitySelect;
                            airline.Activity = AirlineNew.Activity;
                            airline.Logotype = AirlineNew.Logotype;

                            using (AirFlightsDbContext context = new())
                            {
                                var airlineContext = context.Airlines.FirstOrDefault(a => a.Id == AirlineSelect.Id);
                                if(airlineContext != null)
                                {
                                    airlineContext.Title = AirlineNew.Title;
                                    airlineContext.CityId = CitySelect.Id;
                                    airlineContext.City = CitySelect;
                                    airlineContext.Activity = AirlineNew.Activity;
                                    airlineContext.Logotype = AirlineNew.Logotype;
                                    //context.Update(airlineContext);
                                    context.SaveChanges(true);
                                }

                                //context.Airlines.UpdateRange(Airlines);
                                //context.SaveChanges();
                            }
                        },
                        _ =>
                        {
                            return (AirlineSelect is not null);
                        }
                        ));
            }
        }

        public FlightsCommand DeleteAirlineCommand
        {
            get
            {
                return deleteAirlineCommand ??
                    (deleteAirlineCommand = new FlightsCommand(
                        _ =>
                        {
                            int idDelete = AirlineSelect.Id;
                            var airlineDelete = Airlines.FirstOrDefault(a => a.Id == idDelete);
                            Airlines.Remove(airlineDelete);

                            using (AirFlightsDbContext context = new())
                            {
                                airlineDelete = context.Airlines.FirstOrDefault(a => a.Id == idDelete);
                                context.Airlines.Remove(airlineDelete);
                                context.SaveChanges();
                            }
                        }));
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

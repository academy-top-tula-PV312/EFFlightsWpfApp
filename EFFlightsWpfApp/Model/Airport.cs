using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class Airport
{
    public int Id { get; set; }

    public int? CityId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool? Activity { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<Flight> FlightAirportFroms { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightAirportTos { get; set; } = new List<Flight>();

    public virtual ICollection<Airline> Airlines { get; set; } = new List<Airline>();
}

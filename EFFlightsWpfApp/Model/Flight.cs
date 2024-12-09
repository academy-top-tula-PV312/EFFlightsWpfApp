using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class Flight
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public int Duration { get; set; }

    public int? AirplaneId { get; set; }

    public int AirportFromId { get; set; }

    public int AirportToId { get; set; }

    public virtual Airplane? Airplane { get; set; }

    public virtual Airport AirportFrom { get; set; } = null!;

    public virtual Airport AirportTo { get; set; } = null!;

    public virtual ICollection<FlightsSalon> FlightsSalons { get; set; } = new List<FlightsSalon>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

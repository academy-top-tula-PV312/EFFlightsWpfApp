using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class SalonClass
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool Activity { get; set; }

    public virtual ICollection<FlightsSalon> FlightsSalons { get; set; } = new List<FlightsSalon>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}

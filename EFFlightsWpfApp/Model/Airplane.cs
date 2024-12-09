using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class Airplane
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public int ModelId { get; set; }

    public string? Title { get; set; }

    public int AirlineId { get; set; }

    public bool? Activity { get; set; }

    public virtual Airline Airline { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public virtual Model Model { get; set; } = null!;
}

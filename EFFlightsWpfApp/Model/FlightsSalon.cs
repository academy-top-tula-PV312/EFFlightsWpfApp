using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class FlightsSalon
{
    public int FlightId { get; set; }

    public int SalonId { get; set; }

    public decimal? Price { get; set; }

    public virtual Flight Flight { get; set; } = null!;

    public virtual SalonClass Salon { get; set; } = null!;
}

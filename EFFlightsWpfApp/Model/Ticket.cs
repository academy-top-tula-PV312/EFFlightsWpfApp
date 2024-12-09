using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class Ticket
{
    public Guid Number { get; set; }

    public int FlightId { get; set; }

    public int SalonId { get; set; }

    public int PassengerId { get; set; }

    public string Place { get; set; } = null!;

    public virtual Flight Flight { get; set; } = null!;

    public virtual Passenger Passenger { get; set; } = null!;

    public virtual SalonClass Salon { get; set; } = null!;
}

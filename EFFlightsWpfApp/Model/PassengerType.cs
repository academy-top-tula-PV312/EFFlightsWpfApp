using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class PassengerType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public float Factor { get; set; }

    public bool Activity { get; set; }

    public virtual ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();
}

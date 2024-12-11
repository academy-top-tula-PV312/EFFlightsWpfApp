using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EFFlightsWpfApp.Model;

public partial class City
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public virtual ICollection<Airline> Airlines { get; set; } = new List<Airline>();

    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();

    
}

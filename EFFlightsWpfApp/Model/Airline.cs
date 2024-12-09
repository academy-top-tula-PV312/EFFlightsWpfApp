using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class Airline
{
    public int Id { get; set; }

    public int? CityId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public byte[]? Logotype { get; set; }

    public bool? Activity { get; set; }

    public virtual ICollection<Airplane> Airplanes { get; set; } = new List<Airplane>();

    public virtual City? City { get; set; }

    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();
}

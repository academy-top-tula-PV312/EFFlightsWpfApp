using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class AirplanesListFull
{
    public string Number { get; set; } = null!;

    public string Airline { get; set; } = null!;

    public string Maker { get; set; } = null!;

    public string Model { get; set; } = null!;
}

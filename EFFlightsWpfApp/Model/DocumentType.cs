using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class DocumentType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool Activity { get; set; }

    public virtual ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();
}

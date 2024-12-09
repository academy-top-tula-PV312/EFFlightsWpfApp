using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class Model
{
    public int Id { get; set; }

    public int MakerId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool? Activity { get; set; }

    public virtual ICollection<Airplane> Airplanes { get; set; } = new List<Airplane>();

    public virtual Maker Maker { get; set; } = null!;
}

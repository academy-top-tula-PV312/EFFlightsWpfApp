using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class Maker
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool? Activity { get; set; }

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}

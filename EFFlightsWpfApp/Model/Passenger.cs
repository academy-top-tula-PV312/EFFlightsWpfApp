using System;
using System.Collections.Generic;

namespace EFFlightsWpfApp.Model;

public partial class Passenger
{
    public int Id { get; set; }

    public string LastName { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public int TypeId { get; set; }

    public DateOnly BirthDate { get; set; }

    public int DocumentId { get; set; }

    public string Series { get; set; } = null!;

    public string Number { get; set; } = null!;

    public virtual DocumentType Document { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual PassengerType Type { get; set; } = null!;
}

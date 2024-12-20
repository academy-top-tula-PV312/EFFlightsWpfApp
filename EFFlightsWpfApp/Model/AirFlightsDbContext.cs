using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFFlightsWpfApp.Model;

public partial class AirFlightsDbContext : DbContext
{
    public AirFlightsDbContext()
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public AirFlightsDbContext(DbContextOptions<AirFlightsDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public virtual DbSet<Airline> Airlines { get; set; }

    public virtual DbSet<Airplane> Airplanes { get; set; }

    public virtual DbSet<AirplanesListFull> AirplanesListFulls { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightsSalon> FlightsSalons { get; set; }

    public virtual DbSet<Maker> Makers { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<PassengerType> PassengerTypes { get; set; }

    public virtual DbSet<SalonClass> SalonClasses { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=3-0;Initial Catalog=air_flights_db;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airline>(entity =>
        {
            entity.ToTable("airlines");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasDefaultValue(true)
                .HasColumnName("activity");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Logotype).HasColumnName("logotype");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.City).WithMany(p => p.Airlines)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_airlines_cities");
        });

        modelBuilder.Entity<Airplane>(entity =>
        {
            entity.ToTable("airplanes");

            entity.HasIndex(e => e.AirlineId, "IX_airplanes_airline_id");

            entity.HasIndex(e => e.ModelId, "IX_airplanes_model_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasDefaultValue(true)
                .HasColumnName("activity");
            entity.Property(e => e.AirlineId).HasColumnName("airline_id");
            entity.Property(e => e.ModelId).HasColumnName("model_id");
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .HasColumnName("number");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Airline).WithMany(p => p.Airplanes)
                .HasForeignKey(d => d.AirlineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_airplanes_airlines");

            entity.HasOne(d => d.Model).WithMany(p => p.Airplanes)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_airplanes_models");
        });

        modelBuilder.Entity<AirplanesListFull>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("airplanes_list_full");

            entity.Property(e => e.Airline)
                .HasMaxLength(100)
                .HasColumnName("airline");
            entity.Property(e => e.Maker)
                .HasMaxLength(50)
                .HasColumnName("maker");
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .HasColumnName("model");
            entity.Property(e => e.Number)
                .HasMaxLength(50)
                .HasColumnName("number");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.ToTable("airports");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasDefaultValue(true)
                .HasColumnName("activity");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasColumnName("image_url");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.City).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_airports_cities");

            entity.HasMany(d => d.Airlines).WithMany(p => p.Airports)
                .UsingEntity<Dictionary<string, object>>(
                    "AirportsAirline",
                    r => r.HasOne<Airline>().WithMany()
                        .HasForeignKey("AirlineId")
                        .HasConstraintName("FK_airports_airlines_airlines"),
                    l => l.HasOne<Airport>().WithMany()
                        .HasForeignKey("AirportId")
                        .HasConstraintName("FK_airports_airlines_airports"),
                    j =>
                    {
                        j.HasKey("AirportId", "AirlineId");
                        j.ToTable("airports_airlines");
                        j.IndexerProperty<int>("AirportId").HasColumnName("airport_id");
                        j.IndexerProperty<int>("AirlineId").HasColumnName("airline_id");
                    });
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("cities");

            entity.HasIndex(e => e.Title, "UQ_cities_title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.ToTable("document_types");

            entity.HasIndex(e => e.Title, "UQ_document_types_title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasDefaultValue(true)
                .HasColumnName("activity");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Flight>(entity =>
        {
            entity.ToTable("flights");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AirplaneId).HasColumnName("airplane_id");
            entity.Property(e => e.AirportFromId).HasColumnName("airport_from_id");
            entity.Property(e => e.AirportToId).HasColumnName("airport_to_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("date");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");
            entity.Property(e => e.Time).HasColumnName("time");

            entity.HasOne(d => d.Airplane).WithMany(p => p.Flights)
                .HasForeignKey(d => d.AirplaneId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_flights_airplanes");

            entity.HasOne(d => d.AirportFrom).WithMany(p => p.FlightAirportFroms)
                .HasForeignKey(d => d.AirportFromId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_flights_airports_from");

            entity.HasOne(d => d.AirportTo).WithMany(p => p.FlightAirportTos)
                .HasForeignKey(d => d.AirportToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_flights_airports_to");
        });

        modelBuilder.Entity<FlightsSalon>(entity =>
        {
            entity.HasKey(e => new { e.FlightId, e.SalonId });

            entity.ToTable("flights_salons");

            entity.Property(e => e.FlightId).HasColumnName("flight_id");
            entity.Property(e => e.SalonId).HasColumnName("salon_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("price");

            entity.HasOne(d => d.Flight).WithMany(p => p.FlightsSalons)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("FK_flights_salons_flights");

            entity.HasOne(d => d.Salon).WithMany(p => p.FlightsSalons)
                .HasForeignKey(d => d.SalonId)
                .HasConstraintName("FK_flights_salons_salons");
        });

        modelBuilder.Entity<Maker>(entity =>
        {
            entity.ToTable("makers");

            entity.HasIndex(e => e.Title, "UQ_makers_title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasDefaultValue(true)
                .HasColumnName("activity");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.ToTable("models");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasDefaultValue(true)
                .HasColumnName("activity");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasColumnName("image_url");
            entity.Property(e => e.MakerId).HasColumnName("maker_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Maker).WithMany(p => p.Models)
                .HasForeignKey(d => d.MakerId)
                .HasConstraintName("FK_models_makers");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.ToTable("passengers");

            entity.HasIndex(e => new { e.DocumentId, e.Series, e.Number }, "IX_passengers_documnet").IsUnique();

            entity.HasIndex(e => new { e.LastName, e.FirstName, e.MiddleName, e.BirthDate }, "IX_passengers_full_name");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasColumnName("middle_name");
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .HasColumnName("number");
            entity.Property(e => e.Series)
                .HasMaxLength(10)
                .HasColumnName("series");
            entity.Property(e => e.TypeId).HasColumnName("type_id");

            entity.HasOne(d => d.Document).WithMany(p => p.Passengers)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_passengers_document_types");

            entity.HasOne(d => d.Type).WithMany(p => p.Passengers)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_passengers_passenger_types");
        });

        modelBuilder.Entity<PassengerType>(entity =>
        {
            entity.ToTable("passenger_types");

            entity.HasIndex(e => e.Title, "UQ_passenger_types_title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasDefaultValue(true)
                .HasColumnName("activity");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Factor)
                .HasDefaultValueSql("((1.0))")
                .HasColumnName("factor");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<SalonClass>(entity =>
        {
            entity.ToTable("salon_classes");

            entity.HasIndex(e => e.Title, "UQ_salon_classes_title").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activity)
                .HasDefaultValue(true)
                .HasColumnName("activity");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Number);

            entity.ToTable("tickets");

            entity.HasIndex(e => new { e.FlightId, e.PassengerId }, "UQ_tickets_flight_passenger").IsUnique();

            entity.HasIndex(e => new { e.FlightId, e.Place }, "UQ_tickets_flight_place").IsUnique();

            entity.Property(e => e.Number)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("number");
            entity.Property(e => e.FlightId).HasColumnName("flight_id");
            entity.Property(e => e.PassengerId).HasColumnName("passenger_id");
            entity.Property(e => e.Place)
                .HasMaxLength(5)
                .HasColumnName("place");
            entity.Property(e => e.SalonId).HasColumnName("salon_id");

            entity.HasOne(d => d.Flight).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("FK_tickets_flights");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.PassengerId)
                .HasConstraintName("FK_tickets_passengers");

            entity.HasOne(d => d.Salon).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SalonId)
                .HasConstraintName("FK_tickets_salons");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

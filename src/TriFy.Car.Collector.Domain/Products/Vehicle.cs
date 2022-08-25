using System;
using Ardalis.GuardClauses;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace TriFy.Car.Collector.Products
{
    public class Vehicle : Entity<Guid>
    {
        public const int ComplementaryDataMaxLength = 4000;

        public DateTime CreateDate { get; set; }
        public string ComplementaryData { get; set; }
        public Guid? VehicleId { get; set; }
        public bool IsDeleted { get; set; }
        public string LicensePlate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public Vehicle(Guid id)
            : this()
        {
            Guard.Against.NullOrEmpty(id, nameof(id));
            
            Id = id;
        }

        public Vehicle()
        {
            IsDeleted = false;
            CreateDate = DateTime.Now;
        }

        public void Copy(Vehicle vehicle)
        {
            Model = vehicle.Model;
            Brand = vehicle.Brand;
            LicensePlate = vehicle.LicensePlate;
            Year = vehicle.Year;
            ComplementaryData = vehicle.ComplementaryData;
        }
    }
}

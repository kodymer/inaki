using Microsoft.EntityFrameworkCore;
using TriFy.Car.Collector.Products;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace TriFy.Car.Collector.EntityFrameworkCore
{
    public static class CarsPricerDbContextModelCreatingExtensions
    {
        public static void ConfigureCarCollector(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            builder.Entity<Vehicle>(b =>
            {

                b.ToTable(CarCollectorConsts.DbTablePrefix + "LisencePlates", CarCollectorConsts.DbSchema);
                b.HasKey(e => e.Id);
                
                b.Property(e => e.Id).ValueGeneratedOnAdd();
                b.Property(e => e.Model).HasColumnType("nvarchar(MAX)").HasColumnName("ModelName");
                b.Property(e => e.Brand).HasColumnType("nvarchar(MAX)").HasColumnName("BrandName");
                b.Property(e => e.LicensePlate).HasMaxLength(20).HasDefaultValueSql("('')");
                b.Property(e => e.ComplementaryData).HasMaxLength(Vehicle.ComplementaryDataMaxLength).HasColumnName("ComplementeData");
                b.Property(e => e.Year).IsRequired(true);
                b.Property(e => e.IsDeleted).IsRequired(true);
                b.Property(e => e.VehicleId).IsRequired(false);
                b.Property(e => e.CreateDate).HasColumnType("datetime");
                b.HasIndex(e => e.LicensePlate).IsUnique(true);


            });
        }
    }
}
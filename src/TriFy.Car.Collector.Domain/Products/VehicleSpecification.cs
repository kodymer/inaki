using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Specifications;

namespace TriFy.Car.Collector.Products
{
    public class VehicleSpecification : Specification<Vehicle>
    {
        private readonly string _licensePlate;

        public VehicleSpecification(string licensePlate)
        {
            _licensePlate = licensePlate;
        }
        public override Expression<Func<Vehicle, bool>> ToExpression()
        {
            return e => e.LicensePlate == _licensePlate;
        }
    }
}

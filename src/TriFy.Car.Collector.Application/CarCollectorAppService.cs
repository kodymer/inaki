using System;
using System.Collections.Generic;
using System.Text;
using TriFy.Car.Collector.Localization;
using Volo.Abp.Application.Services;

namespace TriFy.Car.Collector
{
    /* Inherit your application services from this class.
     */
    public abstract class CarCollectorAppService : ApplicationService
    {
        protected CarCollectorAppService()
        {
            LocalizationResource = typeof(CarCollectorResource);
        }
    }
}

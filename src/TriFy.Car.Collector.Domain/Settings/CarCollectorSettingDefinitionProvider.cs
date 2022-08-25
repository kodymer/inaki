using System;
using System.IO;
using Volo.Abp.Settings;

namespace TriFy.Car.Collector.Settings
{
    public class CarCollectorSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition(CarCollectorSettings.BasePath, Environment.CurrentDirectory));
            context.Add(new SettingDefinition(CarCollectorSettings.NormalRelativePath));
            context.Add(new SettingDefinition(CarCollectorSettings.ErrorRelativePath));
        }
    }
}

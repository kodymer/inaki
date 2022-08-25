using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace TriFy.Car.Collector.System.IO
{
    public static class DirectoryInfoExtensions
    {

        public static DirectoryInfo GetOrCreateSubdirectory(this DirectoryInfo d, string name)
        {
            Check.NotNull(name, nameof(name));
                
            return d.GetDirectories().FirstOrDefault(d => d.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) ??
                   d.CreateSubdirectory(name);
        }

    }
}

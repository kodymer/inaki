using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace TriFy.Car.Collector.System.IO
{
    public static class FileInfoExtensions
    {

        private static void Move(this FileInfo f, string destinationPath)
        {
            Check.NotNullOrWhiteSpace(destinationPath, nameof(destinationPath));

            if (!File.Exists(destinationPath))
            {
                f.MoveTo(destinationPath);
            }
            else
            {
                f.Delete();
            }
        }

    }
}

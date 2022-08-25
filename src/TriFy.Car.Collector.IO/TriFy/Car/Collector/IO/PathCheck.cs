using System.IO;
using Volo.Abp;
using static System.IO.Directory;

namespace TriFy.Car.Collector.IO
{
    public static class PathCheck
    {
        public static void NotFound(string path)
        {
            Check.NotNullOrWhiteSpace(path, nameof(path));

            if (!Exists(path))
            {
                throw new DirectoryNotFoundException("Could not find the configured directory. Must create the base directory.");
            }
        }

        public static void Directory(string path)
        {

            Check.NotNullOrWhiteSpace(path, nameof(path));
 
            switch (path)
            {
                case var filePath when !string.IsNullOrWhiteSpace(Path.GetExtension(filePath)):
                    throw new DirectoryNotFoundException("The path must be an directory absolute path.");
            }
        }
    }
}

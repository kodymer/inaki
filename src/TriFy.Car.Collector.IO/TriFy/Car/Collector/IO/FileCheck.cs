using System;
using System.IO;

#nullable enable

namespace TriFy.Car.Collector.IO
{
    public static class FileCheck
    {

        public static void Pdf(FileInfo file, string paramName)
        {
            if (file.Extension != FileType.Pdf)
            {
                throw new ArgumentException($"The argument {paramName} is not a PDF.");
            }
        }

        public static void NotPdf(FileInfo file, string paramName)
        {
            if (file.Extension == FileType.Pdf)
            {
                throw new ArgumentException($"The argument {paramName} is a PDF");
            }
        }
    }
}

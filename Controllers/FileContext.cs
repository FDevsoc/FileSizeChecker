using System.Globalization;
using System.Text;

namespace FileSizeChecker.Controllers
{
    public static class FileContext
    {
        static readonly string[] SizeSuffixes =
           { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static public string GetDefaultFilePath()
        {
            string path = Environment.CurrentDirectory;
            DateTime fileDateCreated = DateTime.Now;
            string dateInFileName = fileDateCreated.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            path += @$"\sizes-{dateInFileName}.txt";

            return path;
        }

        static public void WriteLogInFile(string path)
        {
            File.Delete(path);

            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(DirectoryContext.FileLog);
                fstream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        static public void DefaultWriteLog()
        {
            WriteLogInFile(GetDefaultFilePath());
        }

        static public string SizeSuffix(long value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            int mag = (int)Math.Log(value, 1024);

            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }
    }
}

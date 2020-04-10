using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lecture6.Helpers
{
    public class DataLogger
    {
        public const string requestsLogFile = @"requestsLog.txt";
        public static void WriteToLogFile(string file, string text)
        {
            TextWriter tsw = new StreamWriter(file, true);
            tsw.WriteLine(text);
            tsw.Close();

            Console.WriteLine("Error occured: " + text);
        }

        public static void ClearLog(string file)
        {
            if (File.Exists(file))
                File.WriteAllText(file, String.Empty);
            else
                Console.WriteLine("File " + file + " does not exist.");
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;

namespace SocialActivity
{
    public static class ArgumentLogger
    {
        //Last value is return value
        public static List<Tuple<int, int, int, double>> arguments;
        public static String relativeFilePath = @"..\..\..\logger_output.txt";
        public static String delimiter = ",";

        static ArgumentLogger()
        {
            arguments = new List<Tuple<int, int, int, double>>();
        }

        public static void addEntryToList(int arg1, int arg2, int arg3, double ret)
        {
            arguments.Add(new Tuple<int, int, int, double>(arg1, arg2, arg3, ret));
        }

        public static void createLogFile()
        {
            File.Delete(relativeFilePath);
            using (StreamWriter sw = new StreamWriter(relativeFilePath))
            {
                foreach (var t in arguments)
                {
                    sw.WriteLine(t.Item1.ToString() + delimiter
                        + t.Item2.ToString() + delimiter
                        + t.Item3.ToString() + delimiter
                        + Convert.ToString(t.Item4, System.Globalization.CultureInfo.InvariantCulture));
                }
                sw.Close();
            }
        }
    }
}

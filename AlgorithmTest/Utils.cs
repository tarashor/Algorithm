using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AlgorithmTest
{
    static class Utils
    {
        public static int[] GenerateIntArray(int length)
        {
            int[] collection = new int[length];
            Random random = new Random();
            for (int i = 0; i < collection.Length; i++)
            {
                collection[i] = random.Next(2*length);
            }
            return collection;
        }

        public static void PrintArray<T>(T[] collection, TextWriter writer) {
            for (int i = 0; i < collection.Length; i++) {
                writer.Write("{0}\t", collection[i]);
            }
            writer.WriteLine();
        }

        public static void SaveToMFile(int[] x, long[][] y, string[] legends, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                string xline = "x=[";
                for (int i = 0; i < x.Length; i++)
                {
                    xline += x[i].ToString() + " ";
                }
                xline += "];";
                sw.WriteLine(xline);

                for (int j = 0; j < y.Length; j++)
                {
                    string yline = string.Format("y{0}=[", j);
                    for (int i = 0; i < y[j].Length; i++)
                    {
                        yline += y[j][i].ToString() + " ";
                    }
                    yline += "];";
                    sw.WriteLine(yline);
                }
                string hline = "h=plot(";
                for (int j = 0; j < y.Length; j++)
                {
                    hline += string.Format("x, y{0}", j);
                    if (j < y.Length - 1)
                    {
                        hline += ",";
                    }
                }
                hline += ");";

                sw.WriteLine(hline);

                string legendsline = "legend(";
                for (int j = 0; j < legends.Length; j++)
                {
                    legendsline += string.Format("'{0}'", legends[j]);
                    if (j < legends.Length - 1)
                    {
                        legendsline += ",";
                    }
                }
                legendsline += ");";
                sw.WriteLine(legendsline);
            }
        }
    }
}

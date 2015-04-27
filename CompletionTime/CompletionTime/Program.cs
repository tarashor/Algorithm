using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CompletionTime
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Work> works = ReadWorks();
            double res = 0;
            double c = 0;
            for (int i = 0; i < works.Count; i++)
            {
                c += works[i].Length;
                res += c * works[i].Weight;
            }

            Console.WriteLine(res);
            Console.ReadLine();
        }

        private static List<Work> ReadWorks()
        {
            string filename = "jobs.txt";
            List<Work> works = new List<Work>();
            using (StreamReader sr = new StreamReader(filename))
            {
                int count = int.Parse(sr.ReadLine());
                for (int i = 0; i < count; i++) {
                    string line = sr.ReadLine();
                    string[] workArgs = line.Split();
                    Work work = new Work();
                    work.Weight = int.Parse(workArgs[0]);
                    work.Length = int.Parse(workArgs[1]);
                    works.Add(work);
                }
            }
            works.Sort();
            return works;
        }
    }
}

using Algorithm.Sorting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AlgorithmTest
{
    class SortingTest
    {
        public static void testSorting()
        {
            var methods = typeof(Sorting).GetMethods().Where(m => m.Name.Contains("Sort")).Select(m => m.MakeGenericMethod(typeof(int)));
            Action<int[]>[] sortingAlgos = new Action<int[]>[methods.Count()];
            int k = 0;
            foreach (MethodInfo method in methods)
            {
                sortingAlgos[k] = (Action<int[]>)Delegate.CreateDelegate(typeof(Action<int[]>), method);
                k++;
            }

            int N = 10;
            for (int i = 0; i < sortingAlgos.Length; i++)
            {
                testOneSortMethod(N, sortingAlgos[i]);
            }

            Console.WriteLine("------------NORMAL----------");

            List<int> x = new List<int>();
            List<int>[] y = new List<int>[sortingAlgos.Length];
            N = 100000;
            for (int j = 100; j < N; j += 1000)
            {
                Console.WriteLine("Count = {0}", j);
                for (int i = 0; i < sortingAlgos.Length; i++)
                {
                    testOneSortMethod(j, sortingAlgos[i]);
                }
            }
            Console.WriteLine("----------Finished!---------");
            Console.Read();
        }

        public static void TestSortingOctave()
        {
            Action<int[]>[] sortingAlgos = GetListOfSortAlgorithms();

            int N = 10;
            for (int i = 0; i < sortingAlgos.Length; i++)
            {
                testOneSortMethod(N, sortingAlgos[i]);
            }

            Console.WriteLine("------------NORMAL----------");

            N = 100000;
            int delta = 1000;
            List<int> x = new List<int>();
            List<long>[] y = new List<long>[sortingAlgos.Length];
            string[] legends = new string[sortingAlgos.Length];
            for (int j = delta; j < N; j += delta)
            {
                x.Add(j);
                for (int i = 0; i < sortingAlgos.Length; i++)
                {
                    if (y[i] == null) y[i] = new List<long>();
                    y[i].Add(testOneSortMethod(j, sortingAlgos[i]));
                }
            }

            for (int i = 0; i < sortingAlgos.Length; i++)
            {
                legends[i] = sortingAlgos[i].Method.Name;
            }

            saveToMFile(x,y, legends);

            Console.WriteLine("----------Finished!---------");
            Console.Read();
        }

        private static Action<int[]>[] GetListOfSortAlgorithms()
        {
            var methods = typeof(Sorting).GetMethods().Where(m => m.Name.Contains("Sort")).Select(m => m.MakeGenericMethod(typeof(int)));
            Action<int[]>[] sortingAlgos = new Action<int[]>[methods.Count()];
            int k = 0;
            foreach (MethodInfo method in methods)
            {
                sortingAlgos[k] = (Action<int[]>)Delegate.CreateDelegate(typeof(Action<int[]>), method);
                k++;
            }
            return sortingAlgos;
        }

        private static void saveToMFile(List<int> x, List<long>[] y, string[] legends)
        {
            using (StreamWriter sw = new StreamWriter("s.m")) {
                string xline = "x=[";
                for (int i = 0; i < x.Count; i++) {
                    xline += x[i].ToString() + " ";
                }
                xline += "];";
                sw.WriteLine(xline);

                for (int j = 0; j < y.Length; j++)
                {
                    string yline = string.Format("y{0}=[", j);
                    for (int i = 0; i < x.Count; i++)
                    {
                        yline += y[j][i].ToString() + " ";
                    }
                    yline += "];";
                    sw.WriteLine(yline);
                }
                string hline = "h=plot(";
                for (int j = 0; j < y.Length; j++)
                {
                    hline += string.Format("y{0}, x", j);
                    if (j < y.Length - 1) {
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

        private static long testOneSortMethod(int length, Action<int[]> sort)
        {
            int[] collection = Utils.GenerateIntArray(length);
            //Utils.PrintArray(collection, Console.Out);
            var watch = Stopwatch.StartNew();

            sort(collection);

            watch.Stop();
            return watch.ElapsedMilliseconds;
            //Utils.PrintArray(collection, Console.Out);
            //Console.WriteLine("Method{0}, Time={1}ms", sort.Method.Name, elapsedMs);

        }

    }
}

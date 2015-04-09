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
        public static void TestOneMethod() {
            int N = 10;
            int[] collection = Utils.GenerateIntArray(N);
            Utils.PrintArray(collection, Console.Out);
            Sorting.ShellSort(collection);
            Utils.PrintArray(collection, Console.Out);
        }

        public static void TestSortingOctave()
        {
            Action<int[]>[] sortingAlgos = getListOfSortAlgorithms();

            int N = 10;
            for (int i = 0; i < sortingAlgos.Length; i++)
            {
                testOneSortMethod(N, sortingAlgos[i]);
            }

            int maxLengthOfCollection = 100000;
            int numberOfData = 20;

            int[] x;
            long[][] y;
            getPerformenceData(sortingAlgos, maxLengthOfCollection, numberOfData, out x, out y);
            string[] legends = getAlgorithmsNames(sortingAlgos);
            
            Utils.SaveToMFile(x, y, legends, "s4.m");

            Console.WriteLine("Finished!");
            Console.Read();
        }

        private static string[] getAlgorithmsNames(Action<int[]>[] sortingAlgos)
        {
            string[] legends = new string[sortingAlgos.Length];
            for (int i = 0; i < sortingAlgos.Length; i++)
            {
                legends[i] = sortingAlgos[i].Method.Name;
            }
            return legends;
        }

        private static void getPerformenceData(Action<int[]>[] sortingAlgos, int maxLengthOfCollection, int numberOfData, out int[] x, out long[][] y)
        {
            int delta = maxLengthOfCollection / numberOfData;
            int currentLength = delta;

            x = new int[numberOfData];
            y = new long[sortingAlgos.Length][];
            for (int j = 0; j < numberOfData; j++)
            {
                x[j] = currentLength;
                for (int i = 0; i < sortingAlgos.Length; i++)
                {
                    if (y[i] == null)
                    {
                        y[i] = new long[numberOfData];
                    }
                    y[i][j] = testOneSortMethod(currentLength, sortingAlgos[i]);
                }
                currentLength += delta;
            }
        }

        private static Action<int[]>[] getListOfSortAlgorithms()
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

        private static long testOneSortMethod(int length, Action<int[]> sort)
        {
            int[] collection = Utils.GenerateIntArray(length);
            var watch = Stopwatch.StartNew();
            sort(collection);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}

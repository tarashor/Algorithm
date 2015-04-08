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
    }
}

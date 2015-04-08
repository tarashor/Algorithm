using Algorithm.DataStructures;
using Algorithm.Sorting;
using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;

namespace AlgorithmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            testSorting();
        }

        private static void testBinaryTree()
        {
            BinarySearchTree<int, int> tree = new BinarySearchTree<int, int>();
            tree.Insert(15, 15);
            tree.Insert(5, 5);
            tree.Insert(3, 3);
            tree.Insert(12, 12);
            tree.Insert(10, 10);
            tree.Insert(6, 6);
            tree.Insert(7, 7);
            tree.Insert(13, 13);
            tree.Insert(16, 16);
            tree.Insert(20, 20);
            tree.Insert(18, 18);
            tree.Insert(23, 23);
            Console.WriteLine(tree.DrawTree());
            string t = tree.Save();
            tree.Clear();
            string[] s = t.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < s.Length; i++)
            {
                int k = int.Parse(s[i]);
                tree.Insert(k, k);
            }

            Console.WriteLine(tree.DrawTree());

            Console.Read();
        }

        private static void testSorting()
        {
            var methods = typeof(Sorting).GetMethods().Where(m=>m.Name.Contains("Sort")).Select(m=>m.MakeGenericMethod(typeof(int)));
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

            N = 100000;
            for (int i = 0; i < sortingAlgos.Length; i++)
            {
                testOneSortMethod(N, sortingAlgos[i]);
            }
            Console.WriteLine("Finished!");
            Console.Read();
        }

        private static void testOneSortMethod(int length, Action<int[]> sort)
        {
            int[] collection = Utils.GenerateIntArray(length);
            //Utils.PrintArray(collection, Console.Out);
            var watch = Stopwatch.StartNew();

            sort(collection);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            //Utils.PrintArray(collection, Console.Out);
            Console.WriteLine();
            Console.WriteLine("Method{0}, Time={1}ms", sort.Method.Name, elapsedMs);
            
        }
    }
}

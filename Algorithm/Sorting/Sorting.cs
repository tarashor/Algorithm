using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm.Sorting
{
    public class Sorting
    {
        public static void SelectionSort<T>(T[] collection) where T : IComparable<T>
        {
            for (int i = 0; i < collection.Length; i++)
            {
                int index = i;
                for (int j = i + 1; j < collection.Length; j++)
                    if (collection[j].CompareTo(collection[index]) < 0) //Finds smallest number
                        index = j;

                T smallerItem = collection[index];  //Swap
                collection[index] = collection[i];
                collection[i] = smallerItem;
            }
        }

        public static void InsertSort<T>(T[] collection) where T : IComparable<T>
        {
            int i, j;
            for (i = 1; i < collection.Length; i++)
            {
                T value = collection[i];
                j = i - 1;
                while ((j >= 0) && (collection[j].CompareTo(value) > 0))
                {
                    collection[j + 1] = collection[j];
                    j--;
                }
                collection[j + 1] = value;
            }
        }

        public static void BubbleSort<T>(T[] collection) where T : IComparable<T>
        {
            bool swapped = true;
            while (swapped)
            {
                swapped = false;
                for (int i = 0; i < collection.Length - 1; i++)
                {
                    if (collection[i].CompareTo(collection[i + 1]) > 0)
                    {
                        T temp = collection[i];
                        collection[i] = collection[i + 1];
                        collection[i + 1] = temp;
                        swapped = true;
                    }
                }
            }
        }

        public static void MergeSort<T>(T[] collection) where T : IComparable<T>
        {
            mergeSort(collection, 0, collection.Length - 1);
        }

        private static void mergeSort<T>(T[] collection, int startIndex, int endIndex) where T : IComparable<T>
        {
            if (endIndex - startIndex < 1) return;
            int middleIndex = (endIndex + startIndex) / 2;
            if (endIndex - startIndex > 1)
            {
                mergeSort(collection, startIndex, middleIndex);
                mergeSort(collection, middleIndex + 1, endIndex);
            }
            merge(collection, startIndex, middleIndex, endIndex);
        }

        private static void merge<T>(T[] collection, int startIndex, int middleIndex, int endIndex) where T : IComparable<T>
        {
            int lengthB = endIndex - startIndex + 1;
            T[] temp = new T[lengthB];
            int i = startIndex;
            int j = middleIndex + 1;
            for (int k = 0; k < lengthB; k++)
            {
                if ((j > endIndex) || ((i <= middleIndex) && (collection[i].CompareTo(collection[j]) <= 0)))
                {
                    temp[k] = collection[i];
                    i++;
                }
                else
                {
                    temp[k] = collection[j];
                    j++;
                }
            }
            for (int k = 0; k < lengthB; k++)
            {
                collection[startIndex + k] = temp[k];
            }
        }

        public static void QuickSort<T>(T[] collection) where T : IComparable<T>
        {
            quickSort(collection, 0, collection.Length - 1);
        }

        private static void quickSort<T>(T[] collection, int startIndex, int endIndex) where T : IComparable<T>
        {
            if (startIndex >= endIndex) return;
            int i = partition(collection, startIndex, endIndex);
            quickSort(collection, startIndex, i - 1);
            quickSort(collection, i + 1, endIndex);
        }

        private static int partition<T>(T[] collection, int startIndex, int endIndex) where T : IComparable<T>
        {
            T x = collection[endIndex];
            int i = startIndex - 1;
            for (int j = startIndex; j < endIndex; j++)
            {
                if (collection[j].CompareTo(x) <= 0)
                {
                    i++;
                    T t = collection[i];
                    collection[i] = collection[j];
                    collection[j] = t;
                }
            }
            i++;
            T temp = collection[i];
            collection[i] = collection[endIndex];
            collection[endIndex] = temp;
            return i;
        }

        public static void HeapSort<T>(T[] collection) where T : IComparable<T>
        {
            for (int i = collection.Length / 2 - 1; i >= 0; i--)
            {
                shiftDown(collection, i, collection.Length);
            }

            for (int i = collection.Length - 1; i >= 1; i--)
            {
                T temp = collection[0];
                collection[0] = collection[i];
                collection[i] = temp;
                shiftDown(collection, 0, i);
            }
        }

        static void shiftDown<T>(T[] collection, int i, int j) where T : IComparable<T>
        {
            bool done = false;
            int maxChild;

            while ((i * 2 + 1 < j) && (!done))
            {
                if (i * 2 + 1 == j - 1)
                    maxChild = i * 2 + 1;
                else if (collection[i * 2 + 1].CompareTo(collection[i * 2 + 2]) > 0)
                    maxChild = i * 2 + 1;
                else
                    maxChild = i * 2 + 2;

                if (collection[i].CompareTo(collection[maxChild]) < 0)
                {
                    T temp = collection[i];
                    collection[i] = collection[maxChild];
                    collection[maxChild] = temp;
                    i = maxChild;
                }
                else
                {
                    done = true;
                }
            }
        }

        public static void ShellSort<T>(T[] collection) where T : IComparable<T>
        {
            for (int d = collection.Length / 2; d > 0; d /= 2)
                for (int i = d; i < collection.Length; i++)
                    for (int j = i; j >= d && collection[j - d].CompareTo(collection[j]) > 0; j -= d)
                    {
                        T temp = collection[j - d];
                        collection[j - d] = collection[j];
                        collection[j] = temp;
                    }
        }
    }
}

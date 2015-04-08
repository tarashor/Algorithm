using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm.Sorting
{
    public class Sorting
    {
        public static void SelectionSort<T>(T[] collection) where T: IComparable<T> {
            for (int i = 0; i < collection.Length; i++)
            {
                int index = i;
                for (int j = i + 1; j < collection.Length; j++)
                    if (collection[j].CompareTo(collection[index])<0) //Finds smallest number
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
    }
}

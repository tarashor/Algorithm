using Algorithm.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmTest
{
    public class BinaryTreeTest
    {
        public static void testBinaryTree()
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
    }
}

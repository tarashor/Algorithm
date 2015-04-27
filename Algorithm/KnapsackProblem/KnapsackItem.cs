using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem
{
    public class KnapsackItem
    {
        public int Weight { get; set; }
        public double Value { get; set; }

        public static KnapsackItem Parse(string item) {
            KnapsackItem knapsackItem = new KnapsackItem();
            string[] v = item.Split();
            knapsackItem.Value = double.Parse(v[0]);
            knapsackItem.Weight = int.Parse(v[1]);
            return knapsackItem;
        }
    }
}

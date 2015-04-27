using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem
{
    public class KnapsackAlgorithm
    {
        public KnapsackAlgorithm(int weight)
        {
            cache = new Dictionary<PairKey, double>(new PairKeyEqualityComparer(weight));
        }
        public double Straightforward(KnapsackItem[] items, int W, int N) {
            double[,] A = new double[N + 1, W + 1];
            for (int x = 0; x < W + 1; x++) {
                A[0, x] = 0;
            }

            for (int i = 1; i < N + 1; i++) {
                KnapsackItem currentItem = items[i - 1];
                for (int x = 0; x < W + 1; x++)
                {
                    double max = A[i - 1, x];
                    if (x >= currentItem.Weight)
                    {
                        double v = A[i - 1, x - currentItem.Weight] + currentItem.Value;
                        if (v > max)
                        {
                            max = v;
                        }
                    }
                    A[i, x] = max;
                }
            }
            return A[N, W];
        }

        Dictionary<PairKey, double> cache;

        public double Recursion(KnapsackItem[] items, int W, int N)
        {
            
            if (N > 0)
            {
                KnapsackItem currentItem = items[N - 1];
                double res;
                PairKey key = new PairKey() { Count = N, Weight = W };
                if (!cache.TryGetValue(key, out res))
                {
                    res = Recursion(items, W, N - 1);
                    if (W >= currentItem.Weight)
                    {
                        double v = Recursion(items, W - currentItem.Weight, N - 1) + currentItem.Value;
                        if (v > res)
                        {
                            res = v;
                        }
                    }
                    cache.Add(key, res);
                }
                return res;
            }
            else {
                return 0;
            }
        }

        private class PairKeyEqualityComparer : IEqualityComparer<PairKey>
        {
            private int _weight;
            public PairKeyEqualityComparer(int weight) {
                _weight = weight;
            }
            public bool Equals(PairKey x, PairKey y)
            {
                return ((x.Weight == y.Weight) && (x.Count == y.Count));
            }

            public int GetHashCode(PairKey obj)
            {
                int hsh = obj.Weight + (obj.Count - 1)*_weight;
                return hsh.GetHashCode();
            }
        }

        private class PairKey{
            public int Weight { get; set; }
            public int Count { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Graph
{
    public class GraphAlgorithms
    {
        #region The Floyd Warshall Algorithm

        public static double[,] TheFloydWarshallAlgorithm(WeightedGraph<int, double> graph)
        {
            double[,] previousDistances = FillFirstStepOfFloydWarshallAlgorithm(graph);
            double[,] currentDistances = null;
            //Fill first step

            for (int k = 0; k < graph.VerteciesNumber; k++)
            {
                currentDistances = new double[graph.VerteciesNumber, graph.VerteciesNumber];
                for (int i = 0; i < graph.VerteciesNumber; i++)
                {
                    for (int j = 0; j < graph.VerteciesNumber; j++)
                    {
                        double distance = previousDistances[i, j];
                        double kDistance = previousDistances[i, k] + previousDistances[k, j];
                        if (distance > kDistance)
                        {
                            distance = kDistance;
                        }

                        currentDistances[i, j] = distance;
                    }
                }
                previousDistances = currentDistances;
            }

            return currentDistances;
        }

        private static double[,] FillFirstStepOfFloydWarshallAlgorithm(WeightedGraph<int, double> graph)
        {
            double[,] distances = new double[graph.VerteciesNumber, graph.VerteciesNumber];
            for (int i = 0; i < graph.VerteciesNumber; i++)
            {
                for (int j = 0; j < graph.VerteciesNumber; j++)
                {
                    if (i == j) distances[i, j] = 0;
                    else distances[i, j] = double.PositiveInfinity;
                }
            }

            foreach (WeightedEdge<int, double> edge in graph.Edges)
            {
                int i = graph.Vertecies.IndexOf(edge.StartVertex);
                int j = graph.Vertecies.IndexOf(edge.EndVertex);
                distances[i, j] = edge.Weight;
            }

            return distances;
        }

        #endregion

        #region The Prim Algorithm
        public static WeightedGraph<int, double> GetMSTThePrimAlgorithm(WeightedGraph<int, double> graph)
        {
            if (!graph.IsDirected)
            {
                WeightedGraph<int, double> mst = new WeightedGraph<int, double>(false);

                List<Vertex<int>> verteciesNotInMST = new List<Vertex<int>>();
                foreach (Vertex<int> vertex in graph.Vertecies)
                {
                    verteciesNotInMST.Add(vertex);
                }
                List<Vertex<int>> verteciesInMST = new List<Vertex<int>>();
                List<WeightedEdge<int, double>> edgesInMST = new List<WeightedEdge<int, double>>();

                Vertex<int> s = verteciesNotInMST.First();
                verteciesNotInMST.Remove(s);
                verteciesInMST.Add(s);

                while (verteciesNotInMST.Count > 0)
                {
                    WeightedEdge<int, double> minEdge = null;
                    foreach (WeightedEdge<int, double> edge in graph.Edges)
                    {
                        if ((verteciesNotInMST.Contains(edge.StartVertex) && verteciesInMST.Contains(edge.EndVertex)) || (verteciesNotInMST.Contains(edge.EndVertex) && verteciesInMST.Contains(edge.StartVertex)))
                        {
                            if ((minEdge == null) || (minEdge.Weight > edge.Weight))
                            {
                                minEdge = edge;
                            }
                        }
                    }

                    if (minEdge != null)
                    {
                        if (!edgesInMST.Contains(minEdge))
                        {
                            edgesInMST.Add(minEdge);
                        }

                        if (verteciesNotInMST.Contains(minEdge.StartVertex))
                        {
                            verteciesNotInMST.Remove(minEdge.StartVertex);
                            if (!verteciesInMST.Contains(minEdge.StartVertex))
                            {
                                verteciesInMST.Add(minEdge.StartVertex);
                            }

                        }
                        else
                        {
                            if (verteciesNotInMST.Contains(minEdge.EndVertex))
                            {
                                verteciesNotInMST.Remove(minEdge.EndVertex);
                                if (!verteciesInMST.Contains(minEdge.EndVertex))
                                {
                                    verteciesInMST.Add(minEdge.EndVertex);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new GraphException("Graph is not connected!!!");
                    }

                }

                foreach (WeightedEdge<int, double> edge in edgesInMST)
                {
                    Vertex<int> v1 = mst.FindVertex(edge.StartVertex.Data);
                    Vertex<int> v2 = mst.FindVertex(edge.EndVertex.Data);
                    if (v1 == null)
                    {
                        v1 = mst.CreateVertex(edge.StartVertex.Data);
                        mst.AddVertex(v1);
                    }
                    if (v2 == null)
                    {
                        v2 = mst.CreateVertex(edge.EndVertex.Data);
                        mst.AddVertex(v2);
                    }
                    mst.Connect2Verticies(v1, v2, edge.Weight);
                }

                return mst;
            }
            else { return null; }

        }
        #endregion

        #region The Travelling Salesman Problem

        public static TEdgeData TheTravellingSalesmanProblem<TVertexData, TEdgeData>(WeightedGraph<TVertexData, TEdgeData> graph, Func<TEdgeData, TEdgeData, TEdgeData> add, TEdgeData edgeWeightPositiveInfinityValue)
            where TVertexData : IComparable
            where TEdgeData : IComparable
        {
            int[][] groupsOfSets = GroupSets(graph.VerteciesNumber - 1);

            TravellingSalesmanProblemDataForSets<TEdgeData> cacheSWithoutJ = new TravellingSalesmanProblemDataForSets<TEdgeData>(new int[0], graph.VerteciesNumber, edgeWeightPositiveInfinityValue);
            TEdgeData[,] edgesMatrix = graph.GetEdgesMatrix();
            
            for (byte i = 1; i < graph.VerteciesNumber; i++)
            {
                int[] sets = groupsOfSets[i - 1];
                TravellingSalesmanProblemDataForSets<TEdgeData> cacheS = new TravellingSalesmanProblemDataForSets<TEdgeData>(sets, graph.VerteciesNumber, edgeWeightPositiveInfinityValue);

                foreach (int set in sets)
                {
                    for (byte j = 1; j < graph.VerteciesNumber; j++)
                    {
                        int setWithOnlyJ = (1 << (j - 1));

                        if (setWithOnlyJ > set) break;

                        if ((set & setWithOnlyJ) != 0)
                        {
                            int setWithoutJ = set & ~setWithOnlyJ;
                            TEdgeData min = add(cacheSWithoutJ.GetValue(setWithoutJ, 0), edgesMatrix[0, j]);
                            for (byte k = 1; k < graph.VerteciesNumber; k++)
                            {
                                int setWithOnlyK = (1 << (k - 1));

                                if (setWithOnlyK > set) break;

                                if (((set & setWithOnlyK) != 0) && (k != j))
                                {
                                    TEdgeData value = add(cacheSWithoutJ.GetValue(setWithoutJ, k), edgesMatrix[k, j]);
                                    if (min.CompareTo(value) > 0)
                                    {
                                        min = value;
                                    }
                                }
                            }
                            cacheS.SetValue(set, j, min);
                        }
                    }
                }

                cacheSWithoutJ = null;
                cacheSWithoutJ = cacheS;

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }

            byte lastSetKey = (byte)(graph.VerteciesNumber - 2);
            int setN = groupsOfSets[lastSetKey][0];
            TEdgeData minTour = add(cacheSWithoutJ.GetValue(setN, 1), edgesMatrix[1, 0]);
            for (byte j = 2; j < graph.VerteciesNumber; j++)
            {
                TEdgeData value = add(cacheSWithoutJ.GetValue(setN, j), edgesMatrix[j, 0]);
                if (minTour.CompareTo(value) > 0)
                {
                    minTour = value;
                }
            }

            return minTour;
        }

        class TravellingSalesmanProblemDataForSets<TData>
            where TData : IComparable
        {
            private TData[][] _data;
            private Dictionary<int, int> _mapForSets;
            private TData _positiveInfinity;

            public TravellingSalesmanProblemDataForSets(int[] sets, int maxPower, TData positiveInfinity)
            {
                _data = new TData[sets.Length][];
                _mapForSets = new Dictionary<int, int>();

                for (int i = 0; i < sets.Length; i++)
                {
                    _mapForSets.Add(sets[i], i);
                    _data[i] = new TData[maxPower];
                }
                _positiveInfinity = positiveInfinity;
            }

            public TData GetValue(int set, byte i) 
            {
                TData res = default(TData);
                if (i != 0)
                {
                    if (set != 0)
                    {
                        int index;
                        if (_mapForSets.TryGetValue(set, out index))
                        {
                            res = _data[index][i];
                        }
                    }
                }
                else {
                    if (set != 0)
                    {
                        res = _positiveInfinity;
                    }
                }

                return res;
            }
            public void SetValue(int set, byte i, TData value) 
            {
                int index;
                if (_mapForSets.TryGetValue(set, out index))
                {
                    _data[index][i] = value;
                }
            }


        }
        
        public static int[][] GroupSets(int itemsInSet)
        {
            int[][] groups = new int[itemsInSet][];
            int[] pointers = new int[itemsInSet];

            for (int i = 0; i < itemsInSet; i++)
            {
                int k = BinomialCoeficient(itemsInSet, i+1);
                groups[i] = new int[k];
                pointers[i] = 0;
            }

            int maxValue = (1 << itemsInSet);
            for (int i = 1; i < maxValue; i++)
            {
                byte bitsInNumber = BitsInInteger(i, itemsInSet);

                groups[bitsInNumber - 1][pointers[bitsInNumber - 1]] = i;

                pointers[bitsInNumber - 1]++;
 
            }

            return groups;
        }

        public static int BinomialCoeficient(int n, int k)
        {
            if (k == 1) return n;

            if (n == k)
            {
                return 1;
            }
            else 
            {
                if (n < k)
                {
                    return 0;
                }
                else {
                    return BinomialCoeficient(n - 1, k) + BinomialCoeficient(n - 1, k - 1);
                }
            }
        }

        private static byte BitsInInteger(int number, int lengthInBits)
        {
            byte sum = 0;
            for (int i = 0; i <= lengthInBits; i++)
            {
                if ((number & (1 << i)) != 0) {
                    sum++;
                } 
            }
            return sum;
        }


        
        

        public static int GetNext(int v) {
            int t = (v | (v - 1)) + 1;  
            int w = t | ((((t & -t) / (v & -v)) >> 1) - 1);
            return w;
        }
        #endregion

        #region Clustering Problem
        public static double GetMaxSpacingForClusters2<TVertexData>(WeightedGraph<TVertexData, double> graph, int clustersCount)
            where TVertexData : IComparable
        {
            double maxSpacing = 0;
            while (graph.VerteciesNumber > clustersCount)
            {
                WeightedEdge<TVertexData, double> minEdge = GetMinOrMaxEdge(graph, true);
                graph.MergeTwoVerticies(minEdge.StartVertex, minEdge.EndVertex);
            }

            WeightedEdge<TVertexData, double> maxEdge = GetMinOrMaxEdge(graph, true);
            if (maxEdge != null)
            {
                maxSpacing = maxEdge.Weight;
            }
            return maxSpacing;
        }

        private class Cluster<TVertexData> where TVertexData:IComparable 
        {
            public Vertex<TVertexData> Head { get; set; }

        }

        public static double GetMaxSpacingForClusters<TVertexData>(WeightedGraph<TVertexData, double> graph, int clustersCount, Func<TVertexData, TVertexData, TVertexData> newValueFunction) 
            where TVertexData:IComparable
        {
            double maxSpacing = 0;
            while (graph.VerteciesNumber > clustersCount) 
            {
                WeightedEdge<TVertexData, double> minEdge = GetMinOrMaxEdge(graph, true);
                graph.MergeTwoVerticies(minEdge.StartVertex, minEdge.EndVertex, newValueFunction(minEdge.StartVertex.Data, minEdge.EndVertex.Data));
            }

            WeightedEdge<TVertexData, double> maxEdge = GetMinOrMaxEdge(graph, true);
            if (maxEdge != null) { 
                maxSpacing = maxEdge.Weight;
            }
            return maxSpacing;
        }



        private static WeightedEdge<TVertexData, double> GetMinOrMaxEdge<TVertexData>(WeightedGraph<TVertexData, double> graph, bool isMin)
            where TVertexData:IComparable
        {
            WeightedEdge<TVertexData, double> findEdge = graph.Edges.FirstOrDefault() as WeightedEdge<TVertexData, double>;
            foreach (Edge<TVertexData> edge in graph.Edges)
            {
                WeightedEdge<TVertexData, double> weightedEdge = edge as WeightedEdge<TVertexData, double>;
                if (weightedEdge != null) {
                    if (isMin)
                    {
                        if (weightedEdge.Weight < findEdge.Weight)
                        {
                            findEdge = weightedEdge;
                        }
                    }
                    else 
                    {
                        if (weightedEdge.Weight > findEdge.Weight)
                        {
                            findEdge = weightedEdge;
                        }
                    }
                }
            }

            return findEdge;
        }
        
        #endregion
    }

}

using DataStructures.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimumSpanningTree
{
    class Program
    {
        static void Main(string[] args)
        {
            WeightedGraph<int,double> graph = GraphReader.ReadFromFile("edges.txt", false);
            WeightedGraph<int,double> mst = GraphAlgorithms.GetMSTThePrimAlgorithm(graph);

            double sum = 0;
            foreach (Edge<int> edge in mst.Edges)
            {
                WeightedEdge<int,double> wedge = edge as WeightedEdge<int,double>;
                if (wedge != null)
                {
                    sum += wedge.Weight;
                }
            }

            Console.WriteLine(sum);
            Console.ReadLine();
        }
    }
}

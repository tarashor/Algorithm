using DataStructures.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusteringProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            //string filename = "test.txt"; //1002
            //string filename = "test100.txt"; //20
            string filename = "test20.txt"; // 1951
            //string filename = "test9.txt"; // 3
            //string filename = "clustering1.txt";
            int clustersCount = 2;
            WeightedGraph<string, double> graph = GraphReader.ReadFromFile(filename, false);
            double maxSpacing = GraphAlgorithms.GetMaxSpacingForClusters(graph, clustersCount, (s1, s2) =>  s1 + "," + s2);
            Console.WriteLine(maxSpacing);
            Console.ReadLine();
        }
    }
}

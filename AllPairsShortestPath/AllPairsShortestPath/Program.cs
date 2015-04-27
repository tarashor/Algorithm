using DataStructures.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPairsShortestPath
{
    class Program
    {
        static void Main(string[] args)
        {
            WeightedGraph<int, double> graph = GraphReader.ReadFromFile("g3.txt", true);
            double[,] distances = GraphAlgorithms.TheFloydWarshallAlgorithm(graph);
            double min = distances[0, 0];
            bool hasNegativeCycle = false;

            for (int i = 0; i < distances.GetLength(0); i++)
            {
                if (distances[i, i] < 0)
                {
                    hasNegativeCycle = true;
                    break;
                }

                for (int j = 0; j < distances.GetLength(1); j++)
                {
                    if (min > distances[i, j])
                    {
                        min = distances[i, j];
                    }
                }
            }

            if (hasNegativeCycle)
                Console.WriteLine("Has negative cycles!");
            else
            {
                Console.WriteLine("Has NO negative cycles!");
                Console.WriteLine("Max distance: " + min.ToString());
            }

            Console.ReadLine();
        }
    }
}

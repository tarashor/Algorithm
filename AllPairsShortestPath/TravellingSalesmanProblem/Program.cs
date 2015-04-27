using DataStructures.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingSalesmanProblem.GraphReader;

namespace TravellingSalesmanProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            WeightedGraph<Point, float> graph = GraphReader.GraphReader.ReadFromFile("tsp.txt", false);
            float minTour = GraphAlgorithms.TheTravellingSalesmanProblem<Point, float>(graph, (a,b) => a+b, float.PositiveInfinity);
            Console.WriteLine(minTour);
            Console.ReadLine();
        }
    }
}

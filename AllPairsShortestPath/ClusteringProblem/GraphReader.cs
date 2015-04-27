using DataStructures.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusteringProblem
{
    public class GraphReader
    {
        public static WeightedGraph<string, double> ReadFromFile(string filename, bool isDirected)
        {
            WeightedGraph<string, double> graph = null;
            using (StreamReader sr = new StreamReader(filename))
            {
                string firstLine = sr.ReadLine();
                int verticiesCount;

                if (int.TryParse(firstLine.Trim(), out verticiesCount))
                {
                    graph = new WeightedGraph<string, double>(isDirected);
                    string line = null;
                    while((line = sr.ReadLine()) != null)
                    {
                        string[] items = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        double weight;
                        if ( double.TryParse(items[2], out weight))
                        {
                            Vertex<string> vertex1 = graph.FindVertex(items[0]);
                            Vertex<string> vertex2 = graph.FindVertex(items[1]);
                            if (vertex1 == null)
                            {
                                vertex1 = graph.CreateVertex(items[0]);
                                graph.AddVertex(vertex1);
                            }
                            if (vertex2 == null)
                            {
                                vertex2 = graph.CreateVertex(items[1]);
                                graph.AddVertex(vertex2);
                            }

                            graph.Connect2Verticies(vertex1, vertex2, weight);

                        }
                    }

                }
            }
            return graph;
        }
    }
}

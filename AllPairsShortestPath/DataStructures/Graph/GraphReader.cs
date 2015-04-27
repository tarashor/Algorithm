using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Graph
{
    public class GraphReader
    {
        public static WeightedGraph<int, double> ReadFromFile(string filename, bool isDirected)
        {
            WeightedGraph<int, double> graph = null;
            using (StreamReader sr = new StreamReader(filename))
            {
                string firstLine = sr.ReadLine();
                int verticiesCount;
                int edgesCount;
                string[] firstLineSplits = firstLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (int.TryParse(firstLineSplits[0], out verticiesCount) && int.TryParse(firstLineSplits[1], out edgesCount))
                {
                    graph = new WeightedGraph<int, double>(verticiesCount, edgesCount, isDirected);
                    for (int i = 0; i < edgesCount; i++)
                    {
                        string line = sr.ReadLine();
                        string[] items = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        int vertex1Data;
                        int vertex2Data;
                        double weight;
                        if (int.TryParse(items[0], out vertex1Data) && int.TryParse(items[1], out vertex2Data) && double.TryParse(items[2], out weight))
                        {
                            Vertex<int> vertex1 = graph.FindVertex(vertex1Data);
                            Vertex<int> vertex2 = graph.FindVertex(vertex2Data);
                            if (vertex1 == null)
                            {
                                vertex1 = graph.CreateVertex(vertex1Data);
                                graph.AddVertex(vertex1);
                            }
                            if (vertex2 == null)
                            {
                                vertex2 = graph.CreateVertex(vertex2Data);
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

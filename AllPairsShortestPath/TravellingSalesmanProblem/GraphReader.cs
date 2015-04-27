using DataStructures.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesmanProblem.GraphReader
{
    public class GraphReader
    {
        public static WeightedGraph<Point, float> ReadFromFile(string filename, bool isDirected) 
        {
            WeightedGraph<Point, float> graph = null;
            using (StreamReader sr = new StreamReader(filename))
            {
                string firstLine = sr.ReadLine();
                int verticiesCount;

                if (int.TryParse(firstLine, out verticiesCount))
                {
                    int edgesCount = verticiesCount * (verticiesCount - 1) / 2;
                    graph = new WeightedGraph<Point, float>(verticiesCount, edgesCount, isDirected);

                    for (int i = 0; i < verticiesCount; i++) {
                        string line = sr.ReadLine();
                        Point p = Point.ParseLine(line);
                        Vertex<Point> vertex = graph.CreateVertex(p);
                        foreach (Vertex<Point> v in graph.Vertecies) {
                            float weight = (float)Math.Sqrt((v.Data.X - vertex.Data.X) * (v.Data.X - vertex.Data.X) + (v.Data.Y - vertex.Data.Y) * (v.Data.Y - vertex.Data.Y));
                            graph.Connect2Verticies(v, vertex, weight);
                        }

                        graph.AddVertex(vertex);
                    }
                }
            }
            return graph;
        }
    }
}

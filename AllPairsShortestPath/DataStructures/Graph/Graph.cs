using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures.Graph
{
    #region Vertex and Edge

    public class Vertex<TData> where TData : IComparable
    {
        public TData Data { get; set; }

        public List<Edge<TData>> AdjacencyEdges
        {
            get;
            private set;
        }

        public Vertex()
        {
            Data = default(TData);
            AdjacencyEdges = new List<Edge<TData>>();
        }

        public Vertex(TData data)
        {
            Data = data;
            AdjacencyEdges = new List<Edge<TData>>();
        }

    }

    public class Edge<TData> where TData : IComparable
    {
        public Vertex<TData> StartVertex { get; set; }
        public Vertex<TData> EndVertex { get; set; }
    }

    public class WeightedEdge<TVertexData, TEdgeData> : Edge<TVertexData>
        where TVertexData: IComparable
        
    {
        public TEdgeData Weight { get; set; }
    }

    #endregion

    #region Graphs
    public class Graph<TData> where TData : IComparable
    {
        public List<Vertex<TData>> Vertecies { get; private set; }
        public List<Edge<TData>> Edges { get; private set; }
        public int VerteciesNumber { get { return Vertecies.Count; } }
        public int EdgesNumber { get { return Edges.Count; } }

        protected bool isDirected;

        public bool IsDirected
        {
            get { return isDirected; }
        }

        public Graph():this(isDirected:false)
        { }

        public Graph(bool isDirected)
        {
            Vertecies = new List<Vertex<TData>>();
            Edges = new List<Edge<TData>>();
            this.isDirected = isDirected;
        }

        public Graph(int verticiesCount, int edgesCount, bool isDirected)
        {
            Vertecies = new List<Vertex<TData>>(verticiesCount);
            Edges = new List<Edge<TData>>(edgesCount);
            this.isDirected = isDirected;
        }

        #region Vertex

        /// <summary>
        /// Only creates the vertex with appropriate content, but do not add it to graph.
        /// </summary>
        /// <param name="data">The content of vertex.</param>
        /// <returns>The created vertex.</returns>
        public Vertex<TData> CreateVertex(TData data)
        {
            Vertex<TData> vertex = new Vertex<TData>(data);
            return vertex;
        }

        public void AddVertex(Vertex<TData> vertex)
        {
            Vertecies.Add(vertex);
        }

        public void RemoveVertex(Vertex<TData> vertex)
        {
            if (Vertecies.Contains(vertex))
            {
                while(vertex.AdjacencyEdges.Count > 0)
                {
                    Edge<TData> edge = vertex.AdjacencyEdges.FirstOrDefault();
                    RemoveEdge(edge);
                }

                Vertecies.Remove(vertex);
            }
            else 
            {
                throw new GraphException("Cann't remove vertex, because vertex isn't in graph!");
            }
        }

        public Vertex<TData> FindVertex(TData data)
        {
            Vertex<TData> res = null;
            foreach (Vertex<TData> vertex in Vertecies)
            {
                if (vertex.Data.CompareTo(data) == 0)
                {
                    res = vertex;
                    break;
                }
            }
            return res;
        }

        #endregion

        #region Edges

        public void AddEdge(Edge<TData> edge)
        {
            Edges.Add(edge);
            edge.StartVertex.AdjacencyEdges.Add(edge);
            if (!isDirected)
                edge.EndVertex.AdjacencyEdges.Add(edge);
        }

        public void RemoveEdge(Edge<TData> edge)
        {
            if (Edges.Contains(edge))
            {
                Edges.Remove(edge);
                edge.StartVertex.AdjacencyEdges.Remove(edge);
                if (!isDirected)
                    edge.EndVertex.AdjacencyEdges.Remove(edge);
            }
            else
            {
                throw new GraphException("Cann't remove edge, because edge isn't in graph!");
            }
        }

        public IEnumerable<Edge<TData>> FindAllEdges(Vertex<TData> startVertex, Vertex<TData> endVertex)
        {
            List<Edge<TData>> edges = new List<Edge<TData>>();
            foreach (Edge<TData> edge in Edges)
            {
                if (((edge.StartVertex == startVertex) && (edge.EndVertex == endVertex)) || (!isDirected && (edge.StartVertex == endVertex) && (edge.EndVertex == startVertex)))
                {
                    edges.Add(edge);
                }
            }
            return edges;
        }

        public Edge<TData> FindEdge(Vertex<TData> startVertex, Vertex<TData> endVertex)
        {
            IEnumerable<Edge<TData>> edges = FindAllEdges(startVertex, endVertex);
            return edges.FirstOrDefault();
        }
        
        
        #endregion

    }

    public class WeightedGraph<TVertexData, TEdgeData> : Graph<TVertexData>
        where TVertexData : IComparable
        where TEdgeData : IComparable
    {

        public WeightedGraph()
            : base(isDirected: false)
        { }

        public WeightedGraph(bool isDirected)
            : base(isDirected)
        { }

        public WeightedGraph(int verticiesCount, int edgesCount, bool isDirected)
            : base(verticiesCount, edgesCount, isDirected)
        { }

        public Edge<TVertexData> Connect2Verticies(Vertex<TVertexData> startVertex, Vertex<TVertexData> endVertex, TEdgeData weight)
        {
            WeightedEdge<TVertexData, TEdgeData> edge = new WeightedEdge<TVertexData, TEdgeData>();
            edge.StartVertex = startVertex;
            edge.EndVertex = endVertex;
            edge.Weight = weight;
            AddEdge(edge);

            return edge;
        }

        public void MergeTwoVerticies(Vertex<TVertexData> vertex1, Vertex<TVertexData> vertex2, TVertexData newValue)
        {
            Vertex<TVertexData> newVertex = CreateVertex(newValue);
            AddVertex(newVertex);

            foreach (Edge<TVertexData> edge in vertex1.AdjacencyEdges)
            {
                WeightedEdge<TVertexData, TEdgeData> we = edge as WeightedEdge<TVertexData, TEdgeData>;
                if (we != null) 
                {
                    Vertex<TVertexData> adjacencyVertex = we.EndVertex;
                    if (vertex1 == we.EndVertex)
                    {
                        adjacencyVertex = we.StartVertex;
                    }
                    Connect2Verticies(newVertex, adjacencyVertex, we.Weight);
                }

            }

            foreach (Edge<TVertexData> edge in vertex2.AdjacencyEdges)
            {
                WeightedEdge<TVertexData, TEdgeData> wedge = edge as WeightedEdge<TVertexData, TEdgeData>;
                if (wedge != null)
                {
                    Vertex<TVertexData> adjacencyVertex = wedge.EndVertex;
                    if (vertex2 == wedge.EndVertex)
                    {
                        adjacencyVertex = wedge.StartVertex;
                    }

                    if (adjacencyVertex != newVertex)
                    {

                        bool isSmaller = true;
                        IEnumerable<Edge<TVertexData>> edges = FindAllEdges(newVertex, adjacencyVertex);
                        foreach (Edge<TVertexData> e in edges)
                        {
                            WeightedEdge<TVertexData, TEdgeData> we = edge as WeightedEdge<TVertexData, TEdgeData>;
                            isSmaller = ((we != null) && (we.Weight.CompareTo(wedge.Weight) > 0));
                        }

                        if (isSmaller)
                        {
                            Connect2Verticies(newVertex, adjacencyVertex, wedge.Weight);
                        }
                    }
                }

            }

            RemoveVertex(vertex1);
            RemoveVertex(vertex2);
        }

        public TEdgeData[,] GetEdgesMatrix()
        {
            TEdgeData[,] edgesMatrix = new TEdgeData[VerteciesNumber, VerteciesNumber];
            for (int i = 0; i < VerteciesNumber; i++)
            {
                Vertex<TVertexData> vi = Vertecies[i];
                for (int j = 0; j < VerteciesNumber; j++)
                {
                    edgesMatrix[i, j] = default(TEdgeData);
                    Vertex<TVertexData> vj = Vertecies[j];
                    IEnumerable<Edge<TVertexData>> edges = FindAllEdges(vi, vj);
                    if (edges.Count() > 0)
                    {
                        WeightedEdge<TVertexData, TEdgeData> we = edges.First() as WeightedEdge<TVertexData, TEdgeData>;
                        if (we != null)
                        {
                            edgesMatrix[i, j] = we.Weight;
                        }
                    }
                }
            }
            return edgesMatrix;
        }


       
    }
    #endregion

    public class GraphException : Exception
    {
        public GraphException()
            : base() { }

        public GraphException(string message)
            : base(message) { }
    }
}

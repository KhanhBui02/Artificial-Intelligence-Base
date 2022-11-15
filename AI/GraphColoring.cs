/*using System;
using System.Collections.Generic;*/

namespace Graph_Coloring
{
    internal class GraphColoring
    {
        // No. of vertices    
        private int _V;

        private bool _isGraphStartFromZero;

        //Adjacency Lists
        LinkedList<int>[] _adj;

        public struct VerticeRank
        {
            public int vertice;
            public int rank;
            public int color;
        }

        public GraphColoring(int V, bool isGraphStartFromZero)
        {
            if (isGraphStartFromZero == false)
            {
                V++;
            }

            _adj = new LinkedList<int>[V];
            for (int i = 0; i < _adj.Length; i++)
            {
                _adj[i] = new LinkedList<int>();
            }

            _V = V;
            _isGraphStartFromZero = isGraphStartFromZero;
        }

        // Function to add an edge into the graph
        public void AddEdge(int v, int w)
        {
            _adj[v].AddLast(w);
        }

        public VerticeRank[] VerticeRankSort()
        {
            VerticeRank[] vertices = new VerticeRank[_V];

            // Count Rank
            int adjIndex = 0;

            if (_isGraphStartFromZero == false)
            {
                adjIndex = 1;
            }

            for (int i = adjIndex; i < _adj.Length; i++)
            {
                vertices[i].vertice = i;
                vertices[i].rank = 0;
                vertices[i].color = 0;
                for (int verIndex = 0; verIndex < _adj[i].Count; verIndex++)
                {
                    vertices[i].rank++;
                }
            }

            // Sort
            for (int i = 0; i < vertices.Length - 1; i++)
            {
                for (int j = i + 1; j < vertices.Length; j++)
                {
                    if (vertices[i].rank < vertices[j].rank)
                    {
                        VerticeRank temp = vertices[i];
                        vertices[i] = vertices[j];
                        vertices[j] = temp;
                    }
                }
            }

            return vertices;
        }

        public bool IsNeighbour(LinkedList<int> neighbours, int vertice)
        {
            int[] neighboursTemp = new int[neighbours.Count];
            neighbours.CopyTo(neighboursTemp, 0);

            foreach (int nbIndex in neighbours)
            {
                if (vertice == nbIndex)
                {
                    return true;
                }
            }

            return false;
        }

        public void GreedyColoring()
        {
            VerticeRank[] vertices = VerticeRankSort();

            int color = 1;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].color == 0)
                {
                    LinkedList<int> neighbours = _adj[vertices[i].vertice];
                    vertices[i].color = color;

                    for (int j = i + 1; j < vertices.Length; j++)
                    {
                        if (vertices[j].color == 0 && !IsNeighbour(neighbours, vertices[j].vertice))
                        {
                            vertices[j].color = color;
                        }
                    }
                }

                color++;
            }

            PrintResult(vertices);
        }

        public void PrintResult(VerticeRank[] vertices)
        {
            Console.WriteLine("(Vertice, rank): color");
            for (int i = 0; i < vertices.Length; i++)
            {
                if (_isGraphStartFromZero == false && vertices[i].vertice == 0)
                {
                    continue;
                }
                Console.WriteLine("(" + vertices[i].vertice + ", " + vertices[i].rank + "): " + vertices[i].color);
            }
        }

        static void Main(string[] args)
        {
            GraphColoring g = new GraphColoring(10, false);

            g.AddEdge(1, 2);
            g.AddEdge(1, 3);
            g.AddEdge(1, 4);
            g.AddEdge(1, 5);
            g.AddEdge(1, 7);
            g.AddEdge(1, 8);

            g.AddEdge(2, 1);
            g.AddEdge(2, 3);
            g.AddEdge(2, 5);
            g.AddEdge(2, 6);

            g.AddEdge(3, 1);
            g.AddEdge(3, 2);
            g.AddEdge(3, 4);
            g.AddEdge(3, 6);

            g.AddEdge(4, 1);
            g.AddEdge(4, 3);
            g.AddEdge(4, 5);
            g.AddEdge(4, 6);
            g.AddEdge(4, 7);
            g.AddEdge(4, 9);

            g.AddEdge(5, 1);
            g.AddEdge(5, 2);
            g.AddEdge(5, 4);
            g.AddEdge(5, 6);
            g.AddEdge(5, 8);

            g.AddEdge(6, 2);
            g.AddEdge(6, 3);
            g.AddEdge(6, 4);
            g.AddEdge(6, 5);

            g.AddEdge(7, 1);
            g.AddEdge(7, 4);
            g.AddEdge(7, 8);
            g.AddEdge(7, 9);
            g.AddEdge(7, 10);

            g.AddEdge(8, 1);
            g.AddEdge(8, 5);
            g.AddEdge(8, 7);
            g.AddEdge(8, 10);

            g.AddEdge(9, 4);
            g.AddEdge(9, 7);

            g.AddEdge(10, 7);
            g.AddEdge(10, 8);

            Console.Write("Following is Breadth First Traversal(starting from vertex 2)\n");

            g.GreedyColoring();

            Console.ReadKey();
        }
    }
}

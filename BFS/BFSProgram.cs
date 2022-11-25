class BFSProgram
{   
    private int _V;

    LinkedList<int>[] _adj;

    public BFSProgram(int V)
    {
        _adj = new LinkedList<int>[V];
        for (int i = 0; i < _adj.Length; i++)
        {
            _adj[i] = new LinkedList<int>();
        }
        _V = V;
    }

    public void AddEdge(int v, int w)
    {
        _adj[v].AddLast(w);
    }

    // Prints BFS traversal from a given source s
    public void BFS(int s)
    {
        // Mark all the vertices as not visited(By default set as false)
        bool[] visited = new bool[_V];
        for (int i = 0; i < _V; i++)
            visited[i] = false;

        // Create a queue for BFS
        LinkedList<int> queue = new LinkedList<int>();

        // Mark the current node as visited and enqueue it
        visited[s] = true;
        queue.AddLast(s);

        while (queue.Any())
        {
            s = queue.First();
            Console.Write(s + " ");
            queue.RemoveFirst();

            LinkedList<int> list = _adj[s];

            foreach (var val in list)
            {
                if (!visited[val])
                {
                    visited[val] = true;
                    queue.AddLast(val);
                }
            }
        }
    }

    static void Main(string[] args)
    {
        BFSProgram g = new BFSProgram(4);

        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 2);
        g.AddEdge(2, 0);
        g.AddEdge(2, 3);
        g.AddEdge(3, 3);

        Console.WriteLine("BFS bat dau tu 2:");
        g.BFS(2);
    }
}

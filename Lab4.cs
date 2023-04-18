using DM.Interfaces;

namespace DM;

public class Lab4 : ILab
{
    private int _v;
    
    public void Init()
    {
        string fileText = File.ReadAllText(FilePath);
        var rows = fileText.Split('\n');

        int[,] graph = new int[rows.Length, rows.Length];

        for (int i = 0; i < rows.Length; i++)
        {
            var cols = rows[i].Trim().Split(' ');

            for (int j = 0; j < cols.Length; j++)
            {
                graph[i, j] = int.Parse(cols[j]);
            }
        }

        Console.WriteLine("Matrix:");
        for (int i = 0; i < graph.GetLength(0); i++)
        {
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                Console.Write($"{graph[i, j]}\t");
            }

            Console.WriteLine();
        }
        
        _v = graph.GetLength(0);

        Console.WriteLine("\nMax flow: " + FordFulkerson(graph, 0, 5));
    }
    
    int FordFulkerson(int[,] graph, int s, int t)
    {
        
        int[,] rGraph = new int[_v, _v];
        Array.Copy(graph, rGraph, _v * _v);

        int[] parent = new int[_v];
        int maxFlow = 0;

        while (BFS(rGraph, s, t, parent))
        {
            int pathFlow = int.MaxValue;
            for (int v = t; v != s; v = parent[v])
            {
                int u = parent[v];
                pathFlow = Math.Min(pathFlow, rGraph[u, v]);
            }

            for (int v = t; v != s; v = parent[v])
            {
                int u = parent[v];
                rGraph[u, v] -= pathFlow;
                rGraph[v, u] += pathFlow;
            }

            maxFlow += pathFlow;
        }

        return maxFlow;
    }

    bool BFS(int[,] rGraph, int s, int t, int[] parent)
    {
        bool[] visited = new bool[_v];
        for (int i = 0; i < _v; ++i)
        {
            visited[i] = false;
        }

        Queue<int> q = new Queue<int>();
        q.Enqueue(s);
        visited[s] = true;
        parent[s] = -1;

        while (q.Count != 0)
        {
            int u = q.Dequeue();

            for (int v = 0; v < _v; ++v)
            {
                if (visited[v] == false && rGraph[u, v] > 0)
                {
                    q.Enqueue(v);
                    parent[v] = u;
                    visited[v] = true;
                }
            }
        }

        return visited[t];
    }
    
    private const string FilePath = @"Data\Lab4.txt";
}
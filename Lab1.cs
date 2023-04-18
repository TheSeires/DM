using DM.Interfaces;

namespace DM;

public class Lab1 : ILab
{
    public void Init()
    {
        string fileText = File.ReadAllText(FilePath);
        var rows = fileText.Split('\n');

        int[,] matrix = new int[rows.Length, rows.Length];

        Console.WriteLine("Matrix: ");
        
        for (int i = 0; i < rows.Length; i++)
        {
            var cols = rows[i].Trim().Split(' ');
            
            for (int j = 0; j < cols.Length; j++)
            {
                matrix[i, j] = int.Parse(cols[j]);
                Console.Write(matrix[i, j] + "\t");
            }

            Console.WriteLine();
        }

        List<Edge> edges = new List<Edge>();
        
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if(i > j || matrix[i, j] == 0)
                    continue;
                
                Edge edge = new Edge(i, j, matrix[i, j]);
                edges.Add(edge);
            }
        }

        KruskalMST kruskalMst = new KruskalMST(matrix.GetLength(0));

        Console.WriteLine("\nMin weight of spinning tree:");
        Print(kruskalMst.GetSpanningTree(edges));

        Console.WriteLine("\nMax weight of spinning tree:");
        Print(kruskalMst.GetSpanningTree(edges, true));
    }
    
    private void Print(List<Edge> result)
    {
        for (int i = 0; i < result.Count; ++i)
            Console.WriteLine("Edge: ({0}, {1}) = {2}", result[i].Source + 1, result[i].Destination + 1, result[i].Weight);

        Console.WriteLine("Total weight: " + result.Sum(x => x.Weight));
    }

    private class Edge : IComparable<Edge>
    {
        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }

        public int Source { get; set; }
        public int Destination { get; set; }
        public int Weight { get; set; }

        public int CompareTo(Edge other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }
    
    private class KruskalMST
    {
        private readonly int _verticesCount;

        public KruskalMST(int verticesCount) => _verticesCount = verticesCount;

        private int Find(int[] parents, int vertex)
        {
            if (parents[vertex] != vertex)
            {
                parents[vertex] = Find(parents, parents[vertex]);
            }
            return parents[vertex];
        }

        private void Union(int[] parents, int[] ranks, int x, int y)
        {
            int xRoot = Find(parents, x);
            int yRoot = Find(parents, y);

            if (ranks[xRoot] < ranks[yRoot])
            {
                parents[xRoot] = yRoot;
            }
            else if (ranks[xRoot] > ranks[yRoot])
            {
                parents[yRoot] = xRoot;
            }
            else
            {
                parents[yRoot] = xRoot;
                ranks[xRoot]++;
            }
        }

        public List<Edge> GetSpanningTree(List<Edge> edges, bool findMax = false)
        {
            edges.Sort();
            
            if(findMax)
                edges.Reverse();

            List<Edge> spanningTree = new List<Edge>();
            int[] parents = new int[_verticesCount];
            int[] ranks = new int[_verticesCount];
            
            for (int i = 0; i < _verticesCount; i++)
            {
                parents[i] = i;
                ranks[i] = 0;
            }

            foreach (Edge edge in edges)
            {
                if (Find(parents, edge.Source) != Find(parents, edge.Destination))
                {
                    spanningTree.Add(edge);
                    Union(parents, ranks, edge.Source, edge.Destination);
                }
            }

            return spanningTree;
        }
    }
    
    private const string FilePath = @"Data\Lab1.txt";
}
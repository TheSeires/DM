using DM.Interfaces;

namespace DM;

public class Lab2 : ILab
{
    public void Init()
    {
        string fileText = File.ReadAllText(FilePath);
        var rows = fileText.Split('\n');

        int[,] matrix = new int[rows.Length, rows.Length];

        for (int i = 0; i < rows.Length; i++)
        {
            var cols = rows[i].Trim().Split(' ');

            for (int j = 0; j < cols.Length; j++)
            {
                matrix[i, j] = int.Parse(cols[j]);
            }
        }

        Console.WriteLine("Matrix:");
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j]}\t");
            }
            Console.WriteLine();
        }

        int[,]? eulerGraph = GetEulerGraph(matrix);
        int distance = FindMinimumDistance(eulerGraph);
        Console.WriteLine($"\nThe minimum distance the postman must travel is {distance}.");
    }

    int[,]? GetEulerGraph(int[,] graph)
    {
        int n = graph.GetLength(0);
        int[,] degreeMatrix = new int[n, n];
        int[,] eulerGraph = new int[n, n];
        int[] degree = new int[n];
        int oddCount = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (graph[i, j] > 0)
                {
                    degree[i]++;
                    degree[j]++;
                    degreeMatrix[i, j] = degreeMatrix[j, i] = 1;
                    eulerGraph[i, j] = eulerGraph[j, i] = graph[i, j];
                }
            }

            if (degree[i] % 2 == 1)
            {
                oddCount++;
            }
        }

        if (oddCount == 0)
        {
            return eulerGraph;
        }

        if (oddCount == 2)
        {
            int startVertex = 0;
            for (int i = 0; i < n; i++)
            {
                if (degree[i] % 2 == 1)
                {
                    startVertex = i;
                    break;
                }
            }

            for (int i = startVertex + 1; i < n; i++)
            {
                if (degree[i] % 2 == 1)
                {
                    degreeMatrix[startVertex, i] = degreeMatrix[i, startVertex] = 1;
                    eulerGraph[startVertex, i] = eulerGraph[i, startVertex] = INF;
                    break;
                }
            }

            return eulerGraph;
        }
        else
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (degreeMatrix[i, j] == 0)
                    {
                        degreeMatrix[i, j] = degreeMatrix[j, i] = 1;
                        eulerGraph[i, j] = eulerGraph[j, i] = INF;
                        int[,]? tempGraph = GetEulerGraph(eulerGraph);
                        if (tempGraph != null)
                        {
                            return tempGraph;
                        }

                        degreeMatrix[i, j] = degreeMatrix[j, i] = 0;
                        eulerGraph[i, j] = eulerGraph[j, i] = 0;
                    }
                }
            }

            return null;
        }
    }
    
    int FindMinimumDistance(int[,]? graph)
    {
        if (graph == null)
            return 0;
        
        int n = graph.GetLength(0);
        int[,] dist = new int[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                dist[i, j] = graph[i, j];
                if (dist[i, j] == 0 && i != j)
                {
                    dist[i, j] = INF;
                }
            }
        }
        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (dist[i, k] != INF && dist[k, j] != INF && dist[i, k] + dist[k, j] < dist[i, j])
                    {
                        dist[i, j] = dist[i, k] + dist[k, j];
                    }
                }
            }
        }
        int minDistance = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (graph[i, j] > 0)
                {
                    minDistance += dist[i, j];
                }
            }
        }
        return minDistance;
    }

    private const int INF = 999999;
    private const string FilePath = @"Data\Lab2.txt";
}
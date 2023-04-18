using DM.Interfaces;

namespace DM;

public class Lab3 : ILab
{
    int _n;
    int[,] _graph;
    List<int> _path;
    int _minCost = int.MaxValue;
    
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

        _n = matrix.GetLength(0);
        _graph = matrix;
        _path = new List<int>();
        bool[] visited = new bool[_n];
        visited[0] = true;

        BranchAndBound(1, 0, new List<int> { 0 }, visited);

        Console.WriteLine("\nMinimum Cost: " + _minCost);
        Console.Write("Path: ");
        for (var i = 0; i < _path.Count; i++)
        {
            Console.Write(_path[i] + 1);
            
            if (i + 1 != _path.Count) 
                Console.Write(" -> ");
        }
    }
    
    void BranchAndBound(int level, int cost, List<int> currentPath, bool[] visited)
    {
        if (level == _n)
        {
            if (cost + _graph[currentPath[_n - 1], currentPath[0]] < _minCost)
            {
                currentPath.Add(currentPath[0]);
                _path = currentPath;
                _minCost = cost + _graph[currentPath[_n - 1], currentPath[0]];
            }
        }
        else
        {
            for (int i = 0; i < _n; i++)
            {
                if (!visited[i])
                {
                    List<int> newPath = new List<int>(currentPath);
                    newPath.Add(i);
                    bool[] newVisited = (bool[])visited.Clone();
                    newVisited[i] = true;

                    int newCost = cost + _graph[currentPath[level - 1], i];
                    if (newCost + LowerBound(newPath, newVisited) < _minCost)
                    {
                        BranchAndBound(level + 1, newCost, newPath, newVisited);
                    }
                }
            }
        }
    }
    
    int LowerBound(List<int> currentPath, bool[] visited)
    {
        int cost = 0;
        for (int i = 0; i < _n; i++)
        {
            if (!visited[i])
            {
                int min = int.MaxValue;
                for (int j = 0; j < _n; j++)
                {
                    if (i != j && !visited[j] && _graph[i, j] < min)
                    {
                        min = _graph[i, j];
                    }
                }
                cost += min;
            }
        }
        return cost;
    }
    
    private const string FilePath = @"Data\Lab3.txt";
}
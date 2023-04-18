using DM.Interfaces;

namespace DM;

public class Lab5 : ILab
{
    public void Init()
    {
        int[,] matrix1, matrix2;
        
        matrix1 = ReadMatrixFromFile(FilePath1);
        matrix2 = ReadMatrixFromFile(FilePath2);

        PrintMatrix(matrix1);
        Console.WriteLine();
        PrintMatrix(matrix2);
        Console.WriteLine();

        Console.WriteLine($"Graphs are isomorphic: {Isomorphic(matrix1, matrix2)}");
    }
    
    bool Isomorphic(int[,] graph1, int[,] graph2)
    {
        int vertices1 = graph1.GetLength(0);
        int vertices2 = graph2.GetLength(0);

        if (vertices1 != vertices2)
        {
            return false;
        }

        int[] usedVertices2 = new int[vertices2];

        for (int i = 0; i < vertices1; i++)
        {
            int degree1 = 0;
            int degree2 = 0;
            int candidate = -1;

            // Пошук потенційного кандидата для відповідності з поточною вершиною
            for (int j = 0; j < vertices2; j++)
            {
                if (usedVertices2[j] == 0 && graph1[i, i] == graph2[j, j])
                {
                    int k;
                    for (k = 0; k < vertices1; k++)
                    {
                        if (graph1[i, k] != graph1[k, i] && graph1[i, k] != 0 && graph1[i, k] == graph2[j, k])
                        {
                            degree1++;
                        }

                        if (graph2[j, k] != graph2[k, j] && graph2[j, k] != 0 && graph2[j, k] == graph1[i, k])
                        {
                            degree2++;
                        }
                    } if (degree1 == degree2 && (candidate == -1 || degree1 > degree2))
                    {
                        candidate = j;
                    }
                }
            }

            // Якщо не знайдено потенційного кандидата, графи не є ізоморфними
            if (candidate == -1)
            {
                return false;
            }

            usedVertices2[candidate] = 1;
        }

        return true;
    }

    private int[,] ReadMatrixFromFile(string path)
    {
        string fileText = File.ReadAllText(path);
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

        return graph;
    }

    private void PrintMatrix(int[,] graph)
    {
        Console.WriteLine("Matrix:");
        for (int i = 0; i < graph.GetLength(0); i++)
        {
            for (int j = 0; j < graph.GetLength(1); j++)
            {
                Console.Write($"{graph[i, j]}\t");
            }

            Console.WriteLine();
        }
    }
    
    private const string FilePath1 = @"Data\Lab5_1.txt";
    private const string FilePath2 = @"Data\Lab5_2.txt";

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph2D.Array
{
    static class ArrayCreator
    {
        public static async Task<int[][]> GetGraphArray(List<string> list)
        {
            int[][] matrix = new int[list.Count()][];
            for (int i = 0; i < list.Count(); i++)
                matrix[i] = new int[matrix.Count()];

            string[] line;
            for (int i = 0; i < matrix.Length; i++)
            {
                line = list[i].Split(' ');
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (!int.TryParse(line[j], out int value))
                        matrix[i][j] = 0;
                    matrix[i][j] = value;
                }
            }

            return matrix;
        }

        public static Task<int[][]> GenerateGraphArray(int vertices, double propability) => Task.Run(() =>
        {
            Random random = new Random();
            var array = new int[vertices][];
            for (int i = 0; i < vertices; i++)
                array[i] = new int[vertices];

            for (int i = 0; i < vertices; i++)
            {
                for (int j = i; j < vertices; j++)
                {
                    if (i != j && propability > random.NextDouble())
                    {
                        array[i][j] = 1;
                        array[j][i] = 1;
                    }
                    else
                    {
                        array[i][j] = 0;
                        array[j][i] = 0;
                    }
                }
            }

            return array;
        });

        public static Task<int[][]> GenerateGraphArray(int vertices, double propability, int edges) => Task.Run(() =>
        {
            Random random1 = new Random();
            Random random2 = new Random();
            var array = new int[vertices][];
            int numberOfEdges = 0;
            for (int i = 0; i < vertices; i++)
                array[i] = new int[vertices];

            do
            {
                for (int i = 0; i < vertices; i++)
                {
                    for (int j = i; j < vertices; j++)
                    {
                        if (i != j && propability > (random1.Next(1, 10000) / 10000) && numberOfEdges != edges && array[i][j] != 1 && random1.Next(10, 1000) >= random2.Next(10, 1000))
                        {
                            array[i][j] = 1;
                            array[j][i] = 1;
                            numberOfEdges++;
                        }
                    }
                }
            } while (numberOfEdges != edges);

            return array;
        });
    }
}

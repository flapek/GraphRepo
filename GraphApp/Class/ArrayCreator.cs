using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Class
{
    static class ArrayCreator
    {
        public static async Task<int[][]> GetGraphArray(List<string> l)
        {
            int[][] matrix = new int[l.Count()][];
            for (int i = 0; i < l.Count(); i++)
                matrix[i] = new int[matrix.Count()];

            string[] line;
            for (int i = 0; i < matrix.Length; i++)
            {
                line = l[i].Split(' ');
                for (int j = 0; j < matrix.Length; j++)
                {
                    if (!int.TryParse(line[j], out int value))
                        matrix[i][j] = 0;
                    matrix[i][j] = value;
                }
            }

            return matrix;
        }

        public static async Task<int[][]> GenerateGraphArray(int nodes, double propability)
        {
            Random random = new Random();
            var array = new int[nodes][];
            for (int i = 0; i < nodes; i++)
                array[i] = new int[nodes];

            for (int i = 0; i < nodes; i++)
            {
                for (int j = 0; j < nodes; j++)
                {
                    if (i != j && propability > (random.Next(1, 10000) / 10000))
                    {
                        array[i][j] = 1;
                        array[j][i] = 1;
                    }
                }
            }

            return array;
        }

        public static async Task<int[][]> GenerateGraphArray(int nodes, double propability, int edges)
        {
            Random random1 = new Random();
            Random random2 = new Random();
            var array = new int[nodes][];
            int numberOfEdges = 0;
            for (int i = 0; i < nodes; i++)
                array[i] = new int[nodes];

            do
            {
                for (int i = 0; i < nodes; i++)
                {
                    for (int j = i; j < nodes; j++)
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
        }
    }
}

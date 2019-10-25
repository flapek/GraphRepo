using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Class
{
    class ArrayCreator
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
    }
}

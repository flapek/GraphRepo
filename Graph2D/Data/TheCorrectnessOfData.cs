using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Graph2D.Data
{
    static class TheCorrectnessOfData
    {
        /// <summary>
        /// check that the text entered is correct
        /// </summary>

        private static readonly Regex _regex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        public static Task<bool> IsTextAllowed(string text) => Task.FromResult(!_regex.IsMatch(text));
        public static Task<bool> IsAValueInTheRange(double min, double max, string text)
        {
            if (!double.TryParse(text, out double result))
                return Task.FromResult(false);
            if (min <= result && result <= max)
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }
        public static Task<bool> IsTheCorrectNumberOfEdges(string edges, string nodes)
        {
            if (!int.TryParse(edges, out int edgesResult) || !int.TryParse(nodes, out int nodesResult))
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(edgesResult < Calculations.CalculateTheBorderNumberOfEdges(nodesResult));
        }
        public static Task<bool> checkGraphCompact(int[][] graph) => Task.Run(() =>
        {
            int[] array = new int[graph.Length];
            int sum;
            for (int i = 0; i < graph.Length; i++)
            {
                sum = 0;
                for (int j = 0; j < graph.Length; j++)
                {
                    sum += graph[i][j];
                }
                array[i] = sum;
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (!(array[i] > 0))
                {
                    return false;
                }
            }
            return true;
        });
    }
}

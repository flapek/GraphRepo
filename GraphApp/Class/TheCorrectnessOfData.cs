using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphApp.Class
{
    class TheCorrectnessOfData

    {
        /// <summary>
        /// check that the text entered is correct
        /// </summary>

        private static readonly Regex _regex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        public static async Task<bool> IsTextAllowed(string text) => !_regex.IsMatch(text);

        public static async Task<bool> IsAValueInTheRange(double min, double max, string text)
        {
            if (!double.TryParse(text, out double result))
                return false;
            if (min <= result && result <= max)
                return true;
            else
                return false;
        }
        public static async Task<bool> IsTheCorrectNumberOfEdges(string edges, string nodes)
        {
            if (!int.TryParse(edges, out int edgesResult) || !int.TryParse(nodes, out int nodesResult))
            {
                return false;
            }
            return edgesResult < Calculations.CalculateTheBorderNumberOfEdges(nodesResult);
        }
    }
}

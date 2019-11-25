using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph2D.Data
{
    static class Calculations
    {
        public static double CalculateTheBorderNumberOfEdges(int nodes) => 0.5 * (nodes - 1) * nodes;
    }
}

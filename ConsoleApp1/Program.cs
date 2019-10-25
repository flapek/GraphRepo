using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> l = new List<string>();
            l.Add("0 1 1 0 0");
            l.Add("1 0 1 0 0");
            l.Add("1 1 0 1 0");
            l.Add("0 0 1 0 0");
            l.Add("0 0 1 0 0");

            int[][] tab = GetGraphArray(l);

            for (int i = 0; i < tab.Length; i++)
            {
                for (int j = 0; j < tab.Length; j++)
                {
                    Console.Write(tab[i][j] + " ");
                }
                Console.WriteLine();
            }

            Console.ReadLine();

        }

        public static int[][] GetGraphArray(List<string> l)
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

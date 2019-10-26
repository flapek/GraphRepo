using System;
using System.Collections.Generic;
using System.IO;
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
            var path = @"C:\Users\filap\source\repos\Graph\GraphApp\Example\GraphExample.txt";

            int[][] tab = GetGraphArray(ReadFile(path));

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

        public static List<string> ReadFile(string path)
        {
            List<string> list = new List<string>();
            using (StreamReader stream = new StreamReader(path))
            {
                while (stream.Peek() >= 0)
                {
                    list.Add(stream.ReadLine());
                }
            }

            return list;
        }
    }
}

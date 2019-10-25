using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp.Class
{
    class Read
    {
        public static async Task<List<string>> ReadFile(string path)
        { 
            List<string> matrixList = new List<string>();
            using (StreamReader stream = new StreamReader(path))
            {
                while (stream.Peek() >= 0)
                {
                    matrixList.Add(stream.ReadLine());
                }
            }

            return matrixList;
        }

    }
}

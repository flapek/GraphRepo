using System;
using System.Threading.Tasks;
using GraphApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GraphUnitTest
{
    [TestClass]
    public class ReadFileTest
    {
        MainWindow mw = new MainWindow();

        [TestMethod]
        public async void GraphFromFileTestAsync()
        {
            string path = @"C:\Users\filap\source\repos\Graph\GraphApp\Example\GraphExample.txt";
            int[][] result = await mw.ReadFile(path);
            
            int[][] expectedResult = new int[5][];
            for (int i = 0; i < expectedResult.Length; i++)
            {
                expectedResult[i] = new int[5];
                for (int j = 0; j < expectedResult[i].Length; j++)
                {
                    expectedResult[i][j] = 0;
                }
            }

            Assert.AreEqual(expectedResult, result);
        }
    }
}

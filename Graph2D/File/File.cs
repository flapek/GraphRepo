using Graph2D.Array;
using Microsoft.Win32;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Graph2D.File
{
    static class File
    {
        internal static Task<List<string>> ReadFile(string path)
        {
            return Task.Run(() =>
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
            });
        }
        internal static Task<int[][]> OpenFile(string filter, bool multiselect)
        {
            return Task.Run(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = multiselect,
                    Filter = filter,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    var graphList = ReadFile(openFileDialog.FileName);
                    var arrayGraph = ArrayCreator.GetGraphArray(graphList.Result);
                    return arrayGraph.Result;
                }
                return null;
            });
        }
        internal static Task<string> SaveFile(int[][] graphArray, string filter)
        {
            return Task.Run(() =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = filter,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };
                if (saveFileDialog.ShowDialog() == true)
                {
                    GeneratePDF(saveFileDialog.FileName, graphArray);

                    System.Diagnostics.Process.Start(saveFileDialog.FileName);

                    return saveFileDialog.FileName;
                }
                else return string.Empty;
            });
        }
        internal static Task SaveFile(string savedFilePath, int[][] graphArray)
        {
            return Task.Run(() =>
            {
                GeneratePDF(savedFilePath, graphArray);
                System.Diagnostics.Process.Start(savedFilePath);
            });
        }
        private static Point[] GetPoints(int number)
        {
            Random random = new Random();
            Point[] cords = new Point[number];
            for (int i = 0; i < number; i++)
            {
                cords[i] = new Point(random.Next(50, 450), random.Next(50, 700));
            }
            return cords;
        }
        private static void GeneratePDF(string path, int[][] graphArray)
        {
            PdfDocument doc = new PdfDocument();
            PdfPageBase page = doc.Pages.Add();

            Point[] cord = GetPoints(graphArray.Length);

            for (int i = 0; i < graphArray.Length; i++)
                for (int j = i; j < graphArray.Length; j++)
                    if (graphArray[i][j] == 1)
                        page.Canvas.DrawLine(PdfPens.Black,
                            float.Parse(cord[i].X.ToString()) + 10,
                            float.Parse(cord[i].Y.ToString()) + 10,
                            float.Parse(cord[j].X.ToString()) + 10,
                            float.Parse(cord[j].Y.ToString()) + 10);

            for (int i = 0; i < cord.Length; i++)
            {
                page.Canvas.DrawEllipse(PdfBrushes.LightGray,
                    float.Parse(cord[i].X.ToString()),
                    float.Parse(cord[i].Y.ToString()), 30, 30);
                page.Canvas.DrawString($"V{(i + 1).ToString()}",
                            new PdfFont(PdfFontFamily.Helvetica, 10),
                            new PdfSolidBrush(new PdfRGBColor(255, 255, 255)),
                            float.Parse(cord[i].X.ToString()) + 6,
                            float.Parse(cord[i].Y.ToString()) + 6);
            }

            page.Canvas.DrawLine(PdfPens.Black, 10, 740, 585, 740);
            page.Canvas.DrawString(DateTime.UtcNow.ToString(),
                       new PdfFont(PdfFontFamily.Helvetica, 14),
                       new PdfSolidBrush(new PdfRGBColor(0, 0, 0)),
                       330, 740);

            FileStream stream = System.IO.File.Create(path);
            stream.Close();
            FileStream toStream = new FileStream(path, FileMode.Open);
            doc.SaveToStream(toStream);
            toStream.Close();
            doc.Close();
        }

    }
}

using Microsoft.Win32;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphApp.Class
{
    static class File
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

        public static async Task<int[][]> OpenFile(string filter, bool multiselect)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = multiselect;
            openFileDialog.Filter = filter;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                var graphList = Class.File.ReadFile(openFileDialog.FileName);
                var arrayGraph = ArrayCreator.GetGraphArray(graphList.Result);
                return arrayGraph.Result;
            }
            return null;
        }

        public static async Task SaveFile(Point[] cord, int[][] array, string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (saveFileDialog.ShowDialog() == true)
            {
                PdfDocument doc = new PdfDocument();
                PdfPageBase page = doc.Pages.Add();

                for (int i = 0; i < array.Length; i++)
                    for (int j = i; j < array.Length; j++)
                        if (array[i][j] == 1)
                            page.Canvas.DrawLine(PdfPens.Blue, float.Parse(cord[i].X.ToString()) / 2 + 10, float.Parse(cord[i].Y.ToString()) / 2 + 10,
                                float.Parse(cord[j].X.ToString()) / 2 + 10, float.Parse(cord[j].Y.ToString()) / 2 + 10);

                for (int i = 0; i < cord.Length; i++)
                {
                    page.Canvas.DrawEllipse(PdfBrushes.Black, float.Parse(cord[i].X.ToString()) / 2, float.Parse(cord[i].Y.ToString()) / 2, 26, 26);
                    page.Canvas.DrawString("N" + (i + 1),
                                new PdfFont(PdfFontFamily.Helvetica, 10), new PdfSolidBrush(new PdfRGBColor(255, 255, 255)),
                                float.Parse(cord[i].X.ToString()) / 2 + 5, float.Parse(cord[i].Y.ToString()) / 2 + 5);
                }

                page.Canvas.DrawLine(PdfPens.Black, 10, 740, 585, 740);
                page.Canvas.DrawString(DateTime.UtcNow.ToString(),
                           new PdfFont(PdfFontFamily.Helvetica, 14),
                           new PdfSolidBrush(new PdfRGBColor(0, 0, 0)),
                           330, 740);

                var path = saveFileDialog.FileName;
                FileStream stream = System.IO.File.Create(path);
                stream.Close();
                FileStream toStream = new FileStream(path, FileMode.Open);
                doc.SaveToStream(toStream);
                toStream.Close();
                doc.Close();

                System.Diagnostics.Process.Start(path);
            }
        }
    }
}

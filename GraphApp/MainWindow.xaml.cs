using GraphApp.Class;
using GraphApp.Model;
using Microsoft.Win32;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pathToFile { get; set; }

        public MainWindow()
        {
            InitializeComponent();

        }

        #region Window state

        /// <summary>
        /// Use button to change window state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        /// <summary>
        /// Use button to change window state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                MaximizeBtn.ToolTip = "Restore Down";
                WindowState = WindowState.Maximized;
                MaximizeBtnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
            }
            else
            {
                MaximizeBtn.ToolTip = "Maximize";
                WindowState = WindowState.Normal;
                MaximizeBtnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
            }
        }

        /// <summary>
        /// Use button to change window state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBtn_Click(object sender, RoutedEventArgs e) => this.Close();

        /// <summary>
        /// Use button to change window state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            DragMove();
        }

        /// <summary>
        /// Use button to change window state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            if (WindowState == WindowState.Normal)
            {
                MaximizeBtn.ToolTip = "restore down";
                WindowState = WindowState.Maximized;
            }
            else
            {
                MaximizeBtn.ToolTip = "maximize";
                WindowState = WindowState.Normal;
            }
        }

        #endregion

        #region Button 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ClearGraphBtn_Click(object sender, RoutedEventArgs e)
        {
            GraphDrawCanvas.Children.Clear();
        }

        private async void DrawGraphBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!await TheCorrectnessOfData.IsAValueInTheRange(10, 30, TextBoxNodes.Text) || !await TheCorrectnessOfData.IsAValueInTheRange(0.35, 0.45, TextBoxPropability.Text) ||
                !await TheCorrectnessOfData.IsTheCorrectNumberOfEdges(TextBoxEdges.Text, TextBoxNodes.Text))
            {
                MessageBox.Show("Incorect value", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxNodes.Clear();
                return;
            }

            if (TextBoxNodes.Text == String.Empty)
                DrawGraph(await ArrayCreator.GenerateGraphArray(int.Parse(TextBoxNodes.Text), double.Parse(TextBoxPropability.Text)));
            else
                DrawGraph(await ArrayCreator.GenerateGraphArray(int.Parse(TextBoxNodes.Text), double.Parse(TextBoxPropability.Text), int.Parse(TextBoxEdges.Text)));
        }

        #endregion

        #region TextBox

        /// <summary>
        /// Check text before input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TextBoxNodes_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !await TheCorrectnessOfData.IsTextAllowed(e.Text);
        /// <summary>
        /// Check text before input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TextBoxPropability_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !await TheCorrectnessOfData.IsTextAllowed(e.Text);
        /// <summary>
        /// Check text before input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TextBoxEdges_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !await TheCorrectnessOfData.IsTextAllowed(e.Text);
        /// <summary>
        /// Change TextBoxEdges.ToolTip after input text to TextBoxVertex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TextBoxNodes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!await TheCorrectnessOfData.IsTheCorrectNumberOfEdges(TextBoxEdges.Text, TextBoxNodes.Text))
                return;
            TextBoxEdges.ToolTip = $"m < {Calculations.CalculateTheBorderNumberOfEdges(int.Parse(TextBoxNodes.Text))}";

        }

        #endregion

        #region MenuItem

        private async void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                pathToFile = openFileDialog.FileName;
                var graphList = Read.ReadFile(@"C:\Users\filap\source\repos\Graph\GraphApp\Example\GraphExample.txt");
                var arrayGraph = ArrayCreator.GetGraphArray(graphList.Result);
                DrawGraph(arrayGraph.Result);
            }
        }
        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf";
            SaveFile(filter);
        }
        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf|PNG (*.png)|*.png";
            SaveFile(filter);
        }
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => Close();

        #endregion

        #region ComandBinding

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf";
            SaveFile(filter);
        }
        public void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf|PNG (*.png)|*.png";
            SaveFile(filter);
        }
        public void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (openFileDialog.ShowDialog() == true)
            {
                pathToFile = openFileDialog.FileName;
                var graphList = Read.ReadFile(@"C:\Users\filap\source\repos\Graph\GraphApp\Example\GraphExample.txt");
                var arrayGraph = ArrayCreator.GetGraphArray(graphList.Result);
                DrawGraph(arrayGraph.Result);
            }
        }

        #endregion

        #region Method

        private async void DrawGraph(int[][] array)
        {
            Random rnd = new Random();
            var cord = new Point[array.Length];
            int numberOfNode = 0;
            for (int i = 0; i < array.Length; i++)
                for (int j = i; j < array.Length; j++)
                    if (array[i][j] == 1)
                        numberOfNode++;

            for (int i = 0; i < array.Length; i++)
                cord[i] = new Point(rnd.Next(50, 500), rnd.Next(50, 500));

            for (int i = 0; i < array.Length; i++)
                for (int j = i; j < array.Length; j++)
                    if (array[i][j] == 1)
                        GraphDrawCanvas.Children.Add(await Draw.DrawEdge(cord[i], cord[j]));
            for (int i = 0; i < array.Length; i++)
                GraphDrawCanvas.Children.Add(await Draw.DrawNode(new Node()
                {
                    Id = "A" + (i + 1),
                    CenterPoint = cord[i]
                }));

        }

        #endregion

        private void SaveFile(string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = filter;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (saveFileDialog.ShowDialog() == true)
            {
                PdfDocument doc = new PdfDocument();
                PdfPageBase page = doc.Pages.Add();
                page.Canvas.DrawEllipse(PdfBrushes.Black, 100, 100, 20, 20);
                page.Canvas.DrawEllipse(PdfBrushes.Black, 200, 200, 20, 20);
                page.Canvas.DrawLine(PdfPens.Blue, 110, 110, 210, 210);
                page.Canvas.DrawString("A",
                            new PdfFont(PdfFontFamily.Helvetica, 10),
                            new PdfSolidBrush(new PdfRGBColor(255, 255, 255)),
                            105, 105);

                page.Canvas.DrawLine(PdfPens.Black, 10, 740, 585, 740);
                page.Canvas.DrawString(DateTime.UtcNow.ToString(),
                           new PdfFont(PdfFontFamily.Helvetica, 14),
                           new PdfSolidBrush(new PdfRGBColor(0, 0, 0)),
                           330, 740);

                var path = saveFileDialog.FileName;
                FileStream stream = File.Create(path);
                stream.Close();
                FileStream toStream = new FileStream(path, FileMode.Open);
                doc.SaveToStream(toStream);
                toStream.Close();
                doc.Close();
            }
        }
    }
}

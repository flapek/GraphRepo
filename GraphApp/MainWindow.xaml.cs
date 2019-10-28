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
            if (!await TheCorrectnessOfData.IsAValueInTheRange(10, 30, TextBoxNodes.Text) || !await TheCorrectnessOfData.IsAValueInTheRange(0.35, 0.45, TextBoxPropability.Text))
            {
                MessageBox.Show("Incorect value", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxNodes.Clear();
                return;
            }
            //GraphDrawGrid.Children.Add(await Draw.DrawElipse(new Point(0, 0), 100, 100));

            GraphDrawCanvas.Children.Add(await Draw.DrawNode(new Node()
            {
                Id = "A",
                CenterPoint = new Point(100, 100)
            }));

            GraphDrawCanvas.Children.Add(await Draw.DrawNode(new Node()
            {
                Id = "B",
                CenterPoint = new Point(200, 200)
            }));

            GraphDrawCanvas.Children.Add(await Draw.DrawEdge(new Point(100, 100), new Point(200, 200)));
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
            if (!int.TryParse(TextBoxNodes.Text, out int nodes))
                return;
            if (!int.TryParse(TextBoxEdges.Text, out int edges))
                edges = 0;
            if (await TheCorrectnessOfData.IsTheCorrectNumberOfEdges(edges, nodes))
            {
                TextBoxEdges.ToolTip = $"m < {Calculations.CalculateTheBorderNumberOfEdges(nodes)}";
            }
        }

        #endregion

        #region MenuItem

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF (*.pdf)|*.pdf";
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

                var path = saveFileDialog.FileName;
                FileStream stream = File.Create(path);
                stream.Close();
                FileStream toStream = new FileStream(path, FileMode.Open);
                doc.SaveToStream(toStream);
                toStream.Close();
                doc.Close();
            }

            
        }
        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF (*.pdf)|*.pdf|PNG (*.png)|*.png";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (saveFileDialog.ShowDialog() == true)
            {
                
            }
        }

        #endregion
    }
}

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
using System.Windows.Media.Media3D;

namespace GraphApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point3D[] NodeCord { get; set; }
        private int[][] GraphArray { get; set; }
        private double xPosition;
        private double yPosition;
        private Model3DGroup MainModel3Dgroup = new Model3DGroup();

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
        /// Drag top of application to move window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TopGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void ClearGraphBtn_Click(object sender, RoutedEventArgs e) => SpaceToDraw.Children.Clear();

        private async void DrawGraphBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!await TheCorrectnessOfData.IsAValueInTheRange(10, 30, TextBoxNodes.Text) || !await TheCorrectnessOfData.IsAValueInTheRange(0.35, 0.45, TextBoxPropability.Text) ||
                !await TheCorrectnessOfData.IsTheCorrectNumberOfEdges(TextBoxEdges.Text, TextBoxNodes.Text))
            {
                MessageBox.Show("Incorect value", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxNodes.Clear();
                return;
            }

            if (TextBoxNodes.Text == "")
            {
                GraphArray = await ArrayCreator.GenerateGraphArray(int.Parse(TextBoxNodes.Text), double.Parse(TextBoxPropability.Text));
                DrawGraph(GraphArray);
            }
            else
            {
                GraphArray = await ArrayCreator.GenerateGraphArray(int.Parse(TextBoxNodes.Text), double.Parse(TextBoxPropability.Text), int.Parse(TextBoxEdges.Text));
                DrawGraph(GraphArray);
            }
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
            GraphArray = await Class.File.OpenFile("Text files (*.txt)|*.txt", false);
            if (GraphArray != null)
            {
                // DrawGraph(GraphArray);
                //GraphDrawCanvas.Children.Clear();
            }
        }
        private async void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf";
            //await Class.File.SaveFile(NodeCord, GraphArray, filter);
        }
        private async void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf|PNG (*.png)|*.png";
            //await Class.File.SaveFile(NodeCord, GraphArray, filter);
        }
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => Close();

        #endregion

        #region ComandBinding

        private async void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf";
           // await Class.File.SaveFile(NodeCord, GraphArray, filter);
        }
        private async void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf|PNG (*.png)|*.png";
            //await Class.File.SaveFile(NodeCord, GraphArray, filter);
        }
        private async void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GraphArray = await Class.File.OpenFile("Text files (*.txt)|*.txt", false);
            if (GraphArray != null)
            {
                //DrawGraph(GraphArray);
                //GraphDrawCanvas.Children.Clear();
            }
        }

        #endregion

        #region Method

        private async void DrawGraph(int[][] array)
        {
            Random rnd = new Random();
            NodeCord = new Point3D[array.Length];
            int numberOfNode = 0;
            for (int i = 0; i < array.Length; i++)
                for (int j = i; j < array.Length; j++)
                    if (array[i][j] == 1)
                        numberOfNode++;

            for (int i = 0; i < array.Length; i++)
                NodeCord[i] = new Point3D(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());

            //for (int i = 0; i < array.Length; i++)
            //    for (int j = i; j < array.Length; j++)
            //        if (array[i][j] == 1)
            //            GraphDrawCanvas.Children.Add(await Draw.DrawEdge(NodeCord[i], NodeCord[j]));

            for (int i = 0; i < array.Length; i++)
            {
                DefineModel(MainModel3Dgroup, NodeCord[i]);
                ModelVisual3D modelVisual = new ModelVisual3D();
                modelVisual.Content = MainModel3Dgroup;

                Viewport3D.Children.Add(modelVisual);
            }
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
                FileStream stream = System.IO.File.Create(path);
                stream.Close();
                FileStream toStream = new FileStream(path, FileMode.Open);
                doc.SaveToStream(toStream);
                toStream.Close();
                doc.Close();
            }
        }

        #region Viewport3D

        private void SpaceToDraw_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ModelVisual3D obj = null;
            HitTestResult hitTest = VisualTreeHelper.HitTest(SpaceToDraw, e.GetPosition(SpaceToDraw));

            if (hitTest == null)
                return;

            xPosition = e.GetPosition(SpaceToDraw).X;
            yPosition = e.GetPosition(SpaceToDraw).Y;
            MouseMove += Camera_MouseMove;
        }
        private void Camera_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.GetPosition(SpaceToDraw).X > xPosition && e.LeftButton == MouseButtonState.Pressed)
            {
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X + 0.04, PerspectiveCamera.Position.Y, PerspectiveCamera.Position.Z);
                xPosition = e.GetPosition(SpaceToDraw).X;
                yPosition = e.GetPosition(SpaceToDraw).Y;
            }
            if (e.GetPosition(SpaceToDraw).X < xPosition && e.LeftButton == MouseButtonState.Pressed)
            {
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X - 0.04, PerspectiveCamera.Position.Y, PerspectiveCamera.Position.Z);
                xPosition = e.GetPosition(SpaceToDraw).X;
                yPosition = e.GetPosition(SpaceToDraw).Y;
            }
            if (e.GetPosition(SpaceToDraw).Y > yPosition && e.LeftButton == MouseButtonState.Pressed)
            {
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X, PerspectiveCamera.Position.Y - 0.04, PerspectiveCamera.Position.Z);
                xPosition = e.GetPosition(SpaceToDraw).X;
                yPosition = e.GetPosition(SpaceToDraw).Y;
            }
            if (e.GetPosition(SpaceToDraw).Y < yPosition && e.LeftButton == MouseButtonState.Pressed)
            {
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X, PerspectiveCamera.Position.Y + 0.04, PerspectiveCamera.Position.Z);
                xPosition = e.GetPosition(SpaceToDraw).X;
                yPosition = e.GetPosition(SpaceToDraw).Y;
            }
        }

        private void SpaceToDraw_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X, PerspectiveCamera.Position.Y, PerspectiveCamera.Position.Z - 0.2);
            else
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X, PerspectiveCamera.Position.Y, PerspectiveCamera.Position.Z + 0.2);
        }

        //move to outside class
        private async void DefineModel(Model3DGroup model_group, Point3D centerPoint)
        {
            MeshGeometry3D mesh1 = new MeshGeometry3D();
            await Draw.Node(mesh1, centerPoint, 0.05, 10, 50);
            SolidColorBrush brush1 = Brushes.White;
            DiffuseMaterial material1 = new DiffuseMaterial(brush1);
            GeometryModel3D model1 = new GeometryModel3D(mesh1, material1);
            model_group.Children.Add(model1);
        }

        private void SpaceToDraw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            ModelVisual3D obj = null;
            HitTestResult hitTest = VisualTreeHelper.HitTest(SpaceToDraw, e.GetPosition(SpaceToDraw));

            if (hitTest != null)
                obj = hitTest.VisualHit as ModelVisual3D;

            if (obj == null)
                return;

            obj.Transform = new TranslateTransform3D(-e.GetPosition(SpaceToDraw).X / 1000, -e.GetPosition(SpaceToDraw).Y / 1000, 0);
        }

        #endregion


    }
}

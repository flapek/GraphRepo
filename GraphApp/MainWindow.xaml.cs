using GraphApp.Class;
using GraphApp.Model;
using GraphApp.ViewModel;
using Microsoft.Win32;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.ObjectModel;
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
        readonly NodeViewModel NodeViewModel = new NodeViewModel();
        private Point3D[] NodeCord { get; set; }
        private int[][] GraphArray { get; set; }
        private double xPosition;
        private double yPosition;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = NodeViewModel;
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

        private void ClearGraphBtn_Click(object sender, RoutedEventArgs e) => Viewport3D.Children.Clear();

        private async void DrawGraphBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (!await TheCorrectnessOfData.IsAValueInTheRange(10, 30, TextBoxNodes.Text) || !await TheCorrectnessOfData.IsAValueInTheRange(0.35, 0.45, TextBoxPropability.Text))
            //{
            //    MessageBox.Show("Incorect value", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    TextBoxNodes.Clear();
            //    TextBoxPropability.Clear();
            //    return;
            //}
            //if (await TheCorrectnessOfData.IsTheCorrectNumberOfEdges(TextBoxEdges.Text, TextBoxNodes.Text))
            //{
            //    MessageBox.Show("Value out of the range", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    TextBoxEdges.Clear();
            //    return;
            //}

            //if (TextBoxEdges.Text == "")
            //{
            //    GraphArray = await ArrayCreator.GenerateGraphArray(int.Parse(TextBoxNodes.Text), double.Parse(TextBoxPropability.Text));
            //    DrawGraph(GraphArray);
            //}
            //else
            //{
            //    GraphArray = await ArrayCreator.GenerateGraphArray(int.Parse(TextBoxNodes.Text), double.Parse(TextBoxPropability.Text), int.Parse(TextBoxEdges.Text));
            //    DrawGraph(GraphArray);
            //}


            Random rnd = new Random();
            NodeCord = new Point3D[1];
            NodeCord[0] = new Point3D(rnd.NextDouble(), rnd.NextDouble(), rnd.NextDouble());
            Model3DGroup model3DGroup = new Model3DGroup();
            DefineModel(model3DGroup, NodeCord[0]);
            ModelVisual3D modelVisual = new ModelVisual3D
            {
                Content = model3DGroup
            };
            NodeViewModel.modelVisual3Ds.Add(modelVisual);
        }

        #endregion

        #region TextBox

        /// <summary>
        /// Check text before input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TextBoxNodes_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !await TheCorrectnessOfData.IsTextAllowed(e.Text);
        }

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
                DrawGraph(GraphArray);
            }
        }
        private async void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf";
            await Class.File.SaveFile(NodeCord, GraphArray, filter);
        }
        private async void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf|PNG (*.png)|*.png";
            await Class.File.SaveFile(NodeCord, GraphArray, filter);
        }
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => Close();

        #endregion

        #region ComandBinding

        private async void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GraphArray = await Class.File.OpenFile("Text files (*.txt)|*.txt", false);
            if (GraphArray != null)
            {
                DrawGraph(GraphArray);
            }
        }
        private async void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf";
            await Class.File.SaveFile(NodeCord, GraphArray, filter);
        }
        private async void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf|PNG (*.png)|*.png";
            await Class.File.SaveFile(NodeCord, GraphArray, filter);
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
                Model3DGroup model3DGroup = new Model3DGroup();
                DefineModel(model3DGroup, NodeCord[i]);
                ModelVisual3D modelVisual = new ModelVisual3D
                {
                    Content = model3DGroup
                };

                Viewport3D.Children.Add(modelVisual);
            }

        }

        #endregion

        #region Viewport3D

        private void SpaceToDraw_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
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
            if (e.GetPosition(SpaceToDraw).X > xPosition && e.RightButton == MouseButtonState.Pressed)
            {
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X + 0.04, PerspectiveCamera.Position.Y, PerspectiveCamera.Position.Z);
                xPosition = e.GetPosition(SpaceToDraw).X;
                yPosition = e.GetPosition(SpaceToDraw).Y;
            }
            if (e.GetPosition(SpaceToDraw).X < xPosition && e.RightButton == MouseButtonState.Pressed)
            {
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X - 0.04, PerspectiveCamera.Position.Y, PerspectiveCamera.Position.Z);
                xPosition = e.GetPosition(SpaceToDraw).X;
                yPosition = e.GetPosition(SpaceToDraw).Y;
            }
            if (e.GetPosition(SpaceToDraw).Y > yPosition && e.RightButton == MouseButtonState.Pressed)
            {
                PerspectiveCamera.Position = new Point3D(PerspectiveCamera.Position.X, PerspectiveCamera.Position.Y - 0.04, PerspectiveCamera.Position.Z);
                xPosition = e.GetPosition(SpaceToDraw).X;
                yPosition = e.GetPosition(SpaceToDraw).Y;
            }
            if (e.GetPosition(SpaceToDraw).Y < yPosition && e.RightButton == MouseButtonState.Pressed)
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
            MeshGeometry3D mesh = new MeshGeometry3D();
            await Draw.Node(mesh, centerPoint, 0.05, 30, 100);
            SolidColorBrush brush = Brushes.White;
            DiffuseMaterial material = new DiffuseMaterial(brush);
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            model_group.Children.Add(new DirectionalLight(Colors.White, new Vector3D(-0.612372, -0.5, -0.612372)));
            model_group.Children.Add(model);
        }

        private void SpaceToDraw_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            MouseMove += NodeMove;
            xPosition = e.GetPosition(SpaceToDraw).X;
            yPosition = e.GetPosition(SpaceToDraw).Y;
        }

        private void NodeMove(object sender, MouseEventArgs e)
        {
            ModelVisual3D obj = null;
            HitTestResult hitTest = VisualTreeHelper.HitTest(SpaceToDraw, e.GetPosition(SpaceToDraw));

            if (hitTest != null)
                obj = hitTest.VisualHit as ModelVisual3D;

            if (obj == null)
                return;

            var positionX = (e.GetPosition(SpaceToDraw).X - xPosition) / 1000;
            var positionY = (yPosition - e.GetPosition(SpaceToDraw).Y) / 1000;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                obj.Transform = new TranslateTransform3D(positionX, positionY, 0);
                xPosition = e.GetPosition(SpaceToDraw).X;
                yPosition = e.GetPosition(SpaceToDraw).Y;
            }
        }

        #endregion


    }
}

using GraphApp.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            DragMove();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            if (WindowState == WindowState.Normal)
            {
                MaximizeBtn.ToolTip = "Restore Down";
                WindowState = WindowState.Maximized;
            }
            else
            {
                MaximizeBtn.ToolTip = "Maximize";
                WindowState = WindowState.Normal;
            }
        }

        #endregion

        #region Grapg Algorithms

        public async Task<int[][]> ReadFile(string path)
        {
            int[][] matrix;
            List<string> matrixList = new List<string>();
            using (StreamReader stream = new StreamReader(path))
            {
                while (stream.Peek() >= 0)
                {
                    matrixList.Add(stream.ReadLine());
                }
            }

            matrix = new int[matrixList.Count()][];
            for (int i = 0; i < matrixList.Count(); i++)
            {
                matrix[i] = new int[matrix.Count()];
            }

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    matrix[i][j] = 0;
                }
            }

            return matrix;
        }

        #region Draw Elipse

        public System.Windows.Shapes.Path DrawElipse(Point startPoint, int radiusX, int radiusY)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            path.HorizontalAlignment = HorizontalAlignment.Center;
            path.VerticalAlignment = VerticalAlignment.Center;
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = startPoint;
            ellipseGeometry.RadiusX = radiusX;
            ellipseGeometry.RadiusY = radiusY;
            path.Data = ellipseGeometry;
            return path;
        }

        #endregion

        #region Draw Path

        public async Task<System.Windows.Shapes.Path> DrawPath(Point startPoint)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            path.HorizontalAlignment = HorizontalAlignment.Center;
            path.VerticalAlignment = VerticalAlignment.Center;
            LineGeometry lineGeometry = new LineGeometry();
            lineGeometry.StartPoint = startPoint;
            lineGeometry.EndPoint = new Point(startPoint.X + 100, startPoint.Y + 100);
            path.Data = lineGeometry;
            return path;
        }

        #endregion

        #endregion

        #region Button 

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            GraphDrawGrid.Children.Add(DrawElipse(new Point(0, 0), 100, 100));
            GraphDrawGrid.Children.Add(await DrawPath(new Point(0, 0)));
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            GraphDrawGrid.Children.Clear();
        }

        #endregion

        #region TextBox input

        private async void TextBoxVertex_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !await TheCorrectnessOfTheText.IsTextAllowed(e.Text);
            var textBox = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(e.Text) && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                string str = textBox.Text + e.Text;
                if (str.Length <= 2)
                {
                    if (!await TheCorrectnessOfTheText.IsAValueInTheRange(10, 30, str))
                    {
                        MessageBox.Show("Incorect value", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        //do poprawy
        private async void TextBoxPropability_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !await TheCorrectnessOfTheText.IsTextAllowed(e.Text);
            var textBox = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(e.Text) && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                string str = textBox.Text + e.Text;
                if (str.Length <= 4 && str.Length >=3)
                {
                    if (!await TheCorrectnessOfTheText.IsAValueInTheRange(0.35, 0.45, str))
                    {
                        MessageBox.Show("Incorect value", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        #endregion


    }
}

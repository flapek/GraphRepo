using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            GraphDrawGrid.Children.Add(DrawElipse(new Point(0, 300), 100, 300));
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
            }
            else
            {
                MaximizeBtn.ToolTip = "Maximize";
                WindowState = WindowState.Normal;
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
            path.HorizontalAlignment = HorizontalAlignment.Left;
            path.VerticalAlignment = VerticalAlignment.Center;
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = startPoint;
            myEllipseGeometry.RadiusX = radiusX;
            myEllipseGeometry.RadiusY = radiusY;
            path.Data = myEllipseGeometry;
            return path;
        }


        #endregion

        #endregion

        
    }
}

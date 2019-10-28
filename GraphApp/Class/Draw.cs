using GraphApp.Model;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphApp.Class
{
    class Draw
    {
        public static async Task<System.Windows.Shapes.Path> DrawEdge(Point startPoint, Point endPoint)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            LineGeometry lineGeometry = new LineGeometry();
            lineGeometry.StartPoint = startPoint;
            lineGeometry.EndPoint = endPoint;
            path.Data = lineGeometry;
            return path;
        }

        public static async Task<System.Windows.Shapes.Path> DrawNode(Node vertex)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Stroke = Brushes.Black;
            path.Fill = Brushes.Gray;
            path.StrokeThickness = 2;
            path.Name = vertex.Id;
            EllipseGeometry ellipseGeometry = new EllipseGeometry(vertex.CenterPoint, 15, 15);
            path.Data = ellipseGeometry;

            path.MouseEnter += Node_MouseEnter;
            return path;
        }

        private static void Node_MouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Shapes.Path path = sender as System.Windows.Shapes.Path;
            //MessageBox.Show(path.Name);
            path.ToolTip = path.Name;
        }
    }
}

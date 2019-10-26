using GraphApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GraphApp.Class
{
    class Draw
    {
        public static async Task<System.Windows.Shapes.Path> DrawElipse(Point startPoint, int radiusX, int radiusY)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = startPoint;
            ellipseGeometry.RadiusX = radiusX;
            ellipseGeometry.RadiusY = radiusY;
            path.Data = ellipseGeometry;
            return path;
        }

        public static async Task<System.Windows.Shapes.Path> DrawPath(Point startPoint, Point endPoint)
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

        public static async Task<System.Windows.Shapes.Path> DrawVertex(Vertex vertex)
        {
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            path.Name = vertex.Id;
            path.MouseEnter += Vertex_MouseEnter;
            //path.MouseLeave += Vertex_MouseLeave;
            
            EllipseGeometry ellipseGeometry = new EllipseGeometry(vertex.Point, 10, 10);
            
            path.Data = ellipseGeometry;
            return path;

        }

        //private static void Vertex_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    System.Windows.Shapes.Path path = sender as System.Windows.Shapes.Path;
        //    MessageBox.Show(path.Name);
        //}

        private static void Vertex_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            System.Windows.Shapes.Path path = sender as System.Windows.Shapes.Path;
            MessageBox.Show(path.Name);
        }
    }
}

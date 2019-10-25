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
            path.HorizontalAlignment = HorizontalAlignment.Center;
            path.VerticalAlignment = VerticalAlignment.Center;
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = startPoint;
            ellipseGeometry.RadiusX = radiusX;
            ellipseGeometry.RadiusY = radiusY;
            path.Data = ellipseGeometry;
            return path;
        }

        public static async Task<System.Windows.Shapes.Path> DrawPath(Point startPoint)
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

    }
}

using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel view = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            itemsControl.ItemsSource = view.ModelVisual3Ds;
        }

        ModelVisual3D Build3DModel => new ModelVisual3D
        {
            Content = new GeometryModel3D
            {
                Geometry = new MeshGeometry3D
                {
                    TriangleIndices = new Int32Collection(
                            new int[] { 0, 1, 2, 3, 4, 5 }),
                    Positions = new Point3DCollection(
                            new Point3D[]
                            {
                                new Point3D(-0.5,-0.5,0.5),
                                new Point3D( 0.5,-0.5,0.5),
                                new Point3D(0.5,0.5,0.5),
                                new Point3D( 0.5,0.5,0.5),
                                new Point3D(-0.5,0.5,0.5),
                                new Point3D(-0.5,-0.5,0.5)
                            })
                },
                Material = new DiffuseMaterial
                {
                    Color = Colors.Black
                },
                Transform = new TranslateTransform3D
                {
                    OffsetX = 0,
                    OffsetY = 0,
                    OffsetZ = 0
                }
            }
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            view.ModelVisual3Ds.Add(Build3DModel);
        }
    }
}

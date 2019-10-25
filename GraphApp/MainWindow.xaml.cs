using GraphApp.Class;
using System.Threading.Tasks;
using System.Windows;
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

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            GraphDrawGrid.Children.Clear();
        }

        private async void DrawGraphBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!await TheCorrectnessOfTheText.IsAValueInTheRange(10, 30, TextBoxVertex.Text) || !await TheCorrectnessOfTheText.IsAValueInTheRange(0.35, 0.45, TextBoxPropability.Text))
            {
                MessageBox.Show("Incorect value", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxVertex.Clear();
                return;
            }
            GraphDrawGrid.Children.Add(await Draw.DrawElipse(new Point(0, 0), 100, 100));
            GraphDrawGrid.Children.Add(await Draw.DrawPath(new Point(0, 0)));
        }

        #endregion

        #region TextBox input

        private async void TextBoxVertex_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !await TheCorrectnessOfTheText.IsTextAllowed(e.Text);

        private async void TextBoxPropability_PreviewTextInput(object sender, TextCompositionEventArgs e) => e.Handled = !await TheCorrectnessOfTheText.IsTextAllowed(e.Text);

        #endregion

        
    }
}

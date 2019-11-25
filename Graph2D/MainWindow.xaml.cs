using Graph2D.Array;
using Graph2D.Data;
using Graph2D.Model;
using Graph2D.ModelView;
using GraphX.Controls;
using GraphX.PCL.Common.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Graph2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Property

        private int[][] GraphArray { get; set; }
        private string SavedFileName { get; set; }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Window 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ZoomControl.SetViewFinderVisibility(ZoomCtrl, Visibility.Visible);
            ZoomCtrl.ZoomToFill();
            LayoutAlgorithmComboBox.ItemsSource = GetLayoutAlgorithmName();
            LayoutAlgorithmComboBox.SelectedIndex = 0;
        }

        #endregion

        #region Private method

        private void InitializeGraph(EdgeDashStyle edgeDashStyle = EdgeDashStyle.Solid)
        {
            GraphArea.GenerateGraph(true, true);
            GraphArea.SetVerticesDrag(true, true);
            GraphArea.SetEdgesDashStyle(edgeDashStyle);
            GraphArea.ShowAllEdgesLabels(false);
            GraphArea.ShowAllEdgesArrows(false);
            GraphArea.SetVerticesMathShape(VertexShape.Diamond);
            

            ZoomCtrl.ZoomToFill();
        }

        private List<string> GetLayoutAlgorithmName()
        {
            List<string> list = new List<string>();

            foreach (var item in Enum.GetNames(typeof(LayoutAlgorithmTypeEnum)))
                if (item != "Custom")
                    list.Add(item);

            return list;
        }

        

        #endregion

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
        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Close();

        /// <summary>
        /// Drag top of application to move window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TopGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;
            else if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Normal)
                {
                    MaximizeBtn.ToolTip = "restore down";
                    WindowState = WindowState.Maximized;
                    MaximizeBtnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;

                }
                else
                {
                    MaximizeBtn.ToolTip = "maximize";
                    WindowState = WindowState.Normal;
                    MaximizeBtnIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                }
            }
            else
                DragMove();
        }

        #endregion

        #region Button click
        private async void DrawGraphBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!int.TryParse(TextBoxVertices.Text, out int verticesResult) ||
                !double.TryParse(TextBoxPropability.Text, out double propabilityResult))
            {
                return;
            }
            GraphArray = await ArrayCreator.GenerateGraphArray(verticesResult, propabilityResult);
            GraphArea.LogicCore = GraphAreaModel.GraphArea_Setup(Graph.Graph_Setup(GraphArray));
            GraphAreaModel.GraphAreaSetupLayoutAlgorithm((LayoutAlgorithmTypeEnum)LayoutAlgorithmComboBox.SelectedIndex);
                       
            InitializeGraph();
            if (!await TheCorrectnessOfData.checkGraphCompact(GraphArray))
            {
                MessageBox.Show("Graph is not compact!!");
            }
        }

        private void ClearGraphBtn_Click(object sender, RoutedEventArgs e)
        {
            GraphArea.RemoveAllEdges();
            GraphArea.RemoveAllVertices();
        }

        #endregion

        #region Text Box 

        #region Input

        private async void TextBoxVertices_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            e.Handled = !await TheCorrectnessOfData.IsTextAllowed(e.Text);

        private async void TextBoxEdges_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            e.Handled = !await TheCorrectnessOfData.IsTextAllowed(e.Text);

        private async void TextBoxPropability_PreviewTextInput(object sender, TextCompositionEventArgs e) =>
            e.Handled = !await TheCorrectnessOfData.IsTextAllowed(e.Text);


        #endregion

        #endregion

        #region Menu item

        private async void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GraphArray = await File.File.OpenFile("Text files (*.txt)|*.txt", false);
            if (GraphArray != null)
            {
                GraphArea.LogicCore = GraphAreaModel.GraphArea_Setup(Graph.Graph_Setup(GraphArray));
                InitializeGraph();
            }
        }

        private async void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            var filter = "PDF (*.pdf)|*.pdf";
            if (SavedFileName != null || System.IO.File.Exists(SavedFileName))
                await File.File.SaveFile(SavedFileName, GraphArray);
            else
                SavedFileName = await File.File.SaveFile(GraphArray, filter);

        }

        private async void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf";
            await File.File.SaveFile(GraphArray, filter);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => Close();

        #endregion

        #region Command binding

        private async void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GraphArray = await File.File.OpenFile("Text files (*.txt)|*.txt", false);
            if (GraphArray != null)
            {
                GraphArea.LogicCore = GraphAreaModel.GraphArea_Setup(Graph.Graph_Setup(GraphArray));
                InitializeGraph();
            }
        }
        private async void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            var filter = "PDF (*.pdf)|*.pdf";
            if (SavedFileName != null || System.IO.File.Exists(SavedFileName))
                await File.File.SaveFile(SavedFileName, GraphArray);
            else
                SavedFileName = await File.File.SaveFile(GraphArray, filter);
        }

        private async void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var filter = "PDF (*.pdf)|*.pdf";
            await File.File.SaveFile(GraphArray, filter);
        }

        #endregion


        private void LayoutAlgorithmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            GraphAreaModel.GraphAreaSetupLayoutAlgorithm((LayoutAlgorithmTypeEnum)comboBox.SelectedIndex);
            if (GraphArray == null)
                return;
            GraphArea.GenerateGraph();
        }

    }
}

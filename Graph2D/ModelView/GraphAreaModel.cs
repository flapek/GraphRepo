using Graph2D.Model;
using GraphX.Controls;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;
using QuickGraph;

namespace Graph2D.ModelView
{
    /// <summary>
    /// This is custom GraphArea representation using custom data types.
    /// GraphArea is the visual panel component responsible for drawing visuals (vertices and edges).
    /// It is also provides many global preferences and methods that makes GraphX so customizable and user-friendly.
    /// </summary>
    class GraphAreaModel : GraphArea<DataVertexModel, DataEdgeModel, BidirectionalGraph<DataVertexModel, DataEdgeModel>>
    {
        private static GXLogicCoreModel logicCore = new GXLogicCoreModel();
        public static GXLogicCoreModel GraphArea_Setup(Graph graph)
        {
            //Lets create logic core and filled data graph with edges and vertices
            logicCore.Graph = graph;
            //Now we can set parameters for selected algorithm using AlgorithmFactory property. This property provides methods for
            //creating all available algorithms and algo parameters.
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Default parameters are created automaticaly when new default algorithm is set and previous params were NULL
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            //This property sets edge routing algorithm that is used to build route paths according to algorithm logic.
            //For ex., SimpleER algorithm will try to set edge paths around vertices so no edge will intersect any vertex.
            //Bundling algorithm will try to tie different edges that follows same direction to a single channel making complex graphs more appealing.
            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            logicCore.AsyncAlgorithmCompute = false;

            //Finally assign logic core to GraphArea object
            return logicCore;
        }

        public static void GraphAreaSetupLayoutAlgorithm(LayoutAlgorithmTypeEnum layoutAlgorithmTypeEnum)
        {
            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            logicCore.DefaultLayoutAlgorithm = layoutAlgorithmTypeEnum;
        }
    }


}

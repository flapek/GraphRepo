using QuickGraph;
using System.Linq;

namespace Graph2D.Model
{
    /// <summary>
    /// This is our custom data graph derived from BidirectionalGraph class using custom data types.
    /// Data graph stores vertices and edges data that is used by GraphArea and end-user for a variety of operations.
    /// Data graph content handled manually by user (add/remove objects). The main idea is that you can dynamicaly
    /// remove/add objects into the GraphArea layout and then use data graph to restore original layout content.
    /// </summary>
    class Graph : BidirectionalGraph<DataVertexModel, DataEdgeModel>
    {
        public static Graph Graph_Setup(int[][] graphArray)
        {
            var dataGraph = new Graph();
            for (int i = 0; i < graphArray.Length; i++)
                dataGraph.AddVertex(new DataVertexModel("V" + (i + 1)));

            var vertexArray = dataGraph.Vertices.ToList();

            for (int i = 0; i < graphArray.Length; i++)
                for (int j = i; j < graphArray.Length; j++)
                    if (graphArray[i][j] == 1)
                        dataGraph.AddEdge(new DataEdgeModel(vertexArray[i], vertexArray[j]) 
                        { 
                            ReversePath = false
                        });

            return dataGraph;
        }
    }
}

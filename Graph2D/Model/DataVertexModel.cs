using GraphX.PCL.Common.Models;

namespace Graph2D.Model
{
    class DataVertexModel : VertexBase
    {
        /// <summary>
        /// Some string property for example purposes
        /// </summary>
        public string Text { get; set; }

        #region Calculated or static props

        public override string ToString()
        {
            return Text;
        }

        #endregion

        /// <summary>
        /// Default parameterless constructor for this class
        /// (required for YAXLib serialization)
        /// </summary>
        public DataVertexModel() : this(string.Empty)
        {
        }

        public DataVertexModel(string text = "")
        {
            Text = text;
        }
    }
}

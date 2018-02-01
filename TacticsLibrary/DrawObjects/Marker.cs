using System;
using System.Drawing;
using System.Windows.Forms;
using TacticsLibrary.Adapters;
using TacticsLibrary.DrawObjects;

namespace TacticsLibrary.Interfaces
{
    /// <summary>
    /// Encapsulates a ReferencePoint that can be painted a special way
    /// </summary>
    public class Marker : ReferencePoint, IMarker
    {
        /// <summary>
        /// Action that is used to Paint the object
        /// </summary>
        public Action<IGraphics, IReferencePoint> PaintMethod { get; internal set; }

        public Marker(ISensor detectedBy, PointF position) 
            : base(position, new SizeF())
        {

        }

        public override void Draw(IGraphics g)
        {
            PaintMethod?.Invoke(g, this);
        }
    }
}

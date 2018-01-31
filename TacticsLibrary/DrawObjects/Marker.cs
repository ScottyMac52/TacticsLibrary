using System;
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
    }
}

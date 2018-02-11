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
        public Marker(ISensor detectedBy, PointF position) 
            : base(detectedBy, position)
        {

        }
    }
}

using System.Drawing;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.EventHandlers;

namespace TacticsLibrary.Interfaces
{
    /// <summary>
    /// Encapsulates a ReferencePoint that can be painted a special way
    /// </summary>
    public class Marker : ReferencePoint, IMarker
    {
        internal Marker(ISensor detectedBy, PointF position)
            : base(detectedBy, position)
        {
            ReferencePointChanged += Marker_ReferencePointChanged;
        }

        private void Marker_ReferencePointChanged(object sender, ReferencePointChangedEventArgs e)
        {
            if(e.EventType == Enums.UpdateEventTypes.New)
            {
                Logger.Info($"Created Marker: {this}");
            }
        }
    }
}

using log4net;
using System;
using System.Drawing;
using System.Windows.Forms;
using TacticsLibrary.Adapters;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.EventHandlers;

namespace TacticsLibrary.Interfaces
{
    /// <summary>
    /// Encapsulates a ReferencePoint that can be painted a special way
    /// </summary>
    public class Marker : ReferencePoint, IMarker
    {
        public Marker(ISensor detectedBy, PointF position, ILog logger)
            : base(detectedBy, position, logger)
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

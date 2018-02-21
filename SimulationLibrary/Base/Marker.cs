using GraphicsLibrary;
using SimulationLibrary.EventHandlers;
using SimulationLibrary.Interfaces;
using System.Drawing;

namespace SimulationLibrary
{
    /// <summary>
    /// Encapsulates a ReferencePoint that can be painted a special way
    /// </summary>
    public partial class Marker : ReferencePoint 
    {
        public Marker(ISensor detectedBy, PointF position) : base(detectedBy, position)
        {
            ReferencePointChanged += Marker_ReferencePointChanged;
        }

        private void Marker_ReferencePointChanged(object sender, ReferencePointChangedEventArgs e)
        {
            if(e.EventType == UpdateEventTypes.New)
            {
                Logger.Info($"Created Marker: {this}");
            }
        }
    }
}

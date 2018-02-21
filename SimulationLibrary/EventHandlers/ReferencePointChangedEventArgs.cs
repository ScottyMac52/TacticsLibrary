using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace SimulationLibrary.EventHandlers
{
    public class ReferencePointChangedEventArgs : PropertyChangedEventArgs
    {
        public ReferencePointChangedEventArgs(IList<RectangleF> rectanglesForRegion, UpdateEventTypes eventType, string propertyName) 
            : base(propertyName)
        {
            if ((rectanglesForRegion?.Count ?? 0) > 0)
            {
                RectangleRegionsF = new List<RectangleF>();
                RectangleRegionsF.AddRange(rectanglesForRegion);
            }
            EventType = eventType;
            PropertyName = propertyName;
        }

        public List<RectangleF> RectangleRegionsF { get; }

        public UpdateEventTypes EventType { get; }

        public override string PropertyName { get; }
    }
}

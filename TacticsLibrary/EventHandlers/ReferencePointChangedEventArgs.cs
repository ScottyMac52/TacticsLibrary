using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using TacticsLibrary.Enums;

namespace TacticsLibrary.EventHandlers
{
    public class ReferencePointChangedEventArgs : PropertyChangedEventArgs
    {
        public ReferencePointChangedEventArgs(IList<RectangleF> rectanglesForRegion, UpdateEventTypes eventType, string propertyName) 
            : base(propertyName)
        {
            RectangleRegionsF = new List<RectangleF>();
            RectangleRegionsF.AddRange(rectanglesForRegion);
            EventType = eventType;
            PropertyName = propertyName;
        }

        public List<RectangleF> RectangleRegionsF { get; }

        public UpdateEventTypes EventType { get; }

        public override string PropertyName { get; }
    }
}

using System;
using System.Drawing;
using TacticsLibrary.Enums;

namespace TacticsLibrary.TrackingObjects
{
    public class HistoricalPoint
    {
        public Guid UniqueId { get; protected set; }
        public Point Position { get; set; }
        public DateTime TimeStamp { get; set; }

        public ContactTypes ContactType { get; set; }

        public HistoricalPoint()
        {
            UniqueId = Guid.NewGuid();
        }
    }
}

using System;
using System.Drawing;
using TacticsLibrary.Enums;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Interfaces
{
    public interface IContact
    {
        void Draw(IGraphics g);
        Guid UniqueId { get; }
        DateTime TimeStamp { get;  }
        ContactTypes ContactType { get; }
        DateTime LastUpdate { get; }
        Point Position { get; }
        Point RelativePosition { get; }
        PolarCoordinate PolarPosit { get;  }
        Rectangle DetectionWindow { get; }
        System.Threading.Timer TrackTimer { get; }
        event EventHandler UpdatePending;
        string ToString();

        double Speed { get; set; }
        double Altitude { get; set; }
        double Heading { get; set; }

    }
}

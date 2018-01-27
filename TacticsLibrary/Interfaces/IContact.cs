using System;
using System.ComponentModel;
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
        PointF Position { get; }
        PointF RelativePosition { get; }
        PolarCoordinate PolarPosit { get;  }
        RectangleF DetectionWindow { get; }
        System.Threading.Timer TrackTimer { get; }
        event EventHandler UpdatePending;
        string ToString();

        double Speed { get; set; }
        double Altitude { get; set; }
        double Heading { get; set; }
        IRadar DetectedBy { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using TacticsLibrary.Enums;
using TacticsLibrary.DrawObjects;
using static TacticsLibrary.DrawObjects.ReferencePoint;

namespace TacticsLibrary.Interfaces
{
    public interface IContact
    {
        void Draw(IGraphics g);
        Guid UniqueId { get; }
        DateTime TimeStamp { get; }
        ContactTypes ContactType { get; }
        DateTime LastUpdate { get; }
        PointF Position { get; }
        PointF RelativePosition { get; }
        PolarCoordinate PolarPosit { get; }
        RectangleF DetectionWindow { get; }
        string ToString();

        double Speed { get; set; }
        double Altitude { get; set; }
        double Heading { get; set; }
        bool Selected { get; set; }
        bool ShowText { get; set; }
        ISensor DetectedBy { get; }
        event PropertyChangedEventHandler PropertyChanged;

        Thread ProcessThread { get; }
        bool Running { get; set; }
        int? CustomUpdateDuration { get; set; }
        event ReferencePointEventHandler UpdatePending;

    }
}

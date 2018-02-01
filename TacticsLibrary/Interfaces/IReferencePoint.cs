using System;
using System.ComponentModel;
using System.Drawing;
using TacticsLibrary.DrawObjects;
using static TacticsLibrary.DrawObjects.ReferencePoint;

namespace TacticsLibrary.Interfaces
{
    public interface IReferencePoint
    {
        string Name { get; set; }
        PointF Position { get; }
        PointF RelativePosition { get; }
        double Heading { get; set; }
        double Speed { get; set; }
        double Altitude { get; set; }
        RectangleF DetectionWindow { get; }
        bool Selected { get; set; }
        bool ShowText { get; set; }
        ISensor DetectedBy { get; }
        DateTime TimeStamp { get; }
        DateTime LastUpdate { get; }
        PolarCoordinate PolarPosit { get; }
        Guid UniqueId { get; }
        void Draw(IGraphics g);
        event PropertyChangedEventHandler PropertyChanged;
        event ReferencePointEventHandler UpdatePending;

    }
}
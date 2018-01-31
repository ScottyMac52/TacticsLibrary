using System;
using System.ComponentModel;
using System.Drawing;
using TacticsLibrary.Interfaces;
using TacticsLibrary.DrawObjects;

namespace TacticsLibrary.Interfaces
{
    public interface IReferencePoint
    {
        double Altitude { get; set; }
        ISensor DetectedBy { get; }
        RectangleF DetectionWindow { get; }
        double Heading { get; set; }
        DateTime LastUpdate { get; }
        PolarCoordinate PolarPosit { get; }
        PointF Position { get; }
        PointF RelativePosition { get; }
        bool Selected { get; set; }
        bool ShowText { get; set; }
        double Speed { get; set; }
        DateTime TimeStamp { get; }
        Guid UniqueId { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
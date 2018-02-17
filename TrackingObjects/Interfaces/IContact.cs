using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using TacticsLibrary.Enums;
using TacticsLibrary.DrawObjects;
using static TacticsLibrary.DrawObjects.ReferencePoint;
using TacticsLibrary.EventHandlers;

namespace TacticsLibrary.Interfaces
{
    public interface IContact
    {
        void Draw(IGraphics g);
        void Stop();
        void Start();
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
        Thread ProcessThread { get; }
        ManualResetEvent StopEvent { get; }
        bool Running { get;  }
        int? CustomUpdateDuration { get; set; }
        event ReferencePointChangedEventHandler ReferencePointChanged;
    }
}

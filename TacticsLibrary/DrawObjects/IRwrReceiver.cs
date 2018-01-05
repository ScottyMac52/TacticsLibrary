using System;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Adapters;
using TacticsLibrary.Enums;

namespace TacticsLibrary.DrawObjects
{
    public interface IRwrReceiver
    {
        Point BullsEye { get; }
        float CenterPositionX { get; set; }
        float CenterPositionY { get; set; }
        SortedList<Guid, RwrPoint> PlottedPoints { get; }
        float Radius { get; set; }
        int RangeRings { get; set; }
        float RingSep { get; set; }
        void AddPoint(RwrPoint newPoint, ContactTypes contactType);
        void Draw(IGraphics g);
        RwrPoint PlotContact(ReferencePositions refPos, double radius, double degrees, double altitude, int speed, int course, ContactTypes contactType);
    }
}
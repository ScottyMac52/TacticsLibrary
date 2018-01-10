using System;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Adapters;
using TacticsLibrary.Enums;

namespace TacticsLibrary.DrawObjects
{
    public interface IRadar
    {
        Point BullsEye { get; }
        float CenterPositionX { get; set; }
        float CenterPositionY { get; set; }
        SortedList<Guid, PlottedPoint> PlottedPoints { get; }
        float Radius { get; set; }
        int RangeRings { get; set; }
        float RingSep { get; set; }
        void AddPoint(PlottedPoint newPoint, ContactTypes contactType);
        void Draw(IGraphics g);
        PlottedPoint PlotContact(ReferencePositions refPos, double radius, double degrees, double altitude, int speed, int course, ContactTypes contactType);
    }
}
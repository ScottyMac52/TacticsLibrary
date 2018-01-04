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
        List<RwrPoint> PlottedPoints { get; }
        float Radius { get; set; }
        int RangeRings { get; set; }
        bool Ready { get; }
        float RingSep { get; set; }
        void AddPoint(Point pt, string referenceText, ContactTypes contactType);
        void Draw(IGraphics g);
        void Invalidate(Rectangle invalidRect);
        void PlotContact(ReferencePositions refPos, double radius, double theta, ContactTypes contactType);
    }
}
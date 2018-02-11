using System.Drawing;
using TacticsLibrary.Enums;

namespace TacticsLibrary.Interfaces
{
    public interface IDrawContact
    {
        RectangleF DrawArea { get; }
        string ReferenceText { get; }
        SizeF ViewPortExtent { get; }

        void Draw(IGraphics g);
        void DrawVelocityVector();
        void DrawText();
        void DrawArc(RectangleF contactArea, Pen color);
        void DrawUpsidedownArc(RectangleF contactArea, Pen color);
        void DrawCircle(RectangleF contactArea, Pen color);
        void DrawCarat(PointF topLeft, Pen color);
        void DrawUpsidedownCarat(PointF topLeft, Pen color);
    }
}

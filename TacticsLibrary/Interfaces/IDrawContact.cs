using System.Drawing;
using TacticsLibrary.Enums;

namespace TacticsLibrary.Interfaces
{
    public interface IDrawContact
    {
        Rectangle DrawArea { get; }
        string ReferenceText { get; }
        Size ViewPortExtent { get; }

        void Draw(IGraphics g);
        void DrawVelocityVector();
        void DrawText();
        void DrawHostileAir(PointF topLeft);
        void DrawHostileSub(PointF topLeft);
        void DrawHostileSurface(PointF topLeft);
        void DrawArc(Rectangle contactArea, Pen color);
        void DrawUpsidedownArc(Rectangle contactArea, Pen color);
        void DrawCircle(Rectangle contactArea, Pen color);
        void DrawCarat(PointF topLeft, Pen color);
        void DrawUpsidedownCarat(PointF topLeft, Pen color);

    }
}

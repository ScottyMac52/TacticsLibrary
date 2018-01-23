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
        void DrawHostileAir(Point topLeft);
        void DrawHostileSub(Point topLeft);
        void DrawHostileSurface(Point topLeft);
        void DrawArc(Rectangle contactArea, Pen color);
        void DrawUpsidedownArc(Rectangle contactArea, Pen color);
        void DrawCircle(Rectangle contactArea, Pen color);
        void DrawCarat(Point topLeft, Pen color);
        void DrawUpsidedownCarat(Point topLeft, Pen color);

    }
}

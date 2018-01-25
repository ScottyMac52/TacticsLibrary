using System.Drawing;

namespace TacticsLibrary.Interfaces
{
    public interface IGraphics
    {
        Graphics Context { get; }
        void DrawLine(Pen color, PointF start, PointF end);
        void DrawEllipse(Pen pen, RectangleF rect);
        void DrawEllipse(Pen pen, float x, float y, float width, float height);
        void DrawEllipse(Pen pen, Rectangle rect);
        void DrawEllipse(Pen pen, int x, int y, int width, int height);
        void FillRectangle(Brush brush, int x, int y, int width, int height);
        void FillRectangle(Brush brush, float x, float y, float width, float height);
        void DrawString(string referenceText, Font statusFont, Brush blue, PointF topLeft);
        void DrawArc(Pen color, Rectangle contactArea, int v1, int v2);
        void DrawLines(Pen color, PointF[] point);
        void FillEllipse(Brush brush, float v1, float v2, float v3, float v4);
    }
}

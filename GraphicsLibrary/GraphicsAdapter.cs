using System.Drawing;

namespace GraphicsLibrary.Adapters
{
    public class GraphicsAdapter : IGraphics
    {
        private readonly Graphics g;

        public Graphics Context => g;

        public GraphicsAdapter(Graphics graphics)
        {
            g = graphics;
        }

        public void DrawLine(Pen color, PointF start, PointF end)
        {
            g.DrawLine(color, start, end);
        }

        public void DrawRectangle(Pen color, RectangleF rectToDraw)
        {
            g.DrawRectangle(color, rectToDraw.X, rectToDraw.Y, rectToDraw.Width, rectToDraw.Height);
        }

        public void DrawArc(Pen color, RectangleF contactArea, int v1, int v2)
        {
            g.DrawArc(color, contactArea, v1, v2);
        }

        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            g.DrawEllipse(pen, rect);
        }

        public void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            g.DrawEllipse(pen, x, y, width, height);
        }

        public void DrawEllipse(Pen pen, Rectangle rect)
        {
            g.DrawEllipse(pen, rect);
        }

        public void DrawEllipse(Pen pen, int x, int y, int width, int height)
        {
            g.DrawEllipse(pen, x, y, width, height);
        }

        public void DrawLines(Pen color, PointF[] point)
        {
            g.DrawLines(color, point);
        }

        public void DrawString(string text, Font font, Brush brush, PointF point)
        {
            g.DrawString(text, font, brush, point);
        }

        public void FillEllipse(Brush brush, float v1, float v2, float v3, float v4)
        {
            g.FillEllipse(brush, v1, v2, v3, v4);
        }

        public void FillRectangle(Brush brush, int x, int y, int width, int height)
        {
            g.FillEllipse(brush, x, y, width, height);
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            g.FillRectangle(brush, x, y, width, height);
        }
    }
}

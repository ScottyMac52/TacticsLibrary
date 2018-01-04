using System;
using System.Drawing;
using TacticsLibrary.Adapters;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.DrawObjects
{
    public class RwrPoint : HistoricalPoint, IVisibleObjects
    {
        public const int POSITION_OFFSET = 10;

        public string ReferenceText { get; set; }

        public Rectangle DrawArea { get; protected set; }

        public void Draw(IGraphics g)
        {
            var topLeft = new Point(Position.X - POSITION_OFFSET, Position.Y - POSITION_OFFSET);
            var contactArea = new Rectangle(topLeft, new Size(POSITION_OFFSET*2, POSITION_OFFSET*2));
            DrawArea = contactArea;

            // Draw the point of the contact in green
            g.FillRectangle(Brushes.Green, Position.X, Position.Y, 2, 2);

            switch (ContactType)
            {
                case Enums.ContactTypes.AirUnknown:
                    DrawArc(contactArea, g, Pens.Yellow);
                    break;
                case Enums.ContactTypes.AirFriendly:
                    DrawArc(contactArea, g, Pens.Green);
                    break;
                case Enums.ContactTypes.AirEnemy:
                    DrawHostileAir(topLeft, g);
                    break;
                case Enums.ContactTypes.SurfaceUnknown:
                    DrawCircle(contactArea, g, Pens.Yellow);
                    break;
                case Enums.ContactTypes.SurfaceFriendly:
                    DrawCircle(contactArea, g, Pens.Green);
                    break;
                case Enums.ContactTypes.SurfaceEnemy:
                    DrawHostileSurface(topLeft, g);
                    break;
                case Enums.ContactTypes.SubUnknown:
                    DrawUpsidedownArc(contactArea, g, Pens.Yellow);
                    break;
                case Enums.ContactTypes.SubFriendly:
                    DrawUpsidedownArc(contactArea, g, Pens.Green);
                    break;
                case Enums.ContactTypes.SubEnemy:
                    DrawHostileSub(topLeft, g);
                    break;
                case Enums.ContactTypes.MissileSRM:
                    g.FillRectangle(Brushes.Red, Position.X, Position.Y, 2, 2);
                    break;
                case Enums.ContactTypes.MissileMRM:
                    g.FillRectangle(Brushes.Red, Position.X, Position.Y, 3, 3);
                    break;
                case Enums.ContactTypes.MissileCruise:
                    g.FillRectangle(Brushes.Red, Position.X, Position.Y, 4, 4);
                    break;
            }

            Point drawText = DrawArea.Location;
            drawText.Offset(new Point(20, 0));
            g.DrawString(ReferenceText, SystemFonts.StatusFont, Brushes.White, drawText);
        }

        public void Invalidate(Rectangle invalidRect)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws a ^ at the position using the Top Left corner of the rectangle as reference
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="g"></param>
        public void DrawHostileAir(Point topLeft, IGraphics g)
        {
            DrawCarat(topLeft, g, Pens.Red);
        }

        public void DrawHostileSub(Point topLeft, IGraphics g)
        {
            DrawUpsidedownCarat(topLeft, g, Pens.Red);
        }

        public void DrawHostileSurface(Point topLeft, IGraphics g)
        {
            DrawCarat(topLeft, g, Pens.Red);
            DrawUpsidedownCarat(topLeft, g, Pens.Red);
        }

        /// <summary>
        /// Draws an Arc inside the contact area
        /// </summary>
        /// <param name="contactArea"></param>
        /// <param name="g"></param>
        /// <param name="color"></param>
        public void DrawArc(Rectangle contactArea, IGraphics g, Pen color)
        {
            g.DrawArc(color, contactArea, 180, 180);
        }

        public void DrawUpsidedownArc(Rectangle contactArea, IGraphics g, Pen color)
        {
            g.DrawArc(color, contactArea, 0, 180);
        }

        public void DrawCircle(Rectangle contactArea, IGraphics g, Pen color)
        {
            g.DrawEllipse(color, contactArea);
        }

        /// <summary>
        /// Draws a ^ at the position using the Top Left corner of the rectangle as reference
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="g"></param>
        /// <param name="color"></param>
        public void DrawCarat(Point topLeft, IGraphics g, Pen color)
        {
            g.DrawLines(color, new Point[] { new Point(topLeft.X, Position.Y), new Point(Position.X, topLeft.Y), new Point(topLeft.X + (POSITION_OFFSET*2), Position.Y) });
        }

        /// <summary>
        /// Draws an upside down ^ at the position using the Top Left corner of the rectangle as reference
        /// </summary>
        /// <param name="topLeft"></param>
        /// <param name="g"></param>
        /// <param name="color"></param>
        public void DrawUpsidedownCarat(Point topLeft, IGraphics g, Pen color)
        {
            g.DrawLines(color, new Point[] { new Point(topLeft.X, Position.Y), new Point(Position.X, Position.Y + POSITION_OFFSET), new Point(topLeft.X + (POSITION_OFFSET*2), Position.Y) });
        }

    }
}

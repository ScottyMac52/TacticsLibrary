using System;
using System.Drawing;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary.TrackingObjects
{
    public class DrawContact : IDrawContact, IVisibleObjects
    {
        public const int POSITION_OFFSET = 10;
        private const double SECONDS_PER_HOUR = 3600.00;

        public Point CurrentPosition { get; set; }
        public double CurrentSpeed { get; set; }
        public Rectangle DrawArea { get; private set; }
        public ContactTypes ContactType { get; private set; }
        public string ReferenceText { get; private set; }
        public IGraphics GraphicsContext { get; private set; }
        public double VelocityVectorTime { get; private set; }

        public DrawContact(ContactTypes contactType, double velocityVectorTime)
        {
            ContactType = contactType;
            VelocityVectorTime = velocityVectorTime;
        }

        /// <summary>
        /// Draws a line showing the course of the contact that is the equivalent of how far it will travel in VelocityVectorTime seconds
        /// </summary>
        public void DrawVelocityVector()
        {
            // Based on the course and speed calculate the distance in 10 seconds
            // Get the new position based on course and distance traveled
            // Now calculate the contacts movement in that timespan
            var distance = VelocityVectorTime * (CurrentSpeed / SECONDS_PER_HOUR);
            //var newPos = CoordinateConverter.CalculatePointFromDegrees(Position, distance, Course);
            //GraphicsContext.DrawLine(Pens.Green, Position, newPos);
        }

        /// <summary>
        /// Draw this contact
        /// </summary>
        /// <param name="g"></param>
        public void Draw(IGraphics g)
        {
            GraphicsContext = g;

            var topLeft = new Point(CurrentPosition.X - POSITION_OFFSET, CurrentPosition.Y - POSITION_OFFSET);
            DrawArea = new Rectangle(topLeft, new Size(POSITION_OFFSET * 2, POSITION_OFFSET * 2));

            // Draw the point of the contact in green
            GraphicsContext.FillEllipse(Brushes.Green, CurrentPosition.X, CurrentPosition.Y, 2, 2);

            switch (ContactType)
            {
                case Enums.ContactTypes.AirUnknown:
                    DrawArc(DrawArea, Pens.Yellow);
                    break;
                case Enums.ContactTypes.AirFriendly:
                    DrawArc(DrawArea, Pens.Green);
                    break;
                case Enums.ContactTypes.AirEnemy:
                    DrawHostileAir(DrawArea.Location);
                    break;
                case Enums.ContactTypes.SurfaceUnknown:
                    DrawCircle(DrawArea, Pens.Yellow);
                    break;
                case Enums.ContactTypes.SurfaceFriendly:
                    DrawCircle(DrawArea, Pens.Green);
                    break;
                case Enums.ContactTypes.SurfaceEnemy:
                    DrawHostileSurface(DrawArea.Location);
                    break;
                case Enums.ContactTypes.SubUnknown:
                    DrawUpsidedownArc(DrawArea, Pens.Yellow);
                    break;
                case Enums.ContactTypes.SubFriendly:
                    DrawUpsidedownArc(DrawArea, Pens.Green);
                    break;
                case Enums.ContactTypes.SubEnemy:
                    DrawHostileSub(DrawArea.Location);
                    break;
                case Enums.ContactTypes.MissileSRM:
                    GraphicsContext.FillRectangle(Brushes.Red, CurrentPosition.X, CurrentPosition.Y, 2, 2);
                    break;
                case Enums.ContactTypes.MissileMRM:
                    GraphicsContext.FillRectangle(Brushes.Red, CurrentPosition.X, CurrentPosition.Y, 3, 3);
                    break;
                case Enums.ContactTypes.MissileCruise:
                    GraphicsContext.FillRectangle(Brushes.Red, CurrentPosition.X, CurrentPosition.Y, 4, 4);
                    break;
            }

            DrawVelocityVector();
            DrawText();
        }

        /// <summary>
        /// Draws the text to describe the contact 
        /// </summary>
        /// <param name="g"><see cref="IGraphics"/></param>
        public void DrawText()
        {
            Point drawText = DrawArea.Location;
            drawText.Offset(new Point(20, 0));
            GraphicsContext.DrawString(ReferenceText, SystemFonts.StatusFont, Brushes.White, drawText);
        }

        /// <summary>
        /// Draws a ^ at the position using the Top Left corner of the rectangle as reference
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        public void DrawHostileAir(Point topLeft)
        {
            DrawCarat(topLeft, Pens.Red);
        }

        /// <summary>
        /// Draws a hostile subsurface contact
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        public void DrawHostileSub(Point topLeft)
        {
            DrawUpsidedownCarat(topLeft, Pens.Red);
        }

        /// <summary>
        /// Draws a hostile surface contact
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        public void DrawHostileSurface(Point topLeft)
        {
            DrawCarat(topLeft, Pens.Red);
            DrawUpsidedownCarat(topLeft, Pens.Red);
        }

        /// <summary>
        /// Draws an Arc in the contactArea for a friendly air contact
        /// </summary>
        /// <param name="contactArea"><see cref="Rectangle"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawArc(Rectangle contactArea, Pen color)
        {
            // Draw Arc from 180° for 180°
            GraphicsContext.DrawArc(color, contactArea, 180, 180);
        }

        /// <summary>
        /// Draws an upsidedown Arc in the contactArea for a friendly subsurface contact
        /// </summary>
        /// <param name="contactArea"><see cref="Rectangle"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawUpsidedownArc(Rectangle contactArea, Pen color)
        {
            // Draw an Arc from 0° to 180°
            GraphicsContext.DrawArc(color, contactArea, 0, 180);
        }

        /// <summary>
        /// Draws a Circle int he contactArea for a friendly surface contact
        /// </summary>
        /// <param name="contactArea"><see cref="Rectangle"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawCircle(Rectangle contactArea, Pen color)
        {
            GraphicsContext.DrawEllipse(color, contactArea);
        }

        /// <summary>
        /// Draws a ^ at the position using the Top Left for an enemy air contact
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawCarat(Point topLeft, Pen color)
        {
            GraphicsContext.DrawLines(color, new Point[] { new Point(topLeft.X, CurrentPosition.Y), new Point(CurrentPosition.X, topLeft.Y), new Point(topLeft.X + (POSITION_OFFSET * 2), CurrentPosition.Y) });
        }

        /// <summary>
        /// Draws an upside down ^ at the position using the Top Left for an enemy subsurface contact
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawUpsidedownCarat(Point topLeft, Pen color)
        {
            GraphicsContext.DrawLines(color, new Point[] { new Point(topLeft.X, CurrentPosition.Y), new Point(CurrentPosition.X, CurrentPosition.Y + POSITION_OFFSET), new Point(topLeft.X + (POSITION_OFFSET * 2), CurrentPosition.Y) });
        }
    }
}

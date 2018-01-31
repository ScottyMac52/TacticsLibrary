using log4net;
using System;
using System.Drawing;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary.DrawObjects
{
    public class DrawContact : IDrawContact, IVisibleObjects
    {
        public const int POSITION_OFFSET = 10;
        private const double SECONDS_PER_HOUR = 3600.00;
        private const double VELOCITY_VECTOR_FACTOR = 1.0;

        public IContact Contact { get; private set; }
        public ILog Logger { get; private set; }
        public RectangleF DrawArea { get; private set; }
        public string ReferenceText { get; private set; }
        public IGraphics GraphicsContext { get; private set; }
        public double VelocityVectorTime { get; private set; }
        public SizeF ViewPortExtent { get; private set; }

        public DrawContact(ILog logger, IContact contact, double velocityVectorTime, SizeF viewPortExtent)
        {
            Logger = logger;
            Contact = contact;
            VelocityVectorTime = velocityVectorTime;
            ViewPortExtent = viewPortExtent;
        }

        /// <summary>
        /// Draws a line showing the course of the contact that is the equivalent of how far it will travel in VelocityVectorTime seconds
        /// </summary>  
        public void DrawVelocityVector()
        {
            // Based on the course and speed calculate the distance in VelocityVectorTime seconds
            // Get the new position based on course and distance traveled
            // Now calculate the contacts movement in that timespan
            var milesPerSecond = (Contact.Speed / SECONDS_PER_HOUR);
            var distance = VelocityVectorTime * milesPerSecond * VELOCITY_VECTOR_FACTOR;
            // Calculate the position after the distance moved in bearing and distance from the current relative position
            var newPos = CoordinateConverter.CalculatePointFromDegrees(Contact.RelativePosition, new PolarCoordinate(Contact.Heading, distance), CoordinateConverter.ROUND_DIGITS);
            // Draw a line from the current absolute position to the newly estimated position of the contact
            GraphicsContext.DrawLine(Pens.Green, Contact.Position, newPos.GetAbsolutePosition(Contact.DetectedBy.ViewPortExtent));
        }

        /// <summary>
        /// Draw this contact
        /// </summary>
        /// <param name="g"></param>
        public void Draw(IGraphics g)
        {
            GraphicsContext = g;

            var topLeft = new PointF(Contact.Position.X - POSITION_OFFSET, Contact.Position.Y - POSITION_OFFSET);
            DrawArea = new RectangleF(topLeft, new SizeF(POSITION_OFFSET * 2, POSITION_OFFSET * 2));

            // Draw the point of the contact in green
            GraphicsContext.FillEllipse(Brushes.Green, Contact.Position.X, Contact.Position.Y, 2, 2);

            switch (Contact.ContactType)
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
                    GraphicsContext.FillRectangle(Brushes.Red, Contact.Position.X, Contact.Position.Y, 2, 2);
                    break;
                case Enums.ContactTypes.MissileMRM:
                    GraphicsContext.FillRectangle(Brushes.Red, Contact.Position.X, Contact.Position.Y, 3, 3);
                    break;
                case Enums.ContactTypes.MissileCruise:
                    GraphicsContext.FillRectangle(Brushes.Red, Contact.Position.X, Contact.Position.Y, 4, 4);
                    break;
            }

            DrawVelocityVector();
            if(Contact.Selected)
            {
                DrawDetectionWindow(Pens.White);
            }
            if(Contact.ShowText)
            {
                DrawText();
            }
        }

        /// <summary>
        /// Draws the text to describe the contact 
        /// </summary>
        /// <param name="g"><see cref="IGraphics"/></param>
        public void DrawText()
        {
            PointF drawText = DrawArea.Location;
            drawText.Offset(new PointF(20.0F, -20.0F), 4);
            ReferenceText = $"{Contact.PolarPosit} - {Contact.Heading}° ";
            GraphicsContext.DrawString(ReferenceText, SystemFonts.StatusFont, Brushes.White, drawText);
        }

        /// <summary>
        /// Draws a detection window around the contact
        /// </summary>
        /// <param name="color"></param>
        public void DrawDetectionWindow(Pen color)
        {
            GraphicsContext.DrawRectangle(color, Contact.DetectionWindow);
        }

        /// <summary>
        /// Draws a ^ at the position using the Top Left corner of the rectangle as reference
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        public void DrawHostileAir(PointF topLeft)
        {
            DrawCarat(topLeft, Pens.Red);
        }

        /// <summary>
        /// Draws a hostile subsurface contact
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        public void DrawHostileSub(PointF topLeft)
        {
            DrawUpsidedownCarat(topLeft, Pens.Red);
        }

        /// <summary>
        /// Draws a hostile surface contact
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        public void DrawHostileSurface(PointF topLeft)
        {
            DrawCarat(topLeft, Pens.Red);
            DrawUpsidedownCarat(topLeft, Pens.Red);
        }

        /// <summary>
        /// Draws an Arc in the contactArea for a friendly air contact
        /// </summary>
        /// <param name="contactArea"><see cref="Rectangle"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawArc(RectangleF contactArea, Pen color)
        {
            // Draw Arc from 180° for 180°
            GraphicsContext.DrawArc(color, contactArea, 180, 180);
        }

        /// <summary>
        /// Draws an upsidedown Arc in the contactArea for a friendly subsurface contact
        /// </summary>
        /// <param name="contactArea"><see cref="Rectangle"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawUpsidedownArc(RectangleF contactArea, Pen color)
        {
            // Draw an Arc from 0° to 180°
            GraphicsContext.DrawArc(color, contactArea, 0, 180);
        }

        /// <summary>
        /// Draws a Circle int he contactArea for a friendly surface contact
        /// </summary>
        /// <param name="contactArea"><see cref="Rectangle"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawCircle(RectangleF contactArea, Pen color)
        {
            GraphicsContext.DrawEllipse(color, contactArea);
        }

        /// <summary>
        /// Draws a ^ at the position using the Top Left for an enemy air contact
        /// </summary>
        /// <param name="topLeft"><see cref="PointF"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawCarat(PointF topLeft, Pen color)
        {
            GraphicsContext.DrawLines(color, new PointF[] { new PointF(topLeft.X, Contact.Position.Y), new PointF(Contact.Position.X, topLeft.Y), new PointF(topLeft.X + (POSITION_OFFSET * 2), Contact.Position.Y) });
        }

        /// <summary>
        /// Draws an upside down ^ at the position using the Top Left for an enemy subsurface contact
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawUpsidedownCarat(PointF topLeft, Pen color)
        {
            GraphicsContext.DrawLines(color, new PointF[] { new PointF(topLeft.X, Contact.Position.Y), new PointF(Contact.Position.X, Contact.Position.Y + POSITION_OFFSET), new PointF(topLeft.X + (POSITION_OFFSET * 2), Contact.Position.Y) });
        }
    }
}

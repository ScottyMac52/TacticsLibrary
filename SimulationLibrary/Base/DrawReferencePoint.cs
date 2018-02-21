using GraphicsLibrary;
using log4net;
using SimulationLibrary;
using SimulationLibrary.Converters;
using SimulationLibrary.Extensions;
using SimulationLibrary.Interfaces;
using System.Drawing;

namespace DrawingLibrary
{
    public class DrawReferencePoint : IDrawContact, IVisibleObjects
    {
        private const double SECONDS_PER_HOUR = 3600.00;
        private const double VELOCITY_VECTOR_FACTOR = 1.0;

        public IReferencePoint Contact { get; private set; }
        public ILog Logger { get; private set; }
        public RectangleF DrawArea { get; private set; }
        public string ReferenceText { get; private set; }
        public IGraphics GraphicsContext { get; private set; }
        public double VelocityVectorTime { get; private set; }
        public SizeF ViewPortExtent { get; private set; }

        public DrawReferencePoint(ILog logger, IReferencePoint contact, double velocityVectorTime, SizeF viewPortExtent)
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

            var topLeft = new PointF(Contact.Position.X - ReferencePoint.POSITION_OFFSET, Contact.Position.Y - ReferencePoint.POSITION_OFFSET);
            DrawArea = new RectangleF(topLeft, new SizeF(ReferencePoint.POSITION_OFFSET * 2, ReferencePoint.POSITION_OFFSET * 2));

            // Draw the point of the contact in green
            GraphicsContext.FillEllipse(Brushes.Green, Contact.Position.X, Contact.Position.Y, 2, 2);

            switch (Contact.ContactType)
            {
                case ContactTypes.AirUnknown:
                    DrawArc(DrawArea, Pens.Yellow);
                    break;
                case ContactTypes.AirFriendly:
                    DrawArc(DrawArea, Pens.Green);
                    break;
                case ContactTypes.AirEnemy:
                    DrawCarat(DrawArea.Location, Pens.Red);
                    break;
                case ContactTypes.SurfaceUnknown:
                    DrawCircle(DrawArea, Pens.Yellow);
                    break;
                case ContactTypes.SurfaceFriendly:
                    DrawCircle(DrawArea, Pens.Green);
                    break;
                case ContactTypes.SurfaceEnemy:
                    DrawCarat(DrawArea.Location, Pens.Red);
                    DrawUpsidedownCarat(DrawArea.Location, Pens.Red);
                    break;
                case ContactTypes.SubUnknown:
                    DrawUpsidedownArc(DrawArea, Pens.Yellow);
                    break;
                case ContactTypes.SubFriendly:
                    DrawUpsidedownArc(DrawArea, Pens.Green);
                    break;
                case ContactTypes.SubEnemy:
                    DrawUpsidedownCarat(DrawArea.Location, Pens.Red);
                    break;
                case ContactTypes.MissileSRM:
                    GraphicsContext.FillRectangle(Brushes.Red, Contact.Position.X, Contact.Position.Y, 2, 2);
                    break;
                case ContactTypes.MissileMRM:
                    GraphicsContext.FillRectangle(Brushes.Red, Contact.Position.X, Contact.Position.Y, 3, 3);
                    break;
                case ContactTypes.MissileCruise:
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
            ReferenceText = Contact.ToString();
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
            GraphicsContext.DrawLines(color, new PointF[] { new PointF(topLeft.X, Contact.Position.Y), new PointF(Contact.Position.X, topLeft.Y), new PointF(topLeft.X + (ReferencePoint.POSITION_OFFSET * 2), Contact.Position.Y) });
        }

        /// <summary>
        /// Draws an upside down ^ at the position using the Top Left for an enemy subsurface contact
        /// </summary>
        /// <param name="topLeft"><see cref="Point"/></param>
        /// <param name="color"><see cref="Pen"/></param>
        public void DrawUpsidedownCarat(PointF topLeft, Pen color)
        {
            GraphicsContext.DrawLines(color, new PointF[] { new PointF(topLeft.X, Contact.Position.Y), new PointF(Contact.Position.X, Contact.Position.Y + ReferencePoint.POSITION_OFFSET), new PointF(topLeft.X + (ReferencePoint.POSITION_OFFSET * 2), Contact.Position.Y) });
        }
    }
}

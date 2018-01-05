using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TacticsLibrary.Adapters;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.DrawObjects
{
    public class RwrPoint : Control, IVisibleObjects, IEquatable<RwrPoint>
    {
        public const int POSITION_OFFSET = 10;
        public const double VELOCITY_VECTOR_TIME = 10.00;
        private const double SECONDS_PER_HOUR = 3600.00;

        public string ReferenceText { get; protected set; }

        public System.Threading.Timer TrackTimer { get; private set; }

        public Rectangle DrawArea { get; protected set; }

        public Guid UniqueId { get; protected set; }
        public DateTime TimeStamp { get; protected set; }
        public ContactTypes ContactType { get; protected set; }
        public DateTime LastUpdate { get; set; }

        public Point Position { get; protected set; }
        public float Speed { get; protected set; }
        public float Course { get; protected set; }
        public double Altitude { get; protected set; }
        public double Heading { get; protected set; }
        public Point BullsEye { get; protected set; }
        public Point HomePlate { get; protected set; }
        public Point OwnShip { get; protected set; }

        public PolarCompassReference PolarPosit { get; protected set; }

        public IGraphics GraphicsContext { get; private set; }

        public event EventHandler UpdatePending;

        protected virtual void OnUpdatePending(EventArgs e)
        {
            UpdatePending?.Invoke(this, e);
        }

        public RwrPoint(Point ownShip, Point bullsEye, Point homePlate, Point position, double altitude, ContactTypes contactType, float course = 0F, int speed = 0, double heading = 0.00)
        {
            OwnShip = ownShip;
            BullsEye = bullsEye;
            HomePlate = homePlate;
            Position = position;
            Altitude = altitude;
            ContactType = contactType;
            Course = course;
            Speed = speed;
            Heading = heading;
            UniqueId = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            LastUpdate = TimeStamp;
            TrackTimer = new System.Threading.Timer(TimerCall, UniqueId, 0, 1000);

            Show();
            this.MouseClick += RwrPoint_MouseClick;
        }

        private void RwrPoint_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show($"{UniqueId} {Course}° at {Speed} knts Angels {Altitude}");
        }

        #region IEquatable<HistoricalPoint> Implementation

        /// <summary>
        /// Compares this contact with another via the Position ONLY
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(RwrPoint other)
        {
            return Position == other.Position;
        }

        #endregion IEquatable<HistoricalPoint> Implementation

        private void TimerCall(object state)
        {
            if (Speed > 0)
            {
                // Plot new position based on course
                // We know that 10 seconds have passed so we have to calculate the new position and plot it
                // Get the duration since the last update and set the LastUpdate
                var timeSinceLastUpdate = DateTime.UtcNow - LastUpdate;
                LastUpdate = DateTime.UtcNow;

                // Now calculate the contacts movement in that timespan
                var distance = timeSinceLastUpdate.TotalSeconds * (Speed/SECONDS_PER_HOUR);

                // From the last Position now move the contact
                var offset = Position;

                // Get the new position based on course and distance traveled
                var newPos = CoordinateConverter.CalculatePointFromDegrees(offset, distance, Course);

                System.Diagnostics.Debug.Print($"Contact {UniqueId}, Type:{ContactType} Speed:{Speed} knts Course:{Course}° from Position:({Position.X}, {Position.Y}) to ({newPos.X}, {newPos.Y})");

                Position = newPos;

                PolarPosit = CoordinateConverter.CalculateDegreesFromPoint(OwnShip, Position);
                RefreshStatus();
                // Notify the change in Position
                OnUpdatePending(new EventArgs());
            }

        }

        public void RefreshStatus()
        {
            //ReferenceText = $"{PolarPosit?.Radius:F0} miles, {PolarPosit?.Degrees:F0}° -> {Speed} knts";
            var offset = OwnShip;
            var offsetPoint = Position;

            if (Position.X > OwnShip.X)
            {
                offsetPoint.X -= OwnShip.X;
            }
            else if (Position.X < OwnShip.X)
            {
                offsetPoint.X = offset.X - Position.X;
            }
            else
            {
                offsetPoint.X = 0;
            }

            ReferenceText = $"{Position} -> {offsetPoint}";
        }

        public void Draw(IGraphics g)
        {
            GraphicsContext = g;
            var topLeft = new Point(Position.X - POSITION_OFFSET, Position.Y - POSITION_OFFSET);
            var contactArea = new Rectangle(topLeft, new Size(POSITION_OFFSET*2, POSITION_OFFSET*2));
            DrawArea = contactArea;
            DrawContact(g);
            DrawText(g);
            DrawVelocityVector(g);
        }

        /// <summary>
        /// Draws a line showing the course of the contact that is the equivalent of how far it will travel in 10 seconds
        /// </summary>
        /// <param name="g"></param>
        private void DrawVelocityVector(IGraphics g)
        {
            // Based on the course and speed calculate the distance in 10 seconds
            // Get the new position based on course and distance traveled
            // Now calculate the contacts movement in that timespan
            var distance = VELOCITY_VECTOR_TIME * (Speed / SECONDS_PER_HOUR);
            var newPos = CoordinateConverter.CalculatePointFromDegrees(Position, distance, Course);
            g.DrawLine(Pens.Green, Position, newPos);
        }

        public void DrawContact(IGraphics g)
        {
            // Draw the point of the contact in green
            g.FillEllipse(Brushes.Green, Position.X, Position.Y, 2, 2);

            switch (ContactType)
            {
                case Enums.ContactTypes.AirUnknown:
                    DrawArc(DrawArea, g, Pens.Yellow);
                    break;
                case Enums.ContactTypes.AirFriendly:
                    DrawArc(DrawArea, g, Pens.Green);
                    break;
                case Enums.ContactTypes.AirEnemy:
                    DrawHostileAir(DrawArea.Location, g);
                    break;
                case Enums.ContactTypes.SurfaceUnknown:
                    DrawCircle(DrawArea, g, Pens.Yellow);
                    break;
                case Enums.ContactTypes.SurfaceFriendly:
                    DrawCircle(DrawArea, g, Pens.Green);
                    break;
                case Enums.ContactTypes.SurfaceEnemy:
                    DrawHostileSurface(DrawArea.Location, g);
                    break;
                case Enums.ContactTypes.SubUnknown:
                    DrawUpsidedownArc(DrawArea, g, Pens.Yellow);
                    break;
                case Enums.ContactTypes.SubFriendly:
                    DrawUpsidedownArc(DrawArea, g, Pens.Green);
                    break;
                case Enums.ContactTypes.SubEnemy:
                    DrawHostileSub(DrawArea.Location, g);
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
        }

        public void DrawText(IGraphics g)
        {
            Point drawText = DrawArea.Location;
            drawText.Offset(new Point(20, 0));
            g.DrawString(ReferenceText, SystemFonts.StatusFont, Brushes.White, drawText);
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

using System;
using System.Drawing;
using System.Windows.Forms;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.Interfaces;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.DrawObjects
{
    /// <summary>
    /// Encapsulates a contact 
    /// </summary>
    public class Contact : IVisibleObjects, IEquatable<Contact>, IContact
    {
        private const double SECONDS_PER_HOUR = 3600.00;
        public System.Threading.Timer TrackTimer { get; protected set; }
        public Guid UniqueId { get; protected set; }
        public DateTime TimeStamp { get; protected set; }
        public ContactTypes ContactType { get; protected set; }
        public Point Position { get; protected set; }
        public IDrawContact ContactDrawer { get; protected set; }
        public IPolarCoordinate PolarPosit { get; protected set; }
        public Rectangle DetectionWindow { get; protected set; }
        public event EventHandler UpdatePending;

        #region Properties that can be externally changed

        /// <summary>
        /// Current velocity expressed as units per hour
        /// </summary>
        public float Speed { get; set; }
        /// <summary>
        /// True compas course of the contact
        /// </summary>
        public float Course { get; set; }
        /// <summary>
        /// Altitude expressed in units 
        /// </summary>
        public double Altitude { get; set; }
        /// <summary>
        /// Current heading of the contact
        /// </summary>
        public double Heading { get; set; }
        /// <summary>
        /// Last DateTime in UTC that the contact was updated
        /// </summary>
        public DateTime LastUpdate { get; set; }

        #endregion Properties that can be externally changed
        
        protected virtual void OnUpdatePending(EventArgs e)
        {
            UpdatePending?.Invoke(this, e);
        }

        /// <summary>
        /// Creates a plotted point
        /// </summary>
        /// <param name="position"></param>
        /// <param name="altitude"></param>
        /// <param name="contactType"></param>
        /// <param name="course"></param>
        /// <param name="speed"></param>
        /// <param name="heading"></param>
        public Contact(Point position, double altitude, ContactTypes contactType, float course = 0F, int speed = 0, double heading = 0.00)
        {
            Position = position;
            Altitude = altitude;
            ContactType = contactType;
            Course = course;
            Speed = speed;
            Heading = heading;
            UniqueId = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            LastUpdate = TimeStamp;
            PolarPosit = CoordinateConverter.GetPolarCoordinateFromPoint(position);

            // Calculate and set this contacts Detection Window
            var detectStartPoint = Position;
            detectStartPoint.Offset(DrawContact.POSITION_OFFSET, DrawContact.POSITION_OFFSET);
            DetectionWindow = new Rectangle(detectStartPoint, new Size(DrawContact.POSITION_OFFSET, DrawContact.POSITION_OFFSET));
            TrackTimer = new System.Threading.Timer(TimerCall, UniqueId, 0, 1000);
        }

        #region IEquatable<HistoricalPoint> Implementation

        /// <summary>
        /// Compares this contact with another via the Position ONLY
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Contact other)
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
                //var newPos = CoordinateConverter.CalculatePointFromDegrees(offset, distance, Course);

               //  System.Diagnostics.Debug.Print($"Contact {UniqueId}, Type:{ContactType} Speed:{Speed} knts Course:{Course}° from Position:({Position.X}, {Position.Y}) to ({newPos.X}, {newPos.Y})");

                //Position = newPos;

                //PolarPosit = CoordinateConverter.CalculateDegreesFromPoint(OwnShip, Position);
                // Notify the change in Position
                OnUpdatePending(new EventArgs());
            }

        }

        public void Draw(IGraphics g)
        {
            ContactDrawer = new DrawContact(ContactType, 10.0)
            {
                CurrentPosition = Position,
                CurrentSpeed = Speed
            };

            ContactDrawer.Draw(g);
        }

        

    }
}

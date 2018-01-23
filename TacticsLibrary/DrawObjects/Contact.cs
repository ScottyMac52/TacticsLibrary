using System;
using System.Drawing;
using System.Windows.Forms;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
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

        /// <summary>
        /// Track Timer
        /// </summary>
        // TODO: Remove timer from contacts and put in IRadar interface
        public System.Threading.Timer TrackTimer { get; protected set; }
        /// <summary>
        /// Unique Id for the contact
        /// </summary>
        public Guid UniqueId { get; protected set; }
        /// <summary>
        /// Timestamp the contact was added
        /// </summary>
        public DateTime TimeStamp { get; protected set; }
        /// <summary>
        /// Contact type
        /// </summary>
        /// <see cref="ContactTypes"/>
        public ContactTypes ContactType { get; set; }
        /// <summary>
        /// The interface that draws the contact
        /// </summary>
        /// <see cref="IDrawContact"/>
        public IDrawContact ContactDrawer { get; protected set; }
        /// <summary>
        /// Which sensor is tracking this target
        /// </summary>
        /// <see cref="IRadar"/>
        public IRadar DetectedBy { get; protected set; }
        /// <summary>
        /// Contact requires an update
        /// </summary>
        public event EventHandler UpdatePending;

        #region Properties that can be externally changed

        /// <summary>
        /// Position relative to (0,0) in coordinate system
        /// </summary>
        public Point Position { get; internal set; }
        /// <summary>
        /// The current polar position of the contact
        /// </summary>
        public PolarCoordinate PolarPosit { get; internal set; }
        /// <summary>
        /// Current velocity expressed as units per hour
        /// </summary>
        public double Speed { get; set; }
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
        public DateTime LastUpdate { get; internal set; }
        /// <summary>
        /// Contacts pre-calculated detection window
        /// </summary>
        /// <see cref="Rectangle"/>
        public Rectangle DetectionWindow { get; internal set; }
        public Point RelativePosition { get; internal set; }

        #endregion Properties that can be externally changed

        /// <summary>
        /// Update pending handler
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUpdatePending(EventArgs e)
        {
            UpdatePending?.Invoke(this, e);
        }

        /// <summary>
        /// Creates a plotted point
        /// </summary>
        public Contact(IRadar detectedBy)
        {
            DetectedBy = detectedBy;
            UniqueId = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            LastUpdate = TimeStamp;

            // Calculate and set this contacts Detection Window
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

                // Get the new position based on course and distance traveled
                var newPos = CoordinateConverter.CalculatePointFromDegrees(Position, distance, Heading);

                Position = newPos;
                PolarPosit = newPos.GetRelativePosition(DetectedBy.ViewPortExtent).GetPolarCoord();

                // Notify the change in Position
                OnUpdatePending(new EventArgs());
            }

        }

        public void Draw(IGraphics g)
        {
            ContactDrawer = new DrawContact(this, 10.0, DetectedBy.ViewPortExtent);
            ContactDrawer.Draw(g);
        }

        public override string ToString()
        {
            return $"{ContactType}: {PolarPosit.Degrees}° {PolarPosit.Radius} miles {Heading}° {Speed} knts {Altitude} ft";
        }

    }
}

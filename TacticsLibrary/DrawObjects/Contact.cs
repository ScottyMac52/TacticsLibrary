using log4net;
using System;
using System.ComponentModel;
using System.Drawing;
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
    public class Contact : IVisibleObjects, IEquatable<Contact>, IContact, INotifyPropertyChanged
    {
        private const double SECONDS_PER_HOUR = 3600.00;
        private double _speed;
        private double _heading;
        private double _altitude;
        
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
        public event PropertyChangedEventHandler PropertyChanged;

        public ILog Logger { get; protected set; }

        /// <summary>
        /// Position absolute to (0,0) in coordinate system
        /// </summary>
        public PointF Position { get; internal set; }
        /// <summary>
        /// Position relative to the center of the coordinate system 
        /// </summary>
        public PointF RelativePosition => Position.GetRelativePosition(DetectedBy?.ViewPortExtent ?? new SizeF(498, 498));
        /// <summary>
        /// The current polar position of the contact
        /// </summary>
        public PolarCoordinate PolarPosit => GetCurrentPolarPosition();
        /// <summary>
        /// Contacts pre-calculated detection window
        /// </summary>
        /// <see cref="Rectangle"/>
        public RectangleF DetectionWindow => GetDetectionWindow();
        /// <summary>
        /// Last DateTime in UTC that the contact was updated
        /// </summary>
        public DateTime LastUpdate { get; internal set; }


        #region Properties that can be externally changed

        /// <summary>
        /// Current velocity expressed as units per hour
        /// </summary>
        public double Speed { get { return _speed; } set { _speed = value; OnPropertyChanged(nameof(Speed)); } }
        /// <summary>
        /// Altitude expressed in units 
        /// </summary>
        public double Altitude { get { return _altitude; } set { _altitude = value; OnPropertyChanged(nameof(Altitude)); } }
        /// <summary>
        /// Current heading of the contact
        /// </summary>
        public double Heading { get { return _heading; } set { _heading = value; OnPropertyChanged(nameof(Heading)); } }

        #endregion Properties that can be externally changed

        private PolarCoordinate GetCurrentPolarPosition()
        {
            var relPos = RelativePosition;
            var polarPosition = relPos.GetPolarCoord();

            return polarPosition;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private RectangleF GetDetectionWindow()
        {
            var detectionStartOffset = Position.Offset(new PointF(-1 * DrawContact.POSITION_OFFSET, -1 * DrawContact.POSITION_OFFSET), 0);
            return new RectangleF(detectionStartOffset, new Size(DrawContact.POSITION_OFFSET, DrawContact.POSITION_OFFSET));
        }

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
            TrackTimer = new System.Threading.Timer(TimerCall, UniqueId, 0, 10000);

            if(Logger == null)
            {
                Logger = LogManager.GetLogger(typeof(Contact));
            }
        }

        public Contact(IRadar detectedBy, ILog logger) 
            : this(detectedBy)
        {
            Logger = logger;
        }

        #region IEquatable<Contact> Implementation

        /// <summary>
        /// Compares this contact with another via the Position ONLY
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Contact other)
        {
            return Position == other.Position && Altitude == other.Altitude;
        }

        #endregion IEquatable<Concat> Implementation

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
                var newPos = CoordinateConverter.CalculatePointFromDegrees(RelativePosition, new PolarCoordinate(Heading, distance), CoordinateConverter.ROUND_DIGITS);
                Logger.Info($"{UniqueId}: {ContactType} {Position} {RelativePosition} {PolarPosit} moved to");
                Position = newPos.GetAbsolutePosition(DetectedBy.ViewPortExtent);
                Logger.Info($"{Position} {RelativePosition} {PolarPosit}");

                // Notify the change in Position
                OnUpdatePending(new EventArgs());
            }

        }

        public void Draw(IGraphics g)
        {
            ContactDrawer = new DrawContact(Logger, this, 10.0, DetectedBy.ViewPortExtent);
            ContactDrawer.Draw(g);
        }

        public override string ToString()
        {
            return $"{ContactType}: {PolarPosit.Degrees}° {PolarPosit.Radius} miles {Heading}° {Speed} knts {Altitude} ft";
        }

    }
}

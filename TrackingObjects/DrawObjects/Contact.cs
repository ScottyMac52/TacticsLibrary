using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using TacticsLibrary.Converters;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Enums;
using TacticsLibrary.EventHandlers;
using TacticsLibrary.Extensions;

namespace TacticsLibrary.Interfaces
{
    /// <summary>
    /// Encapsulates a contact 
    /// </summary>
    public class Contact : ReferencePoint, IVisibleObjects, IEquatable<Contact>, IContact
    {
        private bool _processThreadRunning = false;

        public const double SECONDS_PER_HOUR = 3600.00;
        public const int DEFAULT_UPDATE_MILISECONDS = 200;

        /// <summary>
        /// <see cref="Thread" that is running this contact>
        /// </summary>
        public Thread ProcessThread { get; protected set; }

        public ManualResetEvent StopEvent { get; protected set; }

        /// <summary>
        /// Allows for a custom sleep time in the processing thread, in ms
        /// </summary>
        public int? CustomUpdateDuration { get; set; }

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
        /// Is the ProcessThread still running
        /// </summary>
        public bool Running => _processThreadRunning;

        /// <summary>
        /// State change notification
        /// </summary>
        public override event ReferencePointChangedEventHandler ReferencePointChanged;

        /// <summary>
        /// Creates a plotted point
        /// </summary>
        internal Contact(ISensor detectedBy, PointF position) : base(detectedBy, position)
        {
            DetectedBy = detectedBy;
            LastUpdate = TimeStamp;
            ReferencePointChanged += Contact_ReferencePointChanged;
        }

        public void Stop()
        {
            Logger?.Info($"Stop Requested: {this}");
            StopEvent.Set();
            _processThreadRunning = false;
        }

        public void Start()
        {
            Logger?.Info($"{this} firing ReferencePointChanged for new");
            StopEvent = new ManualResetEvent(false);
            ProcessThread = new Thread(new ThreadStart(DefaultProcessing));
            ProcessThread.Start();
            _processThreadRunning = true;
            StopEvent.Reset();
        }

        private void Contact_ReferencePointChanged(object sender, ReferencePointChangedEventArgs e)
        {
            switch (e.EventType)
            {
                case UpdateEventTypes.New:
                    break;
                case UpdateEventTypes.Remove:
                    break;
            }
        }

        private void DefaultProcessing()
        {
            while (_processThreadRunning)
            {
                if (Speed > 0)
                {
                    // Plot new position based on course
                    // We know that 10 seconds have passed so we have to calculate the new position and plot it
                    // Get the duration since the last update and set the LastUpdate
                    var timeSinceLastUpdate = DateTime.UtcNow - LastUpdate;
                    LastUpdate = DateTime.UtcNow;
                    // Now calculate the contacts movement in that timespan
                    var distance = timeSinceLastUpdate.TotalSeconds * (Speed / SECONDS_PER_HOUR);
                    // Get the new position based on course and distance traveled
                    var newPos = CoordinateConverter.CalculatePointFromDegrees(RelativePosition, new PolarCoordinate(Heading, distance), CoordinateConverter.ROUND_DIGITS);
                    var newAbsPos = newPos.GetAbsolutePosition(DetectedBy.ViewPortExtent);
                    // Create Region and add the previous window to it
                    var oldRectPos = DetectionWindow;
                    // Set the new Position
                    Position = newAbsPos;
                    // Add the new position to the Region
                    var newRectPos = DetectionWindow;
                    var rectangleList = new List<RectangleF>();
                    rectangleList.AddRange(new List<RectangleF>() { oldRectPos, newRectPos });
                    // Notify the ISensor to repaint
                    ReferencePointChanged?.Invoke(this, new ReferencePointChangedEventArgs(rectangleList, UpdateEventTypes.PositionChange, nameof(Position)));
                    Thread.Sleep(CustomUpdateDuration ?? DEFAULT_UPDATE_MILISECONDS);
                }
            }

            Logger.Info($"{this} has stopped processing");
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

        public override void Draw(IGraphics g)
        {
            ContactDrawer = new DrawContact(Logger, this, 10.0, DetectedBy.ViewPortExtent);
            ContactDrawer.Draw(g);
        }

        public override string ToString()
        {
            return $"{ContactType}: {PolarPosit} {Heading}° {Speed} knts {Altitude} ft";
        }

    }
}

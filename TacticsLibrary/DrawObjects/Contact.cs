using log4net;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;
using TacticsLibrary.DrawObjects;

namespace TacticsLibrary.Interfaces
{
    /// <summary>
    /// Encapsulates a contact 
    /// </summary>
    public class Contact : ReferencePoint, IVisibleObjects, IEquatable<Contact>, IContact
    {
        public const double SECONDS_PER_HOUR = 3600.00;
        public const int DEFAULT_UPDATE_MILISECONDS = 200;
        
        public Thread ProcessThread { get; protected set; }
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

        public bool Running { get; set; }

        public Action ProcessLoop { get; set; }

        /// <summary>
        /// Creates a plotted point
        /// </summary>
        internal Contact(ISensor detectedBy, PointF position) : base(detectedBy, position)
        {
            DetectedBy = detectedBy;
            LastUpdate = TimeStamp;
            ProcessThread = new Thread(new ThreadStart(ProcessLoop ?? DefaultProcessing));
            Running = true;
            ProcessThread.Start();

            if(Logger == null)
            {
                Logger = LogManager.GetLogger(typeof(Contact));
            }
        }

        internal Contact(ISensor detectedBy, PointF position) : this(detectedBy, position)
        {
            Logger = logger;
        }
        
        private void DefaultProcessing()
        {
            while (Running)
            {
                bool _lockTaken = false;
                if (Speed > 0)
                {
                    Monitor.Enter(this, ref _lockTaken);
                    try
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
                        var newRegion = new Region(DetectionWindow);
                        // Set the new Position
                        Position = newAbsPos;
                        // Add the new position to the Region
                        newRegion.Union(DetectionWindow);
                        // Invalidate the Region to cause that are to be repainted

                        // TODO: Need to notify the ISensor to repaint

                        if (_lockTaken)
                        {
                            Monitor.Exit(this);
                        }
                    }
                    catch (SynchronizationLockException SyncEx)
                    {
                        Logger.Error($"{this} lock failed!", SyncEx);
                    }

                    Thread.Sleep(CustomUpdateDuration ?? DEFAULT_UPDATE_MILISECONDS);
                }
            }
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

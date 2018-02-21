using DrawingLibrary;
using GraphicsLibrary;
using SimulationLibrary.Interfaces;
using System;
using System.Drawing;
using System.Threading;

namespace SimulationLibrary
{
    public abstract class BaseContact : ReferencePoint, IContact
    {
        public const double SECONDS_PER_HOUR = 3600.00;
        public const int DEFAULT_UPDATE_MILISECONDS = 200;

        /// <summary>
        /// <see cref="Thread" that is running this contact>
        /// </summary>
        public Thread ProcessThread { get; protected set; }

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
        public virtual bool Running => false;

        /// <summary>
        /// Creates a plotted point
        /// </summary>
        internal BaseContact(ISensor detectedBy, PointF position) : base(detectedBy, position)
        {
            DetectedBy = detectedBy;
            LastUpdate = TimeStamp;
        }

        /// <summary>
        /// The thread that processes this contact
        /// </summary>
        protected abstract void DefaultProcessing();
        /// <summary>
        /// Start the processing
        /// </summary>
        public abstract void Start(Action processAction = null);
        /// <summary>
        /// Stop the processing
        /// </summary>
        public abstract void Stop();

        #region IEquatable<Contact> Implementation

        /// <summary>
        /// Compares this contact with another via the Position ONLY
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(BaseContact other)
        {
            return Position == other.Position && Altitude == other.Altitude;
        }

        #endregion IEquatable<Concat> Implementation

        public override string ToString()
        {
            return $"{ContactType}: {PolarPosit} {Heading}° {Speed} knts {Altitude} ft";
        }
    }
}

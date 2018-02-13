using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using TacticsLibrary.Enums;
using TacticsLibrary.EventHandlers;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary.DrawObjects
{
    /// <summary>
    /// Base class for anything displayed in an <see cref="ISensor"/>
    /// </summary>
    public abstract class ReferencePoint : IReferencePoint
    {
        #region Private fields

        private double _speed;
        private double _heading;
        private double _altitude;
        private bool _selected;
        private bool _showText;

        #endregion

        #region Ctor
        internal ReferencePoint(ISensor detectedBy, PointF position, ILog logger)
        {
            DetectedBy = detectedBy;
            UniqueId = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            Position = position;
            Logger = Logger;
        }
        #endregion
        
        #region Public Properties

        public string Name { get; set; }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Unique Id for the reference 
        /// </summary>
        public Guid UniqueId { get; protected set; }

        /// <summary>
        /// Which sensor is tracking this target
        /// </summary>
        /// <see cref="ISensor"/>
        public ISensor DetectedBy { get; protected set; }

        public ILog Logger { get; protected set; }

        #endregion

        #region Properties that can be externally changed and notifcation is provided

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
        /// <summary>
        /// Selection switch
        /// </summary>
        public bool Selected { get { return _selected; } set { _selected = value; OnPropertyChanged(nameof(Selected)); } }
        /// <summary>
        /// If set to true then the text of the contact is displayed
        /// </summary>
        public bool ShowText { get { return _showText; } set { _showText = value; OnPropertyChanged(nameof(ShowText)); } }

        #endregion  Properties that can be externally changed and notifcation is provided

        #region Properties that are calculated 

        /// <summary>
        /// Timestamp the reference was added
        /// </summary>
        public DateTime TimeStamp { get; protected set; }
        /// <summary>
        /// Last DateTime in UTC that the contact was updated
        /// </summary>
        public DateTime LastUpdate { get; protected set; }
        /// <summary>
        /// Position absolute to (0,0) in coordinate system
        /// </summary>
        public PointF Position { get; protected set; }
        /// <summary>
        /// Position relative to the center of the coordinate system 
        /// </summary>
        public PointF RelativePosition => GetCurrentRelativePosition();
        /// <summary>
        /// The current polar position of the contact
        /// </summary>
        public PolarCoordinate PolarPosit => GetCurrentPolarPosition();
        /// <summary>
        /// Contacts pre-calculated detection window
        /// </summary>
        /// <see cref="Rectangle"/>
        public RectangleF DetectionWindow => GetDetectionWindow();


        #endregion Properties that are calculated 

        #region Events and Handlers

        public virtual event ReferencePointChangedEventHandler ReferencePointChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var eventType = UpdateEventTypes.Unknown;

            switch(propertyName)
            {
                case nameof(Speed):
                    eventType = UpdateEventTypes.SpeedChange;
                    break;
                case nameof(Altitude):
                    eventType = UpdateEventTypes.AltitudeChange;
                    break;
                case nameof(Heading):
                    eventType = UpdateEventTypes.HeadingChange;
                    break;
                case nameof(ShowText):
                    eventType = UpdateEventTypes.ShowTextChange;
                    break;
                case nameof(Selected):
                    eventType = UpdateEventTypes.SelectedChange;
                    break;
            }

            ReferencePointChanged?.Invoke(this, new ReferencePointChangedEventArgs(new List<RectangleF>(), eventType, propertyName));
        }

        #endregion Events and Handlers

        #region Public methods and delegates
        
        /// <summary>
        /// <see cref="Action"/> that is used to draw this reference point
        /// </summary>
        public virtual Action<IGraphics, IReferencePoint> PaintMethod { get; set; }

        /// <summary>
        /// Draws the <see cref="ReferencePoint"/>
        /// </summary>
        /// <param name="g"><see cref="IGraphics"/></param>
        public virtual void Draw(IGraphics g)
        {
            if (PaintMethod == null)
            {
                g.DrawString($"{Name}: {Position} {RelativePosition} {PolarPosit}: {Heading} {Speed} {Altitude}", SystemFonts.StatusFont, Brushes.Red, Position);
            }
            else
            {
                PaintMethod.Invoke(g, this);
            }
        }

        /// <summary>
        /// Human readable label for <see cref="ReferencePoint"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name}: {Position} : {PolarPosit}";
        }

        #endregion

        #region Protected methods that provide calculated values

        /// <summary>
        /// Uses the associated <see cref="ISensor"/> to get the relative position of the current absolute coordinates in the specified ViewPort
        /// </summary>
        /// <returns><see cref="PointF"/></returns>
        protected virtual PointF GetCurrentRelativePosition()
        {
            return Position.GetRelativePosition(DetectedBy?.ViewPortExtent ?? new SizeF(498, 498));
        }

        /// <summary>
        /// Uses the current Relative position to calculate the Polar coordinates
        /// </summary>
        /// <returns><see cref="PolarCoordinate"/></returns>
        protected virtual PolarCoordinate GetCurrentPolarPosition()
        {
            return RelativePosition.GetPolarCoord();
        }

        /// <summary>
        /// Calculates the <see cref="RectangleF"/> 
        /// </summary>
        /// <returns></returns>
        protected virtual RectangleF GetDetectionWindow()
        {
            var detectionStartOffset = Position.Offset(new PointF(-1 * DrawContact.POSITION_OFFSET, -1 * DrawContact.POSITION_OFFSET), 0);
            return new RectangleF(detectionStartOffset, new Size(DrawContact.POSITION_OFFSET * 2, DrawContact.POSITION_OFFSET * 2));
        }

        #endregion
    }
}

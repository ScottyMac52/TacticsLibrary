using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary.DrawObjects
{
    /// <summary>
    /// Base class for anything displayed in an <see cref="ISensor"/>
    /// </summary>
    public class ReferencePoint : IReferencePoint, INotifyPropertyChanged
    {
        private PointF _absolutePosition;
        private double _speed;
        private double _heading;
        private double _altitude;
        private bool _selected;
        private bool _showText;

        public ReferencePoint(PointF position, SizeF size)
        {
            UniqueId = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            Position = position;
        }

        public virtual void Draw(IGraphics g)
        {
            g.DrawString("Test", SystemFonts.StatusFont, Brushes.Red, Position);
        }
        
        public string Name { get; set; }
       
        /// <summary>
        /// Unique Id for the reference 
        /// </summary>
        public Guid UniqueId { get; protected set; }

         /// <summary>
        /// Timestamp the reference was added
        /// </summary>
        public DateTime TimeStamp { get; protected set; }
        /// <summary>
        /// Last DateTime in UTC that the contact was updated
        /// </summary>
        public DateTime LastUpdate { get; internal set; }

        /// <summary>
        /// Which sensor is tracking this target
        /// </summary>
        /// <see cref="ISensor"/>
        public ISensor DetectedBy { get; protected set; }

        #region Properties that can be externally changed

        public PointF Position { get { return _absolutePosition; } protected set { _absolutePosition = value; OnPropertyChanged(nameof(Position)); } }

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

        #endregion Properties that can be externally changed

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// ReferencePoint <see cref="IReferencePoint"/> an update
        /// </summary>
        public delegate void ReferencePointEventHandler(IReferencePoint sender);
        public event ReferencePointEventHandler UpdatePending;
        
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Update pending handler
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUpdatePending(IReferencePoint referencePoint)
        {
            UpdatePending?.Invoke(referencePoint);
        }

        protected virtual PolarCoordinate GetCurrentPolarPosition()
        {
            return RelativePosition.GetPolarCoord();
        }

        protected virtual RectangleF GetDetectionWindow()
        {
            var detectionStartOffset = Position.Offset(new PointF(-1 * DrawContact.POSITION_OFFSET, -1 * DrawContact.POSITION_OFFSET), 0);
            return new RectangleF(detectionStartOffset, new Size(DrawContact.POSITION_OFFSET * 2, DrawContact.POSITION_OFFSET * 2));
        }
    }
}

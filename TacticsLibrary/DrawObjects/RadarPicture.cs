using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.DrawObjects
{
    public class RadarPicture : IRadar, IViewPorts
    {
         /// <summary>
        /// The Radius of the Radar
        /// </summary>
        public float Radius { get; set; }
        /// <summary>
        /// The seperation of the range rings in pixels
        /// </summary>
        public float RingSep { get; set; }
        /// <summary>
        /// HThe number of range rings to display
        /// </summary>
        public int RangeRings { get; set; }
        public SortedList<Guid, IContact> CurrentContacts { get; protected set; }
        public Size ViewPortExtent { get; protected set; }
        public Point BullsEye { get; private set; }
        public Point HomePlate { get; private set; }
        public Point OwnShip => new Point(ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());
        public event EventHandler UpdatePending;

        protected virtual void OnUpdatePending(EventArgs e)
        {
            UpdatePending?.Invoke(this, e);
        }

        public RadarPicture(Point bullsEye, Point homePlate, Size viewPortExtent)
        {
            BullsEye = bullsEye;
            HomePlate = homePlate;
            ViewPortExtent = viewPortExtent;
        }

        public void Draw(IGraphics g)
        {
            g.DrawCircle(Pens.Green, ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight(), Radius);

            var dashedPen = new Pen(new SolidBrush(Color.FromArgb(0, 128, 0)))
            {
                DashStyle = DashStyle.Dot
            };

            for (int ringCounter=0; ringCounter <= RangeRings; ringCounter++)
            {
                var newRadius = Radius - ((ringCounter + 1) * RingSep);
                if(newRadius > 0)
                {
                    g.DrawCircle(dashedPen, ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight(), newRadius);
                }
            }

            // Paint the BullsEye
            g.FillCircle(Brushes.Blue, BullsEye.X, BullsEye.Y, 10);
            g.FillCircle(Brushes.Red, BullsEye.X, BullsEye.Y, 5);

            // Paint Homeplate
            g.FillCircle(Brushes.White, HomePlate.X, HomePlate.Y, 10);
            g.FillCircle(Brushes.Blue, HomePlate.X, HomePlate.Y, 5);

            // Plot all points
            foreach (var item in CurrentContacts)
            {
                item.Value.Draw(g);
            }
        }

        /// <summary>
        /// Plots a contact using the specified Offset the offset can be any point 
        /// </summary>
        /// <param name="offset">Point that represents the startiong point for the plot</param>
        /// <param name="degrees">Bearing from offset in degrees</param>
        /// <param name="radius">Range in nautical miles</param>
        /// <param name="altitude">Altitude of the contact</param>
        /// <param name="speed">Speed of the contact in knts</param>
        /// <param name="course">Course of the contact in degrees</param>
        /// <param name="contactType">Type of contact</param>
        /// <returns></returns>
        public Contact PlotContact(Point offset, double degrees, double radius, double altitude, int speed, int course, ContactTypes contactType = ContactTypes.AirUnknown)
        {
            var polarCoord = new PolarCoordinate(degrees, radius);
            var plotPoint = polarCoord.GetPoint().GetRelativePosition(ViewPortExtent);
            var newPoint = new Contact(plotPoint, altitude, contactType, course, speed);
            AddContact(newPoint, contactType);
            return newPoint;
        }


        /// <summary>
        /// Plots a contact using OwnShip as a default reference but supports BullsEye & HomePlate callouts too
        /// </summary>
        /// <param name="refPos">Where the coordinate (0,0) origin is</param>
        /// <param name="degrees">The angle of the contact in compass degrees</param>
        /// <param name="radius">The number of units</param>
        /// <param name="contactType">The type of contact, defaults to AirUnknown</param>
        /// <param name="altitude"></param>
        /// <param name="speed"></param>
        /// <param name="course"></param>
        public Contact PlotContact(ReferencePositions refPos, double degrees, double radius, double altitude, int speed, int course, ContactTypes contactType = ContactTypes.AirUnknown)
        {
            Point offset = new Point(ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());
            switch (refPos)
            {
                case ReferencePositions.BullsEye:
                    offset = BullsEye;
                    break;
                case ReferencePositions.HomePlate:
                    offset = HomePlate;
                    break;
                default:
                    break;
            }

            return PlotContact(offset, radius, degrees, altitude, speed, course, contactType);
        }

        /// <summary>
        /// Find all contacts on the radar using a point and detection window
        /// </summary>
        /// <param name="checkPoint"><see cref="Point"/>The center point of the search</param>
        /// <param name="detectionWindow"><see cref="Size"/>The size of the detction window</param>
        /// <returns><see cref="List{Contact}"/></returns>
        public List<IContact> FindContact(Point checkPoint, Size detectionWindow)
        {
            var contactList = new List<IContact>();
            checkPoint.Offset(new Point(detectionWindow.GetCenterWidth(), detectionWindow.GetCenterHeight()));
            var detectionArea = new Rectangle(checkPoint, detectionWindow);

            if(CurrentContacts.Values.Any(contact =>
            {
                return detectionArea.IntersectsWith(contact.DetectionWindow);
            }) == true)
            {
                contactList.AddRange(CurrentContacts.Values.ToList().FindAll(match =>
                {
                    return detectionArea.IntersectsWith(match.DetectionWindow);
                }));
            }

            return contactList;
        }
         
        /// <summary>
        /// Adds a point as a type and class of contact 
        /// </summary>
        /// <param name="newPoint">Relative position from (0,0) <see cref="Point"/></param>
        /// <param name="contactType">Contact type <see cref="ContactTypes"/></param>
        public void AddContact(Contact newPoint, ContactTypes contactType)
        {
            if(CurrentContacts == null)
            {
                CurrentContacts = new SortedList<Guid, IContact>();
            }
            if (!CurrentContacts.ContainsKey(newPoint.UniqueId))
            {
                CurrentContacts.Add(newPoint.UniqueId, newPoint);
            }

            newPoint.UpdatePending += NewPoint_UpdatePending;
        }

        public void AddReference(Point refLocation, string refName, Image refImage)
        {
        }

        private void NewPoint_UpdatePending(object sender, EventArgs e)
        {
            OnUpdatePending(new EventArgs());
        }
    }
}

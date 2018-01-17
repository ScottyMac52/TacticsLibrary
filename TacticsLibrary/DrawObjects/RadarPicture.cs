using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using TacticsLibrary.Adapters;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.DrawObjects
{
    public class RadarPicture : IVisibleObjects, IRadar
    {
        /// <summary>
        /// The location of the center of the Radar on the X Axis
        /// </summary>
        public float CenterPositionX { get; set; }
        /// <summary>
        /// The location of the center of the Radar on the Y Axis
        /// </summary>
        public float CenterPositionY { get; set; }
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
        public SortedList<Guid, PlottedPoint> PlottedPoints { get; protected set; }

        public Point BullsEye { get; private set; }
        public Point HomePlate { get; private set; }

        public Point OwnShip => new Point((int) CenterPositionX, (int) CenterPositionY);

        public event EventHandler UpdatePending;

        protected virtual void OnUpdatePending(EventArgs e)
        {
            UpdatePending?.Invoke(this, e);
        }

        public RadarPicture(Point bullsEye, Point homePlate)
        {
            BullsEye = bullsEye;
            HomePlate = homePlate;
        }

        public void Draw(IGraphics g)
        {
            g.DrawCircle(Pens.Green, CenterPositionX, CenterPositionY, Radius);

            var dashedPen = new Pen(new SolidBrush(Color.FromArgb(0, 128, 0)))
            {
                DashStyle = DashStyle.Dot
            };
            for (int ringCounter=0; ringCounter <= RangeRings; ringCounter++)
            {
                var newRadius = Radius - ((ringCounter + 1) * RingSep);
                if(newRadius > 0)
                {
                    g.DrawCircle(dashedPen, CenterPositionX, CenterPositionY, newRadius);
                }
            }

            // Paint the BullsEye
            g.FillCircle(Brushes.Blue, BullsEye.X, BullsEye.Y, 10);
            g.FillCircle(Brushes.Red, BullsEye.X, BullsEye.Y, 5);

            // Paint Homeplate
            g.FillCircle(Brushes.White, HomePlate.X, HomePlate.Y, 10);
            g.FillCircle(Brushes.Blue, HomePlate.X, HomePlate.Y, 5);

            // Plot all points
            foreach (var item in PlottedPoints)
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
        public PlottedPoint PlotContact(Point offset, double degrees, double radius, double altitude, int speed, int course, ContactTypes contactType = ContactTypes.AirUnknown)
        {
            var polarCoord = new PolarCoordinate() { Degrees = degrees, Radius = radius };
            var plotPoint = polarCoord.GetPoint();
            var newPoint = new PlottedPoint(OwnShip, BullsEye, HomePlate, plotPoint, altitude, contactType, course, speed);
            AddPoint(newPoint, contactType);
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
        public PlottedPoint PlotContact(ReferencePositions refPos, double degrees, double radius, double altitude, int speed, int course, ContactTypes contactType = ContactTypes.AirUnknown)
        {
            Point offset = new Point((int)Math.Round(CenterPositionX, 0), (int)Math.Round(CenterPositionY, 0));
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
        /// Adds a point as a type and class of contact 
        /// </summary>
        public void AddPoint(PlottedPoint newPoint, ContactTypes contactType)
        {
            if(PlottedPoints == null)
            {
                PlottedPoints = new SortedList<Guid, PlottedPoint>();
            }
            if (!PlottedPoints.ContainsKey(newPoint.UniqueId))
            {
                PlottedPoints.Add(newPoint.UniqueId, newPoint);
            }

            newPoint.UpdatePending += NewPoint_UpdatePending;
        }

        private void NewPoint_UpdatePending(object sender, EventArgs e)
        {
            OnUpdatePending(new EventArgs());
        }
    }
}

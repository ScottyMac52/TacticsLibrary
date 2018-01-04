using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;
using TacticsLibrary.Adapters;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;

namespace TacticsLibrary.DrawObjects
{
    public class RwrReceiver : IVisibleObjects, IRwrReceiver
    {
        public const double OFFSET_THETA = -90.00;

        public bool Ready { get; private set; }
        public float CenterPositionX { get; set; }
        public float CenterPositionY { get; set; }
        public float Radius { get; set; }
        public float RingSep { get; set; }
        public int RangeRings { get; set; }
        public List<RwrPoint> PlottedPoints { get; protected set; }

        public Point BullsEye { get; private set; }
        public Point HomePlate { get; private set; }

        public RwrReceiver(Point bullsEye, Point homePlate)
        {
            Ready = false;
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
            PlottedPoints?.ForEach(pp =>
            {
                pp.Draw(g);
            });
        }

        /// <summary>
        /// Plots a contact using OwnShip as a default reference but supports BullsEye & HomePlate callouts too
        /// </summary>
        /// <param name="refPos">Where the coordinate (0,0) origin is</param>
        /// <param name="radius">The number of units</param>
        /// <param name="degrees">The angle of the contact in compass degrees</param>
        /// <param name="contactType">The type of contact, defaults to AirUnknown</param>
        public void PlotContact(ReferencePositions refPos, double radius, double degrees, ContactTypes contactType = ContactTypes.AirUnknown)
        {
            Point offset = new Point((int)Math.Round(CenterPositionX, 0), (int)Math.Round(CenterPositionY, 0));
            var originMarker = "O";
            switch (refPos)
            {
                case ReferencePositions.BullsEye:
                    offset = BullsEye;
                    originMarker = "B";
                    break;
                case ReferencePositions.HomePlate:
                    offset = HomePlate;
                    originMarker = "H";
                    break;
                default:
                    break;
            }

            var plotPoint = CalculatePointFromDegrees(offset, radius, degrees);
   
            // Add the point 
            AddPoint(plotPoint, $"{originMarker}:({radius}, {degrees}°)", contactType);
        }

        /// <summary>
        /// Converts the specified polar coordinate into compass based cartesian coordinates
        /// </summary>
        /// <param name="offset">The origin for the to get the distance and degrees from</param>
        /// <param name="radius">Distance in units</param>
        /// <param name="degrees">Compass degrees in degrees</param>
        /// <returns><see cref="Point"/> that references the location</returns>
        public Point CalculatePointFromDegrees(Point offset, double radius, double degrees)
        {
            // Subtract the 90 degree offset to account for conversion from polar to compass coordinates
            var theta = degrees + OFFSET_THETA;
            // Calculate the degrees into Radians 
            var radians = theta * (Math.PI / 180);

            // X is radius * Cos(theta in radians)
            var xValue = radius * Math.Cos(radians);
            // Y is radius * Sin(theta in radians)
            var yValue = radius * Math.Sin(radians);

            // Account for specified offset
            Int32 x = offset.X + (int)Math.Round(xValue, 0);
            Int32 y = offset.Y + (int)Math.Round(yValue, 0);

            return new Point(x, y);
        }



        /// <summary>
        /// Adds a point as a type and class of contact 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="referenceText"></param>
        /// <param name="contactType"></param>
        public void AddPoint(Point pt, string referenceText, ContactTypes contactType)
        {
            if(PlottedPoints == null)
            {
                PlottedPoints = new List<RwrPoint>();
            }

            PlottedPoints.Add(new RwrPoint()
            {
                Position = pt,
                TimeStamp = DateTime.UtcNow,
                ReferenceText = referenceText,
                ContactType = contactType
            });
        }

        public void Invalidate(Rectangle invalidRect)
        {
            throw new NotImplementedException();
        }
    }
}

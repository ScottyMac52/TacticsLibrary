using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Converters
{
    public class CoordinateConverter
    {
        public const double OFFSET_THETA = -90.00;

        public static PolarCompassReference CalculateDegreesFromPoint(Point offset, Point refPoint)
        {
            // Add the offset
            refPoint.Offset(offset);

            // First we need to calulate what R is
            var radius = Math.Sqrt((refPoint.X * refPoint.X) + (refPoint.Y * refPoint.Y));

            // Calculate theta in degrees
            // Guard against division by zero
            var xPoint = refPoint.X == 0 ? int.MinValue : refPoint.X;
            var theta = (180.00 * (Math.Tanh(refPoint.Y / xPoint)) / Math.PI) + (-1*OFFSET_THETA);

            return new PolarCompassReference() { Radius = radius, Degrees = theta };
        }

        public static Point CalculatePointFromDegrees(Point offset, double radius, double degrees)
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

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

    }
}

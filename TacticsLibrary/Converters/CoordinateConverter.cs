using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Extensions;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Converters
{
    public class CoordinateConverter
    {
        public const double OFFSET_THETA = -90.00;
        public const int ROUND_DIGITS = 4;

        public static PolarCoordinate CalculateDegreesFromPoint(Point offset, Point refPoint)
        {
            // Add the offset
            // refPoint.Offset(offset);

            // First we need to calulate what R is
            var radius = Math.Round(Math.Sqrt((refPoint.X * refPoint.X) + (refPoint.Y * refPoint.Y)), ROUND_DIGITS);

            // Calculate theta in degrees
            // Guard against division by zero
            var theta = GetAngleFromSides(radius, refPoint.X, refPoint.Y, ROUND_DIGITS);

            return new PolarCoordinate() { Radius = radius, Degrees = theta };
        }

        /// <summary>
        /// Gets the relative point inside of the viewport represented by the plotted Point
        /// </summary>
        /// <param name="plottedPoint">Coord inate absolute position</param>
        /// <param name="viewPortExtent">Viewport</param>
        /// <returns>Relative position</returns>
        public static Point GetRelativePosition(Point plottedPoint, Rectangle viewPortExtent)
        {
            var actualResult = plottedPoint;

            if(plottedPoint.X >= viewPortExtent.GetCenterWidth())
            {
                actualResult.X = plottedPoint.X - viewPortExtent.GetCenterWidth(); 
            }
            else
            {
                actualResult.X = ((-1) * (viewPortExtent.GetCenterWidth())) + plottedPoint.X;
            }

            if (actualResult.Y >= viewPortExtent.GetCenterHeight())
            {
                actualResult.Y = -1 * (plottedPoint.Y - viewPortExtent.GetCenterHeight());
            }
            else
            {
                var yOffset = actualResult.Y + viewPortExtent.GetCenterHeight();
            }

            return actualResult;
        }

        public static Point CalculatePointFromDegrees(Point offSet, PolarCoordinate polarRef)
        {
            return CalculatePointFromDegrees(offSet, polarRef.Radius, polarRef.Degrees);
        }

        /// <summary>
        /// Gets the adjacent angle theta from the 3 sides
        /// </summary>
        /// <param name="side1"></param>
        /// <param name="side2"></param>
        /// <param name="side3"></param>
        /// <param name="roundingDigits"></param>
        /// <returns></returns>
        public static double GetAngleFromSides(double side1, double side2, double side3, int roundingDigits = 2)
        {
            // this defines the number two
            const int TWO = 2;
            const int TO_DEGREES = 180;
            // this squares the first side
            double side1Squared = side1 * side1;
            // this squares the second side
            double side2Squared = side2 * side2;
            // this squares the third side
            double side3Squared = side3 * side3;
            // this uses formula cos C = (a2 + b2 − c2)/ 2ab
            double toGetCosOfangle = (((side1Squared + side2Squared) - side3Squared)) / (TWO * side1 * side2);

            // this takes the inverse cosine to get angle in radians
            double angle1 = Math.Acos(toGetCosOfangle);
            // this converts the angle to degrees
            double angle1Degrees = angle1 * (TO_DEGREES / Math.PI);
            // this rounds the angle
            double angle1Rounded = Math.Round(angle1Degrees, roundingDigits);
            // this returns the angle from the method
            return angle1Rounded;
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

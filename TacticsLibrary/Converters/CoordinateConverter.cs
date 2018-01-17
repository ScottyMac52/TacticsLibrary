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
        public const int ROUND_DIGITS = 4;
        public const double NORTH = 360.00;
        public const double NORTH_PRIME = 0.00;
        public const double SOUTH = 180.00;
        public const double EAST = 90.00;
        public const double WEST = 270.00;
        public const double NEGATIVE = -1.00;

        /// <summary>
        /// Converts a Polar Coordinate into a Point
        /// </summary>
        /// <param name="polarCoord"></param>
        /// <returns><see cref="Point"/></returns>
        public static Point GetPointFromPolarCoordinate(PolarCoordinate polarCoord)
        {
            var sinFactor = 1.00;
            var cosFactor = 1.00;
            var measuredAngle = 0.00;

            // Have to offset Degrees back to normal based on Quadrant
            // Quadrant 1
            if (polarCoord.Degrees >= NORTH_PRIME && polarCoord.Degrees <= EAST)
            {
                measuredAngle = EAST - polarCoord.Degrees;
            }
            // Quandrant 4, y is negative so sin is negative
            else if(polarCoord.Degrees > EAST && polarCoord.Degrees <= SOUTH)
            {
                measuredAngle = SOUTH - polarCoord.Degrees;
                sinFactor = NEGATIVE;
            }
            // Quadrant 3
            else if(polarCoord.Degrees > SOUTH && polarCoord.Degrees <= WEST)
            {
                measuredAngle = WEST - polarCoord.Degrees;
                sinFactor = NEGATIVE;
                cosFactor = NEGATIVE;
            }
            // Quadrant 2
            else if(polarCoord.Degrees > WEST && polarCoord.Degrees <= NORTH)
            {
                measuredAngle = NORTH - polarCoord.Degrees;
                cosFactor = NEGATIVE;
            }
            else
            {
                throw new ArgumentException($"{polarCoord} is not an acceptable polar coordinate and cannot be converted.");
            }

            var sin = Math.Abs(Math.Sin(DegreesToRadians(measuredAngle))) * sinFactor;
            var cos = Math.Abs(Math.Cos(DegreesToRadians(measuredAngle))) * cosFactor;
            var x = Math.Round(polarCoord.Radius * cos, ROUND_DIGITS);
            var y = Math.Round(polarCoord.Radius * sin, ROUND_DIGITS);
            return new Point((int) Math.Round(x, ROUND_DIGITS), (int) Math.Round(y, ROUND_DIGITS));
        }

        /// <summary>
        /// Converts a plotted point relative to (0,0) on an X/Y axis
        /// </summary>
        /// <param name="plottedPoint">The point to plot</param>
        /// <returns>The <see cref="PolarCoordinate"/> that matches the position</returns>
        public static PolarCoordinate GetPolarCoordinateFromPoint(Point plottedPoint)
        {
            var distance = 0.00;
            var angleC = 0.00;
            var offSetAngle = 0.00;

            if (plottedPoint.Y == 0 || plottedPoint.X == 0)
            {
                if (plottedPoint.X < 0)
                {
                    offSetAngle = WEST;
                }
                else if (plottedPoint.X > 0)
                {
                    offSetAngle = EAST;
                }
                else
                {
                    if (plottedPoint.Y < 0)
                    {
                        offSetAngle = SOUTH;
                    }
                    else if (plottedPoint.Y >= 0)
                    {
                        offSetAngle = NORTH;
                    }
                }
            }
            else
            {
                var adjSqr = (plottedPoint.X * plottedPoint.X);
                var oppSqr = (plottedPoint.Y * plottedPoint.Y);
                var hypSqr = adjSqr + oppSqr;

                // First calculate the hyp, which is the range or distance
                distance = Math.Sqrt(hypSqr);

                var sine = plottedPoint.Y / distance;
                var cos = plottedPoint.X / distance;
                var tan = plottedPoint.X / plottedPoint.Y;

                // Test for Quadrant 1 all pos
                if (!IsNegative(sine) && !IsNegative(cos) && !IsNegative(tan))
                {
                    // Angle is true angle from coordinate (0,0)
                    // Final result is 90 - true angle
                    angleC = Math.Atan(plottedPoint.Y / plottedPoint.X);
                    offSetAngle = EAST;
                }
                // Test for Quandrant 2 sine pos
                else if (!IsNegative(sine) && IsNegative(cos) && IsNegative(tan))
                {
                    angleC = Math.Asin(plottedPoint.Y / distance);
                    offSetAngle = NORTH;
                }
                // Test for Quadrant 3 tan pos
                else if (IsNegative(sine) && IsNegative(cos) && !IsNegative(tan))
                {
                    angleC = Math.Atan(plottedPoint.Y / plottedPoint.X);
                    offSetAngle = WEST;
                }
                // Test for Quandrant 4 cos pos
                else if (IsNegative(sine) && !IsNegative(cos) && IsNegative(tan))
                {
                    angleC = Math.Acos(plottedPoint.X / distance);
                    offSetAngle = SOUTH;
                }
            }

            var angleTheta = offSetAngle - RadianToDegree(angleC);
            return new PolarCoordinate() { Degrees = angleTheta, Radius = distance };
        }

        /// <summary>
        /// Negative test for a double
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static bool IsNegative(double val)
        {
            return val < 0;
        }
             
        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns>degrees</returns>
        private static double RadianToDegree(double radians)
        {
            return radians * (180 / Math.PI);
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.00);
        }

    }
}

using System;
using System.Drawing;
using TacticsLibrary.Models;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Converters
{
    public class CoordinateConverter
    {
        public const int ROUND_DIGITS = 3;
        public const double NORTH_PRIME = 0.00;
        public const double NORTH = 360.00;
        public const double SOUTH = 180.00;
        public const double EAST = 90.00;

        public const double WEST = 270.00;

        //public static PolarCoordinate GetPolarCoordinateFromPoint(PointF plottedPoint, PointF offsetFrom)
        //{

        //}


        /// <summary>
        /// Calculates the polar coordinate in Compass degrees of a floating point Point
        /// </summary>
        /// <param name="plottedPoint"><see cref="PointF"/></param>
        /// <returns><see cref="PolarCoordinate"/></returns>
        public static PolarCoordinate GetPolarCoordinateFromPoint(PointF plottedPoint)
        {
            double sine = 0.00, cos = 0.00, tan = 0.00;
            // Calculate the z
            var z = Math.Sqrt(Square(plottedPoint.X) + Square(plottedPoint.Y));
            var angleTheta = NORTH;
            // Get the sine, cos and tan
            try
            {
                sine = plottedPoint.Y / z;
                cos = plottedPoint.X / z;
                tan = plottedPoint.Y / plottedPoint.X;

                if(double.IsNaN(sine) && double.IsNaN(cos) && double.IsNaN(tan))
                {
                    angleTheta = NORTH_PRIME;
                }
                else if (IsZero(sine) && cos == -1.00 && IsZero(tan))
                {
                    angleTheta = WEST;
                }
                else if (IsZero(sine) && cos == 1.00 && IsZero(tan))
                {
                    angleTheta = EAST;
                }
                else if (!IsNegative(sine) && !IsNegative(cos) && !IsNegative(tan))
                {
                    // Quadrant One 
                    angleTheta = EAST - RadiansToDegrees(Math.Atan(plottedPoint.Y / plottedPoint.X));
                }
                else if (!IsNegative(sine) && IsNegative(cos) && IsNegative(tan))
                {
                    // Quadrant Two 
                    angleTheta = WEST + RadiansToDegrees(Math.Asin(plottedPoint.Y / z));
                }
                else if (IsNegative(sine) && IsNegative(cos) && !IsNegative(tan))
                {
                    // Quadrant Three
                    angleTheta = WEST - RadiansToDegrees(Math.Atan(plottedPoint.Y / plottedPoint.X));
                }
                else if (IsNegative(sine) && !IsNegative(cos) && IsNegative(tan))
                {
                    // Quadrant Four
                    angleTheta = EAST + Math.Abs(RadiansToDegrees(Math.Atan(plottedPoint.Y / plottedPoint.X)));
                }
            }
            catch (DivideByZeroException)
            {
                if (sine == 1.00 && IsZero(cos))
                {
                    angleTheta = NORTH;
                }
                else if (sine == -1 && IsZero(cos))
                {
                    angleTheta = SOUTH;
                }
            }

            return new PolarCoordinate(Math.Round(angleTheta, ROUND_DIGITS), Math.Round(z, ROUND_DIGITS));
        }

        /// <summary>
        /// Calculates the distance between any two points
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static double GetDistance(Point point1, Point point2)
        {
            var a = (double)(point2.X - point1.X);
            var b = (double)(point2.Y - point1.Y);

            return Math.Round(Math.Sqrt(a * a + b * b), ROUND_DIGITS);
        }

        /// <summary>
        /// Wrapper to provide for PolarCoordinate support
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="polarCoordinate"></param>
        /// <param name="roundingDigits"></param>
        /// <returns></returns>
        public static PointF CalculatePointFromDegrees(PointF offset, PolarCoordinate polarCoordinate, int roundingDigits)
        {
            return CalculatePointFromDegrees(offset, polarCoordinate.Degrees, polarCoordinate.Radius, roundingDigits);
        }

        /// <summary>
        /// Using an offset calculates a new position using polar coordinates
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="degrees"></param>
        /// <param name="radius"></param>/*
        /// <param name="roundingDigits"></param>
        /// <returns></returns>
        private static PointF CalculatePointFromDegrees(PointF offset, double degrees, double radius, int roundingDigits)
        {
            // Subtract the 90 degree offset to account for conversion from polar to compass coordinates
            // when the angle is >= 90
            var theta = NORTH_PRIME;

            // Have to offset Degrees back to normal based on Quadrant
            // Quadrant 1
            if (degrees >= NORTH_PRIME && degrees <= EAST)
            {
                theta = EAST - degrees;
            }
            // Quadrant 2
            else if (degrees > WEST && degrees <= NORTH)
            {
                theta = EAST + (NORTH - degrees);
            }
            // Quadrant 3
            else if (degrees > SOUTH && degrees <= WEST)
            {
                theta = WEST - (degrees - SOUTH);
            }
            // Quandrant 4, y is negative so sin is negative
            else if (degrees > EAST && degrees <= SOUTH)
            {
                theta = WEST + (SOUTH - degrees);
            }
            
            // Calculate the degrees into Radians 
            var radians = DegreesToRadians(theta);
             // X is radius * Cos(theta in radians)
            var xValue = radius * Math.Cos(radians);
            // Y is radius * Sin(theta in radians)
            var yValue = radius * Math.Sin(radians);
 
            // Account for specified offset
            var x = offset.X + (float) Math.Round(xValue, roundingDigits);
            var y = offset.Y + (float) Math.Round(yValue, roundingDigits);
 
            return new PointF(x, y);
        }

        private static bool IsNegative(double val)
        {
            return val < 0.00;
        }

        private static bool IsZero(double val)
        {
            return val == 0.00;
        }

        private static double Square(double val)
        {
            return val * val;
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="radians">Radians</param>
        /// <returns>Degrees <seealso cref="double"/></returns>
        private static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns>Radians <seealso cref="double"/></returns>
        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.00);
        }

    }
}

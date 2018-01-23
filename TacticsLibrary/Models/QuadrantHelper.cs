using System;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.Interfaces;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Models
{
    public class QuadrantHelper
    {
        public const double NORTH = 360.00;
        public const double NORTH_PRIME = 0.00;
        public const double SOUTH = 180.00;
        public const double EAST = 90.00;
        public const double WEST = 270.00;
        public const double NEGATIVE = -1.00;

        public double X { get; protected set; }
        public double Y { get; protected set; }
        public double Z { get; protected set; }

        public TrigQuadrant Quadrant => FindQuadrant();
        public PolarCoordinate PolarCoord => CalculatePolarCoordinates();

        protected double Angle => CalculateBaseAngle();
        protected double AngleTheta => GetThetaAngle();

        public double GetThetaAngle()
        {
            var thetaAngle = NORTH_PRIME;
            switch (Quadrant)
            {
                case TrigQuadrant.Unknown:
                    if (Z == 0 || X == 0)
                    {
                        if (IsNegative(Y))
                        {
                            thetaAngle = SOUTH;
                        }
                        else
                        {
                            thetaAngle = NORTH_PRIME;
                        }
                    }
                    break;
                case TrigQuadrant.One:
                    thetaAngle = EAST - Angle;
                    break;
                case TrigQuadrant.Two:
                    thetaAngle = WEST + Angle;
                    break;
                case TrigQuadrant.Three:
                    thetaAngle = WEST - Angle;
                    break;
                case TrigQuadrant.Four:
                    thetaAngle = EAST + Angle;
                    break;
            }
            return thetaAngle;
        }

        /// <summary>
        /// Provate constructor 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private QuadrantHelper(double x, double y)
        {
            X = x;
            Y = y;

            // Calculate z 
            var adjSqr = Square(X);
            var oppSqr = Square(Y);
            var hypSqr = adjSqr + oppSqr;

            // First calculate the hyp, which is the range or distance
            Z = Math.Sqrt(hypSqr);
        }

        private QuadrantHelper(IPolarCoordinate polarCoord)
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
            // Quadrant 2
            else if (polarCoord.Degrees > WEST && polarCoord.Degrees <= NORTH)
            {
                measuredAngle = NORTH - polarCoord.Degrees;
                cosFactor = NEGATIVE;
            }
            // Quadrant 3
            else if (polarCoord.Degrees > SOUTH && polarCoord.Degrees <= WEST)
            {
                measuredAngle = WEST - polarCoord.Degrees;
                sinFactor = NEGATIVE;
                cosFactor = NEGATIVE;
            }
            // Quandrant 4, y is negative so sin is negative
            else if (polarCoord.Degrees > EAST && polarCoord.Degrees <= SOUTH)
            {
                measuredAngle = SOUTH - polarCoord.Degrees;
                sinFactor = NEGATIVE;
            }
            else
            {
                throw new ArgumentException($"{polarCoord} is not an acceptable polar coordinate and cannot be converted.");
            }

            var sin = Math.Abs(Math.Sin(DegreesToRadians(measuredAngle))) * sinFactor;
            var cos = Math.Abs(Math.Cos(DegreesToRadians(measuredAngle))) * cosFactor;
            X = polarCoord.Radius * cos;
            Y = polarCoord.Radius * sin;
            Z = polarCoord.Radius;
        }

        /// <summary>
        /// Factory that creates the Quadrant from two points
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static QuadrantHelper CreateQuadrant(double x, double y)
        {
            return new QuadrantHelper(x, y);
        }

        /// <summary>
        /// Factory that creates the Quadrant from a PolarCoordinate
        /// </summary>
        /// <param name="polarCoord"><see cref="IPolarCoordinate"/></param>
        /// <returns></returns>
        public static QuadrantHelper CreateQuadrant(IPolarCoordinate polarCoord)
        {
            return new QuadrantHelper(polarCoord);
        }

        protected PolarCoordinate CalculatePolarCoordinates()
        {
            return new PolarCoordinate(AngleTheta, Z);
        }

        /// <summary>
        /// Using the position data finds the Quadrant for the angle 
        /// </summary>
        /// <returns>Quadrant for the angle <see cref="TrigQuadrant"/></returns>
        protected TrigQuadrant FindQuadrant()
        {
            var currentQuadrant = TrigQuadrant.Unknown;

            // Division by zero guard
            if (Z == 0 || Y == 0)
            {
                currentQuadrant = TrigQuadrant.One;
            }

            var sine = Y / Z;
            var cos = X / Z;
            var tan = X / Y;

            if (!IsNegative(sine) && !IsNegative(cos) && !IsNegative(tan))
            {
                currentQuadrant = TrigQuadrant.One;
            }
            else if (!IsNegative(sine) && IsNegative(cos) && IsNegative(tan))
            {
                currentQuadrant = TrigQuadrant.Two;
            }
            else if (IsNegative(sine) && IsNegative(cos) && !IsNegative(tan))
            {
                currentQuadrant = TrigQuadrant.Three;
            }
            else if (IsNegative(sine) && !IsNegative(cos) && IsNegative(tan))
            {
                currentQuadrant = TrigQuadrant.Four;
            }
            else
            {
                currentQuadrant = TrigQuadrant.Unknown;
            }

            return currentQuadrant;
        }
              
        protected double CalculateBaseAngle()
        {
            var baseAngle = 0.00;
            // Division by zero guard
            if (Z == 0 || X == 0)
            {
                if(IsNegative(Y))
                {
                    baseAngle = SOUTH;
                }
                else
                {
                    baseAngle = NORTH_PRIME;
                }
            }
            else
            {
                switch (Quadrant)
                {
                    case TrigQuadrant.Unknown:
                        break;
                    case TrigQuadrant.One:
                        baseAngle = Math.Atan(Y / X);
                        break;
                    case TrigQuadrant.Two:
                        baseAngle = Math.Asin(Y / Z);
                        break;
                    case TrigQuadrant.Three:
                        baseAngle = Math.Atan(Y / X);
                        break;
                    case TrigQuadrant.Four:
                        baseAngle = Math.Acos(X / Z);
                        break;
                }
            }
            return RadiansToDegrees(baseAngle);
        }

        public override string ToString()
        {
            return $"{Quadrant}: {X}, {Y} -> {AngleTheta} for {Z}";
        }

        private bool IsNegative(double val)
        {
            return val < 0.00;
        }

        private double Square(double val)
        {
            return val * val;
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="radians">Radians</param>
        /// <returns>Degrees <seealso cref="double"/></returns>
        private double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns>Radians <seealso cref="double"/></returns>
        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180.00);
        }

    }
}

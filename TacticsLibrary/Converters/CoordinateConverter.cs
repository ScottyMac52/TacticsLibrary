using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;
using TacticsLibrary.Models;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Converters
{
    public class CoordinateConverter
    {
        public const int ROUND_DIGITS = 4;

        /// <summary>
        /// Converts a Polar Coordinate into a Point
        /// </summary>
        /// <param name="polarCoord"></param>
        /// <returns><see cref="Point"/></returns>
        public static Point GetPointFromPolarCoordinate(IPolarCoordinate polarCoord)
        {
            var quadrantHelper = QuadrantHelper.CreateQuadrant(polarCoord);
            return new Point((int) Math.Round(quadrantHelper.X, ROUND_DIGITS), (int) Math.Round(quadrantHelper.Y, ROUND_DIGITS));
        }

        /// <summary>
        /// Converts a plotted point relative to (0,0) on an X/Y axis
        /// </summary>
        /// <param name="plottedPoint">The point to plot</param>
        /// <returns>The <see cref="IPolarCoordinate"/> that matches the position</returns>
        public static IPolarCoordinate GetPolarCoordinateFromPoint(Point plottedPoint)
        {
            var quadrantHelper = QuadrantHelper.CreateQuadrant(plottedPoint.X, plottedPoint.Y);
            System.Diagnostics.Debug.Print($"{plottedPoint} -> {quadrantHelper}");
            return new PolarCoordinate(quadrantHelper.PolarCoord.Degrees, quadrantHelper.Z);
  
        }

          public static Point CalculatePointFromDegrees(Point offset, double radius, double degrees)
         {
             // Subtract the 90 degree offset to account for conversion from polar to compass coordinates
             var theta = degrees;
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
}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Converters;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Extensions
{
    public static class NavigationExtensions
    {
        public static Point GetCartesianCoordinates(this PolarCoordinate polarRef, Point offSet)
        {
            return CoordinateConverter.CalculatePointFromDegrees(offSet, polarRef.Radius, polarRef.Degrees);
        }

        public static PolarCoordinate GetPolarCoordinates(this Point point, Point offSet)
        {
            return CoordinateConverter.CalculateDegreesFromPoint(offSet, point);
        }

    }
}

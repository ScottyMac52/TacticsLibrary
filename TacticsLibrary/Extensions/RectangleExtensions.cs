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
    public static class RectanglePointExtensions
    {
        public const int HALF = 2;

        public static int GetCenterWidth(this Rectangle targetRect)
        {
            return targetRect.Width / HALF;
        }

        public static int GetCenterHeight(this Rectangle targetRect)
        {
            return targetRect.Height / HALF;
        }

        public static Point GetRelativePosition(this Point plottedPoint, Rectangle viewPort)
        {
            return PositionConverter.GetRelativePosition(plottedPoint, viewPort);
        }

        public static Point GetAbsolutePosition(this Point relativePosition, Rectangle viewPort)
        {
            return PositionConverter.GetAbsolutePosition(relativePosition, viewPort);
        }
        
        public static PolarCoordinate GetPolarCoord(this Point plottedPoint)
        {
            return CoordinateConverter.GetPolarCoordinateFromPoint(plottedPoint);
        }

        public static Point GetPoint(this PolarCoordinate polarCoord)
        {
            return CoordinateConverter.GetPointFromPolarCoordinate(polarCoord);

        }

    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Converters;
using TacticsLibrary.Interfaces;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Extensions
{
    public static class GeometryTrigExtensions
    {
        public const int HALF = 2;

        public static int GetCenterWidth(this Size targetRect)
        {
            return targetRect.Width / HALF;
        }

        public static int GetCenterHeight(this Size targetRect)
        {
            return targetRect.Height / HALF;
        }

        public static PointF ConvertTo(this Point convertTo)
        {
            return new PointF(convertTo.X, convertTo.Y);
        }

        public static PointF Offset(this PointF targetPoint, PointF offset, int roundDigits)
        {
            var x = offset.X + (float)Math.Round(targetPoint.X, roundDigits);
            var y = offset.Y + (float)Math.Round(targetPoint.Y, roundDigits);
            return new PointF(x, y);
        }

        public static Point ConvertTo(this PointF convertTo)
        {
            return new Point((int) Math.Round(convertTo.X, 0), (int) Math.Round(convertTo.Y, 0));
        }

        public static PointF GetRelativePosition(this PointF plottedPoint, Size viewPort)
        {
            return PositionConverter.GetRelativePosition(plottedPoint, viewPort);
        }

        public static PointF GetAbsolutePosition(this PointF relativePosition, Size viewPort)
        {
            return PositionConverter.GetAbsolutePosition(relativePosition, viewPort);
        }
        
        public static PolarCoordinate GetPolarCoord(this Point plottedPoint)
        {
            return CoordinateConverter.GetPolarCoordinateFromPoint(plottedPoint);
        }

        public static PolarCoordinate GetPolarCoord(this PointF plottedPoint)
        {
            return CoordinateConverter.GetPolarCoordinateFromPoint(plottedPoint);
        }
        
        public static PointF GetPoint(this PolarCoordinate polarCoord, PointF offset, int roundingDigits)
        {
            return CoordinateConverter.CalculatePointFromDegrees(offset, polarCoord, roundingDigits);
        }

    }
}

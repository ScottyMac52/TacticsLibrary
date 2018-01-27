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
        public const float HALF = 2.0F;

        public static float GetCenterWidth(this SizeF targetRect)
        {
            return targetRect.Width / HALF;
        }

        public static float GetCenterHeight(this SizeF targetRect)
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

        public static PointF GetRelativePosition(this PointF plottedPoint, SizeF viewPort)
        {
            return PositionConverter.GetRelativePosition(plottedPoint, viewPort);
        }

        public static PointF GetAbsolutePosition(this PointF relativePosition, SizeF viewPort)
        {
            return PositionConverter.GetAbsolutePosition(relativePosition, viewPort);
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

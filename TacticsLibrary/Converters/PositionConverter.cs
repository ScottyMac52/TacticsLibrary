using System;
using System.Drawing;
using TacticsLibrary.Extensions;

namespace TacticsLibrary.Converters
{
    public sealed class PositionConverter
    {
        public const int NEGATIVE = -1;

        /// <summary>
        /// Gets the relative point inside of the viewport represented by the plotted Point
        /// </summary>
        /// <param name="plottedPoint">Coordinate absolute position <see cref="PointF"/></param>
        /// <param name="viewPortExtent">Viewport <see cref="SizeF"/></param>
        /// <returns>Relative position</returns>
        public static PointF GetRelativePosition(PointF plottedPoint, SizeF viewPortExtent)
        {
            var actualResult = plottedPoint;

            if (plottedPoint.X >= viewPortExtent.GetCenterWidth())
            {
                actualResult.X = plottedPoint.X - viewPortExtent.GetCenterWidth();
            }
            else
            {
                actualResult.X = NEGATIVE * (viewPortExtent.GetCenterWidth() - plottedPoint.X);
            }

            actualResult.Y = viewPortExtent.GetCenterHeight() - plottedPoint.Y;

            return actualResult;
        }

        /// <summary>
        /// Converts a position relative to (0,0) in the center of the view port to an absolute position from (0,0) from the top left corner of the viewport
        /// </summary>
        /// <param name="relativePosition">The relative position from (0,0) in the ViewPortExtent <see cref="PointF"/></param>
        /// <param name="viewPortExtent">The ViewPortExtent <seealso cref="Size"/></param>
        /// <returns>Absolute Postition</returns>
        public static PointF GetAbsolutePosition(PointF relativePosition, SizeF viewPortExtent)
        {
            var actualResult = relativePosition;

            var midX = viewPortExtent.GetCenterWidth();
            var midY = viewPortExtent.GetCenterHeight();

            if(relativePosition.Y <= 0)
            {
                actualResult.Y = midY + Math.Abs(relativePosition.Y);
            }
            else
            {
                actualResult.Y = midY - relativePosition.Y;                    
            }

            actualResult.X = midX + relativePosition.X;
            return actualResult;
        }
    }
}

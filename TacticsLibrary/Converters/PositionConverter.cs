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
        /// <param name="plottedPoint">Coord inate absolute position <see cref="Point"/></param>
        /// <param name="viewPortExtent">Viewport <see cref="Size"/></param>
        /// <returns>Relative position</returns>
        public static Point GetRelativePosition(Point plottedPoint, Size viewPortExtent)
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
        /// Converts a position relative to (0,0) in the view port to a position relative to (-MaxX, MaxY)
        /// </summary>
        /// <param name="relativePosition">The relative position from (0,0) in the ViewPortExtent <see cref="Point"/></param>
        /// <param name="viewPortExtent">The ViewPortExtent <seealso cref="Size"/></param>
        /// <returns>Absolute Postition</returns>
        public static Point GetAbsolutePosition(Point relativePosition, Size viewPortExtent)
        {
            var actualResult = relativePosition;
            actualResult.X = viewPortExtent.GetCenterWidth() + relativePosition.X;
            actualResult.Y = viewPortExtent.GetCenterHeight() - relativePosition.Y;

            return actualResult;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Extensions;

namespace TacticsLibrary.Converters
{
    public class PositionConverter
    {
        /// <summary>
        /// Gets the relative point inside of the viewport represented by the plotted Point
        /// </summary>
        /// <param name="plottedPoint">Coord inate absolute position</param>
        /// <param name="viewPortExtent">Viewport</param>
        /// <returns>Relative position</returns>
        public static Point GetRelativePosition(Point plottedPoint, Rectangle viewPortExtent)
        {
            var actualResult = plottedPoint;

            if (plottedPoint.X >= viewPortExtent.GetCenterWidth())
            {
                actualResult.X = plottedPoint.X - viewPortExtent.GetCenterWidth();
            }
            else
            {
                actualResult.X = ((-1) * (viewPortExtent.GetCenterWidth())) - plottedPoint.X;
            }

            actualResult.Y = viewPortExtent.GetCenterHeight() - plottedPoint.Y;

            return actualResult;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Converters;

namespace TacticsLibrary.Extensions
{
    public static class RectanglePointExtensions
    {
        public static int GetCenterWidth(this Rectangle targetRect)
        {
            return targetRect.Width / 2;
        }

        public static int GetCenterHeight(this Rectangle targetRect)
        {
            return targetRect.Height / 2;
        }

        public static Point GetRelativePosition(this Point plottedPoint, Rectangle viewPort)
        {
            return CoordinateConverter.GetRelativePosition(plottedPoint, viewPort);
        }

    }
}

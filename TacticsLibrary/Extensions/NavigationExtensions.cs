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
    // TODO : Need to work on the Navigation Extensions
    public static class NavigationExtensions
    {


        public static PolarCoordinate GetPolarCoordinates(this Point point, Point offSet)
        {
            return CoordinateConverter.GetPolarCoordinateFromPoint(point);
        }

    }
}

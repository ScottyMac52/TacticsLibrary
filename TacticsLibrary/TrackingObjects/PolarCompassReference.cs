using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Converters;

namespace TacticsLibrary.TrackingObjects
{
    public class PolarCoordinate : IEquatable<PolarCoordinate>
    {
        public double Radius { get; set; }
        public double Degrees { get; set; }

        public bool Equals(PolarCoordinate other)
        {
            return Radius == other.Radius && Degrees == other.Degrees;
        }

        public override string ToString()
        {
            return $"Bearing {Degrees:F2}° {Radius} miles";
        }
    }
}

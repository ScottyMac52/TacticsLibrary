using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsLibrary.TrackingObjects
{
    public class PolarCompassReference : IEquatable<PolarCompassReference>
    {
        public double Radius { get; set; }
        public double Degrees { get; set; }

        public bool Equals(PolarCompassReference other)
        {
            return Radius == other.Radius && Degrees == other.Degrees;
        }

        public override string ToString()
        {
            return $"Bearing {Degrees}° {Radius} miles";
        }
    }
}

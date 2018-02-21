using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLibrary
{
    public partial class PolarCoordinate : IEquatable<PolarCoordinate>
    {
        /// <summary>
        /// Checks for equality inclusive of the Rounding
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PolarCoordinate other)
        {
            return Radius == other.Radius
                && Degrees == other.Degrees;
        }
    }
}

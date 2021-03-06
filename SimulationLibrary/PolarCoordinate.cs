﻿using System;

namespace SimulationLibrary
{
    /// <summary>
    /// Encapsulates a Polar Coordinate as two doubles Degrees and Radius
    /// </summary>
    public partial class PolarCoordinate
    {
        public const int ROUNDING_DIGITS = 4;
        private double _radius;
        private double _degrees;

        public PolarCoordinate(double angleTheta, double z)
        {
            _degrees = angleTheta;
            _radius = z;
        }

        public double Radius { get { return GetRoundedValue(_radius); } set { _radius = value; } }
        public double Degrees { get { return GetRoundedValue(_degrees); } set { _degrees = value; } }

        /// <summary>
        /// Human readable version of the coordinate
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Bearing {Degrees}° {Radius} miles";
        }

        /// <summary>
        /// Returns the specified value rounded by ROUNDING_DIGITS 
        /// </summary>
        /// <param name="origValue">Value to round</param>
        /// <returns>double</returns>
        private double GetRoundedValue(double origValue)
        {
            return Math.Round(origValue, ROUNDING_DIGITS);
        }
    }
}

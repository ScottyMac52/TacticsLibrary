using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsLibrary.Converters;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.Models
{
    /// <summary>
    /// Holds position data, cartersian and polar coordinates for a position 
    /// This is a RELATIVE position ONLY
    /// </summary>
    public class PositionData
    {
        public double X { get; protected set; }
        public double Y { get; protected set; }
        public double Z { get; protected set; }

        public PolarCoordinate PolarCoord { get; protected set; }
        public bool PositionProvided { get; protected set; }
        public TrigQuadrant TrigQuadrant { get; protected set; }

        public PositionData()
        {
            PositionProvided = false;
        }

        public PositionData(Point position) 
            : this(position.X, position.Y)
        {
        }

        public PositionData(PolarCoordinate polarCoord)
            : this(polarCoord.Degrees, polarCoord.Radius)
        {
        }

        public PositionData(int x, int y)
        {
            X = x;
            Y = y;

            PositionProvided = true;
            
            // Determine the Quadrant
            var quadrantHelper = QuadrantHelper.CreateQuadrant(X, Y);
            TrigQuadrant = quadrantHelper.FindQuadrant();


            ConvertToPolarCoordinates();
        }

        public PositionData(double degrees, double radius)
        {
            PositionProvided = true;
            PolarCoord = new PolarCoordinate() { Degrees = degrees, Radius = radius };
            ConvertToCartesianCoordinates();
        }

        private void ConvertToPolarCoordinates()
        {
            if(!PositionProvided)
            {
                throw new ArgumentException($"The cartesian coordinates must be provided");
            }

            var point = new Point(X, Y);
            PolarCoord = point.GetPolarCoord();
        }

        private void ConvertToCartesianCoordinates()
        {
            if (!PositionProvided || PolarCoord == null)
            {
                throw new ArgumentException($"The polar coordinates must be provided");
            }

            var point = PolarCoord.GetPoint();
            X = point.X;
            Y = point.Y;
        }
    }
}

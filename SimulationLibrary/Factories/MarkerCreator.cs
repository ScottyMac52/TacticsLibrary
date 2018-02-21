using SimulationLibrary.Factories.Interfaces;
using SimulationLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationLibrary.Factories
{
    public class MarkerCreator : IReferencePointCreator<Marker>
    {
        public Marker Create(string name, ISensor detectedBy, PointF position)
        {
            return new Marker(detectedBy, position)
            {
                Name = name
            };
        }

        public Marker Create(ISensor detectedBy, PointF position, double heading, double altitude, double speed, ContactTypes contactType, Action processContact = null)
        {
            return new Marker(detectedBy, position)
            {
                Heading = heading,
                Altitude = altitude,
                Speed = speed
            };
        }
    }
}

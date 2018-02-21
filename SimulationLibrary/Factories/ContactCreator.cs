using SimulationLibrary.Factories.Interfaces;
using SimulationLibrary.Interfaces;
using System;
using System.Drawing;

namespace SimulationLibrary.Factories
{
    public class ContactCreator : IReferencePointCreator<Contact>
    {
        /// <summary>
        /// Creates a Contact
        /// </summary>
        /// <param name="detectedBy"></param>
        /// <param name="position"></param>
        /// <param name="heading"></param>
        /// <param name="altitude"></param>
        /// <param name="speed"></param>
        /// <param name="contactType"></param>
        /// <param name="processContact"></param>
        /// <returns></returns>
        public Contact Create(ISensor detectedBy, PointF position, double heading, double altitude, double speed, ContactTypes contactType, Action processContact = null)
        {
            return new Contact(detectedBy, position)
            {
                Heading = heading,
                Altitude = altitude,
                Speed = speed,
                ContactType = contactType
            };
        }

        public Contact Create(string name, ISensor detectedBy, PointF position)
        {
            return new Contact(detectedBy, position)
            {
                Name = name
            };
        }
    }
}

using System;
using System.Drawing;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Enums;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary
{
    public class ReferencePointFactory<T> where T : IReferencePoint
    {
        public IReferencePoint CreateContact(ISensor detectedBy, PointF position, double heading, double altitude, double speed, ContactTypes contactType, Action processContact = null)
        {
            return new Contact(detectedBy, position)
            {
                Heading = heading,
                Altitude = altitude,
                Speed = speed,
                ContactType = contactType,
                ProcessLoop = processContact
            };
        }

        public IReferencePoint CreateMarker(ISensor addedTo, PointF position)
        {
            return new Marker(addedTo, position);
        }

    }
}

using log4net;
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
                ProcessLoop = processContact,
                CustomUpdateDuration = 2500
            };
        }

        public IReferencePoint CreateMarker(string name, ISensor addedTo, PointF position, ILog logger)
        {
            return new Marker(addedTo, position, logger)
            {
                Name = name
            };
        }

    }
}

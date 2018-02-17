using log4net;
using System;
using System.Drawing;
using TacticsLibrary.Enums;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary
{
    public class ReferencePointFactory<T> where T : IReferencePoint
    {
        public ILog Logger => LogManager.GetLogger("ReferencePointFactory");

        public Contact CreateContact(ISensor detectedBy, PointF position, double heading, double altitude, double speed, ContactTypes contactType, Action processContact = null)
        {
            Logger.Info($"{detectedBy} requesting to create a {contactType} contact at {position} traveling {heading}° at {speed} knts.");

            return new Contact(detectedBy, position)
            {
                Heading = heading,
                Altitude = altitude,
                Speed = speed,
                ContactType = contactType,
                CustomUpdateDuration = 2500
            };
        }

        public Marker CreateMarker(string name, ISensor addedTo, PointF position)
        {
            Logger.Info($"{addedTo} requesting a marker named {name} at {position}");
            
            return new Marker(addedTo, position)
            {
                Name = name
            };
        }

    }
}

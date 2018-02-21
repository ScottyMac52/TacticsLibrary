using log4net;
using SimulationLibrary.Factories;
using SimulationLibrary.Interfaces;
using System;
using System.Drawing;

namespace SimulationLibrary
{
    public class ReferencePointFactory<T> where T : IReferencePoint
    {
        public ILog Logger => LogManager.GetLogger("ReferencePointFactory");

        public T CreateReferencePoint(ISensor detectedBy, PointF position, double heading, double altitude, double speed, ContactTypes contactType, Action processContact = null)
        {
            return (T) Activator.CreateInstance(typeof(ReferencePoint), new object[] {detectedBy, position, heading, altitude, speed, contactType});
        }

        public T CreateMarker(string name)
        {
            return (T) Activator.CreateInstance(typeof(Marker), new object[] { });
        }

        public IContact CreateContact(ISensor detectedBy, PointF position, double heading, double altitude, double speed, ContactTypes contactType, Action processContact = null)
        {
            Logger.Info($"{detectedBy} requesting to create a {contactType} contact at {position} traveling {heading}° at {speed} knts.");

            var newContact = (IContact) Activator.CreateInstance(typeof(Contact), new object[] {detectedBy, position});

            newContact.Heading = heading;
            newContact.Altitude = altitude;
            newContact.Speed = speed;
            newContact.ContactType = contactType;
            newContact.CustomUpdateDuration = 2500;
            return newContact;
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

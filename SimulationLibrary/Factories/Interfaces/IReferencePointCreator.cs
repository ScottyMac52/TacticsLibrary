using SimulationLibrary.Interfaces;
using System;
using System.Drawing;

namespace SimulationLibrary.Factories.Interfaces
{
    public interface IReferencePointCreator<T> where T : IReferencePoint
    {
        T Create(ISensor detectedBy, PointF position, double heading, double altitude, double speed, ContactTypes contactType, Action processContact = null);
        T Create(string name, ISensor detectedBy, PointF position);
    }
}

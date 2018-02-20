using System;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Interfaces;

namespace SimulationLibraryTests
{
    internal class MockSensor : ISensor
    {
        public MockSensor()
        {
        }

        public SizeF ViewPortExtent => new SizeF(498, 498);

        public PointF OwnShip => throw new NotImplementedException();

        public Marker BullsEye => throw new NotImplementedException();

        public Marker HomePlate => throw new NotImplementedException();

        public SortedList<Guid, IContact> CurrentContacts => throw new NotImplementedException();

        public float Radius { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int RangeRings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float RingSep { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Action<IGraphics, IReferencePoint> PaintMethod => throw new NotImplementedException();

        public void AddContact(IContact newContact)
        {
            throw new NotImplementedException();
        }

        public void Draw(IGraphics g)
        {
            throw new NotImplementedException();
        }

        public List<IContact> FindContact(System.Drawing.PointF checkPoint, System.Drawing.SizeF detectionWindow, int roundingDigits)
        {
            throw new NotImplementedException();
        }
    }
}
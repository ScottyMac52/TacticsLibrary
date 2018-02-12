using System;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Interfaces;
using TacticsLibrary.Enums;

namespace TacticsLibrary.Interfaces
{
    public interface ISensor
    {
        SizeF ViewPortExtent { get; }
        PointF OwnShip { get; }
        Marker BullsEye { get; }
        Marker HomePlate { get; }
        SortedList<Guid, IContact> CurrentContacts { get; }
        float Radius { get; set; }
        int RangeRings { get; set; }
        float RingSep { get; set; }
        void AddContact(IContact newContact);
        void Draw(IGraphics g);
        List<IContact> FindContact(PointF checkPoint, SizeF detectionWindow, int roundingDigits);
        Action<IGraphics, IReferencePoint> PaintMethod { get; }
    }
}
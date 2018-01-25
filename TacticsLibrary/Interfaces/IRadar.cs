using System;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Enums;

namespace TacticsLibrary.Interfaces
{
    public interface IRadar
    {
        Size ViewPortExtent { get; }
        PointF OwnShip { get; }
        PointF BullsEye { get; }
        PointF HomePlate { get; }
        SortedList<Guid, IContact> CurrentContacts { get; }
        float Radius { get; set; }
        int RangeRings { get; set; }
        float RingSep { get; set; }
        void AddContact(IContact newContact);
        void AddReference(PointF refLocation, string refName, Image refImage);
        void Draw(IGraphics g);
        List<IContact> FindContact(Point checkPoint, Size detectionWindow);
    }
}
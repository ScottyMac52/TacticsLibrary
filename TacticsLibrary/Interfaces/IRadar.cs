using System;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Enums;

namespace TacticsLibrary.Interfaces
{
    public interface IRadar
    {
        Point BullsEye { get; }
        SortedList<Guid, IContact> CurrentContacts { get; }
        float Radius { get; set; }
        int RangeRings { get; set; }
        float RingSep { get; set; }
        void AddContact(Contact newPoint, ContactTypes contactType);
        void AddReference(Point refLocation, string refName, Image refImage);
        void Draw(IGraphics g);
        Contact PlotContact(ReferencePositions refPos, double degrees, double radius, double altitude, int speed, int course, ContactTypes contactType);
        Contact PlotContact(Point offset, double degrees, double radius, double altitude, int speed, int course, ContactTypes contactType = ContactTypes.AirUnknown);
        List<IContact> FindContact(Point checkPoint, Size detectionWindow);
    }
}
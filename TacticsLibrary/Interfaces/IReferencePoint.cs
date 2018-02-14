﻿using System;
using System.Drawing;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.EventHandlers;

namespace TacticsLibrary.Interfaces
{
    public interface IReferencePoint
    {
        #region Read/Write Properties
        string Name { get; set; }
        double Heading { get; set; }
        double Speed { get; set; }
        double Altitude { get; set; }
        bool Selected { get; set; }
        bool ShowText { get; set; }

        #endregion

        #region Read only Properties
        PointF Position { get; }
        PointF RelativePosition { get; }
        RectangleF DetectionWindow { get; }
        ISensor DetectedBy { get; }
        DateTime TimeStamp { get; }
        DateTime LastUpdate { get; }
        PolarCoordinate PolarPosit { get; }
        Guid UniqueId { get; }
        bool Initialized { get; }

        #endregion

        #region Methods, Events and Delegates
        void Draw(IGraphics g);
        event ReferencePointChangedEventHandler ReferencePointChanged;
        Action<IGraphics, IReferencePoint> PaintMethod { get; set; }

        #endregion
    }
}
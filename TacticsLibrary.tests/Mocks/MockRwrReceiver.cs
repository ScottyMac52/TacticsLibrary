using System;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Adapters;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Enums;

namespace TacticsLibrary.tests.Mocks
{
    public class MockRwrReceiver : IRwrReceiver
    {
        public Point BullsEye { get => GetBullsEye?.Invoke() ?? new Point(0, 0); set => SetBullsEye?.Invoke(value); }
        public float CenterPositionX { get => GetCenterPositionX?.Invoke() ?? 0F; set => SetCenterPositionX?.Invoke(value); }
        public float CenterPositionY { get => GetCenterPositionY?.Invoke() ?? 0F; set => SetCenterPositionY?.Invoke(value); }
        public SortedList<Guid, RwrPoint> PlottedPoints => new SortedList<Guid, RwrPoint>();
        public float Radius { get => GetRadius?.Invoke() ?? 0F; set => SetRadius(value); }
        public int RangeRings { get => GetRangeRings?.Invoke() ?? 0; set => SetRangeRings?.Invoke(value); }
        public float RingSep { get => GetRingSep?.Invoke() ?? 0F; set => SetRingSep(value); }

        public Func<Point> GetBullsEye { get; set; }
        public Func<float> GetCenterPositionX { get; set; }
        public Func<float> GetCenterPositionY { get; set; }
        public Func<float> GetRadius { get; set; }
        public Func<int> GetRangeRings { get; set; }
        public Func<float> GetRingSep { get; set; }

        public Action<Point> SetBullsEye { get; set; }
        public Action<float> SetCenterPositionX { get; set; }
        public Action<float> SetCenterPositionY { get; set; }
        public Action<float> SetRadius { get; set; }
        public Action<int> SetRangeRings { get; set; }
        public Action<float> SetRingSep { get; set; }

        public Action<RwrPoint, ContactTypes> MockAddPoint { get; set; }

        public Func<ReferencePositions, double, double, double, int, int, ContactTypes, RwrPoint> MockPlotContact { get; set; }

        public void AddPoint(RwrPoint newPoint, ContactTypes contactType)
        {
            MockAddPoint?.Invoke(newPoint, contactType);
        }

        public void Draw(IGraphics g)
        {
            // No action for Mocked 
        }

        public RwrPoint PlotContact(ReferencePositions refPos, double radius, double degrees, double altitude, int speed, int course, ContactTypes contactType)
        {
            return MockPlotContact(refPos, radius, degrees, altitude, speed, course, contactType);            
        }
    }
}

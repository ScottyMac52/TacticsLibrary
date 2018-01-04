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
        public List<RwrPoint> PlottedPoints => new List<RwrPoint>();
        public float Radius { get => GetRadius?.Invoke() ?? 0F; set => SetRadius(value); }
        public int RangeRings { get => GetRangeRings?.Invoke() ?? 0; set => SetRangeRings?.Invoke(value); }
        public bool Ready => true;
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

        public Action<Point, string, ContactTypes> MockAddPoint { get; set; }

        public Action<ReferencePositions, double, double, ContactTypes> MockPlotContact { get; set; }

        public void AddPoint(Point pt, string referenceText, ContactTypes contactType)
        {
            MockAddPoint?.Invoke(pt, referenceText, contactType);
        }

        public void Draw(IGraphics g)
        {
            // No action for Mocked 
        }

        public void Invalidate(Rectangle invalidRect)
        {
            // No action for Mocked
        }

        public void PlotContact(ReferencePositions refPos, double radius, double theta, ContactTypes contactType)
        {
            MockPlotContact(refPos, radius, theta, contactType);            
        }
    }
}

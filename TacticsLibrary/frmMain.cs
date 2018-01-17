using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TacticsLibrary.Adapters;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Enums;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary
{
    public partial class frmMain : Form
    {
        public const int MAX_DIFF = 500;

        int angle = 0;
        int StartAngle = 0;

        protected RadarPicture ThreatWarningReceiver { get; private set; }

        protected Random RandomNumberGen { get; private set; }

        public frmMain()
        {
            InitializeComponent();
            ThreatWarningReceiver = InitializeRwr();

            RandomNumberGen = new Random((int)DateTime.Now.Ticks);

            //AddRandomPlots(RandomNumberGen.Next(10));

            var friendly = ThreatWarningReceiver.PlotContact(new Point(plotPanel.Width / 2,plotPanel.Height / 2), 360, 100, 20000, 36000, 135, ContactTypes.AirEnemy);
            //var missile = ThreatWarningReceiver.PlotContact(friendly.Position, 180, 250, 20000, 360000, 135, ContactTypes.MissileMRM);

            plotPanel.Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void AddRandomPlots(int nPlots)
        {
            var randomPlots = GetRandomPlots(plotPanel.Width / 2, nPlots);

            randomPlots.ForEach(rp =>
            {
                var contactType = GetRandomContactType();
                var altitude = contactType.ToString().Contains("Air") || contactType.ToString().Contains("Missile") ? RandomNumberGen.Next(50000) : 0.00;
                var course = RandomNumberGen.Next(360);
                var speed = contactType.ToString().Contains("Air") ? RandomNumberGen.Next(300, 10000) : contactType.ToString().Contains("Missile") ? RandomNumberGen.Next(3000, 100000) : RandomNumberGen.Next(50);
                var heading = RandomNumberGen.Next(course);
                ThreatWarningReceiver.PlotContact(ReferencePositions.OwnShip, rp.Degrees, rp.Radius, altitude, speed, course, contactType);
            });

        }

        private List<PolarCoordinate> GetRandomPlots(double rMax, int nPlots)
        {
            var randPlots = new List<PolarCoordinate>();

            for (int i = 0; i < nPlots; i++)
            {
                var randomRange = RandomNumberGen.NextDouble() * rMax;
                var randomBearing = RandomNumberGen.Next(360);

                randPlots.Add(new PolarCoordinate() { Degrees = randomBearing, Radius = randomRange });
            }

            return randPlots;
        }

        private List<Point> GetRandomPoints(double rMax, int nPoints, Point offset)
        {
            var randPoints = new List<Point>();
            for (int i = 0; i < nPoints; i++)
            {
                var r = Math.Sqrt((double)RandomNumberGen.Next() / int.MaxValue) * rMax;
                var theta = (double)RandomNumberGen.Next() / int.MaxValue * 2 * Math.PI;
                var newPoint = new Point((int)(r * Math.Cos(theta)), (int)(r * Math.Sin(theta)));
                newPoint.Offset(offset);
                randPoints.Add(newPoint);
            }
            return randPoints;
        }

        private ContactTypes GetRandomContactType()
        {
            var contactTypes = Enum.GetValues(typeof(ContactTypes));
            return (ContactTypes)contactTypes.GetValue(RandomNumberGen.Next(contactTypes.Length-2));
        }

        private void PlotPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = new GraphicsAdapter(e.Graphics);

            try
            {
                ThreatWarningReceiver.Draw(g);
            }
            catch
            {
            }
        }

        private RadarPicture InitializeRwr()
        {
            var rwrReceiver = new RadarPicture(new Point(123, 90), new Point(100, 200))
            {
                CenterPositionX = plotPanel.Width / 2,
                CenterPositionY = plotPanel.Height / 2,
                Radius = plotPanel.Width / 2,
                RangeRings = 5,
                RingSep = 50
            };

            rwrReceiver.UpdatePending += RwrReceiver_UpdatePending;

            return rwrReceiver;
        }

        private void RwrReceiver_UpdatePending(object sender, EventArgs e)
        {
            plotPanel.Invalidate();
        }

        private void DrawScan()
        {
            new Thread(() =>
            {
                for (int i = 1; i <= 360; i++)
                {
                    StartAngle = 0;
                    angle++;
                    plotPanel.Invalidate();
                }

            }).Start();
        }

        private void plotPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        //    ContactTypes contactType = ContactTypes.AirUnknown;
        //    var typeToPlot = $"{selectType.Text}{selectClass.Text}";

        //    if(Enum.TryParse(typeToPlot, out contactType))
        //    {
        //        var newPoint = new RwrPoint(new Point(e.X, e.Y), 0.00, contactType, decimal.ToInt32(contactCourse.Value), decimal.ToInt32(contactSpeed.Value));
        //        ThreatWarningReceiver.AddPoint(newPoint, contactType);
        //        plotPanel.Invalidate();
        //    }
        }
    }
}
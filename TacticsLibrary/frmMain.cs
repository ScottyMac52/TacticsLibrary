using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TacticsLibrary.Adapters;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Enums;

namespace TacticsLibrary
{
    public partial class frmMain : Form
    {
        public const int MAX_DIFF = 500;

        int angle = 0;
        int StartAngle = 0;

        protected RwrReceiver ThreatWarningReceiver { get; private set; }

        protected Random RandomNumberGen { get; private set; }

        public frmMain()
        {
            InitializeComponent();
            ThreatWarningReceiver = InitializeRwr();

            RandomNumberGen = new Random((int)DateTime.Now.Ticks);
 
            // First determine how many out of a possible 10 points we are going to plot
            var maxPoints = RandomNumberGen.Next(10);

            for(int currentPoint = 1; currentPoint < maxPoints; currentPoint++)
            {
                // Random Contact type
                var contactType = GetRandomContactType();
                // Randomm X
                var xPos = RandomNumberGen.Next(plotPanel.Width);
                // Random Y
                var yPos = RandomNumberGen.Next(plotPanel.Height);

                ThreatWarningReceiver.AddPoint(new Point(xPos - 50, yPos + 10), $"{contactType} : {currentPoint}", contactType);
            }

            // Now add plotted points
            ThreatWarningReceiver.PlotContact(ReferencePositions.BullsEye, 50.00, 270.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.BullsEye, 150.00, 90.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.BullsEye, 225.00, 135.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.BullsEye, 200.00, 210.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.OwnShip, 100.00, 330.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.OwnShip, 150.00, 330.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.OwnShip, 200.00, 330.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.OwnShip, 240.00, 330.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.OwnShip, 95.00, 270.00, GetRandomContactType());
            ThreatWarningReceiver.PlotContact(ReferencePositions.BullsEye, 150.00, 360.00, GetRandomContactType());

            plotPanel.Invalidate();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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

        private RwrReceiver InitializeRwr()
        {
            return new RwrReceiver(new Point(123, 90), new Point(100, 200))
            {
                CenterPositionX = plotPanel.Width / 2,
                CenterPositionY = plotPanel.Height / 2,
                Radius = plotPanel.Width / 2,
                RangeRings = 5,
                RingSep = 50
            };
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
            ContactTypes contactType = ContactTypes.AirUnknown;
            var typeToPlot = $"{selectType.Text}{selectClass.Text}";

            if(Enum.TryParse(typeToPlot, out contactType))
            {
                ThreatWarningReceiver.AddPoint(new Point(e.X, e.Y), $"Added from {e.Button}", contactType);
                plotPanel.Invalidate();
            }
        }
    }
}
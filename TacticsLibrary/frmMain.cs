using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TacticsLibrary.Adapters;
using TacticsLibrary.DrawObjects;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary
{
    public partial class frmMain : Form
    {
        public const int MAX_DIFF = 500;

        int angle = 0;
        int StartAngle = 0;

        protected static ILog Logger => LogManager.GetLogger("frmMain");
        protected IRadar RadarReceiver { get; private set; }
        protected Random RandomNumberGen { get; private set; }

        public frmMain()
        {
            InitializeComponent();
            RadarReceiver = InitializeRadar();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RandomNumberGen = new Random((int)DateTime.Now.Ticks);
            var randomPlots = GenerateRandomPlots(RadarReceiver.ViewPortExtent.GetCenterWidth(), RandomNumberGen.Next(10));
            RefreshContactList();
            plotPanel.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rMax"></param>
        /// <param name="nPlots"></param>
        /// <returns></returns>
        private List<IContact> GenerateRandomPlots(double rMax, int nPlots)
        {
            var randPlots = new List<IContact>();

            for (int i = 0; i < nPlots; i++)
            {
                var randomRange = RandomNumberGen.NextDouble() * rMax;
                var randomBearing = RandomNumberGen.Next(360);
                var contactType = GetRandomContactType();
                var altitude = contactType.ToString().Contains("Air") || contactType.ToString().Contains("Missile") ? RandomNumberGen.Next(50000) : 0.00;
                var speed = contactType.ToString().Contains("Air") ? RandomNumberGen.Next(300, 10000) : contactType.ToString().Contains("Missile") ? RandomNumberGen.Next(3000, 100000) : RandomNumberGen.Next(50);
                var heading = RandomNumberGen.Next(360);
                var polarPosition = new PolarCoordinate(randomBearing, randomRange);
                var newContact = CreateContactAtPolarCoordinate(contactType, polarPosition, heading, speed, altitude);
                Logger.Info($"Adding contact: {newContact} as a random contact.");
                randPlots.Add(newContact);
                RadarReceiver.AddContact(newContact);
            }

            return randPlots;
        }

        private ContactTypes GetRandomContactType()
        {
            var contactTypes = Enum.GetValues(typeof(ContactTypes));
            return (ContactTypes)contactTypes.GetValue(RandomNumberGen.Next(contactTypes.Length-2));
        }

        private void PlotPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = new GraphicsAdapter(e.Graphics);
            RadarReceiver.Draw(g);
        }

        private RadarPicture InitializeRadar()
        {
            var bullsEye = new Point(123, 90);
            var homePlate = new Point(100, 200);
            var radarSize = new Size(plotPanel.ClientSize.Width, plotPanel.ClientSize.Height);
            Logger.Info($"Creating radar picture {radarSize}");
            var rwrReceiver = new RadarPicture(bullsEye, homePlate, radarSize)
            {
                Radius = radarSize.Width / 2,
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
            if (e.Button == MouseButtons.Right)
            {
                ContactTypes contactType = ContactTypes.AirUnknown;
                var typeToPlot = $"{selectType.Text}{selectClass.Text}";
                var absolutePosition = new Point(e.X, e.Y);

                if (Enum.TryParse(typeToPlot, out contactType))
                {
                    var newContact = CreateContactAtPoint(contactType, absolutePosition, 0.00, decimal.ToInt32(contactSpeed.Value));
                    Logger.Info($"Adding contact: {newContact} as a plotted contact {absolutePosition}");
                    RadarReceiver.AddContact(newContact);
                    RefreshContactList();
                    plotPanel.Invalidate();
                }
            }
        }

        /// <summary>
        /// Creates an returns a contact based on the current geometry of the sensors display
        /// </summary>
        /// <param name="contactType">Contact type <see cref="ContactTypes"/></param>
        /// <param name="absolutePosition">Contact absolute position in sensor <see cref="Point"/></param>
        /// <param name="altitude">Contact altitude in feet</param>
        /// <returns></returns>
        private IContact CreateContactAtPoint(ContactTypes contactType, Point absolutePosition, double heading = 0.00, double speed = 0.00, double altitude = 0.00)
        {
            var relativePosition = absolutePosition.GetRelativePosition(RadarReceiver.ViewPortExtent);
            var polarCoord = relativePosition.GetPolarCoord();
            var detectStartPoint = absolutePosition;
            detectStartPoint.Offset(-1 * DrawContact.POSITION_OFFSET, -1 * DrawContact.POSITION_OFFSET);
            var detectionWindow = new Rectangle(detectStartPoint, new Size(DrawContact.POSITION_OFFSET, DrawContact.POSITION_OFFSET));

            var newContact = new Contact(RadarReceiver)
            {
                Position = absolutePosition,
                RelativePosition = relativePosition,
                Speed = speed,
                Heading = heading,
                Altitude = 0.00,
                ContactType = contactType,
                PolarPosit = polarCoord,
                DetectionWindow = detectionWindow
            };

            return newContact;
        }

        private IContact CreateContactAtPolarCoordinate(ContactTypes contactType, PolarCoordinate polarCoord, double heading = 0.00, double speed = 0.00, double altitude = 0.00)
        {
            var absolutePosition = polarCoord.GetPoint().GetAbsolutePosition(RadarReceiver.ViewPortExtent);
            return CreateContactAtPoint(contactType, absolutePosition, heading, speed, altitude);
        }

        private void plotPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var absolutePosition = new Point(e.X, e.Y);
                var contacts = RadarReceiver.FindContact(absolutePosition, new Size(5, 5));
                MessageBox.Show($"Top contact found {contacts.FirstOrDefault()}");
            }
        }

        private void plotPanel_MouseMove(object sender, MouseEventArgs e)
        {
            lblPosition.Text = $"{e.Location}";
            var relativePos = e.Location.GetRelativePosition(plotPanel.ClientSize);
            lblPositionRelative.Text = $"{relativePos}";
            lblPolarPosition.Text = $"{relativePos.GetPolarCoord()}";
        }

        private void RefreshContactList()
        {
            var dataBindList = RadarReceiver.CurrentContacts.Values.ToList();
            gridViewContacts.DataSource = dataBindList;
            gridViewContacts.Refresh();
        }
    }
}
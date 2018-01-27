using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using TacticsLibrary.Adapters;
using TacticsLibrary.Converters;
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
        protected PointF CursorPosition { get; set; }

        public frmMain()
        {
            InitializeComponent();
            RadarReceiver = InitializeRadar();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RandomNumberGen = new Random((int)DateTime.Now.Ticks);
            // var randomPlots = GenerateRandomPlots(RadarReceiver.OwnShip, RadarReceiver.ViewPortExtent.GetCenterWidth(), 1);
            var newPosition = new PointF(RadarReceiver.ViewPortExtent.GetCenterWidth(), RadarReceiver.ViewPortExtent.GetCenterHeight());
            CreateContactAtPoint(ContactTypes.AirUnknown, newPosition, 90, 4500, 36000);
        }

        private void Contact_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Logger.Info($"{((IContact)sender).UniqueId} : {e.PropertyName} has changed.");
            plotPanel.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePosition">Offset addition from this point</param>
        /// <param name="rMax"></param>
        /// <param name="nPlots"></param>
        /// <returns></returns>
        private List<IContact> GenerateRandomPlots(PointF relativePosition, double rMax, int nPlots)
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
                var newContact = CreateContactAtPolarCoordinate(relativePosition, contactType, polarPosition, heading, speed, altitude);
                Logger.Info($"Adding contact: {newContact} as a random contact.");
                newContact.PropertyChanged += Contact_PropertyChanged;
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

            //g.DrawLine(Pens.YellowGreen, RadarReceiver.BullsEye, CursorPosition);
            System.Diagnostics.Debug.Print($"Cursor = {CursorPosition}");
            System.Diagnostics.Debug.Print($"Relative Cursor = {CursorPosition.GetRelativePosition(RadarReceiver.ViewPortExtent)}");
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
                    var newContact = CreateContactAtPoint(contactType, absolutePosition, decimal.ToInt32(contactCourse.Value), decimal.ToInt32(contactSpeed.Value));
                }
            }
        }

        /// <summary>
        /// Method used to add a contact by absolute position
        /// </summary>
        /// <param name="contactType"></param>
        /// <param name="absolutePosition"></param>
        /// <param name="heading"></param>
        /// <param name="speed"></param>
        /// <param name="altitude"></param>
        /// <returns></returns>
        private IContact CreateContactAtPoint(ContactTypes contactType, PointF absolutePosition, double heading = 0.00, double speed = 0.00, double altitude = 0.00)
        {
            var newContact = new Contact(RadarReceiver)
            {
                ContactType = contactType,
                Position = absolutePosition,
                Speed = speed,
                Heading = heading,
                Altitude = 0.00
            };
            Logger.Info($"Adding contact: {contactType} as a plotted contact at {absolutePosition}");
            newContact.PropertyChanged += Contact_PropertyChanged;
            RadarReceiver.AddContact(newContact);
            RefreshContactList();
            plotPanel.Invalidate();
            Logger.Info($"New contact added: {newContact}");
            return newContact;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativeTo"></param>
        /// <param name="contactType"></param>
        /// <param name="polarCoord"></param>
        /// <param name="heading"></param>
        /// <param name="speed"></param>
        /// <param name="altitude"></param>
        /// <returns></returns>
        private IContact CreateContactAtPolarCoordinate(PointF relativeTo, ContactTypes contactType, PolarCoordinate polarCoord, double heading = 0.00, double speed = 0.00, double altitude = 0.00)
        {
            var floatPos = polarCoord.GetPoint(relativeTo, CoordinateConverter.ROUND_DIGITS).GetAbsolutePosition(RadarReceiver.ViewPortExtent);
            var absolutePosition = new Point((int)floatPos.X, (int)floatPos.Y);
            return CreateContactAtPoint(contactType, absolutePosition, heading, speed, altitude);
        }

        private void plotPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var absolutePosition = new Point(e.X, e.Y);
                var contacts = RadarReceiver.FindContact(absolutePosition, new Size(5, 5), CoordinateConverter.ROUND_DIGITS);
                MessageBox.Show($"Top contact found {contacts.FirstOrDefault()}");
            }
        }

        private void plotPanel_MouseMove(object sender, MouseEventArgs e)
        {
            lblPosition.Text = $"{e.Location}";
            CursorPosition = e.Location.ConvertTo();
            var pointF = e.Location.ConvertTo();
            var relativePos = pointF.GetRelativePosition(plotPanel.ClientSize);
            var bullsEyeRelative = RadarReceiver.BullsEye.GetRelativePosition(RadarReceiver.ViewPortExtent);
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
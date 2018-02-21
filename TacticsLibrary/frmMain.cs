using DrawingLibrary;
using GraphicsLibrary.Adapters;
using log4net;
using SimulationLibrary;
using SimulationLibrary.Converters;
using SimulationLibrary.Extensions;
using SimulationLibrary.Factories;
using SimulationLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TacticsLibrary.Interfaces;

namespace TacticsLibrary
{
    public partial class frmMain : Form
    {
        public const int MAX_DIFF = 500;

        protected static ILog Logger => LogManager.GetLogger("frmMain");
        protected ISensor RadarReceiver { get; private set; }
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
            var newPosition = new PointF(RadarReceiver.ViewPortExtent.GetCenterWidth(), RadarReceiver.ViewPortExtent.GetCenterHeight());
            GenerateRandomPlots(RadarReceiver.BullsEye.Position, 100.00, 2);
            GenerateColumns();
            RefreshContactList();

        }

        private void GenerateColumns()
        {
            gridViewContacts.AutoGenerateColumns = false;

            var columns = new DataGridViewColumn[] {

                new DataGridViewButtonColumn()
                {
                    HeaderText = "Remove",
                    Name = "btnRemove",
                    ValueType = typeof(string),
                    Visible = true,
                    CellTemplate = new DataGridViewButtonCell() {  ToolTipText = "Remove from the sensor", Value = "Remove" },
                    Width = 50
                },
                new DataGridViewColumn()
                {
                    DataPropertyName = "PolarPosit",
                    HeaderText = "Polar",
                    Name = "polarPositPosition",
                    ValueType = typeof(string),
                    Visible = true,
                    CellTemplate = new DataGridViewTextBoxCell() { ToolTipText = "Polar position in reference to the center" },
                    Width = 175

                },
                new DataGridViewColumn()
                {
                    DataPropertyName = "Running",
                    HeaderText = "Running",
                    Name = "chkRunning",
                    ValueType = typeof(bool),
                    Visible = true,
                    CellTemplate = new DataGridViewCheckBoxCell() {  ToolTipText = "The contact's processing status" }
                },
                new DataGridViewColumn()
                {
                    DataPropertyName = "Speed",
                    HeaderText = "Speed",
                    Name = "speed",
                    ValueType = typeof(double),
                    Visible = true,
                    CellTemplate = new DataGridViewTextBoxCell() {  ToolTipText = "The contact's speed in knts" }
                },
                new DataGridViewColumn()
                {
                    DataPropertyName = "Heading",
                    HeaderText = "Heading",
                    Name = "heading",
                    ValueType = typeof(double),
                    Visible = true,
                    CellTemplate = new DataGridViewTextBoxCell() {  ToolTipText = "The contact's heading in °" }
                }

            };

            gridViewContacts.CellClick += GridViewContacts_CellClick;

            for (int x=0; x < columns.Length; x++)
            {
                if (gridViewContacts.InvokeRequired)
                {
                    gridViewContacts.Invoke(new MethodInvoker(delegate
                    {
                        gridViewContacts.Columns.Add(columns[x]);
                    }));
                }
                else
                {
                    gridViewContacts.Columns.Add(columns[x]);
                }
            }
        }

        private void GridViewContacts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 0:
                    var contact = gridViewContacts.Rows[e.RowIndex].DataBoundItem as IContact;
                    if(MessageBox.Show(this, $"{contact}", "Delete this contact?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        contact.Stop();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePosition">Offset addition from this point</param>
        /// <param name="rMax"></param>
        /// <param name="nPlots"></param>
        /// <returns></returns>
        private List<IReferencePoint> GenerateRandomPlots(PointF relativePosition, double rMax, int nPlots)
        {
            var randPlots = new List<IReferencePoint>();

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
                randPlots.Add(newContact);
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
            var refFactory = new ReferencePointFactory<Marker>();

            var bullsEye = refFactory.CreateMarker("Bullseye", RadarReceiver, new PointF(123, 90));
            bullsEye.PaintMethod = (graphics, refPoint) =>
            {
                graphics.FillCircle(Brushes.Blue, refPoint.Position.X, refPoint.Position.Y, 10);
                graphics.FillCircle(Brushes.Red, refPoint.Position.X, refPoint.Position.Y, 5);
            };

            var homePlate = refFactory.CreateMarker("Homeplate", RadarReceiver, new PointF(100, 200));
            bullsEye.PaintMethod = (graphics, refPoint) =>
            {
                graphics.FillCircle(Brushes.White, refPoint.Position.X, refPoint.Position.Y, 10);
                graphics.FillCircle(Brushes.Blue, refPoint.Position.X, refPoint.Position.Y, 5);
            };

            var radarSize = new Size(plotPanel.ClientSize.Width, plotPanel.ClientSize.Height);
            Logger.Info($"Creating radar picture {radarSize}");
            var rwrReceiver = new RadarPicture(bullsEye, homePlate, radarSize)
            {
                Radius = radarSize.Width / 2,
                RangeRings = 5,
                RingSep = 50
            };

            //rwrReceiver.UpdatePending += RwrReceiver_UpdatePending;
            return rwrReceiver;
        }

        private void RwrReceiver_UpdatePending(IReferencePoint referencePoint)
        {
            plotPanel.Invalidate();
        }

        private void plotPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContactTypes contactType = ContactTypes.AirUnknown;
                var typeToPlot = $"{selectType.Text}{selectClass.Text}";
                var absolutePosition = new PointF(e.X, e.Y);

                if (Enum.TryParse(typeToPlot, out contactType))
                {
                    var newContact = CreateContactAtPoint(absolutePosition, contactType, decimal.ToInt32(contactCourse.Value), decimal.ToInt32(contactSpeed.Value));
                    RefreshContactList();
                }
            }
        }

        /// <summary>
        /// Method used to add a contact by absolute position
        /// </summary>
        /// <param name="position">Absolute position of contact <see cref="PointF"/></param>
        /// <param name="contactType">Contact type to create <see cref="ContactTypes"/></param>
        /// <param name="heading"></param>
        /// <param name="speed"></param>
        /// <param name="altitude"></param>
        /// <returns></returns>
        protected IReferencePoint CreateContactAtPoint(PointF position, ContactTypes contactType, double heading = 0.00, double speed = 0.00, double altitude = 0.00)
        {
            var contactFactory = new ContactCreator();
            var newContact = contactFactory.Create(RadarReceiver, position, heading, altitude, speed, contactType);
            Logger.Info($"Creating contact: {contactType} as a plotted contact at {position}");
            newContact.ReferencePointChanged += NewContact_ReferencePointChanged;
            RadarReceiver.AddContact(newContact);
            newContact.Start();
            return newContact;
        }

        /// <summary>
        /// Process the <see cref="ReferencePoint"/> change in position in the <see cref="ISensor"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewContact_ReferencePointChanged(object sender, SimulationLibrary.EventHandlers.ReferencePointChangedEventArgs e)
        {
            var refPoint = sender as IContact;

            if(!refPoint?.Running ?? false)
            {
                return;
            }

            // If the contact is off the sensor then we 
            if (refPoint.RelativePosition.GetPolarCoord().Radius > RadarReceiver.Radius)
            {
                // remove it from the contact collection 
                RadarReceiver.CurrentContacts.Remove(refPoint.UniqueId);
                // tell it's thread to stop
                refPoint.Stop();
                // log it
                Logger.Info($"Contact {refPoint} removed as it's off the sensor range.");
            }

            if (Logger?.IsDebugEnabled ?? false)
            {
                Logger.Debug($"{refPoint?.UniqueId} : {e.PropertyName} has changed.");
            }

            // If there are rectangles to process
            if (!refPoint?.Running ?? false)
            {
                return;
            }

            if ((e.RectangleRegionsF?.Count ?? 0) > 0 && refPoint.Running)
            {
                // If we are being called from another thread 
                if (plotPanel.InvokeRequired && refPoint.Running)
                {
                    // use a MethodInvoker to invoke the method
                    plotPanel.Invoke(new MethodInvoker(delegate
                    {
                        plotPanel.Invalidate();
                    }));
                }
                else
                {
                    plotPanel.Invalidate();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startAbsolutePosition">Absolute position where the contact is plotted from</param>
        /// <param name="contactType"></param>
        /// <param name="polarCoord">Bearing and range from absolute position for the new contact</param>
        /// <param name="heading"></param>
        /// <param name="speed"></param>
        /// <param name="altitude"></param>
        /// <returns></returns>
        public IReferencePoint CreateContactAtPolarCoordinate(PointF startAbsolutePosition, ContactTypes contactType, PolarCoordinate polarCoord, double heading = 0.00, double speed = 0.00, double altitude = 0.00)
        {
            var relativePosition = startAbsolutePosition.GetRelativePosition(RadarReceiver.ViewPortExtent);
            var newRelativePosition = CoordinateConverter.CalculatePointFromDegrees(relativePosition, polarCoord, CoordinateConverter.ROUND_DIGITS);
            var newAbsolutePosition = newRelativePosition.GetAbsolutePosition(RadarReceiver.ViewPortExtent);
            return CreateContactAtPoint(newAbsolutePosition, contactType, heading, speed, altitude);
        }

        private void PlotPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var absolutePosition = new Point(e.X, e.Y);
                RadarReceiver
                    .FindContact(absolutePosition, new Size(5, 5), CoordinateConverter.ROUND_DIGITS)
                    .ForEach(contact =>
                    {
                        contact.Selected = !contact.Selected;
                    });
            }
        }

        private void PlotPanel_MouseMove(object sender, MouseEventArgs e)
        {
            lblPosition.Text = $"{e.Location}";
            CursorPosition = e.Location.ConvertTo();
            var pointF = e.Location.ConvertTo();
            var relativePos = pointF.GetRelativePosition(plotPanel.ClientSize);
            var bullsEyeRelative = RadarReceiver.BullsEye.Position.GetRelativePosition(RadarReceiver.ViewPortExtent);
            lblPositionRelative.Text = $"{relativePos}";
            lblPolarPosition.Text = $"{relativePos.GetPolarCoord()}";
        }


        private void RefreshContactList()
        {
            var dataBindList = RadarReceiver?
                .CurrentContacts?
                .Values?
                .Where(contact => contact?.Running ?? false)?
                .ToList();

            if (gridViewContacts.InvokeRequired)
            {
                gridViewContacts.Invoke(new MethodInvoker(delegate
                {
                    gridViewContacts.DataSource = dataBindList;
                    gridViewContacts.Refresh();
                }));
            }
            else
            {
                gridViewContacts.DataSource = dataBindList;
                gridViewContacts.Refresh();
            }

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            RadarReceiver
            .CurrentContacts
            .Values
            .Where(contact => contact?.Running ?? false)
            .ToList()
            .ForEach(cont =>
            {
                cont?.Stop();
            });

            e.Cancel = RadarReceiver.CurrentContacts.Values.Any(cnt =>
            {
                return cnt?.Running ?? false;
            });
        }
    }
}
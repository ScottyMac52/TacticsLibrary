using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;
using TacticsLibrary.TrackingObjects;
using log4net;
using System.Windows.Forms;

namespace TacticsLibrary.DrawObjects
{
    public class RadarPicture : Control, IRadar
    {
        public ILog Logger { get; protected set; }
        /// <summary>
        /// The Radius of the Radar
        /// </summary>
        public float Radius { get; set; }
        /// <summary>
        /// The seperation of the range rings in pixels
        /// </summary>
        public float RingSep { get; set; }
        /// <summary>
        /// HThe number of range rings to display
        /// </summary>
        public int RangeRings { get; set; }
        public SortedList<Guid, IContact> CurrentContacts { get; protected set; }
        public SizeF ViewPortExtent { get; protected set; }
        public PointF BullsEye { get; private set; }
        public PointF HomePlate { get; private set; }
        public PointF OwnShip => new PointF(ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());
        public event EventHandler UpdatePending;

        protected virtual void OnUpdatePending(EventArgs e)
        {
            UpdatePending?.Invoke(this, e);
        }

        public RadarPicture(PointF bullsEye, PointF homePlate, SizeF viewPortExtent, ILog logger = null)
        {
            BullsEye = bullsEye;
            HomePlate = homePlate;
            ViewPortExtent = viewPortExtent;
            CurrentContacts = new SortedList<Guid, IContact>();
            Logger = logger == null ? LogManager.GetLogger(typeof(RadarPicture)) : logger;
            Logger.Info($"Created radar {ViewPortExtent} BullsEye: {BullsEye} HomePlate: {HomePlate}");
        }

        public void Draw(IGraphics g)
        {
            g.DrawCircle(Pens.Green, ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight(), Radius);

            var dashedPen = new Pen(new SolidBrush(Color.FromArgb(0, 128, 0)))
            {
                DashStyle = DashStyle.Dot
            };

            for (int ringCounter=0; ringCounter <= RangeRings; ringCounter++)
            {
                var newRadius = Radius - ((ringCounter + 1) * RingSep);
                if(newRadius > 0)
                {
                    g.DrawCircle(dashedPen, ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight(), newRadius);
                }
            }

            // Paint the BullsEye
            g.FillCircle(Brushes.Blue, BullsEye.X, BullsEye.Y, 10);
            g.FillCircle(Brushes.Red, BullsEye.X, BullsEye.Y, 5);

            // Paint Homeplate
            g.FillCircle(Brushes.White, HomePlate.X, HomePlate.Y, 10);
            g.FillCircle(Brushes.Blue, HomePlate.X, HomePlate.Y, 5);

            // Plot all points
            foreach (var item in CurrentContacts)
            {
                item.Value.Draw(g);
            }
        }

        /// <summary>
        /// Find all contacts on the radar using a point and detection window
        /// </summary>
        /// <param name="checkPoint"><see cref="PointF"/>The center point of the search</param>
        /// <param name="detectionWindow"><see cref="SizeF"/>The size of the detction window</param>
        /// <param name="roundingDigits"></param>
        /// <returns><see cref="List{Contact}"/></returns>
        public List<IContact> FindContact(PointF checkPoint, SizeF detectionWindow, int roundingDigits)
        {
            var contactList = new List<IContact>();
            checkPoint.Offset(new PointF(-1 * detectionWindow.GetCenterWidth(), -1 * detectionWindow.GetCenterHeight()), roundingDigits);
            var detectionArea = new RectangleF(checkPoint, detectionWindow);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug($"FindWindow: {detectionArea}");
            }

            // First check for any hits
            if (CurrentContacts.Values.Any(contact =>
            {
                return detectionArea.IntersectsWith(contact.DetectionWindow);
            }) == true)
            {
                Logger.Info($"Contact(s) found");
                // There were hits so get the contacts
                contactList.AddRange(CurrentContacts.Values.ToList().FindAll(match =>
                {
                    return detectionArea.IntersectsWith(match.DetectionWindow);
                }));
            }

            if (contactList.Count > 0)
            {
                Logger.Info($"Found {contactList.Count} contact(s) in {detectionArea}");
                if (Logger.IsDebugEnabled)
                {
                    contactList.ForEach(contact =>
                    {
                        Logger.Debug($"{contact}");
                    });
                }
            }
            else
            {
                Logger.Info($"No contacts found in {detectionArea}");
            }
            return contactList;
        }
         
        /// <summary>
        /// Adds a point as a type and class of contact 
        /// </summary>
        /// <param name="newContact">Relative position from (0,0) <see cref="IContact"/></param>
        public void AddContact(IContact newContact)
        {
            if(CurrentContacts == null)
            {
                CurrentContacts = new SortedList<Guid, IContact>();
            }
            if (!CurrentContacts.ContainsKey(newContact.UniqueId))
            {
                CurrentContacts.Add(newContact.UniqueId, newContact);
                newContact.UpdatePending += Contact_UpdatePending;
                Logger.Debug($"Added contact: {newContact}");
            }
            else
            {
                Logger.Warn($"Already found reference to contact {newContact}");
            }

        }

        public void AddReference(PointF refLocation, string refName, Image refImage)
        {
        }

        private void Contact_UpdatePending(object sender, EventArgs e)
        {
            Logger.Debug($"Updating contact: {sender as IContact}");
            OnUpdatePending(new EventArgs());
        }
    }
}

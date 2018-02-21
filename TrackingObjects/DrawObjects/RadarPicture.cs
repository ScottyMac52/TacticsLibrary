using GraphicsLibrary;
using GraphicsLibrary.Adapters;
using log4net;
using SimulationLibrary;
using SimulationLibrary.Extensions;
using SimulationLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace TacticsLibrary.Interfaces
{
    public class RadarPicture : ISensor
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
        public Marker BullsEye { get; private set; }
        public Marker HomePlate { get; private set; }
        public PointF OwnShip => new PointF(ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());
        public Action<IGraphics, IReferencePoint> PaintMethod { get; protected set; }

        public RadarPicture(Marker bullsEye, Marker homePlate, SizeF viewPortExtent, ILog logger = null)
        {
            BullsEye = bullsEye;
            HomePlate = homePlate;
            ViewPortExtent = viewPortExtent;
            CurrentContacts = new SortedList<Guid, IContact>();
            Logger = logger == null ? LogManager.GetLogger("RadarPicture") : logger;
            Logger.Info($"Created radar {ViewPortExtent} {BullsEye} {HomePlate}");
        }

        public void Draw(IGraphics g)
        {
            g.DrawCircle(Pens.Green, ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight(), Radius);

            var dashedPen = new Pen(new SolidBrush(Color.FromArgb(0, 128, 0)))
            {
                DashStyle = DashStyle.Dot
            };

            for (int ringCounter = 0; ringCounter <= RangeRings; ringCounter++)
            {
                var newRadius = Radius - ((ringCounter + 1) * RingSep);
                if (newRadius > 0)
                {
                    g.DrawCircle(dashedPen, ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight(), newRadius);
                }
            }

            BullsEye?.PaintMethod?.Invoke(g, BullsEye);
            HomePlate?.PaintMethod?.Invoke(g, HomePlate);

            var activeContacts = CurrentContacts.Where(cont => cont.Value?.Running ?? false == true)
                .ToList();

            activeContacts.ForEach(con => {
                IGraphics newContext = new GraphicsAdapter(g.Context);
                con.Value?.Draw(newContext);
            });
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
                // newContact.UpdateRegion += Contact_UpdatePending;
                Logger.Debug($"Added contact: {newContact}");
            }
            else
            {
                Logger.Warn($"Already found reference to contact {newContact}");
            }

        }

        private void Contact_UpdatePending(IReferencePoint referencePoint)
        {
            Logger.Debug($"Updating contact: {referencePoint}");
            // OnUpdatePending(referencePoint);
        }
    }
}

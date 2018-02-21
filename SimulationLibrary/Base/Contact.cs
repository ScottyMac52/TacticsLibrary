using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using SimulationLibrary.Converters;
using SimulationLibrary.EventHandlers;
using SimulationLibrary.Extensions;
using SimulationLibrary.Interfaces;

namespace SimulationLibrary
{
    public partial class Contact : BaseContact, IContact
    {
        private bool _processThreadRunning = false;

        internal Contact(ISensor detectedBy, PointF position) : base(detectedBy, position)
        {
        }

        public override event ReferencePointChangedEventHandler ReferencePointChanged;

        public override bool Running => _processThreadRunning;

        public override void Start(Action processAction = null)
        {
            Logger?.Info($"{this} firing ReferencePointChanged for new");
            ProcessThread = new Thread(new ThreadStart(processAction ?? DefaultProcessing));
            ProcessThread.Start();
            _processThreadRunning = true;
        }

        public override void Stop()
        {
            Logger?.Info($"Stop Requested: {this}");
            _processThreadRunning = false;
        }

        protected override void DefaultProcessing()
        {
            //int modProcess = 20;

            var previousPositions = new SortedList<long, PointF>();

            while (_processThreadRunning)
            {
                if (Speed > 0)
                {
                    // Plot new position based on course
                    // We know that 10 seconds have passed so we have to calculate the new position and plot it
                    // Get the duration since the last update and set the LastUpdate
                    var timeSinceLastUpdate = DateTime.UtcNow - LastUpdate;
                    LastUpdate = DateTime.UtcNow;
                    // Now calculate the contacts movement in that timespan
                    var distance = timeSinceLastUpdate.TotalSeconds * (Speed / SECONDS_PER_HOUR);
                    // Get the new position based on course and distance traveled
                    var newPos = CoordinateConverter.CalculatePointFromDegrees(RelativePosition, new PolarCoordinate(Heading, distance), CoordinateConverter.ROUND_DIGITS);
                    var newAbsPos = newPos.GetAbsolutePosition(DetectedBy.ViewPortExtent);
                    // Create Region and add the previous window to it
                    var oldRectPos = DetectionWindow;
                    // Set the new Position
                    Position = newAbsPos;
                    // Add the new position to the Region
                    var newRectPos = DetectionWindow;
                    var rectangleList = new List<RectangleF>();
                    rectangleList.AddRange(new List<RectangleF>() { oldRectPos, newRectPos });
                    // Notify the ISensor to repaint
                    ReferencePointChanged?.Invoke(this, new ReferencePointChangedEventArgs(rectangleList, UpdateEventTypes.PositionChange, nameof(Position)));
                    Thread.Sleep(CustomUpdateDuration ?? DEFAULT_UPDATE_MILISECONDS);
                }
            }

            Logger.Info($"{this} has stopped processing");
        }
    }
}
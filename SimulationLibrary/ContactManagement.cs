using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Interfaces;
using TacticsLibrary.Extensions;

namespace SimulationLibrary
{
    public class ContactManagement
    {
        public IReferencePoint ReferencePoint { get; protected set; }

        public float PercentageCourseStability { get; protected set; }

        public float PercentageSpeedStability { get; protected set; }

        public List<PointF> PreviousPositions { get; protected set; }

        private Random _randomizer = new Random(DateTime.UtcNow.Millisecond);

        public double ProposedSpeed => GetProposedSpeed();

        public double ProposedHeading => GetProposedHeading();

        /// <summary>
        /// Creates a <see cref="ContactManagement"/> instance to manage speed, heading and altitude
        /// </summary>
        /// <param name="refPoint"></param>
        /// <param name="percentageCourseStability"></param>
        /// <param name="percentageSpeedStability"></param>
        public ContactManagement(IReferencePoint refPoint, float percentageCourseStability, float percentageSpeedStability)
        {
            ReferencePoint = refPoint;
            PreviousPositions = new List<PointF>() { refPoint.Position };
            PercentageCourseStability = percentageCourseStability;
            PercentageSpeedStability = percentageSpeedStability;
        }

        public void AlterStability(float? percentageCourseStability = null, float? percentageSpeedStability = null)
        {
            if(percentageSpeedStability.HasValue)
            {
                PercentageSpeedStability = percentageSpeedStability.Value;
            }

            if (percentageCourseStability.HasValue)
            {
                PercentageCourseStability = percentageCourseStability.Value;
            }
        }

        public double GetProposedSpeed(Func<double> calculateNewSpeed = null)
        {
            if(PercentageSpeedStability >= 1.0F)
            {
                return ReferencePoint?.Speed ?? 0;
            }
            else
            {
                if (calculateNewSpeed == null)
                {
                    // Determine the amount of change in speed
                    var intPerc = (int)Math.Round(PercentageSpeedStability * 100.00, 0);
                    var degreeOfSpeedChange = _randomizer.Next(intPerc) / 100.0;

                    // Determine if speed will increase or decrease
                    var speedDirection = _randomizer.Next(100);
                    var newSpeed = speedDirection < 50 ? ReferencePoint.Speed - (ReferencePoint.Speed * degreeOfSpeedChange)
                        : ReferencePoint.Speed + (ReferencePoint.Speed * degreeOfSpeedChange);
                    return newSpeed;
                }
                else
                {
                    return calculateNewSpeed.Invoke();
                }
            }
        }
        public double GetProposedHeading(Func<double> calculateNewHeading = null)
        {
            if (PercentageCourseStability >= 1.0F)
            {
                return ReferencePoint?.Heading ?? 0;
            }
            else
            {
                if (calculateNewHeading == null)
                {
                    // Determine the amount of change in heading in percentage
                    var intPerc = (int)Math.Round(PercentageCourseStability * 100.00, 0);
                    var degreeOfCourseChange = _randomizer.Next(intPerc) / 100.00;

                    var courseDirection = _randomizer.Next(100);
                    var newCourse = courseDirection < 50 ? ReferencePoint.Heading - (ReferencePoint.Heading * degreeOfCourseChange)
                        : ReferencePoint.Heading + (ReferencePoint.Heading * degreeOfCourseChange);
                    return newCourse;
                }
                else
                {
                    return calculateNewHeading.Invoke();
                }
            }
        }

    }
}

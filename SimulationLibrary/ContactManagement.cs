using System;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Interfaces;

namespace SimulationLibrary
{
    public class ContactManagement
    {
        /// <summary>
        /// <see cref="IReferencePoint"/> under contact simulation management
        /// </summary>
        public IReferencePoint ReferencePoint { get; protected set; }

        /// <summary>
        /// Percentage chance that speed wil remain the same
        /// </summary>
        public float PercentageCourseStability { get; protected set; }

        /// <summary>
        /// Percentage chance that the heading will remain the same 
        /// </summary>
        public float PercentageSpeedStability { get; protected set; }

        /// <summary>
        /// Previous positions for this <see cref="IReferencePoint"/>
        /// </summary>
        public List<PointF> PreviousPositions { get; protected set; }

        private Random _randomizer = new Random(DateTime.UtcNow.Millisecond);

        /// <summary>
        /// Default calculated new speed based on stability
        /// </summary>
        public double ProposedSpeed => GetProposedSpeed();

        /// <summary>
        /// Default calculated new heading based on stability
        /// </summary>
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

        /// <summary>
        /// Access to stability variables
        /// </summary>
        /// <param name="percentageCourseStability"></param>
        /// <param name="percentageSpeedStability"></param>
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

        /// <summary>
        /// Gets the new proposed speed using the passed in <see cref="Func{T, TResult}"/> or the default algorithm
        /// </summary>
        /// <param name="calculateNewSpeed"></param>
        /// <returns></returns>
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
                    var degreeOfSpeedChange = _randomizer.Next(1, intPerc) / 100.0;

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

        /// <summary>
        /// Gets the new proposed heading using the passed in <see cref="Func{T, TResult}"/> or the default algorithm
        /// </summary>
        /// <param name="calculateNewHeading"></param>
        /// <returns></returns>
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
                    var degreeOfCourseChange = _randomizer.Next(1, intPerc) / 100.00;

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

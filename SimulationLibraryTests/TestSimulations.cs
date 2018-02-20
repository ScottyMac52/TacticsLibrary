using System;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulationLibrary;
using TacticsLibrary;
using TacticsLibrary.Enums;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;

namespace SimulationLibraryTests
{
    [TestClass]
    public class TestSimulations
    {
        internal ISensor SensorUnderTest { get; private set; }
        internal ReferencePointFactory<Contact> ContactFactory { get; private set; }

        [TestInitialize]
        public void Setup()
        {
            ContactFactory = new ReferencePointFactory<Contact>();
            SensorUnderTest = new MockSensor();
        }

        [TestMethod]
        public void TestThatSpeedRemainsSameWithStableFactor()
        {
            // ARRANGE
            var testContact = ContactFactory.CreateContact(SensorUnderTest, new PointF(100, 100), 360.0, 1200.00, 200.00, ContactTypes.AirFriendly);
            var simulationObject = new ContactManagement(testContact, 1.0F, 1.0F);

            // ACT
            var newSpeed = simulationObject.ProposedSpeed;
            var newHeading = simulationObject.ProposedHeading;

            // ASSERT
            Assert.AreEqual(testContact.Speed, newSpeed);
            Assert.AreEqual(testContact.Heading, newHeading);
        }

        [TestMethod]
        public void TestThatSpeedChangesWithUnstableFactor()
        {
            // ARRANGE
            var testContact = ContactFactory.CreateContact(SensorUnderTest, new PointF(100, 100), 360.0, 1200.00, 200.00, ContactTypes.AirFriendly);
            var simulationObject = new ContactManagement(testContact, 1.0F, .10F);

            // ACT
            var newSpeed = simulationObject.ProposedSpeed;
            var newHeading = simulationObject.ProposedHeading;

            // ASSERT
            Assert.AreNotEqual(testContact.Speed, newSpeed);
            Assert.AreEqual(testContact.Heading, newHeading);
        }

        [TestMethod]
        public void TestThatSpeedIsCalculatedUsingFunc()
        {
            // ARRANGE
            var testContact = ContactFactory.CreateContact(SensorUnderTest, new PointF(100, 100), 360.0, 1200.00, 200.00, ContactTypes.AirFriendly);
            var simulationObject = new ContactManagement(testContact, 1.0F, .10F);
            var expectedSpeed = 201.00;

            // ACT
            var newSpeed = simulationObject.GetProposedSpeed(() => { return 201.00; });
            var newHeading = simulationObject.ProposedHeading;

            // ASSERT
            Assert.AreEqual(expectedSpeed, newSpeed);
            Assert.AreEqual(testContact.Heading, newHeading);

        }

        [TestMethod]
        public void TestThatHeadingIsCalculatedUsingFunc()
        {
            // ARRANGE
            var testContact = ContactFactory.CreateContact(SensorUnderTest, new PointF(100, 100), 360.0, 1200.00, 200.00, ContactTypes.AirFriendly);
            var simulationObject = new ContactManagement(testContact, .99F, 1.0F);
            var expectedHeading = 270.00;

            // ACT
            var newSpeed = simulationObject.ProposedSpeed;
            var newHeading = simulationObject.GetProposedHeading(() => { return 270.00; });

            // ASSERT
            Assert.AreEqual(testContact.Speed, newSpeed);
            Assert.AreEqual(expectedHeading, newHeading);

        }


        [TestMethod]
        [Ignore]
        public void TestThatHeadingIsCalculatedUsingFuncThatUsesAverageOfPreviousPositions()
        {
            // ARRANGE
            var testContact = ContactFactory.CreateContact(SensorUnderTest, new PointF(100, 100), 360.0, 1200.00, 200.00, ContactTypes.AirFriendly);
            var simulationObject = new ContactManagement(testContact, .99F, 1.0F);
            simulationObject.PreviousPositions.AddRange(new PointF[] { new PointF(1.1F, 1.1F), new PointF(2.1F, 1.1F), new PointF(3.1F, 1.1F) });
            var expectedHeading = 270.00;

            // ACT
            var newSpeed = simulationObject.ProposedSpeed;
            var newHeading = simulationObject.GetProposedHeading(() => { return simulationObject.PreviousPositions.Average(pp => pp.GetRelativePosition(testContact.DetectedBy.ViewPortExtent).GetPolarCoord().Degrees); });

            // ASSERT
            Assert.AreEqual(testContact.Speed, newSpeed);
            Assert.AreEqual(expectedHeading, newHeading);

        }

    }
}

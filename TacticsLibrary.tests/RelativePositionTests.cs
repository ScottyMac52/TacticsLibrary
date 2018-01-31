using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TacticsLibrary.Converters;
using TacticsLibrary.Extensions;
using TacticsLibrary.DrawObjects;

namespace TacticsLibrary.tests
{
    [TestClass]
    public class RelativePositionTests
    {
        private readonly SizeF ViewPortExtent = new SizeF(496.00F, 496.00F);
        private readonly PointF BullsEyeAbsolute = new PointF(123.00F, 90.00F);

        [TestMethod]
        public void TestEnsureThatOffsetIsCalculatedCorrectly()
        {
            // ARRANGE
            // Start with relative position
            var startOffset = new PointF(-1, 0);
            var offsetPosition = new PolarCoordinate(90.00, 1);
            var expectedResult = new PointF(0F, 0F);

            // ACT
            var actual = CoordinateConverter.CalculatePointFromDegrees(startOffset, offsetPosition, 3);

            // ASSERT
            Assert.AreEqual(expectedResult, actual);
        }

        [TestMethod]
        public void TestEnsureThatPlots10MilesEastOfAreCorrectRelativeToBullsEye()
        {
            // ARRANGE
            // Get Position of bullseye in relative coordinates
            var bullsEyeRelative = BullsEyeAbsolute.GetRelativePosition(ViewPortExtent);
            // We are going to plot at 90° 1 unit relative to BullsEye
            var offsetPosition = new PolarCoordinate(90.00, 10);
            var expectedResult = BullsEyeAbsolute.Offset(new PointF(10.0F, 0.0F), CoordinateConverter.ROUND_DIGITS);

            // ACT
            var newRelativePosition = CoordinateConverter.CalculatePointFromDegrees(bullsEyeRelative, offsetPosition, 3);
            var actual = newRelativePosition.GetAbsolutePosition(ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult, actual);

        }

        [TestMethod]
        public void TestThatPostionRelativeToOneUnitRightandOneUnitAboveIs45DegreesOneUnit()
        {
            // ARRANGE
            var startPoint = new PointF(0, 0);
            var refPoint = new PointF(1, 1);
            var expectedResult = new PolarCoordinate(45.00, 1.414);

            // ACT and ASSERT
            AssertCheckRefPointResults(startPoint, refPoint, expectedResult);
        }


        [TestMethod]
        public void TestThatPostionRelativeToOneUnitLeftandOneUnitDownIs225DegreesOneUnit()
        {
            // ARRANGE
            var startPoint = new PointF(0, 0);
            var refPoint = new PointF(-1, -1);
            var expectedResult = new PolarCoordinate(225.00, 1.414);

            // ACT and ASSERT
            AssertCheckRefPointResults(startPoint, refPoint, expectedResult);
        }


        [TestMethod]
        public void TestThatPostionRelativeToOneUnitLeftandOneUnitUpis315DegreesOneUnit()
        {
            // ARRANGE
            var startPoint = new PointF(0, 0);
            var refPoint = new PointF(-1, 1);
            var expectedResult = new PolarCoordinate(315.00, 1.414);

            // ACT and ASSERT
            AssertCheckRefPointResults(startPoint, refPoint, expectedResult);
        }

        [TestMethod]
        public void TestThatPostionRelativeToOneUnitUpis0DegreesOneUnit()
        {
            // ARRANGE
            var startPoint = new PointF(0, 0);
            var refPoint = new PointF(0, 1);
            var expectedResult = new PolarCoordinate(0.00, 1.00);

            // ACT and ASSERT
            AssertCheckRefPointResults(startPoint, refPoint, expectedResult);
        }

        [TestMethod]
        public void TestThatPostionRelativeToOneUnitDownis180DegreesOneUnit()
        {
            // ARRANGE
            var startPoint = new PointF(0, 0);
            var refPoint = new PointF(0, -1);
            var expectedResult = new PolarCoordinate(180.00, 1.00);

            // ACT and ASSERT
            AssertCheckRefPointResults(startPoint, refPoint, expectedResult);
        }

        [TestMethod]
        public void TestThatPostionRelativeToOneUnitLeftIs270DegreesOneUnit()
        {
            // ARRANGE
            var startPoint = new PointF(0, 0);
            var refPoint = new PointF(-1, 0);
            var expectedResult = new PolarCoordinate(270.00, 1.00);

            // ACT and ASSERT
            AssertCheckRefPointResults(startPoint, refPoint, expectedResult);
        }

        private void AssertCheckRefPointResults(PointF startPoint, PointF refPoint, PolarCoordinate expectedResult)
        {
            // ACT
            var newPoint = startPoint.Offset(refPoint, 3);
            var actual = newPoint.GetPolarCoord();

            // ASSERT
            Assert.IsTrue(expectedResult.Equals(actual), $"{expectedResult} <> Actual result: {actual}");
        }
    }
}

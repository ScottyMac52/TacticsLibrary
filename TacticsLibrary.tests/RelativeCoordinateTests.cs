using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using TacticsLibrary.Converters;

namespace TacticsLibrary.tests
{
    [TestClass]
    public class RelativeCoordinateTests
    {
        public Rectangle ViewPortExtent { get; set; }

        [TestInitialize]
        public void Setup()
        {
            ViewPortExtent = new Rectangle(new Point(0, 0), new Size(500, 500));
        }

        [TestMethod]
        public void TestConversionOfAbsCenterCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(250, 250);
            var expectedResult = new Point(0, 0);

            // ACT
            var actualResult = CoordinateConverter.GetRelativePosition(plottedPoint, ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult, $"Expected: {expectedResult} Actual: {actualResult}");
        }


        [TestMethod]
        public void TestConversionOfAbsTopLeftCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(0,0);
            var expectedResult = new Point(-250, 250);

            // ACT
            var actualResult = CoordinateConverter.GetRelativePosition(plottedPoint, ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult, $"Expected: {expectedResult} Actual: {actualResult}");
        }


        [TestMethod]
        public void TestConversionOfAbsToRightCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(500, 0);
            var expectedResult = new Point(250, 250);

            // ACT
            var actualResult = CoordinateConverter.GetRelativePosition(plottedPoint, ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult, $"Expected: {expectedResult} Actual: {actualResult}");
        }

    }
}

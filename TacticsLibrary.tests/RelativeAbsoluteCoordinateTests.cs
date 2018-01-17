using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Converters;
using TacticsLibrary.Extensions;

namespace TacticsLibrary.tests
{
    [TestClass]
    public class RelativeAbsoluteCoordinateTests
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
            var actualResult = plottedPoint.GetRelativePosition(ViewPortExtent);

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
            var actualResult = plottedPoint.GetRelativePosition(ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult, $"Expected: {expectedResult} Actual: {actualResult}");
        }

        [TestMethod]
        public void TestConversionOfAbsTopRightCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(500, 0);
            var expectedResult = new Point(250, 250);

            // ACT
            var actualResult = plottedPoint.GetRelativePosition(ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult, $"Expected: {expectedResult} Actual: {actualResult}");
        }

        [TestMethod]
        public void TestConversionOfAbsBottomRightCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(500, 500);
            var expectedResult = new Point(250, -250);

            // ACT
            var actualResult = plottedPoint.GetRelativePosition(ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult, $"Expected: {expectedResult} Actual: {actualResult}");
        }

       
    }
}

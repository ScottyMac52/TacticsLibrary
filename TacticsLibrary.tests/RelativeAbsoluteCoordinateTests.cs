using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Extensions;

namespace TacticsLibrary.tests
{
    [TestClass]
    public class RelativeAbsoluteCoordinateTests
    {
        public Rectangle ViewPortExtent { get; set; }
        private List<int> _expectedPositiveYCoords = new List<int>();
        private List<int> _expectedNegativeYCoords = new List<int>();

        [TestInitialize]
        public void Setup()
        {
            ViewPortExtent = new Rectangle(new Point(0, 0), new Size(500, 500));

            for (int intY = 0; intY < ViewPortExtent.GetCenterHeight(); intY++)
            {
                _expectedPositiveYCoords.Add(intY);
                _expectedNegativeYCoords.Add(-1 * intY);
            }
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

        [TestMethod]
        public void TestConversionOfYAbove0CoordinateToRelative()
        {
            // ARRANGE
            var positionCounter = 0;
            for (int y = 0; y < ViewPortExtent.GetCenterHeight(); y++)
            {
                var plottedPoint = new Point(0, y);
                var expectedResult = new Point(0, _expectedNegativeYCoords[positionCounter++]);
                TestYCoord(plottedPoint, ViewPortExtent, expectedResult);
            }
        }


        private void TestYCoord(Point plottedPoint, Rectangle viewPort, Point expectedResult)
        {
            // ACT
            var actualResult = plottedPoint.GetRelativePosition(ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult.Y, actualResult.Y, $"Expected: {expectedResult.Y} Actual: {actualResult.Y}");

        }

    }
}

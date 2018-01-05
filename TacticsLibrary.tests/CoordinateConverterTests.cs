using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TacticsLibrary.TrackingObjects;
using TacticsLibrary.Converters;
using System.Drawing;

namespace TacticsLibrary.tests
{
    [TestClass]
    public class CoordinateConverterTests
    {
        [TestMethod]
        public void EnsureThat90DegreesIsConverted()
        {
            // ARRANGE
            var expectedResult = new Point(10, 0);
            var offset = new Point(0, 0);
            var polarCoord = new PolarCompassReference() { Degrees = 90.00, Radius = 10 };

            // ACT
            var refPoint = CoordinateConverter.CalculatePointFromDegrees(offset, polarCoord.Radius, polarCoord.Degrees);

            // ASSERT
            Assert.IsTrue(refPoint == expectedResult, "There was a conversion error for 90°!");
        }

        [TestMethod]
        public void EnsureThatPointIsConvertedTo90()
        {
            // ARRANGE
            var testPoint = new Point(10, 0);
            var offset = new Point(0, 0);
            var expectedResult = new PolarCompassReference() { Degrees = 90.00, Radius = 10 };

            // ACT
            var polarRef = CoordinateConverter.CalculateDegreesFromPoint(offset, testPoint);

            // ASSERT
            Assert.IsTrue(polarRef.Equals(expectedResult), $"There was a conversion error for {testPoint} - expected: {expectedResult} Got {polarRef}");
        }

        public void EnsureThatPointIsConvertedTo180()
        {
            // ARRANGE
            var testPoint = new Point(0, -20);
            var offset = new Point(0, 0);
            var expectedResult = new PolarCompassReference() { Degrees = 180.00, Radius = 20 };

            // ACT
            var polarRef = CoordinateConverter.CalculateDegreesFromPoint(offset, testPoint);

            // ASSERT
            Assert.IsTrue(polarRef.Equals(expectedResult), $"There was a conversion error for {testPoint} - expected: {expectedResult} Got {polarRef}");
        }



        [TestMethod]
        public void EnsureThat360DegreesIsConverted()
        {
            // ARRANGE
            var expectedResult = new Point(0, -10);
            var offset = new Point(0, 0);
            var polarCoord = new PolarCompassReference() { Degrees = 360.00, Radius = 10 };

            // ACT
            var refPoint = CoordinateConverter.CalculatePointFromDegrees(offset, polarCoord.Radius, polarCoord.Degrees);

            // ASSERT
            Assert.IsTrue(refPoint == expectedResult, "There was a conversion error!");
        }
    }
}

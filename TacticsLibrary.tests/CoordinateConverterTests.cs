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

        // Test Passed
        [TestMethod]
        [TestCategory("Unit")]
        public void TestAngleDeterminiationAllZeros()
        {
            // ARRANGE
            var side1 = 0D;
            var side2 = 0D;
            var side3 = 0D;
            var expectedAngle = double.NaN;

            // ACT
            var actualAngle = CoordinateConverter.GetAngleFromSides(side1, side2, side3);

            // ASSERT
            Assert.AreEqual(expectedAngle, actualAngle, $"Expected Angle {expectedAngle}° Got {actualAngle}°");
        }

        // Test Passed
        [TestMethod]
        [TestCategory("Unit")]
        public void TestAngleDeterminationForX5Y3()
        {
            // ARRANGE
            var side1 = 5.830952D;
            var side2 = 5D;
            var side3 = 3D;
            var expectedAngle = 30.96D;

            // ACT
            var actualAngle = CoordinateConverter.GetAngleFromSides(side1, side2, side3);

            // ASSERT
            Assert.AreEqual(expectedAngle, actualAngle, $"Expected Angle {expectedAngle}° Got {actualAngle}°");
        }
        

        [TestMethod]
        public void EnsureThat90DegreesIsConverted()
        {
            // ARRANGE
            var expectedResult = new Point(10, 0);
            var offset = new Point(0, 0);
            var polarCoord = new PolarCoordinate() { Degrees = 90.00, Radius = 10 };

            // ACT
            var refPoint = CoordinateConverter.CalculatePointFromDegrees(offset, polarCoord.Radius, polarCoord.Degrees);

            // ASSERT
            Assert.IsTrue(refPoint == expectedResult, "There was a conversion error for 90°!");
        }

        [TestMethod]
        public void EnsureThatPointIsConvertedToSelectedDegrees()
        {
            // ARRANGE
            var testPoint = new Point(5, 3);
            var offset = new Point(0, 0);
            var expectedResult = new PolarCoordinate() { Degrees = 30.96377, Radius = 5.831 };

            // ACT
            var polarRef = CoordinateConverter.CalculateDegreesFromPoint(offset, testPoint);

            // ASSERT
            Assert.IsTrue(polarRef.Equals(expectedResult), $"There was a conversion error for {testPoint} - expected: {expectedResult} Got {polarRef}");
        }

        [TestMethod]
        public void EnsureThatPointIsConvertedTo45()
        {
            // ARRANGE
            var testPoint = new Point(5, 5);
            var offset = new Point(0, 0);
            var expectedResult = new PolarCoordinate() { Degrees = 44.9997, Radius = 7.0711 };

            // ACT
            var polarRef = CoordinateConverter.CalculateDegreesFromPoint(offset, testPoint);

            // ASSERT
            Assert.IsTrue(polarRef.Equals(expectedResult), $"There was a conversion error for {testPoint} - expected: {expectedResult} Got {polarRef}");
        }

        [TestMethod]
        public void EnsureThat345TriangleIsConverted()
        {
            // ARRANGE
            var testPoint = new Point(4, 3);
            var offset = new Point(0, 0);
            var expectedResult = new PolarCoordinate() { Degrees = 36.8699, Radius = 5 };

            // ACT
            var polarRef = CoordinateConverter.CalculateDegreesFromPoint(offset, testPoint);

            // ASSERT
            Assert.IsTrue(polarRef.Equals(expectedResult), $"There was a conversion error for {testPoint} - expected: {expectedResult} Got {polarRef}");
        }

        [TestMethod]
        public void EnsureThat360DegreesAreConverted()
        {
            // ARRANGE
            var testPoint = new Point(0, 5);
            var offset = new Point(0, 0);
            var expectedResult = new PolarCoordinate() { Degrees = 90, Radius = 5 };

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
            var expectedResult = new PolarCoordinate() { Degrees = 180.00, Radius = 20 };

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
            var polarCoord = new PolarCoordinate() { Degrees = 360.00, Radius = 10 };

            // ACT
            var refPoint = CoordinateConverter.CalculatePointFromDegrees(offset, polarCoord.Radius, polarCoord.Degrees);

            // ASSERT
            Assert.IsTrue(refPoint == expectedResult, "There was a conversion error!");
        }
    }
}

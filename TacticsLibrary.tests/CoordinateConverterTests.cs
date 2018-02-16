using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using TacticsLibrary.Converters;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;
using TacticsLibrary.DrawObjects;

namespace TacticsLibrary.tests
{
    /// <summary>
    /// Test fixture that checks the bidirectional conversion between cartesian and polar coordinates
    /// </summary>
    [TestClass]
    public class CoordinateConverterTests
    {
        private const int ROUND_FACTOR = 3;
        private readonly Size ViewPortExtent = new Size(248, 248);

        #region Quandrant 1 Tests

        [TestMethod]
        [TestCategory("Quadrant1")]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant1_1()
        {
            // ARRANGE
            var testPoint = new Point(10, 10);
            var expectedResult = new PolarCoordinate(45.00, 14.142);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant1")]
        public void Test1_15Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.259F, .966F);
            var expectedResult = new PolarCoordinate(15.009, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant1")]
        public void Test1_30Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.5F, .866F);
            var expectedResult = new PolarCoordinate(30.001, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant1")]
        public void Test1_45Degrees()
        {
            // ARRANGE
            var testPoint = new Point(50, 50);
            var expectedResult = new PolarCoordinate(45.00, 70.711);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant1")]
        public void Test1_60Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.866F, .5F);
            var expectedResult = new PolarCoordinate(59.999, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant1")]
        public void Test1_75Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.966F, .259F);
            var expectedResult = new PolarCoordinate(74.991, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant1")]
        public void Test1_90Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(1F, 0F);
            var expectedResult = new PolarCoordinate(90.00, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }


        #endregion Quandrant 1 Tests

        #region Quandrant 2 Tests

        [TestMethod]
        [TestCategory("Quadrant2")]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant2()
        {
            // ARRANGE
            var testPoint = new PointF(-10, 10);
            var expectedResult = new PolarCoordinate(315.00, 14.142);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant2")]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant2_1()
        {
            // ARRANGE
            var testPoint = new PointF(-150F, 56F);
            var expectedResult = new PolarCoordinate(290.472, 160.112);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty, 0);
        }

        [TestMethod]
        [TestCategory("Quadrant2")]
        public void Test2_345Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.259F, .966F);
            var expectedResult = new PolarCoordinate(344.991, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant2")]
        public void Test2_330Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.5F, .866F);
            var expectedResult = new PolarCoordinate(329.999, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant2")]
        public void Test2_315Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.707F, .707F);
            var expectedResult = new PolarCoordinate(315, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant2")]
        public void Test2_300Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.866F, .5F);
            var expectedResult = new PolarCoordinate(300.001, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant2")]
        public void Test2_285Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.966F, .259F);
            var expectedResult = new PolarCoordinate(285.009, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant2")]
        public void Test2_270Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-1F, 0);
            var expectedResult = new PolarCoordinate(270.000, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }
        
        #endregion Quandrant 2 Tests

        #region Quandrant 3 Tests

        [TestMethod]
        [TestCategory("Quadrant3")]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant3()
        {
            // ARRANGE
            var testPoint = new PointF(-150, -150);
            var expectedResult = new PolarCoordinate(225.00, 212.132);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant3")]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant3_1()
        {
            // ARRANGE
            var testPoint = new PointF(-225, -225);
            var expectedResult = new PolarCoordinate(225.00, 318.198);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }


        [TestMethod]
        [TestCategory("Quadrant3")]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant3_2()
        {
            // ARRANGE
            var testPoint = new PointF(-125, -225);
            var expectedResult = new PolarCoordinate(209.055, 257.391);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty, 0);
        }

        [TestMethod]
        [TestCategory("Quadrant3")]
        public void Test3_255Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.966F , -.259F);
            var expectedResult = new PolarCoordinate(254.991, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant3")]
        public void Test3_240Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.866F, -.5F);
            var expectedResult = new PolarCoordinate(239.999, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant3")]
        public void Test3_225Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.707F, -.707F);
            var expectedResult = new PolarCoordinate(225, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant3")]
        public void Test3_210Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.5F, -.866F);
            var expectedResult = new PolarCoordinate(210.001, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant3")]
        public void Test3_195Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(-.259F, -.966F);
            var expectedResult = new PolarCoordinate(195.009, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant3")]
        public void Test3_180Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(0, -1F);
            var expectedResult = new PolarCoordinate(180, 1);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        #endregion Quandrant 3 Tests

        #region Quandrant 4 Tests

        [TestMethod]
        [TestCategory("Quadrant4")]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant4()
        {
            // ARRANGE
            var testPoint = new PointF(150, -150);
            var expectedResult = new PolarCoordinate(135.00, 212.132);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant4")]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant4_1()
        {
            // ARRANGE
            var testPoint = new PointF(225, -225);
            var expectedResult = new PolarCoordinate(135.00, 318.198);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant4")]
        public void Test4_165Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.259F, -.966F);
            var expectedResult = new PolarCoordinate(164.991, 1.00);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant4")]
        public void Test4_150Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.5F, -.866F);
            var expectedResult = new PolarCoordinate(149.999, 1.00);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant4")]
        public void Test4_135Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.707F, -.707F);
            var expectedResult = new PolarCoordinate(135, 1.00);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant4")]
        public void Test4_120Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.866F, -.5F);
            var expectedResult = new PolarCoordinate(120.001, 1.00);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        [TestMethod]
        [TestCategory("Quadrant4")]
        public void Test4_105Degrees()
        {
            // ARRANGE
            var testPoint = new PointF(.966F, -.259F);
            var expectedResult = new PolarCoordinate(105.009, 1.00);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult, PointF.Empty);
        }

        #endregion Quandrant 4 Tests
         
        #region Coordinate Conversion from offset tests

        [TestMethod]
        public void TestOffsetConversionAt45()
        {
            // ARRANGE
            var expectedResult = new PointF(7.778F, 7.778F);
            var expectedAbsResult = new PointF(131.778F, 116.222F);
            var startPoint = new PointF(.707F, .707F);

            // ACT
            var actualResult = CoordinateConverter.CalculatePointFromDegrees(startPoint, new PolarCoordinate(45, 10), 3);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedAbsResult, actualResult.GetAbsolutePosition(ViewPortExtent));
        }

        [TestMethod]
        public void TestOffsetConversionAt135Degrees()
        {
            // ARRANGE
            var expectedResult = new PointF(7.77810669F, -7.77810669F);
            var expectedAbsResult = new PointF(131.7781F, 131.7781F);
            var startPoint = new PointF(.70710678F, -.70710678F);

            // ACT
            var actualResult = CoordinateConverter.CalculatePointFromDegrees(startPoint, new PolarCoordinate(135, 10), 3);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(expectedAbsResult, actualResult.GetAbsolutePosition(ViewPortExtent));
        }

        #endregion Coordinate Conversion from offset tests

        #region Helpers

        private void ActAndAssertOnConversion(PointF plottedPoint, PolarCoordinate expectedPolarCoord, PointF offset, int roundingFactor = ROUND_FACTOR)
        {
            // ACT
            var actualResult = plottedPoint.GetPolarCoord();
            var resultPoint = actualResult.GetPoint(offset == PointF.Empty ? new PointF(0F, 0F) : offset, roundingFactor);

            // ASSERT

            Assert.IsTrue(actualResult.Equals(expectedPolarCoord), $"Expected {expectedPolarCoord} Got {actualResult}");
            Assert.AreEqual(plottedPoint, resultPoint);
        }


        #endregion Helpers
    }
}

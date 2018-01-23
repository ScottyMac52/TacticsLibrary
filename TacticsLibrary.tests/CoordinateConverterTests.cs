using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using TacticsLibrary.Converters;
using TacticsLibrary.Extensions;
using TacticsLibrary.Interfaces;
using TacticsLibrary.TrackingObjects;

namespace TacticsLibrary.tests
{
    /// <summary>
    /// Test fixture that checks the bidirectional conversion between cartesian and polar coordinates
    /// </summary>
    [TestClass]
    public class CoordinateConverterTests
    {
        #region Quandrant 1 Tests

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant1()
        {
            // ARRANGE
            var testPoint = new Point(50, 50);
            var expectedResult = new PolarCoordinate(45.00, 70.7107);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }


        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant1_1()
        {
            // ARRANGE
            var testPoint = new Point(10, 10);
            var expectedResult = new PolarCoordinate(45.00, 14.1421);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        #endregion Quandrant 1 Tests

        #region Quandrant 2 Tests
        
        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant2()
        {
            // ARRANGE
            var testPoint = new Point(-10, 10);
            var expectedResult = new PolarCoordinate(315.00, 14.1421);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant2_1()
        {
            // ARRANGE
            var testPoint = new Point(-150, 56);
            var expectedResult = new PolarCoordinate(290.4723, 160.1125);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        #endregion Quandrant 2 Tests

        #region Quandrant 3 Tests

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant3()
        {
            // ARRANGE
            var testPoint = new Point(-150, -150);
            var expectedResult = new PolarCoordinate(225.00, 212.132);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant3_1()
        {
            // ARRANGE
            var testPoint = new Point(-225, -225);
            var expectedResult = new PolarCoordinate(225.00, 318.1981);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }


        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant3_2()
        {
            // ARRANGE
            var testPoint = new Point(-125, -225);
            var expectedResult = new PolarCoordinate(209.0546, 257.3908);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }


        #endregion Quandrant 3 Tests

        #region Quandrant 4 Tests

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant4()
        {
            // ARRANGE
            var testPoint = new Point(150, -150);
            var expectedResult = new PolarCoordinate(135.00, 212.132);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant4_1()
        {
            // ARRANGE
            var testPoint = new Point(225, -225);
            var expectedResult = new PolarCoordinate(135.00, 318.1981);

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        #endregion Quandrant 4 Tests

        #region Distance Tests

        [TestMethod]
        public void TestDistanceCalculationFor345Triangle()
        {
            // ARRANGE
            var expectedDistance = 5.0;
            var testPoint1 = new Point(1, 1);
            var testPoint2 = new Point(4, 5);

            // ACT
            var actualDistance = CoordinateConverter.GetDistance(testPoint1, testPoint2);

            // ASSERT
            Assert.AreEqual(expectedDistance, actualDistance);
        }

        [TestMethod]
        public void TestDistanceCalculationForNegative345Triangle()
        {
            // ARRANGE
            var expectedDistance = 5.0;
            var testPoint1 = new Point(-1, -1);
            var testPoint2 = new Point(-4, -5);

            // ACT
            var actualDistance = CoordinateConverter.GetDistance(testPoint1, testPoint2);

            // ASSERT
            Assert.AreEqual(expectedDistance, actualDistance);
        }

        [TestMethod]
        public void TestForZeroDistance()
        {
            // ARRANGE
            var expectedDistance = 0.0;
            var testPoint1 = new Point(4, 5);
            var testPoint2 = new Point(4, 5);

            // ACT
            var actualDistance = CoordinateConverter.GetDistance(testPoint1, testPoint2);

            // ASSERT
            Assert.AreEqual(expectedDistance, actualDistance);
        }


        #endregion Distance Tests

        #region Helpers
        
        private void ActAndAssertOnConversion(Point plottedPoint, PolarCoordinate expectedPolarCoord)
        {
            // ACT
            var actualResult = plottedPoint.GetPolarCoord();
            var resultPoint = actualResult.GetPoint();

            // ASSERT
            Assert.IsTrue(actualResult.Equals(expectedPolarCoord));
            Assert.AreEqual(plottedPoint, resultPoint);
        }


        #endregion Helpers
    }
}

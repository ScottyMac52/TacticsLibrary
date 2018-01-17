using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using TacticsLibrary.Extensions;

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
            var expectedResult = 45.00;

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }


        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant1_1()
        {
            // ARRANGE
            var testPoint = new Point(10, 10);
            var expectedResult = 45.00;

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
            var expectedResult = 315.00;

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant2_1()
        {
            // ARRANGE
            var testPoint = new Point(-150, 56);
            var expectedResult = 339.5277;

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
            var expectedResult = 225.00;

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant3_1()
        {
            // ARRANGE
            var testPoint = new Point(-225, -225);
            var expectedResult = 225.00;

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
            var expectedResult = 135.00;

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        [TestMethod]
        public void TestAngleFromPointUsingRelativeCoordsQuandrant4_1()
        {
            // ARRANGE
            var testPoint = new Point(225, -225);
            var expectedResult = 135.00;

            // ACT & ASSERT
            ActAndAssertOnConversion(testPoint, expectedResult);
        }

        #endregion Quandrant 4 Tests

        #region Helpers

        private void ActAndAssertOnConversion(Point plottedPoint, double expectedDegrees)
        {
            // ACT
            var actualResult = plottedPoint.GetPolarCoord();
            var resultPoint = actualResult.GetPoint();

            // ASSERT
            Assert.AreEqual(expectedDegrees, actualResult.Degrees);
            Assert.AreEqual(plottedPoint, resultPoint);
        }

        #endregion Helpers
    }
}

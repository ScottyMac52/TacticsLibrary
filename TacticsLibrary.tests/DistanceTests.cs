using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TacticsLibrary.Converters;

namespace TacticsLibrary.tests
{
    [TestClass]
    public class DistanceTests
    {
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
    }
}

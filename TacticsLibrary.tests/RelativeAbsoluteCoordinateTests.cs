using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;
using System.Collections.Generic;
using System.Drawing;
using TacticsLibrary.Converters;
using TacticsLibrary.Extensions;


namespace TacticsLibrary.tests
{
    [TestClass]
    public class RelativeAbsoluteCoordinateTests
    {
        public Size ViewPortExtent { get; set; }

        [TestInitialize]
        public void Setup()
        {
            ViewPortExtent = new Size(500, 500);
        }

        [TestMethod]
        public void TestConversionOfAbsCenterCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());
            var expectedResult = new Point(0, 0);

            // ACT and ASSERT
            VerifyCoordinates(plottedPoint, expectedResult);
        }


        [TestMethod]
        public void TestConversionOfAbsTopLeftCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(0,0);
            var expectedResult = new Point( PositionConverter.NEGATIVE * ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());

            // ACT and ASSERT
            VerifyCoordinates(plottedPoint, expectedResult);
        }

        [TestMethod]
        public void TestConversionOfAbsTopRightCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(ViewPortExtent.Width, 0);
            var expectedResult = new Point(ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());

            // ACT and ASSERT
            VerifyCoordinates(plottedPoint, expectedResult);
        }

        [TestMethod]
        public void TestConversionOfAbsBottomRightCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new Point(ViewPortExtent.Width, ViewPortExtent.Height);
            var expectedResult = new Point(ViewPortExtent.GetCenterWidth(), PositionConverter.NEGATIVE * ViewPortExtent.GetCenterHeight());

            // ACT and ASSERT
            VerifyCoordinates(plottedPoint, expectedResult);
        }

        private void VerifyCoordinates(Point plottedPoint, Point expectedResult)
        {
            // ACT
            var actualResult = plottedPoint.GetRelativePosition(ViewPortExtent);
            var verifyResult = actualResult.GetAbsolutePosition(ViewPortExtent);

            // ASSERT
            Assert.AreEqual(expectedResult, actualResult, $"Expected: {expectedResult} Actual: {actualResult}");
            Assert.AreEqual(plottedPoint, verifyResult, $"Expected: {plottedPoint} Actual: {verifyResult}");
        }

    }
}

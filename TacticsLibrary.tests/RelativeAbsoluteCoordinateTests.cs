using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using TacticsLibrary.Converters;
using TacticsLibrary.Extensions;


namespace TacticsLibrary.tests
{
    [TestClass]
    public class RelativeAbsoluteCoordinateTests
    {
        public SizeF ViewPortExtent { get; private set; }

        [TestInitialize]
        public void Setup()
        {
            ViewPortExtent = new SizeF(496, 496);
        }

        [TestMethod]
        public void TestConversionOfAbsCenterCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new PointF(ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());
            var expectedResult = new PointF(0, 0);

            // ACT and ASSERT
            VerifyCoordinates(plottedPoint, expectedResult);
        }


        [TestMethod]
        public void TestConversionOfAbsTopLeftCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new PointF(0,0);
            var expectedResult = new PointF( PositionConverter.NEGATIVE * ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());

            // ACT and ASSERT
            VerifyCoordinates(plottedPoint, expectedResult);
        }

        [TestMethod]
        public void TestConversionOfAbsTopRightCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new PointF(ViewPortExtent.Width, 0);
            var expectedResult = new PointF(ViewPortExtent.GetCenterWidth(), ViewPortExtent.GetCenterHeight());

            // ACT and ASSERT
            VerifyCoordinates(plottedPoint, expectedResult);
        }

        [TestMethod]
        public void TestConversionOfAbsBottomRightCoordinateToRelative()
        {
            // ARRANGE
            var plottedPoint = new PointF(ViewPortExtent.Width, ViewPortExtent.Height);
            var expectedResult = new PointF(ViewPortExtent.GetCenterWidth(), PositionConverter.NEGATIVE * ViewPortExtent.GetCenterHeight());

            // ACT and ASSERT
            VerifyCoordinates(plottedPoint, expectedResult);
        }

        private void VerifyCoordinates(PointF plottedPoint, PointF expectedResult)
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

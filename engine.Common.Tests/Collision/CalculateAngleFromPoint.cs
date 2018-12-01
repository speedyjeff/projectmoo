using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace engine.Common.Tests
{
    [TestClass]
    public class CalculateAngleFromPoint
    {
        [TestMethod]
        public void Basic()
        { 
            var index = 0;
            foreach (var input in new Quad[]
                {
                    new Quad() { X1 = 0, Y1 = 0, X2 = 0, Y2 = -1, Value = 0 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = 1, Y2 = -1, Value = 45 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = 1, Y2 = 0, Value = 90 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = 1, Y2 = 1, Value = 135 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = 0, Y2 = 1, Value = 180 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = -1, Y2 = 1, Value = 225 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = -1, Y2 = 0, Value = 270 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = -1, Y2 = -1, Value = 315 },
                })
            {
                var result = Collision.CalculateAngleFromPoint(input.X1, input.Y1, input.X2, input.Y2);

                Assert.AreEqual(input.Value, result, string.Format("Test {0}", index));

                index++;
            }
        }

        [TestMethod]
        public void NonZero()
        {
            var index = 0;
            foreach (var input in new Quad[]
                {
                    new Quad() { X1 = 1, Y1 = -1, X2 = 1, Y2 = -2, Value = 0 },
                    new Quad() { X1 = 1, Y1 = -1, X2 = 2, Y2 = -2, Value = 45 },
                    new Quad() { X1 = 1, Y1 = 1, X2 = 2, Y2 = 1, Value = 90 },
                    new Quad() { X1 = 1, Y1 = 1, X2 = 2, Y2 = 2, Value = 135 },
                    new Quad() { X1 = 1, Y1 = 1, X2 = 1, Y2 = 2, Value = 180 },
                    new Quad() { X1 = -1, Y1 = 1, X2 = -2, Y2 = 2, Value = 225 },
                    new Quad() { X1 = -1, Y1 = 1, X2 = -2, Y2 = 1, Value = 270 },
                    new Quad() { X1 = -1, Y1 = -1, X2 = -2, Y2 = -2, Value = 315 },
                })
            {
                var result = Collision.CalculateAngleFromPoint(input.X1, input.Y1, input.X2, input.Y2);

                Assert.AreEqual(input.Value, result, string.Format("Test {0}", index));

                index++;
            }
        }
    }
}

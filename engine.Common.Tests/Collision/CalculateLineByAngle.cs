using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace engine.Common.Tests
{
    [TestClass]
    public class CalculateLineByAngle
    {
        [TestMethod]
        public void Basic()
        {
            var index = 0;
            foreach (var input in new Quad[]
                {
                    new Quad() { X1 = 0, Y1 = 0, X2 = 0, Y2 = -1, Value = 0, Distance = 1 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = 1, Y2 = -1, Value = 45, Distance = (float)Math.Sqrt(2) },
                    new Quad() { X1 = 0, Y1 = 0, X2 = 1, Y2 = 0, Value = 90, Distance = 1 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = 1, Y2 = 1, Value = 135, Distance = (float)Math.Sqrt(2) },
                    new Quad() { X1 = 0, Y1 = 0, X2 = 0, Y2 = 1, Value = 180, Distance = 1 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = -1, Y2 = 1, Value = 225, Distance = (float)Math.Sqrt(2) },
                    new Quad() { X1 = 0, Y1 = 0, X2 = -1, Y2 = 0, Value = 270, Distance = 1 },
                    new Quad() { X1 = 0, Y1 = 0, X2 = -1, Y2 = -1, Value = 315, Distance = (float)Math.Sqrt(2) },
                })
            {
                float x1, y1, x2, y2;
                var result = Collision.CalculateLineByAngle(input.X1, input.Y1, input.Value, input.Distance, 
                    out x1, out y1, out x2, out y2);

                Assert.IsTrue(result, string.Format("Test {0}", index));
                Assert.AreEqual(input.X1, x1, string.Format("Test {0}", index));
                Assert.AreEqual(input.Y1, y1, string.Format("Test {0}", index));
                Assert.AreEqual(input.X2, x2, 0.0001, string.Format("Test {0}", index));
                Assert.AreEqual(input.Y2, y2, 0.0001, string.Format("Test {0}", index));

                index++;
            }
        }

        [TestMethod]
        public void NonZero()
        {
            var index = 0;
            foreach (var input in new Quad[]
                {
                    new Quad() { X1 = 1, Y1 = -1, X2 = 1, Y2 = -2, Value = 0, Distance = 1 },
                    new Quad() { X1 = 1, Y1 = -1, X2 = 2, Y2 = -2, Value = 45, Distance = (float)Math.Sqrt(2) },
                    new Quad() { X1 = 1, Y1 = 1, X2 = 2, Y2 = 1, Value = 90, Distance = 1 },
                    new Quad() { X1 = 1, Y1 = 1, X2 = 2, Y2 = 2, Value = 135, Distance = (float)Math.Sqrt(2) },
                    new Quad() { X1 = 1, Y1 = 1, X2 = 1, Y2 = 2, Value = 180, Distance = 1 },
                    new Quad() { X1 = -1, Y1 = 1, X2 = -2, Y2 = 2, Value = 225, Distance = (float)Math.Sqrt(2) },
                    new Quad() { X1 = -1, Y1 = 1, X2 = -2, Y2 = 1, Value = 270, Distance = 1 },
                    new Quad() { X1 = -1, Y1 = -1, X2 = -2, Y2 = -2, Value = 315, Distance = (float)Math.Sqrt(2) },
                })
            {
                float x1, y1, x2, y2;
                var result = Collision.CalculateLineByAngle(input.X1, input.Y1, input.Value, input.Distance, 
                    out x1, out y1, out x2, out y2);

                Assert.IsTrue(result, string.Format("Test {0}", index));
                Assert.AreEqual(input.X1, x1, string.Format("Test {0}", index));
                Assert.AreEqual(input.Y1, y1, string.Format("Test {0}", index));
                Assert.AreEqual(input.X2, x2, 0.0001, string.Format("Test {0}", index));
                Assert.AreEqual(input.Y2, y2, 0.0001, string.Format("Test {0}", index));

                index++;
            }
        }
    }
}

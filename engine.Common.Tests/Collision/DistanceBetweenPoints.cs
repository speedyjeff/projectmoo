using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using engine.Common;

namespace engine.Common.Tests
{
    [TestClass]
    public class DistanceBetweenPoints
    {
        [TestMethod]
        public void Basic()
        {
            var index = 0;
            foreach (var input in new Quad[]
                {
                    new Quad() { X1 = 0, Y1 = 0, X2 = 1, Y2 = 1, Value = (float)Math.Sqrt(2) },
                    new Quad() { X1 = 0, Y1 = 0, X2 = -1, Y2 = -1, Value = (float)Math.Sqrt(2) },
                    new Quad() { X1 = 1, Y1 = 1, X2 = 0, Y2 = 0, Value = (float)Math.Sqrt(2) },
                    new Quad() { X1 = -1, Y1 = -1, X2 = 1, Y2 = 1, Value = (float)Math.Sqrt(8) }
                })
            {
                var result = Collision.DistanceBetweenPoints(input.X1, input.Y1, input.X2, input.Y2);

                Assert.AreEqual(input.Value, result, string.Format("Test {0}", index));

                index++;
            }
        }
    }
}

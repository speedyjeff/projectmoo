using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace engine.Common.Tests
{
    [TestClass]
    public class LineIntersectingRectangle
    {
        [TestMethod]
        public void Edges()
        {
            // rectangle
            float x, y;
            float width, height;
            x = y = 1;
            width = height = 50;

            var index = 0;
            foreach (var input in new Quad[]
            {
                // top
                new Quad()
                {
                    X1 = 10,
                    Y1 = -35,
                    X2 = 10,
                    Y2 = 10,
                    Result = true
                },
                // bottom
                new Quad()
                {
                    X1 = 10,
                    Y1 = 10,
                    X2 = 10,
                    Y2 = 35,
                    Result = true
                },
                // left
                new Quad()
                {
                    X1 = -35,
                    Y1 = 10,
                    X2 = 10,
                    Y2 = 10,
                    Result = true
                },
                // right
                new Quad()
                {
                    X1 = 10,
                    Y1 = 10,
                    X2 = 35,
                    Y2 = 10,
                    Result = true
                }
            })
            {
                var result = Collision.LineIntersectingRectangle(
                    input.X1, input.Y1, input.X2, input.Y2,
                    x, y, width, height);

                Assert.AreEqual(input.Result, result, string.Format("Test {0}", index));
                index++;
            }
        }

        [TestMethod]
        public void Corner()
        {
            // rectangle
            float x, y;
            float width, height;
            x = y = 25;
            width = height = 50;

            var index = 0;
            foreach (var input in new Quad[]
            {
                // corner
                new Quad()
                {
                    X1 = 40,
                    Y1 = -10,
                    X2 = 60,
                    Y2 = 10,
                    Result = true
                }
            })
            {
                var result = Collision.LineIntersectingRectangle(
                    input.X1, input.Y1, input.X2, input.Y2,
                    x, y, width, height);

                Assert.AreEqual(input.Result, result, string.Format("Test {0}", index));
                index++;
            }
        }
    }
}

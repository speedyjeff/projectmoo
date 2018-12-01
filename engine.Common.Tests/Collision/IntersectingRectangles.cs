using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace engine.Common.Tests
{
    [TestClass]
    public class IntersectingRectangles
    {
        // 4 scenarios... from the point of view of the second object
        //  1) wider
        //  2) narrower
        //  3) from left
        //  4) from right

        [TestMethod]
        public void Top()
        {
            float x3, y3, x4, y4;

            x3 = y3 = 10;
            x4 = y4 = 50;

            var index = 0;
            foreach(var input in new Quad[]
            {
                // wider
                new Quad()
                {
                    X1 = 1,
                    X2 = 60,
                    Y1 = 1,
                    Y2 = 15,
                    Result = true
                },
                // narrow
                new Quad()
                {
                    X1 = 15,
                    X2 = 45,
                    Y1 = 1,
                    Y2 = 15,
                    Result = true
                },
                // left
                new Quad()
                {
                    X1 = 1,
                    X2 = 40,
                    Y1 = 1,
                    Y2 = 15,
                    Result = true
                },
                // right
                new Quad()
                {
                    X1 = 40,
                    X2 = 60,
                    Y1 = 1,
                    Y2 = 15,
                    Result = true
                }
            })
            {
                var result = Collision.IntersectingRectangles(
                    input.X1, input.Y1, input.X2, input.Y2,
                    x3, y3, x4, y4);

                Assert.AreEqual(input.Result, result, string.Format("Test {0}", index));
                index++;
            }
        }

        [TestMethod]
        public void Bottom()
        {
            float x3, y3, x4, y4;

            x3 = y3 = 10;
            x4 = y4 = 50;

            var index = 0;
            foreach (var input in new Quad[]
            {
                // wider
                new Quad()
                {
                    X1 = 1,
                    X2 = 60,
                    Y1 = 40,
                    Y2 = 55,
                    Result = true
                },
                // narrow
                new Quad()
                {
                    X1 = 15,
                    X2 = 45,
                    Y1 = 40,
                    Y2 = 55,
                    Result = true
                },
                // left
                new Quad()
                {
                    X1 = 1,
                    X2 = 40,
                    Y1 = 40,
                    Y2 = 55,
                    Result = true
                },
                // right
                new Quad()
                {
                    X1 = 40,
                    X2 = 60,
                    Y1 = 40,
                    Y2 = 55,
                    Result = true
                }
            })
            {
                var result = Collision.IntersectingRectangles(
                    input.X1, input.Y1, input.X2, input.Y2,
                    x3, y3, x4, y4);

                Assert.AreEqual(input.Result, result, string.Format("Test {0}", index));
                index++;
            }
        }

        [TestMethod]
        public void Left()
        {
            float x3, y3, x4, y4;

            x3 = y3 = 10;
            x4 = y4 = 50;

            var index = 0;
            foreach (var input in new Quad[]
            {
                // wider
                new Quad()
                {
                    X1 = 1,
                    X2 = 20,
                    Y1 = 1,
                    Y2 = 55,
                    Result = true
                },
                // narrow
                new Quad()
                {
                    X1 = 1,
                    X2 = 20,
                    Y1 = 15,
                    Y2 = 45,
                    Result = true
                },
                // top
                new Quad()
                {
                    X1 = 1,
                    X2 = 20,
                    Y1 = 1,
                    Y2 = 15,
                    Result = true
                },
                // bottom
                new Quad()
                {
                    X1 = 1,
                    X2 = 20,
                    Y1 = 45,
                    Y2 = 55,
                    Result = true
                }
            })
            {
                var result = Collision.IntersectingRectangles(
                    input.X1, input.Y1, input.X2, input.Y2,
                    x3, y3, x4, y4);

                Assert.AreEqual(input.Result, result, string.Format("Test {0}", index));
                index++;
            }
        }


        [TestMethod]
        public void Right()
        {
            float x3, y3, x4, y4;

            x3 = y3 = 10;
            x4 = y4 = 50;

            var index = 0;
            foreach (var input in new Quad[]
            {
                // wider
                new Quad()
                {
                    X1 = 20,
                    X2 = 60,
                    Y1 = 1,
                    Y2 = 55,
                    Result = true
                },
                // narrow
                new Quad()
                {
                    X1 = 20,
                    X2 = 60,
                    Y1 = 15,
                    Y2 = 45,
                    Result = true
                },
                // top
                new Quad()
                {
                    X1 = 20,
                    X2 = 60,
                    Y1 = 1,
                    Y2 = 15,
                    Result = true
                },
                // bottom
                new Quad()
                {
                    X1 = 20,
                    X2 = 60,
                    Y1 = 45,
                    Y2 = 55,
                    Result = true
                }
            })
            {
                var result = Collision.IntersectingRectangles(
                    input.X1, input.Y1, input.X2, input.Y2,
                    x3, y3, x4, y4);

                Assert.AreEqual(input.Result, result, string.Format("Test {0}", index));
                index++;
            }
        }
    }
}

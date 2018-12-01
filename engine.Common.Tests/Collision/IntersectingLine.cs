using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace engine.Common.Tests
{
    [TestClass]
    public class IntersectingLine
    {
        [TestMethod]
        public void Parallel()
        {
            var index = 0;
            foreach(var input in new Octa[]
            {
                // zero based
                new Octa()
                {
                    X1 = 0,
                    X3 = 0,
                    X2 = 5,
                    X4 = 5,
                    Y1 = 0,
                    Y2 = 0,
                    Y3 = 5,
                    Y4 = 5,
                    Result = false
                },
                // non-zero
                new Octa()
                {
                    X1 = -1,
                    X3 = -1,
                    X2 = 5,
                    X4 = 5,
                    Y1 = 1,
                    Y2 = 1,
                    Y3 = 5,
                    Y4 = 5,
                    Result = false
                }
            })
            {
                bool result = Collision.IntersectingLine(
                                input.X1, input.Y1,
                                input.X2, input.Y2,
                                input.X3, input.Y3,
                                input.X4, input.Y4);

                Assert.AreEqual(input.Result, result, String.Format("Test {0}", index));
                index++;
            }
        }

        [TestMethod]
        public void Perpendicular()
        {
            var index = 0;
            foreach (var input in new Octa[]
            {
                // middle-middle
                new Octa()
                {
                    // static
                    X1 = 1,
                    Y1 = 3,
                    X2 = 5,
                    Y2 = 3,
                    // crossing
                    X3 = 3,
                    Y3 = 1,
                    X4 = 3,
                    Y4 = 5,
                    Result = true
                },
                // shallow one end
                new Octa()
                {
                    // static
                    X1 = 1,
                    Y1 = 3,
                    X2 = 5,
                    Y2 = 3,
                    // crossing
                    X3 = 4.9999f,
                    Y3 = 2.9999f,
                    X4 = 4.9999f,
                    Y4 = 5,
                    Result = true
                }
                ,
                // shallow the other end
                new Octa()
                {
                    // static
                    X1 = 1,
                    Y1 = 3,
                    X2 = 5,
                    Y2 = 3,
                    // crossing
                    X3 = 1.0001f,
                    Y3 = -10,
                    X4 = 1.0001f,
                    Y4 = 3.000001f,
                    Result = true
                }
            })
            {
                bool result = Collision.IntersectingLine(
                                input.X1, input.Y1,
                                input.X2, input.Y2,
                                input.X3, input.Y3,
                                input.X4, input.Y4);

                Assert.AreEqual(input.Result, result, String.Format("Test {0}", index));
                index++;
            }
        }

        [TestMethod]
        public void CrossCross()
        {
            var index = 0;
            foreach (var input in new Octa[]
            {
                // shallow
                new Octa()
                {
                    // static
                    X1 = 1,
                    Y1 = 0.95f,
                    X2 = 5,
                    Y2 = 1.05f,
                    // crossing
                    X3 = 1,
                    Y3 = 1.05f,
                    X4 = 5,
                    Y4 = 0.95f,
                    Result = true
                },
                // shallow, just miss
                new Octa()
                {
                    // static
                    X1 = 1,
                    Y1 = 1,
                    X2 = 2.5f,
                    Y2 = 5,
                    // crossing
                    X3 = 3,
                    Y3 = 1,
                    X4 = 2.51f,
                    Y4 = 5,
                    Result = false
                }
            })
            {
                bool result = Collision.IntersectingLine(
                                input.X1, input.Y1,
                                input.X2, input.Y2,
                                input.X3, input.Y3,
                                input.X4, input.Y4);

                Assert.AreEqual(input.Result, result, String.Format("Test {0}", index));
                index++;
            }
        }
    }
}

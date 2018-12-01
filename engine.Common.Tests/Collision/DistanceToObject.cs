using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace engine.Common.Tests
{
    [TestClass]
    public class DistanceToObject
    {
        [TestMethod]
        public void Basic()
        {
            float x1, y1, x2, y2;
            float w1, h1, w2, h2;

            // object 1
            x1 = 100;
            y1 = 100;
            w1 = 1000;
            h1 = 10;

            // object 2
            x2 = 250;
            y2 = 250;
            w2 = 50;
            h2 = 50;

            var value = Collision.DistanceToObject(
                x1, y1, w1, h1,
                x2, y2, w2, h2);

            // shortest distance should be object 1 center and object 2 upper left corner
            var dist = Collision.DistanceBetweenPoints(
                x1, y1,
                x2 - (w2 / 2), y2 - (h2 / 2));

            Assert.AreEqual(value, dist);

        }
    }
}

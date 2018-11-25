using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooPlayer : Player
    {
        public MooPlayer()
        {
            XPMax = 50;
            HandCapacity = 5;
            Secondary = new Element[HandCapacity];

            Take(new MooAxe());
        }

        public float Wood { get; set; }
        public float Food { get; set; }
        public float Rock { get; set; }
        public int XP { get; set; }

        public int Level { get; set; }

        public const int XPIncrease = 10;
        public int XPMax { get; set; }

        public override void Draw(IGraphics g)
        {
            // calculate location for fists/object in hand
            float x1, y1, x2, y2;
            Collision.CalculateLineByAngle(X, Y, Angle, Width / 2, out x1, out y1, out x2, out y2);

            // draw what is in their hand
            if (Primary != null && Primary is MooObject)
            {
                var obj = Primary as MooObject;
                g.Image(obj.ImagePath, x2, y2, obj.Width, obj.Height);
            }

            // draw body
            g.Ellipse(new RGBA() { R = 255, A = 255 }, X - Width / 2, Y - Height / 2, Width, Height, true);

            // draw a fist
            g.Ellipse(Color, x2, y2, Width / 3, Width / 3);

            base.Draw(g);
        }
    }
}

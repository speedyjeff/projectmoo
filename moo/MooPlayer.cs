using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

            // default to holding an axe
            Primary = new MooAxe();
        }

        public float Wood { get; set; }
        public float Food { get; set; }
        public float Rock { get; set; }
        public int XP { get; set; }

        public int Level { get; set; }

        public const int XPIncrease = 10;
        public int XPMax { get; set; }
        public const int MaxLevel = 100;

        public override void Draw(IGraphics g)
        {
            // calculate location for fists/object in hand
            float x1, y1, x2, y2;
            Collision.CalculateLineByAngle(X, Y, Angle, Width / 2, out x1, out y1, out x2, out y2);

            // draw what is in their hand
            var loaded = 0f;
            if (Primary != null && Primary.Image != null)
            {
                g.Image(Primary.Image.Image, x2, y2, Primary.Width, Primary.Height);
                if (Primary is RangeWeapon gun)
                {
                    loaded = gun.PercentReloaded();
                }
            }

            // draw body
            g.Ellipse(new RGBA() { R = 255, A = 255 }, X - Width / 2, Y - Height / 2, Width, Height, true);

            // draw a fist
            g.Ellipse(Color, x2, y2, Width / 3, Width / 3);

            // if the weapon is loading, draw a progress bar
            if (loaded > 0 && loaded < 1)
            {
                g.Rectangle(new RGBA() { R = 0, G = 0, B = 255, A = 200 }, X + (Width / 2), Y + (Height / 2), (Width / 2) * loaded, Height / 4, true);
                g.Rectangle(RGBA.Black, X + (Width / 2), Y + (Height / 2), Width / 2, Height / 4, false);
            }

            base.Draw(g);
        }
    }
}

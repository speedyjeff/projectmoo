using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;
using engine.Common.Entities.AI;

namespace moo
{
    class MooTurret : AI
    {
        public MooTurret()
        {
            Name = "MooTurret";
            Health = 20;
            Height = 50;
            Width = 50;

            // initialize the gun (a bow)
            Take(new MooBow()
            {
                Delay = Constants.GlobalClock * 20
            });
        }

        public const int WoodCraftCost = 100;
        public const int RockCraftCost = 100;
        public const int FoodCraftCost = 100;
        public const int LevelCraftCost = 20;

        public override void Draw(IGraphics g)
        {
            // draw the turret
            g.Rectangle(new RGBA() { R = 74, G = 74, B = 172, A = 255 }, X - (Width / 2), Y - (Height / 2), Width, Height, true);
            g.Ellipse(new RGBA() { R = 90, G = 90, B = 90, A = 255 }, X - (Width / 4), Y - (Height / 4), Width / 2, Height / 2, true);

            // display that it is out of ammo
            if ((Primary as MooBow).Clip == 0)
            {
                g.Ellipse(new RGBA() { R = 255, A = 255 }, X - (Width / 2), Y - (Height / 2), 10, 10, true);
            }
            else
            {
                // display the loading symbol
                var loaded = (Primary as MooBow).PercentReloaded();
                if (loaded > 0 && loaded < 1)
                {
                    g.Rectangle(new RGBA() { R = 0, G = 0, B = 255, A = 200 }, X + (Width / 2), Y + (Height / 2), (Width / 2) * loaded, Height / 4, true);
                    g.Rectangle(RGBA.Black, X + (Width / 2), Y + (Height / 2), Width / 2, Height / 4, false);
                }

                // indicate that it has ammo
                g.Ellipse(new RGBA() { G = 255, A = 255 }, X - (Width / 2), Y - (Height / 2), 10, 10, true);
            }

            base.Draw(g);
        }

        public override ActionEnum Action(List<Element> elements, float angleToCenter, bool inZone, ref float xdelta, ref float ydelta, ref float angle)
        {
            // never move
            xdelta = ydelta = 0;

            AI target = null;
            float min = Int32.MaxValue;

            // find the closet AI
            foreach(var elem in elements)
            {
                if (elem.Id == Id) continue;
                if (elem is AI && !(elem is MooTurret))
                {
                    var ai = elem as AI;

                    var dist = Collision.DistanceBetweenPoints(X, Y, ai.X, ai.Y);

                    if (dist < min)
                    {
                        min = dist;
                        target = ai;
                    }
                }
            }

            // get an angle to that AI
            if (target != null)
            {
                angle = Collision.CalculateAngleFromPoint(X, Y, target.X, target.Y);

                // and fire
                return ActionEnum.Attack;
            }

            return ActionEnum.None;
        }
    }
}

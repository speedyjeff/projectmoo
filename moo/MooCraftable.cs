using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooCraftable : MooObject
    {
        public MooCraftable()
        {
            CanAcquire = true;
            Height = Width = 50;
        }

        public void Place(MooPlayer player)
        {
            // place the item a the end of their hand
            float x1, y1, x2, y2;
            Collision.CalculateLineByAngle(player.X, player.Y, player.Angle, player.Width, out x1, out y1, out x2, out y2);
            X = x2;
            Y = y2;

            // make the item solid
            CanMove = false;
            TakesDamage = true;
            ShowDamage = true;
            CanAcquire = false;
            IsSolid = true;
        }
    }
}

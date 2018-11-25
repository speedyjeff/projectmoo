using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooHud : Menu
    {
        public MooHud(MooPlayer me)
        {
            Me = me;
        }

        public override void Draw(IGraphics g)
        {
            // resources
            g.Rectangle(RGBA.Black, g.Width - 100, g.Height - 75, 100, 20, false);
            g.Text(RGBA.Black, g.Width - 100, g.Height - 71, "Wood " + Me.Wood.ToString("f1"), 8);

            g.Rectangle(RGBA.Black, g.Width - 100, g.Height - 95, 100, 20, false);
            g.Text(RGBA.Black, g.Width - 100, g.Height - 91, "Rock " + Me.Rock.ToString("f1"), 8);

            g.Rectangle(RGBA.Black, g.Width - 100, g.Height - 115, 100, 20, false);
            g.Text(RGBA.Black, g.Width - 100, g.Height - 111, "Food " + Me.Food.ToString("f1"), 8);

            // xp
            var wxp = 300;
            var hxp = 10;
            var xxp = (g.Width - wxp) / 2;
            var yxp = g.Height - 115;
            var vwxp = wxp * ((float)Me.XP / (float)Me.XPMax);
            g.Rectangle(RGBA.White, xxp, yxp, vwxp, hxp, true);
            g.Rectangle(RGBA.Black, xxp, yxp, wxp, hxp, false);

            // stats
            g.Text(RGBA.Black, xxp - 130, yxp, "Level  : " + Me.Level.ToString("f1"), 16);
            g.Text(RGBA.Black, xxp - 130, yxp + 20, "Health : " + Me.Health.ToString("f1"), 16);
            g.Text(RGBA.Black, xxp - 130, yxp + 40, "Shield : " + Me.Shield.ToString("f1"), 16);

            // hand
            var count = Me.HandCapacity + 1 /* primary */;
            var wbox = 50; // (g.Width - (2*xbox))/ 5;
            var hbox = 50;
            var xbox = (g.Width - (wbox*count)) / 2;
            var ybox = g.Height - 95;

            // display what is in the hand
            if (Me.Primary != null && Me.Primary is MooObject)
            {
                // put the axe in the first box
                var obj = Me.Primary as MooObject;
                g.Image(obj.ImagePath, xbox, ybox, wbox, hbox);
            }
            g.Rectangle(RGBA.Black, xbox, ybox, wbox, hbox, false);
            // others
            for (int i = 0; i < Me.HandCapacity; i++)
            {
                if (Me.Secondary[i] != null && Me.Secondary[i] is MooObject)
                {
                    // put the axe in the first box
                    var obj = Me.Secondary[i] as MooObject;
                    g.Image(obj.ImagePath, xbox + (wbox * (i+1)), ybox, wbox, hbox);
                }
                g.Rectangle(RGBA.Black, xbox + (wbox * (i+1)), ybox, wbox, hbox, false);
            }

            base.Draw(g);
        }

        #region private
        private MooPlayer Me;
        #endregion
    }
}

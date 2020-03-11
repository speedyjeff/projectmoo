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
    class MooHud : Menu
    {
        public MooHud(MooPlayer me)
        {
            Me = me;
            Images = new Dictionary<string, IImage>();
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
            var rounds = -1;
            if (Me.Primary != null && Me.Primary.Image != null)
            {
                // put the axe in the first box
                g.Image(Me.Primary.Image.Image, xbox, ybox, wbox, hbox);
                if (Me.Primary is RangeWeapon) rounds = (Me.Primary as RangeWeapon).Clip;
            }
            g.Rectangle(RGBA.Black, xbox, ybox, wbox, hbox, false);
            if (rounds > 0) g.Text(RGBA.White, xbox + (wbox / 3), ybox, rounds.ToString(), 16);
            // others
            for (int i = 0; i < Me.HandCapacity; i++)
            {
                rounds = -1;
                if (Me.Secondary[i] != null && Me.Secondary[i].Image != null)
                {
                    g.Image(Me.Secondary[i].Image.Image, xbox + (wbox * (i+1)), ybox, wbox, hbox);
                    if (Me.Secondary[i] is RangeWeapon) rounds = (Me.Secondary[i] as RangeWeapon).Clip;
                }
                g.Rectangle(RGBA.Black, xbox + (wbox * (i+1)), ybox, wbox, hbox, false);
                if (rounds > 0) g.Text(RGBA.White, xbox + (wbox * (i + 1)) + (wbox / 3), ybox, rounds.ToString(), 16);
            }

            base.Draw(g);
        }

        #region private
        private MooPlayer Me;
        private Dictionary<string, IImage> Images;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IImage GetImage(IGraphics g, string path)
        {
            IImage img = null;
            if (!Images.TryGetValue(path, out img))
            {
                img = g.CreateImage(path);
                Images.Add(path, img);
            }
            return img;
        }
        #endregion
    }
}

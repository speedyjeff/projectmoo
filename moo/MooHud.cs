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

            // this is a static length array that can be used without an lock to hold
            // elements that are close and should be displayed on the hud
            ProximityArray = new Element[10];
        }

        public override void Draw(IGraphics g)
        {
            // resources
            g.Rectangle(RGBA.Black, g.Width - 100, g.Height - 75, 100, 20, false);
            g.Text(RGBA.Black, g.Width - 100, g.Height - 71, $"Wood {Me.Wood:f1}", 8);

            g.Rectangle(RGBA.Black, g.Width - 100, g.Height - 95, 100, 20, false);
            g.Text(RGBA.Black, g.Width - 100, g.Height - 91, $"Rock {Me.Rock:f1}", 8);

            g.Rectangle(RGBA.Black, g.Width - 100, g.Height - 115, 100, 20, false);
            g.Text(RGBA.Black, g.Width - 100, g.Height - 111, $"Food {Me.Food:f1}", 8);

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
                if (Me.Primary is RangeWeapon gun) rounds = gun.Clip;
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
                    if (Me.Secondary[i] is RangeWeapon gun) rounds = gun.Clip;
                }
                g.Rectangle(RGBA.Black, xbox + (wbox * (i+1)), ybox, wbox, hbox, false);
                if (rounds > 0) g.Text(RGBA.White, xbox + (wbox * (i + 1)) + (wbox / 3), ybox, rounds.ToString(), 16);
            }

            // show what is in the distance
            for (int i = 0; i < ProximityArray.Length; i++)
            {
                var elem = ProximityArray[i];
                if (elem != null)
                {
                    // translate from world coords into hud coords

                    // get the angle that points to the element (from Me)
                    var angle = Collision.CalculateAngleFromPoint(Me.X, Me.Y, elem.X, elem.Y);
                    // get coords for a point in that direction
                    Collision.CalculateLineByAngle(x: g.Width / 2, y: g.Height / 2, angle, distance: Math.Max(g.Width / 2, g.Height / 2), out float x1, out float y1, out float x2, out float y2);
                    // clip at the edge
                    if (x2 < 0) x1 = 0;
                    if (x2 > g.Width-30) x2 = g.Width-30;
                    if (y2 < 0) y2 = 0;
                    if (y2 > g.Height-50) y2 = g.Height-50;

                    // choose the color based on the type of element
                    var color = RGBA.Black;
                    var type = elem.GetType();
                    if (type == typeof(MooAxe)) color = new RGBA() { R = 200, G = 200, B = 200, A = 255 };
                    else if (type == typeof(MooBow)) color = new RGBA() { R = 185, G = 122, B = 87, A = 255 };
                    else if (type == typeof(MooFood)) color = new RGBA() { R = 240, G = 130, B = 80, A = 255 };
                    else if (type == typeof(MooGold)) color = new RGBA() { R = 214, G = 214, B = 44, A = 255 };
                    else if (type == typeof(MooRock)) color = new RGBA() { R = 180, G = 180, B = 180, A = 255 };
                    else if (type == typeof(MooRockBox)) color = new RGBA() { R = 180, G = 180, B = 180, A = 255 };
                    else if (type == typeof(MooSword)) color = new RGBA() { R = 159, G = 252, B = 253, A = 255 };
                    else if (type == typeof(MooTree)) color = new RGBA() { R = 50, G = 200, B = 80, A = 255 };
                    else if (type == typeof(MooTurret)) color = new RGBA() { R = 0, G = 0, B = 0, A = 255 };
                    else if (type == typeof(MooWoodBox)) color = new RGBA() { R = 80, G = 0, B = 0, A = 255 };
                    else if (type == typeof(MooZombie)) color = new RGBA() { R = 0, G = 128, B = 64, A = 200 };

                    // place an indicator
                    g.Ellipse(color, x2, y2, width: 20, height: 20, fill: true, border: false);
                }
            }

            base.Draw(g);
        }

        #region private
        private MooPlayer Me;
        private Element[] ProximityArray;
        #endregion

        #region internal
        internal void ReceivePlayerView(Player player, ActionDetails details)
        {
            if (Me.Id != player.Id) return;

            // store the view of the player
            for(int i=0; i< ProximityArray.Length; i++)
            {
                if (i < details.Elements.Count &&
                    details.Elements[i] is not MooWall &&
                    details.Elements[i].Id != Me.Id)
                {
                    ProximityArray[i] = details.Elements[i];
                }
                else ProximityArray[i] = null;
            }
        }
        #endregion
    }
}

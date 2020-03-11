using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooRockBox : MooCraftable
    {
        public MooRockBox()
        {
            Name = "MooRockBox";
            Height = Width = 50;
            Health = 15;
        }

        public override ImageSource Image => new ImageSource(path: @"media\rockbox.png");

        public const int RockCraftCost = 10;
        public const int LeelCraftCost = 5;

        public override void Draw(IGraphics g)
        {
            if (!CanAcquire)
            {
                g.Rectangle(new RGBA() { R = 127, G = 61, B = 127, A = 127 }, X - (Width / 2), Y - (Height / 2), Width, Height);
            }
            base.Draw(g);
        }
    }
}

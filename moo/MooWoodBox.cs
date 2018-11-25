using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooWoodBox : MooCraftable
    {
        public MooWoodBox()
        {
            Name = "MooWoodBox";
            Height = Width = 50;
            Health = 5;
        }

        public override string ImagePath => @"media\woodbox.png";

        public const int CraftCost = 10;

        public override void Draw(IGraphics g)
        {
            if (!CanAcquire)
            {
                g.Rectangle(new RGBA() { R = 131, G = 61, B = 61, A = 255 }, X - (Width / 2), Y - (Height / 2), Width, Height);
            }
            base.Draw(g);
        }
    }
}

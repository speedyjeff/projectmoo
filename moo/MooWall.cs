﻿using System;
using System.Collections.Generic;
using System.Text;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    public enum WallDirection { Horiztonal, Vertical };

    class MooWall : Obstacle
    {
        public MooWall(WallDirection dir, float length, float thickness) : base()
        {
            IsSolid = true;
            if (dir == WallDirection.Horiztonal)
            {
                Width = length;
                Height = thickness;
            }
            else if (dir == WallDirection.Vertical)
            {
                Width = thickness;
                Height = length;
            }
            else throw new Exception("Unknown wall direction : " + dir);
        }

        public override void Draw(IGraphics g)
        {
            g.Rectangle(new RGBA() { R = 131, G = 61, B = 61, A = 255 }, X-(Width/2), Y-(Height/2), Width, Height);
            base.Draw(g);
        }
    }
}

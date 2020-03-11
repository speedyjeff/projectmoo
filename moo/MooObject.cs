using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooObject : Tool
    {
        public MooObject()
        {
            Width = Height = 50;
            CanAcquire = true;
        }

        public override void Draw(IGraphics g)
        {
            if (CanAcquire)
            {
                g.Image(Image.Image, X, Y, Width, Height);
            }

            base.Draw(g);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace engine.Common.Entities
{
    public class Health : Element
    {
        public Health()
        {
            CanAcquire = true;
            Health = 25;
            Name = "Health";

            Height = 50;
            Width = 50;
        }

        public override void Draw(IGraphics g)
        {
            g.Rectangle(RGBA.White, X - (Width / 2), Y - (Height / 2), Width, Height);
            var red = new RGBA() { R = 255, G = 0, B = 0, A = 200 };
            g.Line(red, X - (Width / 2), Y, X + (Width / 2), Y, 5f);
            g.Line(red, X, Y - (Height / 2), X, Y + (Height / 2), 5f);
            base.Draw(g);
        }
    }
}

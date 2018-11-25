using System;
using System.Collections.Generic;
using System.Text;

namespace engine.Common.Entities
{
    public class ShotTrajectory : EphemerialElement
    {
        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Damage { get; set; }

        public ShotTrajectory() : base()
        {
            Duration = 10;
        }

        public override void Draw(IGraphics g)
        {
            // determine the thickness of the bullet by the damage (1..5)
            var thickness = (Damage/ 100f) * 20;
            if (thickness > 5) thickness = 5;
            else if (thickness < 1) thickness = 1;
            g.Line(new RGBA() { A = 255, R = 255 }, X1, Y1, X2, Y2, thickness);
            base.Draw(g);
        }
    }
}

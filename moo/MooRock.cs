using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooRock : MooObstacle
    {
        public MooRock()
        {
            Name = "MooRock";
        }

        public const float Gathered = 0.25f;

        public override ImageSource Image => new ImageSource("rock", moo.Images["rock"]);
    }
}

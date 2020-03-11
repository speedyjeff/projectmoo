using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooGold : MooObstacle
    {
        public MooGold()
        {
            Name = "MooGold";
        }

        public override ImageSource Image => new ImageSource(path: @"media\gold.png");
    }
}

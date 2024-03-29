﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooTree : MooObstacle
    {
        public MooTree()
        {
            Name = "MooTree";
        }

        public const float Gathered = 1;

        public override ImageSource Image => new ImageSource("tree", moo.Images["tree"]);
    }
}

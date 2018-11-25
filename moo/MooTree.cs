using System;
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

        public override string ItemImagePath => @"media\tree.png";
    }
}

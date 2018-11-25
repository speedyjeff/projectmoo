using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooAxe : MooObject
    {
        public MooAxe()
        {
            Name = "MooAxe";
            Damage = 5;
            Distance = (int)(Width);
        }

        public override string ImagePath => @"media\axe.png";
    }
}

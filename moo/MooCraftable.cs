using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooCraftable : MooObject
    {
        public MooCraftable()
        {
            CanAcquire = true;
            Height = Width = 50;
        }
    }
}

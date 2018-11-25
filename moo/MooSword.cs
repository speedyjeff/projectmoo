using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooSword : MooObject
    {
        public MooSword()
        {
            Name = "MooSword";
            Damage = 10;
        }

        public override string ImagePath => @"media\sword.png";

        public const int WoodCraftCost = 100;
        public const int RockCraftCost = 150;
    }
}

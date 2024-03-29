﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooBow : RangeWeapon
    {
        public MooBow()
        {
            Name = "MooBow";
            Width = Height = 50;
            // capacity
            ClipCapacity = 100;

            // get the bow ready
            AddAmmo(ArrowChunk); // default, non-expandable
            Reload(); // start loaded

            // damage
            Damage = 75;
            Distance = 600;
            Spread = 0;
            Delay = Constants.GlobalClock * 100;
        }

        public override string EmptySoundPath() => "emptybow";
        public override string FiredSoundPath() => "bow";
        public override string ReloadSoundPath() => "";
        public override ImageSource Image => new ImageSource("bow", moo.Images["bow"]);

        public const int WoodCraftCost = 40;
        public const int RockCraftCost = 10;
        public const int LevelCraftCost = 10;

        public const int ArrowChunk = 20;
    }
}

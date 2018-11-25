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
            ClipCapacity = 20;

            // get the bow ready
            AddAmmo(20); // default, non-expandable
            Reload(); // start loaded

            // damage
            Damage = 100;
            Distance = 600;
            Spread = 0;
            Delay = Constants.GlobalClock * 100;
        }

        public override string EmptySoundPath() => @"media\emptybow.wav";
        public override string FiredSoundPath() => @"media\bow.wav";
        public override string ImagePath => @"media\bow.png";

        public const int WoodCraftCost = 40;
        public const int RockCraftCost = 10;
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooFood : MooObstacle
    {
        public MooFood()
        {
            Name = "MooFood";
        }

        public const int HealthPerFood = 1;
        public const float Gathered = 1;

        public override string ItemImagePath => @"media\food.png";
    }
}
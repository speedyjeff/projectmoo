using System;
using System.Collections.Generic;
using System.Text;

namespace engine.Common.Entities
{
    public class Obstacle : Element
    {
        public Obstacle() : base()
        {
            CanMove = false;
            TakesDamage = false;
            ShowDamage = true;
            IsSolid = true;
            Health = 0;
        }
    }
}

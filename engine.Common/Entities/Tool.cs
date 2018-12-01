using System;
using System.Collections.Generic;
using System.Text;

namespace engine.Common.Entities
{
    public enum AttackStateEnum { None, NeedsReload, Fired, FiredWithContact, FiredAndKilled, NoRounds, Reloaded, FullyLoaded, LoadingRound, Melee, MeleeWithContact, MeleeAndKilled };

    public class Tool : Element
    {
        public Tool() : base()
        {
            CanMove = false;
            TakesDamage = false;
            ShowDamage = true;
            IsSolid = true;
            Health = 0;
        }

        // damage
        public int Distance { get; set; }
        public float Spread { get; protected set; } // degrees
        public int Damage { get; set; }
        public int Delay { get; set; } // ms

        public virtual string UsedSoundPath() => "media/meele.wav";
    }
}

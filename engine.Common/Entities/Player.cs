using System;
using System.Collections.Generic;
using System.Text;

namespace engine.Common.Entities
{
    public enum ActionEnum { None, SwitchPrimary, Pickup, Drop, Reload, Attack, Move, COUNT };

    public class Player : Element
    {
        public Player() : base()
        {
            CanMove = true;
            TakesDamage = true;
            IsSolid = true;
            Health = 50;
            Shield = 0;
            Color = new RGBA() { R = 255, A = 255 };
            Kills = 0;
            HandCapacity = 1;
            Secondary = new Element[HandCapacity];

            // hit box
            Height = 50;
            Width = 50;

            // special melee
            Fists = new Tool() { Distance = (int)Width, Damage = 1 };
        }

        // in hands
        public int HandCapacity { get; set; }
        public Element Primary { get; private set; }
        public Element[] Secondary { get; set; }

        public Tool Fists { get; private set; }

        public RGBA Color { get; protected set; }

        public int Kills { get; set; }

        public int Ranking { get; set; }

        public virtual string HurtSoundPath => "media/hurt.wav";

        public bool Take(Element item)
        {
            if (item is Ammo)
            {
                if (Primary != null && Primary is RangeWeapon)
                {
                    var gun = (Primary as RangeWeapon);
                    gun.AddAmmo((int)item.Health);
                    return true;
                }
            }
            else if (item is Shield)
            {
                if (Shield < Constants.MaxSheld)
                {
                    Shield += item.Shield;
                    if (Shield > Constants.MaxSheld) Shield = Constants.MaxSheld;
                    return true;
                }
            }
            else if (item is Health)
            {
                if (Health < Constants.MaxHealth)
                {
                    Health += item.Health;
                    if (Health > Constants.MaxHealth) Health = Constants.MaxHealth;
                    return true;
                }
            }
            else if (item.CanAcquire)
            {
                if (Primary == null)
                {
                    Primary = item;
                    return true;
                }

                // seek an open slot
                int index = -1;
                for (int i = 0; i < Secondary.Length; i++)
                {
                    if (Secondary[i] == null) index = i;
                }

                if (index >= 0)
                {
                    Secondary[index] = Primary;
                    Primary = item;
                    return true;
                }

                return false;
            }
            else throw new Exception("Unknow item : " + item.GetType());

            return false;
        }

        public virtual AttackStateEnum Attack()
        {
            // check if we have a primary weapon
            if (Primary == null) return AttackStateEnum.Melee;

            if (Primary is RangeWeapon)
            {
                var gun = (Primary as RangeWeapon);

                // check if there is a round in the clip
                int rounds;
                gun.RoundsInClip(out rounds);
                if (rounds <= 0)
                {
                    if (gun.HasAmmo()) return AttackStateEnum.NeedsReload;
                    else return AttackStateEnum.NoRounds;
                }
                // check if gun ready to fire
                if (!gun.CanShoot()) return AttackStateEnum.LoadingRound;

                bool fired = gun.Shoot();
                if (fired) return AttackStateEnum.Fired;
                else throw new Exception("Failed to fire");
            }

            return AttackStateEnum.Melee;
        }

        public AttackStateEnum Reload()
        {
            if (Primary == null) return AttackStateEnum.None;

            if (Primary is RangeWeapon)
            {
                var gun = (Primary as RangeWeapon);

                // check if we have a primary weapon
                if (!gun.HasAmmo()) return AttackStateEnum.NoRounds;
                // check if there are rounds
                if (gun.RoundsInClip(out int rounds)) return AttackStateEnum.FullyLoaded;

                bool reload = gun.Reload();
                if (reload) return AttackStateEnum.Reloaded;
                else throw new Exception("Failed to reload");
            }

            return AttackStateEnum.None;
        }

        public bool SwitchPrimary()
        {
            // push the primary to the end of the secondary hand, and pop off the next
            // from the hand

            // shift the secondary hand
            var tmp = Secondary[0];
            for(int i=1; i<Secondary.Length; i++)
            {
                // shift
                Secondary[i - 1] = Secondary[i];
            }
            Secondary[Secondary.Length - 1] = Primary;

            // move the first in the hand to Primary
            Primary = tmp;

            return (Primary != null);
        }

        public Element DropPrimary()
        {
            if (Primary == null) return null;
            var tmp = Primary;
            Primary = null;
            return tmp;
        }

        public virtual void Feedback(ActionEnum action, object item, bool result)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace engine.Common.Entities
{
    public class RangeWeapon : Tool
    {
        // ammo
        public int Ammo { get; private set; }
        public int ClipCapacity { get; protected set; }
        public int Clip { get; private set; }

        public virtual string EmptySoundPath() => "media/empty.wav";
        public virtual string ReloadSoundPath() => "media/reload.wav";
        public virtual string FiredSoundPath() => "";

        public RangeWeapon() : base()
        {
            CanAcquire = true;
            IsSolid = false;
            Shotdelay = new Stopwatch();
            ResetShotdelay();
        }

        public override void Draw(IGraphics g)
        {
            g.Image(ImagePath, X - Width / 2, Y - Height / 2, Width, Height);
            base.Draw(g);
        }

        public bool HasAmmo()
        {
            return Ammo > 0;
        }

        public void AddAmmo(int ammo)
        {
            if (ammo > 0)
            {
                Ammo += ammo;
            }
        }

        public void ChangeClipCapacity(int capacity)
        {
            ClipCapacity += capacity;
            if (ClipCapacity <= 0) throw new Exception("Must have a positive clip capacity");
        }

        // returns true if full
        public bool RoundsInClip(out int rounds)
        {
            rounds = Clip;
            return Clip >= ClipCapacity;
        }

        public virtual bool CanReload()
        {
            if (!HasAmmo()) return false;
            if (RoundsInClip(out int rounds)) return false;
            return true;
        }

        public virtual bool Reload()
        {
            if (!CanReload()) return false;
            int delta = ClipCapacity - Clip;
            if (delta > Ammo) delta = Ammo;
            Clip += delta;
            Ammo -= delta;
            ResetShotdelay();
            return true;
        }

        public virtual bool CanShoot()
        {
            return (Clip > 0) && CheckShotdelay();
        }

        public virtual bool Shoot()
        {            
            if (!CanShoot()) return false;
            Clip--;
            ResetShotdelay();
            return true;
        }

        public float PercentReloaded()
        {
            // 100%
            if (Shotdelay.ElapsedMilliseconds > Delay) return 1.0f;
            return (float)Shotdelay.ElapsedMilliseconds / (float)Delay;
        }

        #region private
        private Stopwatch Shotdelay;

        private void ResetShotdelay()
        {
            Shotdelay.Stop(); Shotdelay.Reset(); Shotdelay.Start();
        }

        private bool CheckShotdelay()
        {
            return (Shotdelay.ElapsedMilliseconds > Delay);
        }
        #endregion
    }
}

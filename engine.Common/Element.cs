﻿using System;
using System.Collections.Generic;
using System.Text;

namespace engine.Common.Entities
{
    public class Element
    {
        // id
        public int Id { get; private set; }

        // center
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Angle
        {
            get
            {
                return _angle;
            }
            set
            {
                if (value < 0) value *= -1;
                if (value > 360) value = value % 360;
                _angle = value;
            }
        }

        // bounds (hit box)
        public float Height { get; protected set; }
        public float Width { get; protected set; }

        // attributes
        public float Health { get; set; } = 0;
        public float Shield { get; set; } = 0;
        public bool CanMove { get; protected set; } = false;
        public bool TakesDamage { get; protected set; } = false;
        public bool ShowDamage { get; protected set; } = false;
        public bool IsSolid { get; protected set; } = false;
        public bool CanAcquire { get; protected set; } = false;
        public bool IsTransparent { get; protected set; } = false;
        public string Name { get; set; } = "";

        public bool IsDead => (TakesDamage ? Health <= 0 : false);

        public Element()
        {
            Id = GetNextId();
            X = Y = 0;
            Z = Constants.Ground;
        }

        public virtual void Draw(IGraphics g)
        {
            if (Constants.Debug_ShowHitBoxes) g.Rectangle(RGBA.Black, X-(Width/2), Y-(Height/2), Width, Height, false);
            if (CanAcquire)
            {
                g.Text(RGBA.Black, X - Width / 2, Y - Height / 2 - 20, string.Format("[{0}] {1}", Constants.Pickup2, Name));
            }
            if (TakesDamage && ShowDamage && Z == Constants.Ground)
            {
                g.Text(RGBA.Black, X - Width / 2, Y - Height / 2 - 20, string.Format("{0:0}/{1:0}", Health, Shield));
            }
        }

        public void Move(float xDelta, float yDelta)
        {
            X += xDelta;
            Y += yDelta;
        }

        public void ReduceHealth(float damage)
        {
            if (Shield > 0)
            {
                if (Shield > damage)
                {
                    Shield -= damage;
                    return;
                }
                damage -= Shield;
                Shield = 0;
            }
            if (Health > damage)
            {
                Health -= damage;
                return;
            }
            Health = 0;
            return;
        }

        #region private
        private float _angle;
        private static int NextId = 0;
        private static int GetNextId()
        {
            return System.Threading.Interlocked.Increment(ref NextId);
        }
        #endregion
    }
}

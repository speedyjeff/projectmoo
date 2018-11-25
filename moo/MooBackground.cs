using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    class MooBackground : Background
    {
        public MooBackground(int width, int height) : base(width, height)
        {
            Name = "MooBackground";

            Landmarks = new List<Landmark>();

            // lake
            Landmarks.Add(new Landmark()
            {
                Color = new RGBA() { G = 56, B = 255, A = 255 },
                Left = Width / 10,
                Top = Height / 10,
                Width = Width / 4,
                Height = Height / 4,
                IsRound = true,
                Pace = 0.5f,
                Damage = 0
            });
            // river
            Landmarks.Add(new Landmark()
            {
                Color = new RGBA() { G = 200, B = 235, A = 255 },
                Left = 0,
                Top = (3 * Height) / 4,
                Width = Width,
                Height = Height / 10,
                IsRound = false,
                Pace = 1f,
                Damage = 0
            });
            // desert
            Landmarks.Add(new Landmark()
            {
                Color = new RGBA() { R = 230, G = 230, B = 170, A = 255 },
                Left = Width / 2,
                Top = 0,
                Width = Width / 2,
                Height = Height / 2,
                IsRound = false,
                Pace = 4f,
                Damage = 0.01f
            });
        }

        public override void Draw(IGraphics g)
        {
            // clear first
            base.Draw(g);

            // place the landmarks
            foreach (var l in Landmarks)
            {
                if (l.IsRound)
                {
                    g.Ellipse(l.Color, l.Left, l.Top, l.Width, l.Height, true);
                }
                else
                {
                    g.Rectangle(l.Color, l.Left, l.Top, l.Width, l.Height, true);
                }
            }
        }

        public override float Pace(float x, float y)
        {
            var l = InLandmark(x, y);
            if (l != null) return l.Pace;
            else return base.Pace(x, y);
        }

        public override float Damage(float x, float y)
        {
            var l = InLandmark(x, y);
            if (l != null) return l.Damage;
            else return base.Damage(x, y);
        }

        #region private
        class Landmark
        {
            public RGBA Color;
            public float Top;
            public float Left;
            public float Width;
            public float Height;

            public bool IsRound;

            public float Pace;
            public float Damage;
        }
        private List<Landmark> Landmarks;

        private Landmark InLandmark(float x, float y)
        {
            // iterate through the landmarks and return one that this value is part of
            foreach (var l in Landmarks)
            {
                // TODO respect the circle
                    // point within a bounding box
                    if (x > l.Left && x < (l.Left + l.Width) &&
                        y > l.Top && y < (l.Top + l.Height))
                    {
                        return l;
                    }
            }

            return null;
        }
        #endregion
    }
}

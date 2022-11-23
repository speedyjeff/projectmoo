using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using engine.Common;
using engine.Common.Entities;
using engine.Common.Entities.AI;

namespace moo
{
    class MooZombie : AI
    {
        public MooZombie(MooPlayer lunch)
        {
            Name = "MooZombie";
            Width = 40;
            Height = 50;
            Speed = 0.5f;
            Rand = new Random();
            ShowDefaultDrawing = false;

            // establish the brainz sign
            ShowBrainz = false;
            BrainzDuration = new Stopwatch();
            BrainzDuration.Start();

            // the player to chase
            Lunch = lunch;

            // zombie attack
            Fists = new Tool() { Distance = (int)Width, Damage = 10 };
        }

        public const int HordeSize = 10;

        public bool IsLarge { get; set; }

        public float Speed
        {
            get { return MovementSpeed; }
            set
            {
                // must be between 0... and 1
                if (value <= 0) MovementSpeed = 0.01f;
                else if (value > 1) MovementSpeed = 1f;
                else MovementSpeed = value;
            }
        }

        public override void Draw(IGraphics g)
        {
            if (IsLarge)
            {
                g.Ellipse(new RGBA() { R = 0, G = 128, B = 64, A = 200 }, X - (Width / 4), Y - (Height / 4), Width /4, Height /4, true);
                g.Ellipse(new RGBA() { R = 0, G = 128, B = 64, A = 200 }, X - (Width / 2), Y - (Height / 2), Width*1.5f, Height*1.5f, true);
            }
            else
            {
                g.Ellipse(new RGBA() { R = 0, G = 128, B = 64, A = 200 }, X - (Width / 2), Y - (Height / 2), Width, Height, true);
            }

            // show brainz sign
            if (ShowBrainz)
            {
                g.Rectangle(RGBA.White, X, Y - (Height / 2), Width*2, (Height / 2), true);
                g.Ellipse(RGBA.White, X, Y, (Width / 4), (Height / 4), true);
                g.Text(RGBA.Black, X, Y - (Height / 2), "Brainz...", 14);
            }

            base.Draw(g);
        }

        public override void Update()
        {
            // determine if we should show our brainz sign
            if (ShowBrainz)
            {
                if (BrainzDuration.ElapsedMilliseconds > MaxBrainzDuration)
                {
                    ShowBrainz = false;
                }
            }
            else if (Rand.Next() % 20 == 0)
            {
                ShowBrainz = true;
                BrainzDuration.Stop(); BrainzDuration.Reset(); BrainzDuration.Start();
            }
        }

        public override ActionEnum Action(List<Element> elements, float angleToCenter, bool inZone, ref float xdelta, ref float ydelta, ref float zdelta, ref float angle)
        {
            // calculate the direction towards 'Lunch'
            angle = Collision.CalculateAngleFromPoint(X, Y, Lunch.X, Lunch.Y);

            // move towards 'Lunch'
            float x1, y1, x2, y2;
            Collision.CalculateLineByAngle(X, Y, angle, Speed, out x1, out y1, out x2, out y2);
            xdelta = x2 - x1;
            ydelta = y2 - y1;

            // always melee
            return ActionEnum.Attack;
        }

        public override void Feedback(ActionEnum action, object item, bool result)
        {
            if (action == ActionEnum.Move && !result)
            {
                // we tried to move and failed... take damage
                ReduceHealth(BumpingDamage);
            }
        }

        #region private
        private MooPlayer Lunch;
        private const int BumpingDamage = 1;
        private float MovementSpeed = 0f;
        private Random Rand;

        private bool ShowBrainz;
        private const int MaxBrainzDuration = 2000; // 2 seconds
        private Stopwatch BrainzDuration;
        #endregion
    }
}

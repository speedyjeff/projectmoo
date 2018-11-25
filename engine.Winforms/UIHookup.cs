using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using engine.Common;

namespace engine.Winforms
{
    public class UIHookup
    {
        // Pre-requisit, must set ...
        //   DoubleBuffered = true
        //   Height
        //   Width
        public UIHookup(Control home, World world)
        {
            // retain out home control
            Home = home;
            World = world;

            try
            {
                home.SuspendLayout();

                // default handlers
                Home.Resize += OnResize;
                Home.Paint += OnPaint;

                // double buffer
                Surface = new WritableGraphics(BufferedGraphicsManager.Current, Home.CreateGraphics(), Home.Height, Home.Width);
                Sound = new Sounds();

                // initialize the graphics
                World.InitializeGraphics(
                    Surface,
                    Sound
                    );

                // timers
                OnPaintTimer = new Timer();
                OnPaintTimer.Interval = Common.Constants.GlobalClock / 2;
                OnPaintTimer.Tick += OnPaintTimer_Tick;
                OnMoveTimer = new Timer();
                OnMoveTimer.Interval = Common.Constants.GlobalClock / 2;
                OnMoveTimer.Tick += OnMoveTimer_Tick;

                // setup callbacks
                Home.KeyPress += OnKeyPressed;
                Home.MouseUp += OnMouseUp;
                Home.MouseDown += OnMouseDown;
                Home.MouseMove += OnMouseMove;
                Home.MouseWheel += OnMouseWheel;

                OnPaintTimer.Start();
            }
            finally
            {
                Home.ResumeLayout();
            }
        }

        public void ProcessCmdKey(Keys keyData)
        {
            // user input
            if (keyData == Keys.Left) World.KeyPress(Common.Constants.LeftArrow);
            else if (keyData == Keys.Right) World.KeyPress(Common.Constants.RightArrow);
            else if (keyData == Keys.Up) World.KeyPress(Common.Constants.UpArrow);
            else if (keyData == Keys.Down) World.KeyPress(Common.Constants.DownArrow);
            else if (keyData == Keys.Space) World.KeyPress(Common.Constants.Space);
            else if (keyData == Keys.Escape) World.KeyPress(Common.Constants.Esc);

            // command control
            else if (keyData == Keys.Tab) throw new Exception("NYI - show menu"); // show a menu
        }

        #region private
        private WritableGraphics Surface;
        private Sounds Sound;

        private Control Home;

        private World World;

        private Timer OnPaintTimer;
        private Timer OnMoveTimer;

        private void OnPaintTimer_Tick(object sender, EventArgs e)
        {
            Stopwatch duration = new Stopwatch();
            duration.Start();
            World.Paint();
            Home.Refresh();
            duration.Stop();
            if (duration.ElapsedMilliseconds > (Common.Constants.GlobalClock / 2) - 5) System.Diagnostics.Debug.WriteLine("**Paint Duration {0} ms", duration.ElapsedMilliseconds);
        }

        private void OnMoveTimer_Tick(object sender, EventArgs e)
        {
            World.KeyPress(Common.Constants.RightMouse);
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Surface.RawRender(e.Graphics);
        }

        private void OnResize(object sender, EventArgs e)
        {
            Surface.RawResize(Home.CreateGraphics(), Home.Height, Home.Width);
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            World.Mousewheel(e.Delta);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // translate the location into an angle relative to the mid point
            //        360/0
            //   270         90
            //         180

            // Width/2 and Height/2 act as the center point
            float angle = Common.Collision.CalculateAngleFromPoint(Home.Width / 2.0f, Home.Height / 2.0f, e.X, e.Y);

            World.Mousemove(e.X, e.Y, angle);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) World.KeyPress(Common.Constants.LeftMouse);
            else if (e.Button == MouseButtons.Right) OnMoveTimer.Start();
            else if (e.Button == MouseButtons.Middle) World.KeyPress(Common.Constants.MiddleMouse);
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) OnMoveTimer.Stop();
        }

        private void OnKeyPressed(object sender, KeyPressEventArgs e)
        {
            World.KeyPress(e.KeyChar);
            e.Handled = true;
        }
        #endregion
    }
}

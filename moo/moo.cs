using engine.Common;
using engine.Winforms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace moo
{
    public partial class moo : Form
    {
        public moo()
        {
            InitializeComponent();

            SuspendLayout();
            {
                // 
                // moo
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(1500, 800);
                this.Name = "moo";
                this.Text = "moo";
                this.ResumeLayout(false);
                this.DoubleBuffered = true;
            }
            ResumeLayout();

            // load resources
            LoadResources(System.Reflection.Assembly.GetExecutingAssembly());

            // initialize world
            var generator = new MooWorld();

            int width = 0;
            int height = 0;
            var players = generator.GetPlayers();
            var obstacles = generator.GetObstacles(out width, out height);

            var background = new MooBackground(width, height) { GroundColor = new RGBA { R = 100, G = 255, B = 100, A = 255 } };
            var world = new World(new WorldConfiguration()
            {
                Width = width,
                Height = height,
                CenterIndicator = false,
                StartMenu = new MooStartMenu(),
                EnableZoom = true,
                DisplayStats = true,
                HUD = new MooHud(players[0] as MooPlayer)
            },
                players,
                obstacles,
                background
                );
            generator.World = world;
            world.OnAttack += generator.Contact;
            world.OnBeforeKeyPressed += generator.TakeAction;
            UI = new UIHookup(this, world);
        }

        public static Dictionary<string, IImage> Images;

        #region protected
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            UI.ProcessCmdKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region private
        private UIHookup UI;

        private void LoadResources(System.Reflection.Assembly assem)
        {
            // load images
            Images = engine.Winforms.Resources.LoadImages(assem);

            // mark all as transparent
            foreach (var img in Images.Values) img.MakeTransparent(RGBA.White);

            // load sounds
            var sounds = engine.Winforms.Resources.Load(assem, ".wav");

            // pre-load all the sounds
            foreach(var kvp in sounds)
            {
                // must keep the stream alive for the lifetime of the sound
                Sounds.Preload(kvp.Key, new MemoryStream(kvp.Value));
            }
        }
        #endregion
    }
}

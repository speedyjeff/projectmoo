using System;
using System.Windows.Forms;
using engine.Common;
using engine.Common.Entities;
using engine.Winforms;

namespace moo
{
    partial class moo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private UIHookup UI;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
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
                    DisableZoom = false,
                    DisplayStats = true,
                    HUD = new MooHud(players[0] as MooPlayer)
                }, 
                players, 
                obstacles,
                background
                );
            generator.World = world;
            world.OnContact += generator.Contact;
            world.OnBeforeKeyPressed += generator.TakeAction;
            UI = new UIHookup(this, world);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            UI.ProcessCmdKey(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}


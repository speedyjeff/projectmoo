using System;
using System.Collections.Generic;
using System.Text;

using engine.Common;
using engine.Common.Entities;

namespace moo
{
    public class MooStartMenu : Menu
    {
        public MooStartMenu() : base()
        {
        }

        public string KeyboardLayoutPath => "media/keyboard.png";
        public string MouseLayoutPath => "media/mouse.png";

        public override void Draw(IGraphics g)
        {
            var width = 650;
            var height = 300;

            var left = (g.Width - width) / 2;
            var top = (g.Height - height) / 2;


            g.DisableTranslation();
            {
                g.Rectangle(new RGBA() { R = 255, G = 255, B = 255, A = 200 }, left, top, width, height);
                left += 10;
                top += 10;
                g.Text(RGBA.Black, left, top, "Welcome to Project Moo", 32);
                top += 50;
                g.Text(RGBA.Black, left, top, "Shortly you wake up in a  foreign land surrounded by resources.");
                top += 20;
                g.Text(RGBA.Black, left, top, "Gather what you can to make weapons and defenses before the ");
                top += 20;
                g.Text(RGBA.Black, left, top, "horde of zombies arrive.");
                top += 20;
                g.Image(KeyboardLayoutPath, left, top, 190, 140);
                g.Image(MouseLayoutPath, left + 250, top, 120, 170);
                // crafting menu
                g.Text(RGBA.Black, left + 250 + 130, top, "Crafting Menu", 16);
                top += 20;
                g.Text(RGBA.Black, left + 250 + 140, top, "[3] Wood Box (10 wood)", 12);
                top += 20;
                g.Text(RGBA.Black, left + 250 + 140, top, "[4] Rock Box (10 rock)", 12);
                top += 20;
                g.Text(RGBA.Black, left + 250 + 140, top, "[8] Sword (100 wood, 150 rock)", 12);
                top += 40;
                g.Text(new RGBA() { R = 0, G = 127, B = 64, A = 255 }, left + 250 + 140, top, "[9] Zombies", 12);
                top += 50;
                g.Text(RGBA.Black, left, top, "[esc] to start");
            }
            g.EnableTranslation();

            base.Draw(g);
        }
    }
}

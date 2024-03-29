﻿using System;
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

        public override void Draw(IGraphics g)
        {
            // draw
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
                g.Image(KeyboardLayoutImage.Image, left, top, 190, 140);
                g.Image(MouseLayoutImage.Image, left + 230, top, 120, 170);
                // crafting menu
                g.Text(RGBA.Black, left + 250 + 120, top, "Crafting Menu", 16);
                top += 20;
                g.Text(RGBA.Black, left + 250 + 130, top, string.Format("[3] Wood Box ({0} wood, {1} level)", MooWoodBox.WoodCraftCost, MooWoodBox.LevelCraftCost), 8);
                top += 15;
                g.Text(RGBA.Black, left + 250 + 130, top, string.Format("[4] Rock Box ({0} rock, {1} level)", MooRockBox.RockCraftCost, MooRockBox.LeelCraftCost), 8);
                top += 15;
                g.Text(RGBA.Black, left + 250 + 130, top, string.Format("[5] Bow ({0} wood, {1} rock, {2} level)", MooBow.WoodCraftCost, MooBow.RockCraftCost, MooBow.LevelCraftCost), 8);
                top += 15;
                g.Text(RGBA.Black, left + 250 + 130, top, string.Format("    or Arrows ({0} wood)", MooBow.WoodCraftCost), 8);
                top += 15;
                g.Text(RGBA.Black, left + 250 + 130, top, string.Format("[6] Sword ({0} wood, {1} rock, {2} level)", MooSword.WoodCraftCost, MooSword.RockCraftCost, MooSword.LevelCraftCost), 8);
                top += 15;
                g.Text(RGBA.Black, left + 250 + 130, top, string.Format("[7] Turret ({0} wood, {1} rock, {2}, food, {3} level)", MooTurret.WoodCraftCost, MooTurret.RockCraftCost, MooTurret.FoodCraftCost, MooTurret.LevelCraftCost), 8);
                top += 15;
                //g.Text(RGBA.Black, left + 250 + 130, top, string.Format("[8] Landmine ({0} rock, {1}, food, {2} level)", MooTurret.RockCraftCost, MooTurret.FoodCraftCost, MooTurret.LevelCraftCost), 8);
                top += 15;
                g.Text(new RGBA() { R = 0, G = 127, B = 64, A = 255 }, left + 250 + 130, top, "[z] Zombies", 8);
                top += 25;
                g.Text(RGBA.Black, left, top, "[esc] to start");
            }
            g.EnableTranslation();

            base.Draw(g);
        }

        #region private
        private ImageSource KeyboardLayoutImage => new ImageSource("keyboard", moo.Images["keyboard"]);
        private ImageSource MouseLayoutImage => new ImageSource("mouse", moo.Images["mouse"]);
        #endregion
    }
}
